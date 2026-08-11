namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    partial class EditTextDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditTextDialog));
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.okButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.abortButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.flowLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.label13 = new Pflegehaushaltsbuch.FormControls.Label();
            this.unicodeTextbox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.unicodeButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.richTextBox = new Pflegehaushaltsbuch.FormControls.RichTextBox();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.listBox = new Pflegehaushaltsbuch.FormControls.ListBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.okButton);
            this.flowLayoutPanel1.Controls.Add(this.abortButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.okButton.BorderColor = System.Drawing.Color.DimGray;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.okButton.Name = "okButton";
            this.okButton.Radius = -1F;
            this.okButton.UseVisualStyleBackColor = false;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // abortButton
            // 
            resources.ApplyResources(this.abortButton, "abortButton");
            this.abortButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.abortButton.BorderColor = System.Drawing.Color.DimGray;
            this.abortButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.abortButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.abortButton.Name = "abortButton";
            this.abortButton.Radius = -1F;
            this.abortButton.UseVisualStyleBackColor = false;
            // 
            // flowLayoutPanel2
            // 
            resources.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel2, 2);
            this.flowLayoutPanel2.Controls.Add(this.label13);
            this.flowLayoutPanel2.Controls.Add(this.unicodeTextbox);
            this.flowLayoutPanel2.Controls.Add(this.unicodeButton);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Name = "label13";
            // 
            // unicodeTextbox
            // 
            this.unicodeTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.unicodeTextbox, "unicodeTextbox");
            this.unicodeTextbox.Name = "unicodeTextbox";
            // 
            // unicodeButton
            // 
            resources.ApplyResources(this.unicodeButton, "unicodeButton");
            this.unicodeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.unicodeButton.BorderColor = System.Drawing.Color.DimGray;
            this.unicodeButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.unicodeButton.Name = "unicodeButton";
            this.unicodeButton.Radius = -1F;
            this.unicodeButton.UseVisualStyleBackColor = false;
            this.unicodeButton.Click += new System.EventHandler(this.unicodeButton_Click);
            // 
            // richTextBox
            // 
            this.richTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.richTextBox, "richTextBox");
            this.richTextBox.Name = "richTextBox";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.richTextBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.listBox, 1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // listBox
            // 
            resources.ApplyResources(this.listBox, "listBox");
            this.listBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listBox.FormattingEnabled = true;
            this.listBox.Items.AddRange(new object[] {
            resources.GetString("listBox.Items"),
            resources.GetString("listBox.Items1"),
            resources.GetString("listBox.Items2"),
            resources.GetString("listBox.Items3"),
            resources.GetString("listBox.Items4"),
            resources.GetString("listBox.Items5"),
            resources.GetString("listBox.Items6"),
            resources.GetString("listBox.Items7"),
            resources.GetString("listBox.Items8"),
            resources.GetString("listBox.Items9"),
            resources.GetString("listBox.Items10"),
            resources.GetString("listBox.Items11"),
            resources.GetString("listBox.Items12"),
            resources.GetString("listBox.Items13"),
            resources.GetString("listBox.Items14"),
            resources.GetString("listBox.Items15"),
            resources.GetString("listBox.Items16"),
            resources.GetString("listBox.Items17"),
            resources.GetString("listBox.Items18"),
            resources.GetString("listBox.Items19"),
            resources.GetString("listBox.Items20"),
            resources.GetString("listBox.Items21"),
            resources.GetString("listBox.Items22"),
            resources.GetString("listBox.Items23"),
            resources.GetString("listBox.Items24"),
            resources.GetString("listBox.Items25"),
            resources.GetString("listBox.Items26"),
            resources.GetString("listBox.Items27"),
            resources.GetString("listBox.Items28"),
            resources.GetString("listBox.Items29"),
            resources.GetString("listBox.Items30")});
            this.listBox.Name = "listBox";
            this.listBox.Sorted = true;
            this.listBox.DoubleClick += new System.EventHandler(this.listBox_DoubleClick);
            // 
            // EditTextDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "EditTextDialog";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.Button okButton;
        private FormControls.Button abortButton;
        private FormControls.FlowLayoutPanel flowLayoutPanel2;
        private FormControls.RichTextBox richTextBox;
        private FormControls.Label label13;
        private FormControls.TextBox unicodeTextbox;
        private FormControls.Button unicodeButton;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.ListBox listBox;
    }
}
