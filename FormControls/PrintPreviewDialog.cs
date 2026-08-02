using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Data.Graphics;
using Pflegehaushaltsbuch.Databases;
using System.Diagnostics;
using System.Security.Policy;
using System.Net;
using Pflegehaushaltsbuch.Properties;
using Pflegehaushaltsbuch;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom print Preview Dialog control used by the application user interface.
    /// </summary>
    public partial class printPreviewDialog : System.Windows.Forms.PrintPreviewDialog
    {
        string documentName = string.Empty;
        /// <summary>
        /// Creates a new Print Preview Dialog instance and initializes the required state.
        /// </summary>
        public printPreviewDialog(SQLBase sql, PrintDocument document, string documentPath, string documentName)
        {
            InitializeComponent();
            smtpServerBox.Text = sql.Company.SMTP_Host;
            smtpUsernameBox.Text = sql.Company.SMTP_User;
            smtpKeywordBox.Text = sql.Company.SMTP_Password;
            fromEmailBox.Text = sql.User.Email;
            documentName = documentPath;
            DocumentName = documentName;
            Document = document;
            this.sql = sql;
            printerBox.DropDown += m_comboBox_DropDown;
            foreach (string name in PrinterSettings.InstalledPrinters)
                printerBox.Items.Add(name);
            if (printerBox.Items.Count>0)
                printerBox.SelectedItem = new PrintDocument().PrinterSettings.PrinterName;
//printerBox.Invalidate();
            Controls.RemoveAt(2);
            printPreviewControl = Controls[1] as PrintPreviewControl;
            Controls.RemoveAt(1);
            panel.Controls.Add(printPreviewControl, 1, 0);
            zoomBox.MouseWheel += zoomBox_MouseWheel;
            //printPreviewControl.MouseWheel += zoomBox_MouseWheel;
            //printPreviewControl.MouseClick += printPreviewControl_MouseClick;
            printPreviewControl.MouseWheel += printPreviewControl_MouseWheel;
            printsBox.DataBindings.Add("Text", document.PrinterSettings, "Copies", true, DataSourceUpdateMode.OnPropertyChanged);
        }
        PrintPreviewControl printPreviewControl;
        public string Client { get; set; }
        public string DocumentName { get; set; }
        public int Pages { get; set; }
        private SQLBase sql;
        /// <summary>
        /// Handles the print PDF lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnPrintPDF(Stream outStream);
        public event OnPrintPDF PrintPDF;
        void printPreviewControl_MouseWheel(object sender, MouseEventArgs e)
        {
            int current = Win32.GetScrollPos(printPreviewControl.Handle, 1);
            Win32.SetScrollPos(printPreviewControl.Handle, 1, current -= e.Delta, true);
            Win32.PostMessage(printPreviewControl.Handle, (uint)Win32.WM_VSCROLL, 4 + 0x10000 * current, 0);
        }
        /// <summary>
        /// Updates the email data and refreshes the related application state.
        /// </summary>
        public void UpdateEmail(string email)
        {
            toEmailBox.Text = email;
        }
        void printPreviewControl_MouseClick(object sender, MouseEventArgs e)
        {
            /*
            if (zoomBox.Focused)
                return;
            zoomBox.Focus();
             * */
        }
        /// <summary>
        /// Handles the shown lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (Program.DesignMode)
                return;
            zoomBox.Text = printPreviewControl.Zoom.ToString("p0");
            Document.BeginPrint += Document_BeginPrint;
            Document.EndPrint += Document_EndPrint;
        }
        void Document_BeginPrint(object sender, PrintEventArgs e)
        {
            Document.PrinterSettings.ToPage = 0;
        }
        void Document_EndPrint(object sender, PrintEventArgs e)
        {
            Pages = Document.PrinterSettings.ToPage;
            rowBox.Text = Pages.ToString();
        }
        void pageBox_MouseWheel(object sender, MouseEventArgs e)
        {
            int value;
            if (!int.TryParse(pageBox.Text, out value))
                return;
            value += Math.Min(1, Math.Max(-1, e.Delta));
            value = Math.Max(0, value);
            pageBox.Text = value.ToString();
        }
        void zoomBox_MouseWheel(object sender, MouseEventArgs e)
        {
            printPreviewControl.Zoom = Math.Max(0.01, printPreviewControl.Zoom + ((double)Math.Min(1, Math.Max(-1, e.Delta))) * 0.05);
            zoomBox.Text = printPreviewControl.Zoom.ToString("p0");
        }
        /// <summary>
        /// Handles the drop Down event for m combo Box and updates the related state.
        /// </summary>
        private void m_comboBox_DropDown(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            using (System.Drawing.Graphics graphics = CreateGraphics())
            {
                int maxWidth = 0;
                foreach (object obj in printerBox.Items)
                {
                    System.Drawing.SizeF area = graphics.MeasureString(obj.ToString(), printerBox.Font);
                    maxWidth = Math.Max((int)area.Width, maxWidth);
                }
                printerBox.DropDownWidth = maxWidth;
                printerBox.Width = maxWidth+15;
            }
        }
        /// <summary>
        /// Handles the text Changed event for zoom Box and updates the related state.
        /// </summary>
        private void zoomBox_TextChanged(object sender, EventArgs e)
        {
            if (!zoomBox.Focused)
                return;
            double value;
            if (double.TryParse(zoomBox.Text, out value))
            {
                printPreviewControl.Zoom = Math.Max(0.01, value * 0.01);
            }
        }
        /// <summary>
        /// Handles the click event for pdf Button and updates the related state.
        /// </summary>
        private void pdfButton_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Settings.Default.documentPath, documentName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            int sufixNumber = 1;
            string sufix = string.Empty;
            string filename =
                DocumentName +
                "_" +
                (DateTime.Now.ToShortDateString().Replace("\\", "_").Replace("/", "_").Replace(".", "_"));
            filename = Path.Combine(path, filename);
            while (File.Exists(filename + sufix + ".pdf"))
            {
                sufixNumber++;
                sufix = "(" + sufixNumber + ")";
            }
            filename = filename + sufix + ".pdf";
            using (FileStream fs = File.Create(filename))
            {
                if (PrintPDF != null)
                    PrintPDF(fs);
            }
            MessageBox.ShowDialog(this, Messages.document_saved + filename);
        }
        /// <summary>
        /// Handles the 1 event for printer Box Selected Index Changed and updates the related state.
        /// </summary>
        private void printerBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            if (Document != null)
                Document.PrinterSettings.PrinterName = printerBox.SelectedItem.ToString();
        }
        /// <summary>
        /// Handles the click event for print Button and updates the related state.
        /// </summary>
        private void printButton_Click(object sender, EventArgs e)
        {
            Document.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            Document.OriginAtMargins = true;
            Document.Print();
        }
        /// <summary>
        /// Handles the text Changed event for page Box and updates the related state.
        /// </summary>
        private void pageBox_TextChanged(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            int value;
            if (!int.TryParse(pageBox.Text, out value))
                return;
            if (value < 0)
            {
                pageBox.Text = "1";
                return;
            }
            /*
            printPreviewControl.StartPage = value;
            
            if (printPreviewControl.StartPage != value)
                pageBox.Text = printPreviewControl.StartPage.ToString();
             * */
        }
        /// <summary>
        /// Handles the text Changed event for row Box and updates the related state.
        /// </summary>
        private void rowBox_TextChanged(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            int value;
            if (!int.TryParse(rowBox.Text, out value))
                return;
            if (value < 1)
            {
                rowBox.Text = "1";
                return;
            }
            printPreviewControl.Rows = value;
        }
        /// <summary>
        /// Handles the click event for auto Zoom Button and updates the related state.
        /// </summary>
        private void autoZoomButton_Click(object sender, EventArgs e)
        {
            printPreviewControl.AutoZoom = true;
        }
        /// <summary>
        /// Handles the click event for email Button and updates the related state.
        /// </summary>
        private void emailButton_Click(object sender, EventArgs e)
        {
            string emailFrom = sql.User.Email;
            string emailTo = toEmailBox.Text.Trim();
            //if (!sql.Company.IsSMTPValid)
            //    throw new Exception(Messages.printpreview_email_missing_host);
            if (!Data.Company.IsValidEmail(emailTo))
                throw new Exception(Messages.printpreview_missing_valid_email);
            if (!Data.Company.IsValidEmail(emailFrom))
                throw new Exception(Messages.printpreview_email_missing_account);
            //var uri = new System.Uri("a:\\text.txt");
            //    emailTo,
            //    "test",
            //    "", "",
            //    "a:\\text.txt"));// uri.AbsoluteUri));
            ////address, subject, cc, bcc, body))
            string path = Path.Combine(Settings.Default.documentPath, documentName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            int sufixNumber = 1;
            string sufix = string.Empty;
            string filename =
                DocumentName +
                "_" +
                (DateTime.Now.ToShortDateString().Replace("\\", "_").Replace("/", "_").Replace(".", "_"));
            filename = Path.Combine(path, filename);
            while (File.Exists(filename + sufix + ".pdf"))
            {
                sufixNumber++;
                sufix = "(" + sufixNumber + ")";
            }
            filename = filename + sufix + ".pdf";
            using (MemoryStream ms = new MemoryStream())
            {
                if (PrintPDF != null)
                    PrintPDF(ms);
                using (FileStream fs = File.Create(filename))
                {
                    var buffer = ms.ToArray();
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();
                }
            }
            if (!File.Exists(filename))
                throw new Exception(Messages.printpreview_file_create_failed);
            MailMessage mail = new MailMessage(emailFrom, emailTo);
            mail.Subject = DocumentName.Replace("_", " ") + " " + DateTime.Now.ToShortDateString();
            mail.Body = string.Format(Messages.printpreview_important_email, sql.User.Name, sql.User.Phone);
            mail.Attachments.Add(new Attachment(filename));
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.SendCompleted += SmtpClient_SendCompleted;
            smtpClient.Host = smtpServerBox.Text.Trim();
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new System.Net.NetworkCredential(smtpUsernameBox.Text.Trim(), smtpKeywordBox.Text.Trim());
            smtpClient.EnableSsl = true;
            smtpClient.SendAsync(mail, mail);
        }
        /// <summary>
        /// Handles the send Completed event for smtp Client and updates the related state.
        /// </summary>
        private void SmtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    SmtpClient_SendCompleted(sender, e);
                });
                return;
            }
            var mail = e.UserState as MailMessage;
            SmtpClient smtp = (sender as SmtpClient);
            if (e.Cancelled == false && e.Error == null)
            {
                MessageBox.ShowDialog(this, string.Format(Messages.printpreview_email_message_sent, mail.To));
            }
            else
            {
                MessageBox.ShowDialog(this, string.Format(Messages.printpreview_email_message_failed, mail.To));
            }
            smtp.Dispose();
        }
        /// <summary>
        /// Handles the send Completed event for smtp and updates the related state.
        /// </summary>
        private void Smtp_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            object[] objs = (object[])e.UserState;
            var data = objs[0] as MailMessage;
            var form = objs[1] as Form;
            form.Invoke((MethodInvoker)delegate
            {
                MessageBox.ShowDialog(this, string.Format(Messages.printpreview_email_send, data.To));
            });
        }
        /// <summary>
        /// Handles the validated event for zoom Box and updates the related state.
        /// </summary>
        private void zoomBox_Validated(object sender, EventArgs e)
        {
            zoomBox.Text = printPreviewControl.Zoom.ToString("p0");
        }
        [DllImport("winspool.Drv", EntryPoint = "DocumentPropertiesW", SetLastError = true,
ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        static extern int DocumentProperties(IntPtr hwnd, IntPtr hPrinter,
                    [MarshalAs(UnmanagedType.LPWStr)] string pDeviceName,
                    IntPtr pDevModeOutput, IntPtr pDevModeInput, int fMode);
        [DllImport("kernel32.dll")]
        static extern IntPtr GlobalLock(IntPtr hMem);
        [DllImport("kernel32.dll")]
        static extern bool GlobalUnlock(IntPtr hMem);
        [DllImport("kernel32.dll")]
        static extern bool GlobalFree(IntPtr hMem);
        IntPtr hDevMode, pDevMode, devModeData;
        /// <summary>
        /// Handles the click event for email Settings Label and updates the related state.
        /// </summary>
        private void emailSettingsLabel_Click(object sender, EventArgs e)
        {
            emailPanel.Visible = !emailPanel.Visible;
        }
        /// <summary>
        /// Handles the click event for printer Settings Button and updates the related state.
        /// </summary>
        private void printerSettingsButton_Click(object sender, EventArgs e)
        {
            Document.PrinterSettings.PrinterName = printerBox.SelectedItem.ToString();
            hDevMode = Document.PrinterSettings.GetHdevmode();
            pDevMode = GlobalLock(hDevMode);
            int sizeNeeded = DocumentProperties(this.Handle, IntPtr.Zero, Document.PrinterSettings.PrinterName, IntPtr.Zero, pDevMode, 0);
            devModeData = Marshal.AllocHGlobal(sizeNeeded);
            DocumentProperties(this.Handle, IntPtr.Zero, printerBox.SelectedItem.ToString(), devModeData, pDevMode, 14);
            GlobalUnlock(hDevMode);
            Document.PrinterSettings.SetHdevmode(devModeData);
            Document.PrinterSettings.DefaultPageSettings.SetHdevmode(devModeData);
            GlobalFree(hDevMode);
            Marshal.FreeHGlobal(devModeData);
        }
    }
}
