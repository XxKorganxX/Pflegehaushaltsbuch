using System.Diagnostics;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.UIA3;

namespace Pflegehaushaltsbuch.UiTests;

internal sealed class PflegehaushaltsbuchApp : IDisposable
{
    private readonly UIA3Automation automation;
    private readonly Application application;
    private readonly Process process;

    private PflegehaushaltsbuchApp(Application application, UIA3Automation automation)
    {
        this.application = application;
        this.automation = automation;
        process = Process.GetProcessById(application.ProcessId);
    }

    public Window MainWindow { get; private set; } = null!;

    public static bool HasConfiguredCredentials =>
        !string.IsNullOrWhiteSpace(GetParameterOrEnvironment("UiUser", "PFLEGEHAUSHALTSBUCH_UI_USER"));

    public static PflegehaushaltsbuchApp Start()
    {
        string appPath = ResolveApplicationPath();
        var application = Application.Launch(appPath);
        var automation = new UIA3Automation();
        var app = new PflegehaushaltsbuchApp(application, automation);
        app.MainWindow = Retry.WhileNull(
            () => application.GetMainWindow(automation),
            timeout: TimeSpan.FromSeconds(20),
            interval: TimeSpan.FromMilliseconds(250)).Result
            ?? throw new InvalidOperationException("Pflegehaushaltsbuch did not show a main window within 20 seconds.");

        Retry.WhileFalse(
            () => app.MainWindow.IsAvailable,
            timeout: TimeSpan.FromSeconds(5),
            interval: TimeSpan.FromMilliseconds(100));

        app.TryLoginWithConfiguredCredentials();

        return app;
    }

    public AutomationElement? FindByAutomationId(string automationId)
    {
        return MainWindow.FindFirstDescendant(cf => cf.ByAutomationId(automationId));
    }

    public AutomationElement WaitForElement(string automationId, TimeSpan? timeout = null)
    {
        return Retry.WhileNull(
            () => FindByAutomationId(automationId),
            timeout: timeout ?? TimeSpan.FromSeconds(10),
            interval: TimeSpan.FromMilliseconds(250)).Result
            ?? throw new InvalidOperationException($"UI element '{automationId}' was not found.");
    }

    public Window WaitForWindowContaining(string automationId, TimeSpan? timeout = null)
    {
        return Retry.WhileNull(
            () => FindTopLevelWindowContaining(automationId),
            timeout: timeout ?? TimeSpan.FromSeconds(10),
            interval: TimeSpan.FromMilliseconds(250)).Result
            ?? throw new InvalidOperationException($"Window containing UI element '{automationId}' was not found.");
    }

    public AutomationElement WaitForElementIn(Window window, string automationId, TimeSpan? timeout = null)
    {
        return Retry.WhileNull(
            () => window.FindFirstDescendant(cf => cf.ByAutomationId(automationId)),
            timeout: timeout ?? TimeSpan.FromSeconds(10),
            interval: TimeSpan.FromMilliseconds(250)).Result
            ?? throw new InvalidOperationException($"UI element '{automationId}' was not found in window '{window.Name}'.");
    }

    public bool IsEnabled(string automationId)
    {
        return WaitForElement(automationId).Properties.IsEnabled.Value;
    }

    public void Click(string automationId)
    {
        var element = WaitForElement(automationId);
        element.Patterns.Invoke.Pattern.Invoke();
    }

    public void ClickIn(Window window, string automationId)
    {
        var element = WaitForElementIn(window, automationId);
        element.Patterns.Invoke.Pattern.Invoke();
    }

    public void RequireAuthenticatedNavigation()
    {
        if (!HasConfiguredCredentials)
            Assert.Ignore("Set UiUser/UiPassword or PFLEGEHAUSHALTSBUCH_UI_USER/PFLEGEHAUSHALTSBUCH_UI_PASSWORD to run authenticated UI workflows.");

        bool navigationEnabled = Retry.WhileFalse(
            () => FindByAutomationId("clientManagementButton")?.Properties.IsEnabled.Value == true,
            timeout: TimeSpan.FromSeconds(10),
            interval: TimeSpan.FromMilliseconds(250)).Result;

        Assert.That(navigationEnabled, Is.True, "Login did not enable the main navigation. Check database configuration and UI credentials.");
    }

    public bool WaitForExit(TimeSpan timeout)
    {
        process.Refresh();
        if (process.HasExited)
            return true;

        return process.WaitForExit((int)timeout.TotalMilliseconds);
    }

