namespace Pflegehaushaltsbuch.Forms
{
    partial class BankForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BankForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label19 = new Pflegehaushaltsbuch.FormControls.Label();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.updateButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.bookButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.printButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.backButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tableLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.view = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.dateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.noteColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookToColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookCategoryColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handsignColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel4 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.toDateBox = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
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
            this.tableLayoutPanel1.Controls.Add(this.label19, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Gradiant = true;
            this.label19.Name = "label19";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.updateButton);
            this.flowLayoutPanel1.Controls.Add(this.bookButton);
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
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
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
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel2.Border = true;
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.view, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 0);
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
            this.dateColumn,
            this.noteColumn,
            this.bookToColumn,
            this.bookCategoryColumn,
            this.amountColumn,
            this.handsignColumn});
            resources.ApplyResources(this.view, "view");
            this.view.Name = "view";
            this.view.ReadOnly = true;
            this.view.RowHeadersVisible = false;
            this.view.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(177)))), ((int)(((byte)(210)))));
            this.view.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.view.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.view.StandardTab = true;
            this.view.VirtualMode = true;
            // 
            // dateColumn
            // 
            this.dateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dateColumn.DataPropertyName = "date";
            resources.ApplyResources(this.dateColumn, "dateColumn");
            this.dateColumn.Name = "dateColumn";
            this.dateColumn.ReadOnly = true;
            // 
            // noteColumn
            // 
            this.noteColumn.DataPropertyName = "note";
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.noteColumn.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.noteColumn, "noteColumn");
            this.noteColumn.Name = "noteColumn";
            this.noteColumn.ReadOnly = true;
            // 
            // bookToColumn
            // 
            this.bookToColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.bookToColumn.DataPropertyName = "account";
            resources.ApplyResources(this.bookToColumn, "bookToColumn");
            this.bookToColumn.Name = "bookToColumn";
            this.bookToColumn.ReadOnly = true;
            // 
            // bookCategoryColumn
            // 
            this.bookCategoryColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.bookCategoryColumn.DataPropertyName = "book_cat";
            resources.ApplyResources(this.bookCategoryColumn, "bookCategoryColumn");
            this.bookCategoryColumn.Name = "bookCategoryColumn";
            this.bookCategoryColumn.ReadOnly = true;
            // 
            // amountColumn
            // 
            this.amountColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.amountColumn.DataPropertyName = "amount";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "C";
            this.amountColumn.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.amountColumn, "amountColumn");
            this.amountColumn.Name = "amountColumn";
            this.amountColumn.ReadOnly = true;
            // 
            // handsignColumn
            // 
            this.handsignColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.handsignColumn.DataPropertyName = "handsign";
            resources.ApplyResources(this.handsignColumn, "handsignColumn");
            this.handsignColumn.Name = "handsignColumn";
            this.handsignColumn.ReadOnly = true;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.toDateBox, 7, 0);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
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
            // BankForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BankForm";
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
            this.PerformLayout();

        }

        #endregion

        private Pflegehaushaltsbuch.FormControls.DataGridView view;
        private Pflegehaushaltsbuch.FormControls.FlowLayoutPanel flowLayoutPanel1;
        private Pflegehaushaltsbuch.FormControls.Button bookButton;
        private Pflegehaushaltsbuch.FormControls.Button backButton;
        private Pflegehaushaltsbuch.FormControls.TableLayoutPanel tableLayoutPanel1;
        private Pflegehaushaltsbuch.FormControls.Button updateButton;
        private Pflegehaushaltsbuch.FormControls.Button printButton;
        private Pflegehaushaltsbuch.FormControls.Label label19;
        private Pflegehaushaltsbuch.FormControls.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn noteColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bookToColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bookCategoryColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn handsignColumn;
        private FormControls.TableLayoutPanel tableLayoutPanel4;
        private FormControls.DateTimeBox toDateBox;
        private FormControls.Label label1;
        private FormControls.TextBox totalAmountBox;
        private FormControls.DateTimeBox fromDateBox;
        private FormControls.Label fromToLabel;
        private FormControls.CheckBox periodCheckBox;
        private FormControls.Button exportButton;
    }
}