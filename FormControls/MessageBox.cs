using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.FormControls
{
    /// <summary>
    /// Represents a custom message Box control used by the application user interface.
    /// </summary>
    public class MessageBox : Forms.Form
    {
        private FlowLayoutPanel flowLayoutPanel1;
        private Button okButton;
        private Button cancelButton;
        private Button yesButton;
        private Button noButton;
        private Label msgBox;
        private FlowLayoutPanel flowLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel1;
        /// <summary>
        /// Creates a new Message Box instance and initializes the required state.
        /// </summary>
        public MessageBox(string msg, string title)
        {
            InitializeComponent();
            Text = title;
            this.msgBox.Text = msg;
            okButton.Visible = false;
            cancelButton.Visible = false;
            yesButton.Visible = false;
            noButton.Visible = false;
            msgBox.Select();
            msgBox.Focus();
        }
        /// <summary>
        /// Creates a new Message Box instance and initializes the required state.
        /// </summary>
        public MessageBox(string msg, string title, MessageBoxButtons messageBoxButtons)
        {
            InitializeComponent();
            Text = title;
            this.msgBox.Text = msg;
            //this.msg.Text = "My Test\nMy Test1\nMy Test2\nMy Test3\nMy Test4\nMy Test5\nMy Test6\nMy Test7\nMy Test8\nMy Test9\nMy Test10\nMy Test11\nMy Test12\nMy Test13\nMy Test14\nMy Test15\n";
            //this.msg.Text = msg.Substring(0, Math.Min(msg.Length, this.msg.MaxLength));
            if (messageBoxButtons == MessageBoxButtons.OK)
                cancelButton.Visible = false;
            if (messageBoxButtons == MessageBoxButtons.YesNo ||
                messageBoxButtons == MessageBoxButtons.YesNoCancel)
            {
                okButton.Visible = false;
                cancelButton.Visible = false;
                yesButton.Visible = true;
                noButton.Visible = true;
            }
            
            msgBox.Select();
            msgBox.Focus();
        }

        /// <summary>
        /// Runs the show operation and updates the related application state.
        /// </summary>
        public static MessageBox Show(IWin32Window owner, string msg)
        {
            MessageBox msgBox = new MessageBox(msg, msg);
            msgBox.StartPosition = FormStartPosition.CenterScreen;
            msgBox.Show(owner);
            return msgBox;
        }
        /// <summary>
        /// Runs the show Dialog operation and updates the related application state.
        /// </summary>
        public static DialogResult ShowDialog(string msg)
        {
            MessageBox msgBox = new MessageBox(msg, string.Empty, MessageBoxButtons.OK);
            return msgBox.ShowDialog();
        }
        /// <summary>
        /// Runs the show Dialog operation and updates the related application state.
        /// </summary>
        public static DialogResult ShowDialog(IWin32Window owner, string msg)
        {
            MessageBox msgBox = new MessageBox(msg, "Info", MessageBoxButtons.OK);
            return msgBox.ShowDialog(owner);
        }
        /// <summary>
        /// Runs the show Dialog operation and updates the related application state.
        /// </summary>
        public static DialogResult ShowDialog(IWin32Window owner, string msg, string title)
        {
            MessageBox msgBox = new MessageBox(msg, title, MessageBoxButtons.OK);
            return msgBox.ShowDialog(owner);
        }
        /// <summary>
        /// Runs the show Dialog operation and updates the related application state.
        /// </summary>
        public static DialogResult ShowDialog(IWin32Window owner, string msg, string title, MessageBoxButtons buttons)
        {
            MessageBox msgBox = new MessageBox(msg, title, buttons);
            return msgBox.ShowDialog(owner);
        }
        /// <summary>
        /// Runs the show Dialog operation and updates the related application state.
        /// </summary>
        public static DialogResult ShowDialog(IWin32Window owner, string msg, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            MessageBox msgBox = new MessageBox(msg, title, buttons);
            return msgBox.ShowDialog(owner);
        }
        /// <summary>
        /// Runs the initialize Component operation and updates the related application state.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageBox));
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.okButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.cancelButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.yesButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.noButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.flowLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.msgBox = new Pflegehaushaltsbuch.FormControls.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.Border = true;
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tableLayoutPanel1_MouseMove);
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.okButton);
            this.flowLayoutPanel1.Controls.Add(this.cancelButton);
            this.flowLayoutPanel1.Controls.Add(this.yesButton);
            this.flowLayoutPanel1.Controls.Add(this.noButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tableLayoutPanel1_MouseMove);
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.BackColor = System.Drawing.Color.Transparent;
            this.okButton.BorderColor = System.Drawing.Color.Black;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.okButton.Name = "okButton";
            this.okButton.Radius = -1F;
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.BackColor = System.Drawing.Color.Transparent;
            this.cancelButton.BorderColor = System.Drawing.Color.Black;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Radius = -1F;
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // yesButton
            // 
            resources.ApplyResources(this.yesButton, "yesButton");
            this.yesButton.BackColor = System.Drawing.Color.Transparent;
            this.yesButton.BorderColor = System.Drawing.Color.Black;
            this.yesButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.yesButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.yesButton.Name = "yesButton";
            this.yesButton.Radius = -1F;
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // noButton
            // 
            resources.ApplyResources(this.noButton, "noButton");
            this.noButton.BackColor = System.Drawing.Color.Transparent;
            this.noButton.BorderColor = System.Drawing.Color.Black;
            this.noButton.DialogResult = System.Windows.Forms.DialogResult.No;
            this.noButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.noButton.Name = "noButton";
            this.noButton.Radius = -1F;
            this.noButton.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            resources.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
            this.flowLayoutPanel2.Controls.Add(this.msgBox);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            // 
            // msgBox
            // 
            resources.ApplyResources(this.msgBox, "msgBox");
            this.msgBox.ForeColor = System.Drawing.Color.White;
            this.msgBox.IsSelectable = true;
            this.msgBox.Name = "msgBox";
            this.msgBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tableLayoutPanel1_MouseMove);
            // 
            // MessageBox
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MessageBox";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        /// <summary>
        /// Handles the mouse Move event for table Layout Panel1 and updates the related state.
        /// </summary>
        private void tableLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            WindowMove(null, e);
        }
        /// <summary>
        /// Handles the click event for msg and updates the related state.
        /// </summary>
        private void msg_Click(object sender, EventArgs e)
        {
        }
    }
}
