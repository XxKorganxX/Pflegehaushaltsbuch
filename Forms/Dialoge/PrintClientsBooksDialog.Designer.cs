namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    partial class PrintClientsBooksDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintClientsBooksDialog));
            this.okButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.cancelButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.label7 = new Pflegehaushaltsbuch.FormControls.Label();
            this.clientView = new Pflegehaushaltsbuch.FormControls.ListBox();
            this.dateTimeBox = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.accountText = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.printerBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.okButton.BorderColor = System.Drawing.Color.DimGray;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Name = "okButton";
            this.okButton.Radius = -1F;
            this.okButton.UseVisualStyleBackColor = false;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cancelButton.BorderColor = System.Drawing.Color.DimGray;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Radius = -1F;
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Name = "label7";
            // 
            // clientView
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.clientView, 2);
            resources.ApplyResources(this.clientView, "clientView");
            this.clientView.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.clientView.Name = "clientView";
            this.clientView.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            // 
            // dateTimeBox
            // 
            resources.ApplyResources(this.dateTimeBox, "dateTimeBox");
            this.dateTimeBox.BackColor = System.Drawing.Color.Transparent;
            this.dateTimeBox.Days = false;
            this.dateTimeBox.Name = "dateTimeBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dateTimeBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.clientView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.accountText, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.printerBox, 0, 5);
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
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // accountText
            // 
            this.accountText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.accountText, 2);
            resources.ApplyResources(this.accountText, "accountText");
            this.accountText.Name = "accountText";
            // 
            // printerBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.printerBox, 2);
            resources.ApplyResources(this.printerBox, "printerBox");
            this.printerBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.printerBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.printerBox.FormattingEnabled = true;
            this.printerBox.Name = "printerBox";
            // 
            // PrintClientsBooksDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintClientsBooksDialog";
            this.Shown += new System.EventHandler(this.PDFBooksForm_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormControls.Button okButton;
        private FormControls.Button cancelButton;
        private FormControls.Label label7;
        private FormControls.ListBox clientView;
        private FormControls.DateTimeBox dateTimeBox;
        private FormControls.Label label1;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private Pflegehaushaltsbuch.FormControls.ComboBox printerBox;
        private FormControls.Label label2;
        private FormControls.TextBox accountText;
    }
}