    public void Dispose()
    {
        try
        {
            process.Refresh();
            if (!process.HasExited)
            {
                process.CloseMainWindow();
                if (!process.WaitForExit(2000))
                    process.Kill();
            }
        }
        catch
        {
            // Test cleanup must not hide the original test failure.
        }

        automation.Dispose();
    }

    private void TryLoginWithConfiguredCredentials()
    {
        string? username = GetConfiguredUsername();
        if (string.IsNullOrWhiteSpace(username))
            return;

        string password = GetParameterOrEnvironment("UiPassword", "PFLEGEHAUSHALTSBUCH_UI_PASSWORD") ?? string.Empty;
        Window? loginWindow = FindWindowContainingWithRetry("userNameBox", TimeSpan.FromSeconds(10));
        if (loginWindow == null)
            return;

        SetValue(loginWindow, "userNameBox", username!);
        SetValue(loginWindow, "passwordBox", password);
        Invoke(loginWindow, "connectButton");

        Retry.WhileTrue(
            () => loginWindow.IsAvailable,
            timeout: TimeSpan.FromSeconds(15),
            interval: TimeSpan.FromMilliseconds(250));
    }

    private Window? FindWindowContainingWithRetry(string automationId, TimeSpan timeout)
    {
        return Retry.WhileNull(
            () => FindTopLevelWindowContaining(automationId),
            timeout: timeout,
            interval: TimeSpan.FromMilliseconds(250)).Result;
    }

    private Window? FindTopLevelWindowContaining(string automationId)
    {
        return application.GetAllTopLevelWindows(automation)
            .FirstOrDefault(window => window.FindFirstDescendant(cf => cf.ByAutomationId(automationId)) != null);
    }

    private static void SetValue(Window window, string automationId, string value)
    {
        var element = window.FindFirstDescendant(cf => cf.ByAutomationId(automationId))
            ?? throw new InvalidOperationException($"Login element '{automationId}' was not found.");

        element.Patterns.Value.Pattern.SetValue(value);
    }

    private static void Invoke(Window window, string automationId)
    {
        var element = window.FindFirstDescendant(cf => cf.ByAutomationId(automationId))
            ?? throw new InvalidOperationException($"Login element '{automationId}' was not found.");

        element.Patterns.Invoke.Pattern.Invoke();
    }

    private static string? GetConfiguredUsername()
    {
        string? username = GetParameterOrEnvironment("UiUser", "PFLEGEHAUSHALTSBUCH_UI_USER");
        if (!string.IsNullOrWhiteSpace(username))
            return username;

        string? password = GetParameterOrEnvironment("UiPassword", "PFLEGEHAUSHALTSBUCH_UI_PASSWORD");
        return string.IsNullOrWhiteSpace(password) ? null : "Admin";
    }

    private static string? GetParameterOrEnvironment(string parameterName, string environmentName)
    {
        string? value = TestContext.Parameters.Get(parameterName);
        if (!string.IsNullOrWhiteSpace(value))
            return value;

        return Environment.GetEnvironmentVariable(environmentName);
    }

    private static string ResolveApplicationPath()
    {
        string? configuredPath =
            TestContext.Parameters.Get("AppPath")
            ?? Environment.GetEnvironmentVariable("PFLEGEHAUSHALTSBUCH_APP_PATH");

        if (!string.IsNullOrWhiteSpace(configuredPath))
            return EnsureFileExists(configuredPath);

        string configuration =
#if DEBUG
            "Debug";
#else
            "Release";
#endif

        DirectoryInfo? directory = new(TestContext.CurrentContext.TestDirectory);
        while (directory != null)
        {
            string candidate = Path.Combine(directory.FullName, "bin", configuration, "Pflegehaushaltsbuch.exe");
            if (File.Exists(candidate))
                return candidate;

            string projectCandidate = Path.Combine(directory.FullName, "Pflegehaushaltsbuch.csproj");
            if (File.Exists(projectCandidate))
                return EnsureFileExists(Path.Combine(directory.FullName, "bin", configuration, "Pflegehaushaltsbuch.exe"));

            directory = directory.Parent;
        }

        throw new FileNotFoundException(
            "Pflegehaushaltsbuch.exe was not found. Build Pflegehaushaltsbuch.csproj first or set PFLEGEHAUSHALTSBUCH_APP_PATH.");
    }

    private static string EnsureFileExists(string path)
    {
        string fullPath = Path.GetFullPath(path);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException("Pflegehaushaltsbuch.exe was not found.", fullPath);

        return fullPath;
    }
}