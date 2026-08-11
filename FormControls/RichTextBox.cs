using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
//using Word = Microsoft.Office.Interop.Word;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom rich Text Box control used by the application user interface.
    /// </summary>
    public partial class RichTextBox : System.Windows.Forms.RichTextBox
    {
        /// <summary>
        /// Creates a new Rich Text Box instance and initializes the required state.
        /// </summary>
        public RichTextBox()
        {
            ControlStyles styles = new ControlStyles();
            if (BackColor == Color.Transparent)
                styles = ControlStyles.SupportsTransparentBackColor|ControlStyles.OptimizedDoubleBuffer;
            else
                styles = ControlStyles.OptimizedDoubleBuffer;
            SetStyle(styles, true);
            BorderStyle = BorderStyle.None;
        
            VScroll += RichTextBox_VScroll;
            HScroll += RichTextBox_HScroll;
        }
        /// <summary>
        /// Handles the text Changed event for rich Text Box and updates the related state.
        /// </summary>
        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {
            SuspendLayout();
            var caretPos = SelectionStart;
            var texts = Text.Split(new char[] {' '});
            Clear();
            int currentCount = 0;
            int count = texts.Length;
            //Word.ProofreadingErrors errors = doc.SpellingErrors;
            foreach (var text in texts)
            {
                currentCount++;
                //SelectionStart = text.Length;
                SelectionColor = ForeColor;
                AppendText(text);
                if (currentCount < count)
                    AppendText(" ");
            }
            
            SelectionStart = caretPos;
            SelectionColor = ForeColor;
            ResumeLayout();
            return;
            /*
            string text = Text;
            int length = text.Length;
            if (length == 0)
                return;
            int startIndex = 0;
            Clear();
            do
            {
                int wordEnd = text.IndexOf(' ', startIndex);
                string subText = "";
                if (wordEnd == -1)
                {
                    subText = text.Substring(startIndex);
                    wordEnd = startIndex + subText.Length;
                }
                else
                    subText = text.Substring(startIndex, wordEnd - startIndex);
                startIndex = wordEnd;
                //Zum nächsten Wort gehen
                while (startIndex < length && !char.IsLetter(text[startIndex]))
                    startIndex++;
            }
            while (startIndex < length);
            SelectionColor = ForeColor;
            ResumeLayout();
            */
        }
        bool OnMouseOver = false;
        /// <summary>
        /// Runs the paint Border operation and updates the related application state.
        /// </summary>
        private void PaintBorder()
        {
            using (var g = CreateGraphics())
            {
                if (OnMouseOver || Focused)
                {
                    g.DrawRectangle(ControlColors.AccentPen, ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                }
                else
                    ControlPaint.DrawBorder(g, ClientRectangle, Color.Black,
                        ButtonBorderStyle.Solid);
            }
            
        }
        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            PaintBorder();
        }
        /// <summary>
        /// Handles the h Scroll event for rich Text Box and updates the related state.
        /// </summary>
        private void RichTextBox_HScroll(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the v Scroll event for rich Text Box and updates the related state.
        /// </summary>
        private void RichTextBox_VScroll(object sender, EventArgs e)
        {
            Invalidate();
            PaintBorder();
            //Refresh();       
        }
        /// <summary>
        /// Handles the text Changed lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            PaintBorder();
        }
        /// <summary>
        /// Handles the mouse Move lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }
        /// <summary>
        /// Handles the mouse Enter lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            PaintBorder();
        }
        /// <summary>
        /// Handles the mouse Leave lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            PaintBorder();
        }
        /// <summary>
        /// Handles the mouse Wheel lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            //Refresh();
        }
        //protected override void OnCreateControl()
        //{
        //    base.OnCreateControl();
        //    //HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
        //    //VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        //    textbox.SpellCheck.IsEnabled = true;
        //    textbox.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
        //    textbox.TextWrapping = System.Windows.TextWrapping.Wrap;
        //    textbox.AcceptsReturn = true;
        //    textbox.DataContext = this;
        //    var host = new System.Windows.Forms.Integration.ElementHost();
        //    host.Child = textbox;
        //    host.Dock = DockStyle.Fill;
        //    Controls.Add(host);
        //}
        //System.Windows.Controls.TextBox textbox;
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Runs the fire Property Changed operation and updates the related application state.
        /// </summary>
        protected void FirePropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        //public int SelectionStart
        //{
        //    get { return textbox.SelectionStart; }
        //    set { textbox.SelectionStart = value; }
        //}
        //public int SelectionLength
        //{
        //    get { return textbox.SelectionLength; }
        //    set { textbox.SelectionLength = value; }
        //}
        //public override string Text
        //{
        //    get { return textbox.Text; }
        //    set
        //    {
        //        //text1 = value;
        //        textbox.Text = value;
        //        //FirePropertyChanged("Text1");
        //        //textbox.Dispatcher.BeginInvoke(
        //        // new Action(() => textbox.Text = value));
        //    } //textbox.UpdateLayout(); }
        //}
        //public bool ReadOnly
        //{
        //    get { return textbox.IsReadOnly; }
        //    set { textbox.IsReadOnly = value; } //textbox.UpdateLayout(); }
        //}
        //overrride GetCont
        //System.Windows.Forms.Integration.ElementHost
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        var cp = base.CreateParams;
        //        cp.ExStyle &= (~Win32.WS_EX_CLIENTEDGE);
        //        {
        //                cp.ExStyle |= Win32.WS_EX_CLIENTEDGE;
        //                break;
        //                break;
        //        }
        //        return cp;
        //    }
        //}
        //protected override void WndProc(ref Message m)
        //{
            
        //    if ( m.Msg == Win32.WM_PAINT
        //    )
        //    {
        //        Win32.SetBkMode(Handle, 1);
        //        Win32.SetBkColor(Handle, Color.White.ToArgb());
        //        Win32.SetTextColor(Handle, ControlColors.AccentColor.ToArgb());
        //        base.WndProc(ref m);
        //        Win32.SetBkMode(Handle, 2);
        //        //HDC hEdit = (HDC)wParam;
        //        //SetTextColor(hEdit, RGB(0, 0, 0));
        //        //SetBkColor(hEdit, RGB(255, 255, 255));
        //        // Do not return a brush created by CreateSolidBrush(...) because you'll get a memory leak
        //        //return (INT_PTR)GetStockObject(WHITE_BRUSH);
        //    }
        //    else
        //        base.WndProc(ref m);
        //}
    }
}
