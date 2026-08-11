using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.FormControls;
using Pflegehaushaltsbuch.Properties;
using System;
using System.ComponentModel;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class PrintPreviewDialogPresenter
    {
        private readonly IPrintPreviewDialogContract view;
        private readonly SqlSession session;
        private readonly PrintDocument document;
        private readonly string documentPath;

        public PrintPreviewDialogPresenter(IPrintPreviewDialogContract view, SqlSession session, PrintDocument document, string documentPath, string documentName)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            this.view = view;
            this.session = session;
            this.document = document;
            this.documentPath = documentPath;
            view.DocumentName = documentName;
        }

        public virtual void Initialize()
        {
            view.SmtpServer = session.SQL.Company.SMTP_Host;
            view.SmtpUser = session.SQL.Company.SMTP_User;
            view.SmtpPassword = session.SQL.Company.SMTP_Password;
            view.FromEmail = session.SQL.User.Email;
            view.Document = document;
            view.BindPrinterDropDown();

            foreach (string name in PrinterSettings.InstalledPrinters)
            {
                view.AddPrinter(name);
            }

            view.SelectDefaultPrinter();
            view.MovePreviewControlIntoPanel();
            view.BindZoomMouseWheel();
            view.BindPreviewMouseWheel();
            view.BindCopies(document);
        }

        public virtual void ScrollPreview(int delta)
        {
            view.ScrollPreview(delta);
        }

        public virtual void UpdateEmail(string email)
        {
            view.ToEmail = email;
        }

        public virtual void Shown()
        {
            if (Program.DesignMode)
            {
                return;
            }

            view.ZoomText = view.PreviewZoom.ToString("p0");
            view.BindDocumentPrintEvents(view.Document);
        }

        public virtual void BeginPrint()
        {
            view.Document.PrinterSettings.ToPage = 0;
        }

        public virtual void EndPrint()
        {
            view.Pages = view.Document.PrinterSettings.ToPage;
            view.RowText = view.Pages.ToString();
        }

        public virtual void ChangePageByWheel(int delta)
        {
            int value;
            if (!int.TryParse(view.PageText, out value))
            {
                return;
            }

            value += Math.Min(1, Math.Max(-1, delta));
            value = Math.Max(0, value);
            view.PageText = value.ToString();
        }

        public virtual void ChangeZoomByWheel(int delta)
        {
            view.PreviewZoom = Math.Max(0.01, view.PreviewZoom + ((double)Math.Min(1, Math.Max(-1, delta))) * 0.05);
            view.ZoomText = view.PreviewZoom.ToString("p0");
        }

        public virtual void PrinterDropDown()
        {
            if (Program.DesignMode)
            {
                return;
            }

            view.MeasurePrinterDropDown();
        }

        public virtual void ZoomTextChanged()
        {
            if (!view.ZoomFocused)
            {
                return;
            }

            double value;
            if (double.TryParse(view.ZoomText, out value))
            {
                view.PreviewZoom = Math.Max(0.01, value * 0.01);
            }
        }

        public virtual void SavePdf()
        {
            string filename = CreatePdfFileName();
            using (FileStream fs = File.Create(filename))
            {
                view.RaisePrintPdf(fs);
            }

            view.ShowDocumentSaved(filename);
        }

        public virtual void PrinterSelected()
        {
            if (Program.DesignMode)
            {
                return;
            }

            if (view.Document != null)
            {
                view.Document.PrinterSettings.PrinterName = view.SelectedPrinter;
            }
        }

        public virtual void Print()
        {
            view.PrintDocument();
        }

        public virtual void PageTextChanged()
        {
            if (Program.DesignMode)
            {
                return;
            }

            int value;
            if (!int.TryParse(view.PageText, out value))
            {
                return;
            }

            if (value < 0)
            {
                view.PageText = "1";
            }
        }

        public virtual void RowTextChanged()
        {
            if (Program.DesignMode)
            {
                return;
            }

            int value;
            if (!int.TryParse(view.RowText, out value))
            {
                return;
            }

            if (value < 1)
            {
                view.RowText = "1";
                return;
            }

            view.PreviewRows = value;
        }

        public virtual void AutoZoom()
        {
            view.PreviewAutoZoom = true;
        }

        public virtual void SendEmail()
        {
            string emailFrom = session.SQL.User.Email;
            string emailTo = view.ToEmail.Trim();

            if (!Data.Company.IsValidEmail(emailTo))
            {
                throw new Exception(Messages.printpreview_missing_valid_email);
            }

            if (!Data.Company.IsValidEmail(emailFrom))
            {
                throw new Exception(Messages.printpreview_email_missing_account);
            }

            string filename = CreatePdfFileName();
            using (MemoryStream ms = new MemoryStream())
            {
                view.RaisePrintPdf(ms);
                using (FileStream fs = File.Create(filename))
                {
                    byte[] buffer = ms.ToArray();
                    fs.Write(buffer, 0, buffer.Length);
                }
            }

            if (!File.Exists(filename))
            {
                throw new Exception(Messages.printpreview_file_create_failed);
            }

            MailMessage mail = new MailMessage(emailFrom, emailTo);
            mail.Subject = view.DocumentName.Replace("_", " ") + " " + DateTime.Now.ToShortDateString();
            mail.Body = string.Format(Messages.printpreview_important_email, session.SQL.User.Name, session.SQL.User.Phone);
            mail.Attachments.Add(new Attachment(filename));

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.SendCompleted += SendCompleted;
            smtpClient.Host = view.SmtpServer.Trim();
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new NetworkCredential(view.SmtpUser.Trim(), view.SmtpPassword.Trim());
            smtpClient.EnableSsl = true;
            smtpClient.SendAsync(mail, mail);
        }

        public virtual void SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MailMessage mail = e.UserState as MailMessage;
            SmtpClient smtp = sender as SmtpClient;

            if (e.Cancelled == false && e.Error == null)
            {
                view.ShowEmailSent(mail);
            }
            else
            {
                view.ShowEmailFailed(mail);
            }

            if (smtp != null)
            {
                smtp.Dispose();
            }
        }

        public virtual void ZoomValidated()
        {
            view.ZoomText = view.PreviewZoom.ToString("p0");
        }

        public virtual void ToggleEmailSettings()
        {
            view.EmailSettingsVisible = !view.EmailSettingsVisible;
        }

        public virtual void ShowPrinterSettings()
        {
            view.ApplyPrinterSettings();
        }

        private string CreatePdfFileName()
        {
            string path = Path.Combine(Settings.Default.documentPath, documentPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            int sufixNumber = 1;
            string sufix = string.Empty;
            string filename =
                view.DocumentName +
                "_" +
                DateTime.Now.ToShortDateString().Replace("\\", "_").Replace("/", "_").Replace(".", "_");
            filename = Path.Combine(path, filename);

            while (File.Exists(filename + sufix + ".pdf"))
            {
                sufixNumber++;
                sufix = "(" + sufixNumber + ")";
            }

            return filename + sufix + ".pdf";
        }
    }
}
