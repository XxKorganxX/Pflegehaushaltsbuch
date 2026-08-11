using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom print Preview Dialog control used by the application user interface.
    /// </summary>
    public partial class PrintPreviewDialog : System.Windows.Forms.PrintPreviewDialog, IPrintPreviewDialogContract
    {
        private readonly PrintPreviewDialogPresenter presenter;

        /// <summary>
        /// Creates a new Print Preview Dialog instance and initializes the required state.
        /// </summary>
        public PrintPreviewDialog(SqlSession session, PrintDocument document, string documentPath, string documentName)
        {
            InitializeComponent();
            presenter = new PrintPreviewDialogPresenter(this, session, document, documentPath, documentName);
            presenter.Initialize();
        }

        /// <summary>
        /// Handles the print PDF lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnPrintPDF(Stream outStream);
        public event OnPrintPDF PrintPDF;

        private PrintPreviewControl printPreviewControl;
        private IntPtr hDevMode;
        private IntPtr pDevMode;
        private IntPtr devModeData;

        public string Client { get; set; }
        public string DocumentName { get; set; }
        public int Pages { get; set; }

        [DllImport("winspool.Drv", EntryPoint = "DocumentPropertiesW", SetLastError = true,
            ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int DocumentProperties(IntPtr hwnd, IntPtr hPrinter,
            [MarshalAs(UnmanagedType.LPWStr)] string pDeviceName,
            IntPtr pDevModeOutput, IntPtr pDevModeInput, int fMode);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern bool GlobalFree(IntPtr hMem);

        PrintPreviewControl IPrintPreviewDialogContract.PreviewControl
        {
            get { return printPreviewControl; }
        }

        string IPrintPreviewDialogContract.SmtpServer
        {
            get { return smtpServerBox.Text; }
            set { smtpServerBox.Text = value; }
        }

        string IPrintPreviewDialogContract.SmtpUser
        {
            get { return smtpUsernameBox.Text; }
            set { smtpUsernameBox.Text = value; }
        }

        string IPrintPreviewDialogContract.SmtpPassword
        {
            get { return smtpKeywordBox.Text; }
            set { smtpKeywordBox.Text = value; }
        }

        string IPrintPreviewDialogContract.FromEmail
        {
            get { return fromEmailBox.Text; }
            set { fromEmailBox.Text = value; }
        }

        string IPrintPreviewDialogContract.ToEmail
        {
            get { return toEmailBox.Text; }
            set { toEmailBox.Text = value; }
        }

        string IPrintPreviewDialogContract.SelectedPrinter
        {
            get { return printerBox.SelectedItem == null ? string.Empty : printerBox.SelectedItem.ToString(); }
            set { printerBox.SelectedItem = value; }
        }

        int IPrintPreviewDialogContract.PrinterCount
        {
            get { return printerBox.Items.Count; }
        }

        string IPrintPreviewDialogContract.ZoomText
        {
            get { return zoomBox.Text; }
            set { zoomBox.Text = value; }
        }

        bool IPrintPreviewDialogContract.ZoomFocused
        {
            get { return zoomBox.Focused; }
        }

        string IPrintPreviewDialogContract.PageText
        {
            get { return pageBox.Text; }
            set { pageBox.Text = value; }
        }

        string IPrintPreviewDialogContract.RowText
        {
            get { return rowBox.Text; }
            set { rowBox.Text = value; }
        }

        double IPrintPreviewDialogContract.PreviewZoom
        {
            get { return printPreviewControl.Zoom; }
            set { printPreviewControl.Zoom = value; }
        }

        int IPrintPreviewDialogContract.PreviewRows
        {
            get { return printPreviewControl.Rows; }
            set { printPreviewControl.Rows = value; }
        }

        bool IPrintPreviewDialogContract.PreviewAutoZoom
        {
            get { return printPreviewControl.AutoZoom; }
            set { printPreviewControl.AutoZoom = value; }
        }

        bool IPrintPreviewDialogContract.EmailSettingsVisible
        {
            get { return emailPanel.Visible; }
            set { emailPanel.Visible = value; }
        }

        void IPrintPreviewDialogContract.AddPrinter(string printerName)
        {
            printerBox.Items.Add(printerName);
        }

        void IPrintPreviewDialogContract.SelectDefaultPrinter()
        {
            if (printerBox.Items.Count > 0)
            {
                printerBox.SelectedItem = new PrintDocument().PrinterSettings.PrinterName;
            }
        }

        void IPrintPreviewDialogContract.MovePreviewControlIntoPanel()
        {
            Controls.RemoveAt(2);
            printPreviewControl = Controls[1] as PrintPreviewControl;
            Controls.RemoveAt(1);
            panel.Controls.Add(printPreviewControl, 1, 0);
        }

        void IPrintPreviewDialogContract.ScrollPreview(int delta)
        {
            int current = Win32.GetScrollPos(printPreviewControl.Handle, 1);
            Win32.SetScrollPos(printPreviewControl.Handle, 1, current -= delta, true);
            Win32.PostMessage(printPreviewControl.Handle, (uint)Win32.WM_VSCROLL, 4 + 0x10000 * current, 0);
        }

        void IPrintPreviewDialogContract.MeasurePrinterDropDown()
        {
            using (Graphics graphics = CreateGraphics())
            {
                int maxWidth = 0;
                foreach (object obj in printerBox.Items)
                {
                    SizeF area = graphics.MeasureString(obj.ToString(), printerBox.Font);
                    maxWidth = Math.Max((int)area.Width, maxWidth);
                }

                printerBox.DropDownWidth = maxWidth;
                printerBox.Width = maxWidth + 15;
            }
        }

        void IPrintPreviewDialogContract.PrintDocument()
        {
            Document.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            Document.OriginAtMargins = true;
            Document.Print();
        }

        void IPrintPreviewDialogContract.ShowDocumentSaved(string filename)
        {
            MessageBox.ShowDialog(this, Messages.document_saved + filename);
        }

        public void ShowMessage(string msg)
        {
            MessageBox.ShowDialog(this, msg);
        }

        public void ShowError(string msg)
        {
            MessageBox.ShowDialog(this, msg, Messages.error_caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public bool ConfirmMessage(string msg)
        {
            return MessageBox.ShowDialog(this, msg, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public void ApplyUserRights(Pflegehaushaltsbuch.Forms.UserRights rights)
        {
        }

        void IPrintPreviewDialogContract.ShowEmailSent(MailMessage mail)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    ((IPrintPreviewDialogContract)this).ShowEmailSent(mail);
                });
                return;
            }

            MessageBox.ShowDialog(this, string.Format(Messages.printpreview_email_message_sent, mail.To));
        }

        void IPrintPreviewDialogContract.ShowEmailFailed(MailMessage mail)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    ((IPrintPreviewDialogContract)this).ShowEmailFailed(mail);
                });
                return;
            }

            MessageBox.ShowDialog(this, string.Format(Messages.printpreview_email_message_failed, mail.To));
        }

        void IPrintPreviewDialogContract.ApplyPrinterSettings()
        {
            Document.PrinterSettings.PrinterName = printerBox.SelectedItem.ToString();
            hDevMode = Document.PrinterSettings.GetHdevmode();
            pDevMode = GlobalLock(hDevMode);
            int sizeNeeded = DocumentProperties(Handle, IntPtr.Zero, Document.PrinterSettings.PrinterName, IntPtr.Zero, pDevMode, 0);
            devModeData = Marshal.AllocHGlobal(sizeNeeded);
            DocumentProperties(Handle, IntPtr.Zero, printerBox.SelectedItem.ToString(), devModeData, pDevMode, 14);
            GlobalUnlock(hDevMode);
            Document.PrinterSettings.SetHdevmode(devModeData);
            Document.PrinterSettings.DefaultPageSettings.SetHdevmode(devModeData);
            GlobalFree(hDevMode);
            Marshal.FreeHGlobal(devModeData);
        }

        private void printPreviewControl_MouseWheel(object sender, MouseEventArgs e)
        {
            presenter.ScrollPreview(e.Delta);
        }

        /// <summary>
        /// Updates the email data and refreshes the related application state.
        /// </summary>
        public void UpdateEmail(string email)
        {
            presenter.UpdateEmail(email);
        }

        private void printPreviewControl_MouseClick(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Handles the shown lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            presenter.Shown();
        }

        private void Document_BeginPrint(object sender, PrintEventArgs e)
        {
            presenter.BeginPrint();
        }

        private void Document_EndPrint(object sender, PrintEventArgs e)
        {
            presenter.EndPrint();
        }

        private void pageBox_MouseWheel(object sender, MouseEventArgs e)
        {
            presenter.ChangePageByWheel(e.Delta);
        }

        private void zoomBox_MouseWheel(object sender, MouseEventArgs e)
        {
            presenter.ChangeZoomByWheel(e.Delta);
        }

        /// <summary>
        /// Handles the drop Down event for m combo Box and updates the related state.
        /// </summary>
        private void m_comboBox_DropDown(object sender, EventArgs e)
        {
            presenter.PrinterDropDown();
        }

        /// <summary>
        /// Handles the text Changed event for zoom Box and updates the related state.
        /// </summary>
        private void zoomBox_TextChanged(object sender, EventArgs e)
        {
            presenter.ZoomTextChanged();
        }

        /// <summary>
        /// Handles the click event for pdf Button and updates the related state.
        /// </summary>
        private void pdfButton_Click(object sender, EventArgs e)
        {
            presenter.SavePdf();
        }

        /// <summary>
        /// Handles the 1 event for printer Box Selected Index Changed and updates the related state.
        /// </summary>
        private void printerBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            presenter.PrinterSelected();
        }

        /// <summary>
        /// Handles the click event for print Button and updates the related state.
        /// </summary>
        private void printButton_Click(object sender, EventArgs e)
        {
            presenter.Print();
        }

        /// <summary>
        /// Handles the text Changed event for page Box and updates the related state.
        /// </summary>
        private void pageBox_TextChanged(object sender, EventArgs e)
        {
            presenter.PageTextChanged();
        }

        /// <summary>
        /// Handles the text Changed event for row Box and updates the related state.
        /// </summary>
        private void rowBox_TextChanged(object sender, EventArgs e)
        {
            presenter.RowTextChanged();
        }

        /// <summary>
        /// Handles the click event for auto Zoom Button and updates the related state.
        /// </summary>
        private void autoZoomButton_Click(object sender, EventArgs e)
        {
            presenter.AutoZoom();
        }

        /// <summary>
        /// Handles the click event for email Button and updates the related state.
        /// </summary>
        private void emailButton_Click(object sender, EventArgs e)
        {
            presenter.SendEmail();
        }

        /// <summary>
        /// Handles the send Completed event for smtp Client and updates the related state.
        /// </summary>
        private void SmtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            presenter.SendCompleted(sender as SmtpClient, e);
        }

        /// <summary>
        /// Handles the send Completed event for smtp and updates the related state.
        /// </summary>
        private void Smtp_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            presenter.SendCompleted(sender as SmtpClient, e);
        }

        /// <summary>
        /// Handles the validated event for zoom Box and updates the related state.
        /// </summary>
        private void zoomBox_Validated(object sender, EventArgs e)
        {
            presenter.ZoomValidated();
        }

        /// <summary>
        /// Handles the click event for email Settings Label and updates the related state.
        /// </summary>
        private void emailSettingsLabel_Click(object sender, EventArgs e)
        {
            presenter.ToggleEmailSettings();
        }

        /// <summary>
        /// Handles the click event for printer Settings Button and updates the related state.
        /// </summary>
        private void printerSettingsButton_Click(object sender, EventArgs e)
        {
            presenter.ShowPrinterSettings();
        }

        void IPrintPreviewDialogContract.RaisePrintPdf(Stream outStream)
        {
            if (PrintPDF != null)
            {
                PrintPDF(outStream);
            }
        }

        void IPrintPreviewDialogContract.BindDocumentPrintEvents(PrintDocument document)
        {
            document.BeginPrint += Document_BeginPrint;
            document.EndPrint += Document_EndPrint;
        }

        void IPrintPreviewDialogContract.BindPrinterDropDown()
        {
            printerBox.DropDown += m_comboBox_DropDown;
        }

        void IPrintPreviewDialogContract.BindPreviewMouseWheel()
        {
            printPreviewControl.MouseWheel += printPreviewControl_MouseWheel;
        }

        void IPrintPreviewDialogContract.BindZoomMouseWheel()
        {
            zoomBox.MouseWheel += zoomBox_MouseWheel;
        }

        void IPrintPreviewDialogContract.BindCopies(PrintDocument document)
        {
            printsBox.DataBindings.Clear();
            printsBox.DataBindings.Add("Text", document.PrinterSettings, "Copies", true, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}
