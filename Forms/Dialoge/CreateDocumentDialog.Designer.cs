namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    partial class CreateDocumentDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateDocumentDialog));
            this.cancelButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.okButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.richTextBox = new Pflegehaushaltsbuch.FormControls.RichTextBox();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.fileBox = new Pflegehaushaltsbuch.FormControls.RichTextBox();
            this.label3 = new Pflegehaushaltsbuch.FormControls.Label();
            this.dateBox = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            this.label4 = new Pflegehaushaltsbuch.FormControls.Label();
            this.clientBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cancelButton.BorderColor = System.Drawing.Color.DimGray;
            this.cancelButton.Checked = false;
            this.cancelButton.CheckedState = false;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.PaintBackGround = true;
            this.cancelButton.Radius = -1F;
            this.cancelButton.RoundEdges = false;
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.okButton.BorderColor = System.Drawing.Color.DimGray;
            this.okButton.Checked = false;
            this.okButton.CheckedState = false;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.ForeColor = System.Drawing.Color.White;
            this.okButton.Name = "okButton";
            this.okButton.PaintBackGround = true;
            this.okButton.Radius = -1F;
            this.okButton.RoundEdges = false;
            this.okButton.UseVisualStyleBackColor = false;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // richTextBox
            // 
            resources.ApplyResources(this.richTextBox, "richTextBox");
            this.richTextBox.Name = "richTextBox";
            // 
            // label1
            // 
            this.label1.AttachRegion = false;
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.DrawLine = false;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Gradiant = false;
            this.label1.LinePadding = 0;
            this.label1.Name = "label1";
            this.label1.Radius = 0F;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.Border = false;
            this.tableLayoutPanel1.BorderColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.BorderWidth = 1F;
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.fileBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dateBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.richTextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.clientBox, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.okButton);
            this.flowLayoutPanel1.Controls.Add(this.cancelButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // label2
            // 
            this.label2.AttachRegion = false;
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.DrawLine = false;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Gradiant = false;
            this.label2.LinePadding = 0;
            this.label2.Name = "label2";
            this.label2.Radius = 0F;
            // 
            // fileBox
            // 
            resources.ApplyResources(this.fileBox, "fileBox");
            this.fileBox.Name = "fileBox";
            // 
            // label3
            // 
            this.label3.AttachRegion = false;
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.DrawLine = false;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Gradiant = false;
            this.label3.LinePadding = 0;
            this.label3.Name = "label3";
            this.label3.Radius = 0F;
            // 
            // dateBox
            // 
            resources.ApplyResources(this.dateBox, "dateBox");
            this.dateBox.Days = true;
            this.dateBox.Name = "dateBox";
            // 
            // label4
            // 
            this.label4.AttachRegion = false;
            resources.ApplyResources(this.label4, "label4");
            this.label4.DrawLine = false;
            this.label4.Gradiant = false;
            this.label4.LinePadding = 0;
            this.label4.Name = "label4";
            this.label4.Radius = 0F;
            // 
            // clientBox
            // 
            resources.ApplyResources(this.clientBox, "clientBox");
            this.clientBox.FormattingEnabled = true;
            this.clientBox.Name = "clientBox";
            // 
            // CreateDocumentDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CreateDocumentDialog";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormControls.Button cancelButton;
        private FormControls.Button okButton;
        private Pflegehaushaltsbuch.FormControls.RichTextBox richTextBox;
        private Pflegehaushaltsbuch.FormControls.Label label1;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.Label label2;
        private FormControls.RichTextBox fileBox;
        private FormControls.Label label3;
        private FormControls.DateTimeBox dateBox;
        private FormControls.Label label4;
        private FormControls.ComboBox clientBox;
    }
}
