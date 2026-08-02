namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    partial class CashBookDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CashBookDialog));
            this.okButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.cancelButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.typeLabel = new Pflegehaushaltsbuch.FormControls.Label();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label3 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label4 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.bookingToBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.label6 = new Pflegehaushaltsbuch.FormControls.Label();
            this.bookingCategoryBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.clientLookUpBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label5 = new Pflegehaushaltsbuch.FormControls.Label();
            this.amountBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.quittanceButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.clientList = new Pflegehaushaltsbuch.FormControls.CheckedListBox();
            this.payInDate = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            this.bookTextHost = new System.Windows.Forms.Integration.ElementHost();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
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
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Name = "label4";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // bookingToBox
            // 
            resources.ApplyResources(this.bookingToBox, "bookingToBox");
            this.bookingToBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.bookingToBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bookingToBox.FormattingEnabled = true;
            this.bookingToBox.Name = "bookingToBox";
            this.toolTip.SetToolTip(this.bookingToBox, resources.GetString("bookingToBox.ToolTip"));
            this.bookingToBox.SelectedIndexChanged += new System.EventHandler(this.bookingBox_SelectedIndexChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Name = "label6";
            // 
            // bookingCategoryBox
            // 
            resources.ApplyResources(this.bookingCategoryBox, "bookingCategoryBox");
            this.bookingCategoryBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.bookingCategoryBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bookingCategoryBox.FormattingEnabled = true;
            this.bookingCategoryBox.Name = "bookingCategoryBox";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Border = true;
            this.tableLayoutPanel1.Controls.Add(this.clientLookUpBox, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.amountBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.bookingToBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.bookingCategoryBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.clientList, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.payInDate, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.bookTextHost, 1, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // clientLookUpBox
            // 
            this.clientLookUpBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.clientLookUpBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.clientLookUpBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.clientLookUpBox, "clientLookUpBox");
            this.clientLookUpBox.Name = "clientLookUpBox";
            this.clientLookUpBox.TabStop = false;
            this.clientLookUpBox.TextChanged += new System.EventHandler(this.clientLookUpBox_TextChanged);
            this.clientLookUpBox.Validating += new System.ComponentModel.CancelEventHandler(this.clientLookUpBox_Validating);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.tableLayoutPanel1.SetColumnSpan(this.label5, 3);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Gradiant = true;
            this.label5.Name = "label5";
            // 
            // amountBox
            // 
            this.amountBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.amountBox, "amountBox");
            this.amountBox.Name = "amountBox";
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
            // clientList
            // 
            this.clientList.CheckOnClick = true;
            resources.ApplyResources(this.clientList, "clientList");
            this.clientList.FormattingEnabled = true;
            this.clientList.Name = "clientList";
            this.tableLayoutPanel1.SetRowSpan(this.clientList, 4);
            this.clientList.Sorted = true;
            // 
            // payInDate
            // 
            resources.ApplyResources(this.payInDate, "payInDate");
            this.payInDate.Days = true;
            this.payInDate.Name = "payInDate";
            this.payInDate.ShowYear = true;
            // 
            // bookTextHost
            // 
            this.bookTextHost.BackColorTransparent = true;
            resources.ApplyResources(this.bookTextHost, "bookTextHost");
            this.bookTextHost.Name = "bookTextHost";
            this.bookTextHost.Child = null;
            // 
            // toolTip
            // 
            this.toolTip.BackColor = System.Drawing.Color.Empty;
            this.toolTip.ForeColor = System.Drawing.Color.Black;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "Hinweis:";
            // 
            // CashBookDialog
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
            this.Name = "CashBookDialog";
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
        private FormControls.Label label6;
        private Pflegehaushaltsbuch.FormControls.ComboBox bookingCategoryBox;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ToolTip toolTip;
        private FormControls.TextBox amountBox;
        private FormControls.Label label5;
        private FormControls.CheckedListBox clientList;
        private FormControls.TextBox clientLookUpBox;
        private FormControls.Button quittanceButton;
        private FormControls.DateTimeBox payInDate;
        private System.Windows.Forms.Integration.ElementHost bookTextHost;
    }
}
