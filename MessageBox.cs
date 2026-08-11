using Microsoft.Data.SqlClient;
using MySqlConnector;
using System;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch
{
    /// <summary>
    /// Routes application message boxes to the custom FormControls message box.
    /// </summary>
    public static class MessageBox
    {
        private static string GetErrorMessage(Exception err)
        {
            if (err == null)
                return Messages.unknown_error;

            if (err is SqlException sqlErr)
                return string.Format(Messages.sql_error, sqlErr.Number, sqlErr.Message);

            if (err is MySqlException mySqlErr)
                return string.Format(Messages.mysql_error, mySqlErr.Number, mySqlErr.Message);

            return err.Message;
        }

        public static void ShowError(IWin32Window owner, Exception err)
        {
            string message = GetErrorMessage(err);

            if (Application.OpenForms.Count > 0)
            {
                var form = Application.OpenForms[0];

                if (form != null && !form.IsDisposed && form.IsHandleCreated && form.InvokeRequired)
                {
                    form.Invoke((MethodInvoker)delegate
                    {
                        ShowDialog(owner, message, Messages.error_caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                    return;
                }
            }

            ShowDialog(owner, message, Messages.error_caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ShowErrorDialog(IWin32Window owner, Exception err, MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            return ShowDialog(
                owner,
                GetErrorMessage(err),
                Messages.error_caption,
                buttons,
                MessageBoxIcon.Error);
        }

        public static DialogResult Show(string text)
        {
            return FormControls.MessageBox.ShowDialog(text);
        }

        public static DialogResult Show(string text, string caption)
        {
            return FormControls.MessageBox.ShowDialog(null, text, caption);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return FormControls.MessageBox.ShowDialog(null, text, caption, buttons);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return FormControls.MessageBox.ShowDialog(null, text, caption, buttons, icon);
        }

        public static DialogResult Show(IWin32Window owner, string text)
        {
            return FormControls.MessageBox.ShowDialog(owner, text);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            return FormControls.MessageBox.ShowDialog(owner, text, caption);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            return FormControls.MessageBox.ShowDialog(owner, text, caption, buttons);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return FormControls.MessageBox.ShowDialog(owner, text, caption, buttons, icon);
        }

        public static DialogResult ShowDialog(string text)
        {
            return FormControls.MessageBox.ShowDialog(text);
        }

        public static DialogResult ShowDialog(IWin32Window owner, string text)
        {
            return FormControls.MessageBox.ShowDialog(owner, text);
        }

        public static DialogResult ShowDialog(IWin32Window owner, string text, string caption)
        {
            return FormControls.MessageBox.ShowDialog(owner, text, caption);
        }

        public static DialogResult ShowDialog(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            return FormControls.MessageBox.ShowDialog(owner, text, caption, buttons);
        }

        public static DialogResult ShowDialog(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return FormControls.MessageBox.ShowDialog(owner, text, caption, buttons, icon);
        }
    }
}
