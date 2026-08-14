using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
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
            try
            {
                Document.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                Document.OriginAtMargins = true;
                Document.Print();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
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

        void IPrintPreviewDialogContract.ApplyPrinterSettings()
        {
            if (printerBox.SelectedItem == null)
            {
                ShowError(Messages.print_select_printer);
                return;
            }

            try
            {
                Document.PrinterSettings.PrinterName = printerBox.SelectedItem.ToString();
                hDevMode = Document.PrinterSettings.GetHdevmode();
                pDevMode = GlobalLock(hDevMode);
                int sizeNeeded = DocumentProperties(Handle, IntPtr.Zero, Document.PrinterSettings.PrinterName, IntPtr.Zero, pDevMode, 0);
                if (sizeNeeded <= 0)
                    return;

                devModeData = Marshal.AllocHGlobal(sizeNeeded);
                DocumentProperties(Handle, IntPtr.Zero, printerBox.SelectedItem.ToString(), devModeData, pDevMode, 14);
                Document.PrinterSettings.SetHdevmode(devModeData);
                Document.PrinterSettings.DefaultPageSettings.SetHdevmode(devModeData);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                if (pDevMode != IntPtr.Zero)
                {
                    GlobalUnlock(hDevMode);
                    pDevMode = IntPtr.Zero;
                }

                if (hDevMode != IntPtr.Zero)
                {
                    GlobalFree(hDevMode);
                    hDevMode = IntPtr.Zero;
                }

                if (devModeData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(devModeData);
                    devModeData = IntPtr.Zero;
                }
            }
        }

        private void printPreviewControl_MouseWheel(object sender, MouseEventArgs e)
        {
            presenter.ScrollPreview(e.Delta);
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
        /// Handles the validated event for zoom Box and updates the related state.
        /// </summary>
        private void zoomBox_Validated(object sender, EventArgs e)
        {
            presenter.ZoomValidated();
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
