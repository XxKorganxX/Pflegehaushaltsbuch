namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    partial class CashOfficeBookDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CashOfficeBookDialog));
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.amountBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label4 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label5 = new Pflegehaushaltsbuch.FormControls.Label();
            this.bookingKindBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.label6 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label3 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.okButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.cancelButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.dateBox = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            this.bookTextHost = new System.Windows.Forms.Integration.ElementHost();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel1.Controls.Add(this.amountBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.bookingKindBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.dateBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.bookTextHost, 1, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // amountBox
            // 
            this.amountBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.amountBox, "amountBox");
            this.amountBox.Name = "amountBox";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.tableLayoutPanel1.SetColumnSpan(this.label5, 2);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Gradiant = true;
            this.label5.Name = "label5";
            // 
            // bookingKindBox
            // 
            resources.ApplyResources(this.bookingKindBox, "bookingKindBox");
            this.bookingKindBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.bookingKindBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bookingKindBox.FormattingEnabled = true;
            this.bookingKindBox.Items.AddRange(new object[] {
            resources.GetString("bookingKindBox.Items"),
            resources.GetString("bookingKindBox.Items1")});
            this.bookingKindBox.Name = "bookingKindBox";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Name = "label6";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.okButton);
            this.flowLayoutPanel1.Controls.Add(this.cancelButton);
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
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cancelButton.BorderColor = System.Drawing.Color.DimGray;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Radius = -1F;
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // dateBox
            // 
            resources.ApplyResources(this.dateBox, "dateBox");
            this.dateBox.Days = true;
            this.dateBox.Name = "dateBox";
            // 
            // bookTextHost
            // 
            resources.ApplyResources(this.bookTextHost, "bookTextHost");
            this.bookTextHost.Name = "bookTextHost";
            this.bookTextHost.Child = null;
            // 
            // CashOfficeBookDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CashOfficeBookDialog";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.TextBox amountBox;
        private FormControls.Label label4;
        private FormControls.Label label5;
        private Pflegehaushaltsbuch.FormControls.ComboBox bookingKindBox;
        private FormControls.Label label6;
        private FormControls.Label label3;
        private FormControls.Label label2;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.Button okButton;
        private FormControls.Button cancelButton;
        private FormControls.DateTimeBox dateBox;
        private System.Windows.Forms.Integration.ElementHost bookTextHost;
    }
}
