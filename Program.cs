using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.FormControls;
using Pflegehaushaltsbuch.Forms;
using Pflegehaushaltsbuch.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch
{
    /// <summary>
    /// Provides helper methods for program operations used by the application.
    /// </summary>
    static class Program
    {
        private static bool designMode = true;
        public static bool DesignMode { get { return designMode; } set { designMode = value; } }
        public static ColorSet colorSet;
        [STAThread]
        static void Main()
        {
            if (string.IsNullOrWhiteSpace(Settings.Default.documentPath))
            {
                Settings.Default.documentPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Pflegehaushaltsbuch");
                Settings.Default.Save();
            }
            colorSet = new ColorSet();
            Pflegehaushaltsbuch.FormControls.ListBox.ColorSet = colorSet;
            Pflegehaushaltsbuch.FormControls.DataGridView.ColorSet = colorSet;
            Pflegehaushaltsbuch.FormControls.ComboBox.ColorSet = colorSet;
            DesignMode = false;
            if (Settings.Default.RestorePreviousSettings)
            {
                Settings.Default.Upgrade();
                Settings.Default.RestorePreviousSettings = false;
                Settings.Default.Save();
            }
            Forms.Form.baseFont = new System.Drawing.Font(
                Forms.Form.baseFont.FontFamily,
                Settings.Default.FontSize);
            Forms.Form.BackColorMode = Settings.Default.BackgroundColorMode;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CultureInfo cu = string.IsNullOrWhiteSpace(Settings.Default.language)
                ? CultureInfo.InstalledUICulture
                : new CultureInfo(Settings.Default.language);
            Application.CurrentCulture = cu;
            Thread.CurrentThread.CurrentCulture = cu;
            Thread.CurrentThread.CurrentUICulture = cu;
            CultureInfo.DefaultThreadCurrentCulture = cu;
            CultureInfo.DefaultThreadCurrentUICulture = cu;

            Application.ThreadException += (sender, e) =>
            {
                MessageBox.ShowError(null, e.Exception);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                MessageBox.ShowError(null, e.ExceptionObject as Exception);
            };

            var session = new SqlSession();
            Application.Run(new MainForm(session));
        }
    }
}
