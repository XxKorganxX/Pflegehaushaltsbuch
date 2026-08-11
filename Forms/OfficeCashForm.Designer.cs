namespace Pflegehaushaltsbuch.Forms
{
    partial class OfficeCashForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OfficeCashForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.updateButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.bookButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.stornoButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.printButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.backButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.label7 = new Pflegehaushaltsbuch.FormControls.Label();
            this.tableLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.view = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookCat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handSignColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel4 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.toDateBox = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            this.label3 = new Pflegehaushaltsbuch.FormControls.Label();
            this.totalAmountBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.fromDateBox = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            this.fromToLabel = new Pflegehaushaltsbuch.FormControls.Label();
            this.periodCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.exportButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.view)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.updateButton);
            this.flowLayoutPanel1.Controls.Add(this.bookButton);
            this.flowLayoutPanel1.Controls.Add(this.stornoButton);
            this.flowLayoutPanel1.Controls.Add(this.printButton);
            this.flowLayoutPanel1.Controls.Add(this.exportButton);
            this.flowLayoutPanel1.Controls.Add(this.backButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // updateButton
            // 
            resources.ApplyResources(this.updateButton, "updateButton");
            this.updateButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.updateButton.BorderColor = System.Drawing.Color.DimGray;
            this.updateButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.updateButton.Name = "updateButton";
            this.updateButton.Radius = -1F;
            this.updateButton.UseVisualStyleBackColor = false;
            // 
            // bookButton
            // 
            resources.ApplyResources(this.bookButton, "bookButton");
            this.bookButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bookButton.BorderColor = System.Drawing.Color.DimGray;
            this.bookButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bookButton.Name = "bookButton";
            this.bookButton.Radius = -1F;
            this.bookButton.UseVisualStyleBackColor = false;
            this.bookButton.Click += new System.EventHandler(this.bookButton_Click);
            // 
            // stornoButton
            // 
            resources.ApplyResources(this.stornoButton, "stornoButton");
            this.stornoButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.stornoButton.BorderColor = System.Drawing.Color.DimGray;
            this.stornoButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.stornoButton.Name = "stornoButton";
            this.stornoButton.Radius = -1F;
            this.stornoButton.UseVisualStyleBackColor = false;
            // 
            // printButton
            // 
            resources.ApplyResources(this.printButton, "printButton");
            this.printButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.printButton.BorderColor = System.Drawing.Color.DimGray;
            this.printButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.printButton.Name = "printButton";
            this.printButton.Radius = -1F;
            this.printButton.UseVisualStyleBackColor = false;
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // backButton
            // 
            resources.ApplyResources(this.backButton, "backButton");
            this.backButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.backButton.BorderColor = System.Drawing.Color.DimGray;
            this.backButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.backButton.Name = "backButton";
            this.backButton.Radius = -1F;
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Gradiant = true;
            this.label7.Name = "label7";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel2.Border = true;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 3);
            this.tableLayoutPanel2.Controls.Add(this.view, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel2.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // view
            // 
            this.view.AllowUserToAddRows = false;
            this.view.AllowUserToDeleteRows = false;
            this.view.AllowUserToResizeColumns = false;
            this.view.AllowUserToResizeRows = false;
            this.view.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.view.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.view.BackgroundColor = System.Drawing.Color.White;
            this.view.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.view.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idColumn,
            this.dateColumn,
            this.descriptionColumn,
            this.bookCat,
            this.amountColumn,
            this.handSignColumn});
            resources.ApplyResources(this.view, "view");
            this.view.Name = "view";
            this.view.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.view.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.view.RowHeadersVisible = false;
            this.view.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.view.StandardTab = true;
            // 
            // idColumn
            // 
            this.idColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.idColumn.DataPropertyName = "document_id";
            dataGridViewCellStyle1.Format = "000";
            dataGridViewCellStyle1.NullValue = null;
            this.idColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.idColumn.FillWeight = 113.9086F;
            resources.ApplyResources(this.idColumn, "idColumn");
            this.idColumn.Name = "idColumn";
            this.idColumn.ReadOnly = true;
            // 
            // dateColumn
            // 
            this.dateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dateColumn.DataPropertyName = "date";
            this.dateColumn.FillWeight = 113.9086F;
            resources.ApplyResources(this.dateColumn, "dateColumn");
            this.dateColumn.Name = "dateColumn";
            this.dateColumn.ReadOnly = true;
            // 
            // descriptionColumn
            // 
            this.descriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descriptionColumn.DataPropertyName = "note";
            this.descriptionColumn.FillWeight = 113.9086F;
            resources.ApplyResources(this.descriptionColumn, "descriptionColumn");
            this.descriptionColumn.Name = "descriptionColumn";
            this.descriptionColumn.ReadOnly = true;
            // 
            // bookCat
            // 
            this.bookCat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.bookCat.DataPropertyName = "book_cat";
            resources.ApplyResources(this.bookCat, "bookCat");
            this.bookCat.Name = "bookCat";
            this.bookCat.ReadOnly = true;
            // 
            // amountColumn
            // 
            this.amountColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.amountColumn.DataPropertyName = "amount";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "C";
            dataGridViewCellStyle2.NullValue = null;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.amountColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.amountColumn.FillWeight = 113.9086F;
            resources.ApplyResources(this.amountColumn, "amountColumn");
            this.amountColumn.Name = "amountColumn";
            this.amountColumn.ReadOnly = true;
            // 
            // handSignColumn
            // 
            this.handSignColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.handSignColumn.DataPropertyName = "handsign";
            resources.ApplyResources(this.handSignColumn, "handSignColumn");
            this.handSignColumn.Name = "handSignColumn";
            this.handSignColumn.ReadOnly = true;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.toDateBox, 7, 0);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.totalAmountBox, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.fromDateBox, 5, 0);
            this.tableLayoutPanel4.Controls.Add(this.fromToLabel, 6, 0);
            this.tableLayoutPanel4.Controls.Add(this.periodCheckBox, 4, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // toDateBox
            // 
            resources.ApplyResources(this.toDateBox, "toDateBox");
            this.toDateBox.Days = false;
            this.toDateBox.Name = "toDateBox";
            this.toDateBox.ShowYear = true;
            this.toDateBox.ValueChanged += new Pflegehaushaltsbuch.FormControls.DateTimeBox.UpdateDistanceDelegate(this.date_ValueChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // totalAmountBox
            // 
            this.totalAmountBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.totalAmountBox, "totalAmountBox");
            this.totalAmountBox.Name = "totalAmountBox";
            this.totalAmountBox.ReadOnly = true;
            this.totalAmountBox.TabStop = false;
            // 
            // fromDateBox
            // 
            resources.ApplyResources(this.fromDateBox, "fromDateBox");
            this.fromDateBox.Days = false;
            this.fromDateBox.Name = "fromDateBox";
            this.fromDateBox.ShowYear = true;
            this.fromDateBox.ValueChanged += new Pflegehaushaltsbuch.FormControls.DateTimeBox.UpdateDistanceDelegate(this.date_ValueChanged);
            // 
            // fromToLabel
            // 
            resources.ApplyResources(this.fromToLabel, "fromToLabel");
            this.fromToLabel.Name = "fromToLabel";
            // 
            // periodCheckBox
            // 
            resources.ApplyResources(this.periodCheckBox, "periodCheckBox");
            this.periodCheckBox.BorderColor = System.Drawing.Color.DimGray;
            this.periodCheckBox.CheckedImage = null;
            this.periodCheckBox.Name = "periodCheckBox";
            this.periodCheckBox.UnCheckedImage = null;
            this.periodCheckBox.UseVisualStyleBackColor = true;
            this.periodCheckBox.Click += new System.EventHandler(this.periodCheckBox_Click);
            // 
            // exportButton
            // 
            resources.ApplyResources(this.exportButton, "exportButton");
            this.exportButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.exportButton.BorderColor = System.Drawing.Color.DimGray;
            this.exportButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.exportButton.Name = "exportButton";
            this.exportButton.Radius = -1F;
            this.exportButton.UseVisualStyleBackColor = false;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // OfficeCashForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "OfficeCashForm";
            this.Enter += new System.EventHandler(this.CashOfficeForm_Enter);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.view)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.Button updateButton;
        private FormControls.Button bookButton;
        private FormControls.Button stornoButton;
        private FormControls.Button printButton;
        private FormControls.Button backButton;
        private FormControls.Label label7;
        private FormControls.TableLayoutPanel tableLayoutPanel2;
        private FormControls.DataGridView view;
        private System.Windows.Forms.DataGridViewTextBoxColumn idColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bookCat;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn handSignColumn;
        private FormControls.TableLayoutPanel tableLayoutPanel4;
        private FormControls.DateTimeBox toDateBox;
        private FormControls.Label label3;
        private FormControls.TextBox totalAmountBox;
        private FormControls.DateTimeBox fromDateBox;
        private FormControls.Label fromToLabel;
        private FormControls.CheckBox periodCheckBox;
        private FormControls.Button exportButton;
    }
}
