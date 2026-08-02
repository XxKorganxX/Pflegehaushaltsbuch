namespace Pflegehaushaltsbuch.UiTests;

[TestFixture]
[Apartment(System.Threading.ApartmentState.STA)]
[NonParallelizable]
internal sealed class PflegehaushaltsbuchUiTests : UiTestFixtureBase
{
    private static readonly string[] MainMenuAutomationIds =
    {
        "cashButton",
        "bankingButton",
        "clientManagementButton",
        "advisorButton",
        "creditButton",
        "recordButton",
        "cashOfficeControlbutton",
        "accountHoldingsButton",
        "userRightsButton",
        "officeCashButton"
    };

    [Test, Order(1)]
    public void Application_Starts_And_Shows_Main_Window()
    {
        Assert.That(App.MainWindow.Name, Does.Contain("Pflege").And.Contain("Haushaltsbuch"));
        Assert.That(App.FindByAutomationId("tabControl1"), Is.Not.Null);
    }

    [Test, Order(2)]
    public void Main_Menu_Contains_Navigation_Buttons()
    {
        foreach (string automationId in MainMenuAutomationIds)
            Assert.That(App.FindByAutomationId(automationId), Is.Not.Null, $"Missing UI element '{automationId}'.");
    }

    [Test, Order(3)]
    public void Main_Menu_Navigation_State_Is_Readable()
    {
        var buttons = MainMenuAutomationIds
            .Select(id => new { Id = id, Element = App.FindByAutomationId(id) })
            .ToArray();

        Assert.That(buttons.All(item => item.Element != null), Is.True);
        Assert.That(buttons.Any(item => item.Element!.Properties.IsEnabled.IsSupported), Is.True);
    }

    [Test, Order(4)]
    public void Login_Enables_Business_Navigation()
    {
        App.RequireAuthenticatedNavigation();

        Assert.That(App.IsEnabled("clientManagementButton"), Is.True);
        Assert.That(App.IsEnabled("creditButton"), Is.True);
    }

    [Test, Order(5)]
    public void Clients_Form_Can_Be_Opened_After_Login()
    {
        App.RequireAuthenticatedNavigation();

        App.Click("clientManagementButton");

        Assert.That(App.WaitForElement("clientsView"), Is.Not.Null);
        Assert.That(App.WaitForElement("activeClientsBox"), Is.Not.Null);
        Assert.That(App.WaitForElement("insertButton"), Is.Not.Null);
        Assert.That(App.WaitForElement("clientBooksButton"), Is.Not.Null);
    }

    [Test, Order(6)]
    public void Clients_Create_Dialog_Validates_Required_Name_And_Can_Be_Cancelled()
    {
        App.RequireAuthenticatedNavigation();

        App.Click("clientManagementButton");
        if (!App.IsEnabled("insertButton"))
            Assert.Ignore("The configured UI user has no insert rights for clients.");

        App.Click("insertButton");
        var createClientDialog = App.WaitForWindowContaining("debitorNrBox");

        Assert.That(App.WaitForElementIn(createClientDialog, "titleBox"), Is.Not.Null);
        Assert.That(App.WaitForElementIn(createClientDialog, "nameBox"), Is.Not.Null);
        Assert.That(App.WaitForElementIn(createClientDialog, "streetBox"), Is.Not.Null);
        Assert.That(App.WaitForElementIn(createClientDialog, "zipcodeBox"), Is.Not.Null);
        Assert.That(App.WaitForElementIn(createClientDialog, "cityBox"), Is.Not.Null);
        Assert.That(App.WaitForElementIn(createClientDialog, "bornBox"), Is.Not.Null);
        Assert.That(App.WaitForElementIn(createClientDialog, "saldoBox"), Is.Not.Null);

        App.ClickIn(createClientDialog, "okButton");
        var validationDialog = App.WaitForWindowContaining("msgBox");
        Assert.That(validationDialog.Name, Is.Not.Empty);
        App.ClickIn(validationDialog, "okButton");

        App.ClickIn(createClientDialog, "cancelButton");
        Assert.That(App.WaitForElement("clientsView"), Is.Not.Null);
    }

    [Test, Order(7)]
    public void Assistants_Form_Can_Be_Opened_After_Login()
    {
        App.RequireAuthenticatedNavigation();

        App.Click("creditButton");

        Assert.That(App.WaitForElement("view"), Is.Not.Null);
        Assert.That(App.WaitForElement("nameBox"), Is.Not.Null);
        Assert.That(App.WaitForElement("createButton"), Is.Not.Null);
        Assert.That(App.WaitForElement("payOutButton"), Is.Not.Null);
    }

    [Test, Order(8)]
    public void Application_Can_Be_Terminated_By_Test_Harness()
    {
        Assert.That(App.MainWindow.IsAvailable, Is.True);
    }
}