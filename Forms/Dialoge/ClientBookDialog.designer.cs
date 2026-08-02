namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    partial class ClientBookDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientBookDialog));
            this.okButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.cancelButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.typeLabel = new Pflegehaushaltsbuch.FormControls.Label();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label3 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label4 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.bookingToBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.clientBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.clientLabel = new Pflegehaushaltsbuch.FormControls.Label();
            this.label6 = new Pflegehaushaltsbuch.FormControls.Label();
            this.bookingCategoryBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.clientIdBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.quittanceButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.amountBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label5 = new Pflegehaushaltsbuch.FormControls.Label();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.payInDate = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            this.bookTextHost = new System.Windows.Forms.Integration.ElementHost();
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
            // typeLabel
            // 
            resources.ApplyResources(this.typeLabel, "typeLabel");
            this.typeLabel.ForeColor = System.Drawing.Color.White;
            this.typeLabel.Name = "typeLabel";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Name = "label4";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // bookingToBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.bookingToBox, 2);
            resources.ApplyResources(this.bookingToBox, "bookingToBox");
            this.bookingToBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.bookingToBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bookingToBox.FormattingEnabled = true;
            this.bookingToBox.Name = "bookingToBox";
            // 
            // clientBox
            // 
            resources.ApplyResources(this.clientBox, "clientBox");
            this.clientBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.clientBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clientBox.FormattingEnabled = true;
            this.clientBox.Name = "clientBox";
            this.clientBox.TabStop = false;
            // 
            // clientLabel
            // 
            resources.ApplyResources(this.clientLabel, "clientLabel");
            this.clientLabel.BackColor = System.Drawing.Color.Transparent;
            this.clientLabel.ForeColor = System.Drawing.Color.White;
            this.clientLabel.Name = "clientLabel";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Name = "label6";
            // 
            // bookingCategoryBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.bookingCategoryBox, 2);
            resources.ApplyResources(this.bookingCategoryBox, "bookingCategoryBox");
            this.bookingCategoryBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.bookingCategoryBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bookingCategoryBox.FormattingEnabled = true;
            this.bookingCategoryBox.Name = "bookingCategoryBox";
            // 
            // clientIdBox
            // 
            resources.ApplyResources(this.clientIdBox, "clientIdBox");
            this.clientIdBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.clientIdBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clientIdBox.FormattingEnabled = true;
            this.clientIdBox.Name = "clientIdBox";
            this.clientIdBox.TabStop = false;
            // 
            // quittanceButton
            // 
            resources.ApplyResources(this.quittanceButton, "quittanceButton");
            this.quittanceButton.BackColor = System.Drawing.Color.Transparent;
            this.quittanceButton.BorderColor = System.Drawing.Color.White;
            this.quittanceButton.CheckedState = true;
            this.quittanceButton.Name = "quittanceButton";
            this.quittanceButton.Radius = -1F;
            this.quittanceButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel1.Controls.Add(this.amountBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.bookingToBox, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.bookingCategoryBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.clientLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.clientIdBox, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.clientBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.payInDate, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.bookTextHost, 1, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // amountBox
            // 
            this.amountBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.amountBox, 2);
            resources.ApplyResources(this.amountBox, "amountBox");
            this.amountBox.Name = "amountBox";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.tableLayoutPanel1.SetColumnSpan(this.label5, 3);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Gradiant = true;
            this.label5.Name = "label5";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 3);
            this.flowLayoutPanel1.Controls.Add(this.okButton);
            this.flowLayoutPanel1.Controls.Add(this.cancelButton);
            this.flowLayoutPanel1.Controls.Add(this.quittanceButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // payInDate
            // 
            resources.ApplyResources(this.payInDate, "payInDate");
            this.tableLayoutPanel1.SetColumnSpan(this.payInDate, 2);
            this.payInDate.Days = true;
            this.payInDate.Name = "payInDate";
            // 
            // bookTextHost
            // 
            this.bookTextHost.BackColor = System.Drawing.Color.Red;
            this.bookTextHost.BackColorTransparent = true;
            this.tableLayoutPanel1.SetColumnSpan(this.bookTextHost, 2);
            resources.ApplyResources(this.bookTextHost, "bookTextHost");
            this.bookTextHost.Name = "bookTextHost";
            this.bookTextHost.Child = null;
            // 
            // ClientBookDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.typeLabel);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClientBookDialog";
            this.Load += new System.EventHandler(this.BookingForm_Load);
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
        private FormControls.Label typeLabel;
        private FormControls.Label label2;
        private FormControls.Label label3;
        private FormControls.Label label4;
        private FormControls.Label label1;
        private Pflegehaushaltsbuch.FormControls.ComboBox bookingToBox;
        private Pflegehaushaltsbuch.FormControls.ComboBox clientBox;
        private FormControls.Label clientLabel;
        private FormControls.Label label6;
        private Pflegehaushaltsbuch.FormControls.ComboBox bookingCategoryBox;
        private Pflegehaushaltsbuch.FormControls.ComboBox clientIdBox;
        private FormControls.Button quittanceButton;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.Label label5;
        private FormControls.TextBox amountBox;
        private FormControls.DateTimeBox payInDate;
        private System.Windows.Forms.Integration.ElementHost bookTextHost;
    }
}
