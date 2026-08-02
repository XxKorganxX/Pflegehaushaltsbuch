namespace Pflegehaushaltsbuch.UiTests;

internal abstract class UiTestFixtureBase
{
    protected PflegehaushaltsbuchApp App { get; private set; } = null!;

    [OneTimeSetUp]
    public void StartApplication()
    {
        App = PflegehaushaltsbuchApp.Start();
    }

    [OneTimeTearDown]
    public void StopApplication()
    {
        App.Dispose();
    }
}