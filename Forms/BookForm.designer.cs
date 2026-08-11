namespace Pflegehaushaltsbuch.Forms
{
    partial class BookForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BookForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.noteBinding = new System.Windows.Forms.BindingSource(this.components);
            this.accountBinding = new System.Windows.Forms.BindingSource(this.components);
            this.bookPanel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.bookView = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.numberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookTextColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookCategoryColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookToColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clientColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel3 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label5 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label6 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label7 = new Pflegehaushaltsbuch.FormControls.Label();
            this.accountStatusBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.lastBookBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.commentBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.tableLayoutPanel4 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.toDateBox = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.totalAmountBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.fromDateBox = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            this.fromToLabel = new Pflegehaushaltsbuch.FormControls.Label();
            this.periodCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.clientNameBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label19 = new Pflegehaushaltsbuch.FormControls.Label();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.updateButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.bookButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.stornoButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.printAccountButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.exportButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.backButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tableLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.noteBinding)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountBinding)).BeginInit();
            this.bookPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bookView)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bookPanel
            // 
            resources.ApplyResources(this.bookPanel, "bookPanel");
            this.bookPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.bookPanel.Border = true;
            this.bookPanel.Controls.Add(this.bookView, 0, 2);
            this.bookPanel.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.bookPanel.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.bookPanel.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.bookPanel.Name = "bookPanel";
            // 
            // bookView
            // 
            this.bookView.AllowUserToAddRows = false;
            this.bookView.AllowUserToDeleteRows = false;
            this.bookView.AllowUserToResizeColumns = false;
            this.bookView.AllowUserToResizeRows = false;
            this.bookView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.bookView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.bookView.BackgroundColor = System.Drawing.Color.White;
            this.bookView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.bookView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.numberColumn,
            this.dateColumn,
            this.bookTextColumn,
            this.bookCategoryColumn,
            this.bookToColumn,
            this.amountColumn,
            this.hsColumn,
            this.clientColumn});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Orange;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.bookView.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.bookView, "bookView");
            this.bookView.MultiSelect = false;
            this.bookView.Name = "bookView";
            this.bookView.ReadOnly = true;
            this.bookView.RowHeadersVisible = false;
            this.bookView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(177)))), ((int)(((byte)(210)))));
            this.bookView.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.bookView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.bookView.StandardTab = true;
            // 
            // numberColumn
            // 
            this.numberColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.numberColumn.DataPropertyName = "document_id";
            this.numberColumn.FillWeight = 15.22843F;
            resources.ApplyResources(this.numberColumn, "numberColumn");
            this.numberColumn.Name = "numberColumn";
            this.numberColumn.ReadOnly = true;
            this.numberColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dateColumn
            // 
            this.dateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dateColumn.DataPropertyName = "date";
            this.dateColumn.FillWeight = 116.9543F;
            resources.ApplyResources(this.dateColumn, "dateColumn");
            this.dateColumn.Name = "dateColumn";
            this.dateColumn.ReadOnly = true;
            // 
            // bookTextColumn
            // 
            this.bookTextColumn.DataPropertyName = "note";
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.bookTextColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.bookTextColumn.FillWeight = 116.9543F;
            resources.ApplyResources(this.bookTextColumn, "bookTextColumn");
            this.bookTextColumn.Name = "bookTextColumn";
            this.bookTextColumn.ReadOnly = true;
            // 
            // bookCategoryColumn
            // 
            this.bookCategoryColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.bookCategoryColumn.DataPropertyName = "book_cat";
            this.bookCategoryColumn.FillWeight = 116.9543F;
            resources.ApplyResources(this.bookCategoryColumn, "bookCategoryColumn");
            this.bookCategoryColumn.Name = "bookCategoryColumn";
            this.bookCategoryColumn.ReadOnly = true;
            this.bookCategoryColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // bookToColumn
            // 
            this.bookToColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.bookToColumn.DataPropertyName = "book_to";
            resources.ApplyResources(this.bookToColumn, "bookToColumn");
            this.bookToColumn.Name = "bookToColumn";
            this.bookToColumn.ReadOnly = true;
            // 
            // amountColumn
            // 
            this.amountColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.amountColumn.DataPropertyName = "amount";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "C";
            dataGridViewCellStyle2.NullValue = null;
            this.amountColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.amountColumn.FillWeight = 116.9543F;
            resources.ApplyResources(this.amountColumn, "amountColumn");
            this.amountColumn.Name = "amountColumn";
            this.amountColumn.ReadOnly = true;
            // 
            // hsColumn
            // 
            this.hsColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.hsColumn.DataPropertyName = "handsign";
            this.hsColumn.FillWeight = 116.9543F;
            resources.ApplyResources(this.hsColumn, "hsColumn");
            this.hsColumn.Name = "hsColumn";
            this.hsColumn.ReadOnly = true;
            // 
            // clientColumn
            // 
            this.clientColumn.DataPropertyName = "id";
            resources.ApplyResources(this.clientColumn, "clientColumn");
            this.clientColumn.Name = "clientColumn";
            this.clientColumn.ReadOnly = true;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.accountStatusBox, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.lastBookBox, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.commentBox, 1, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Name = "label7";
            // 
            // accountStatusBox
            // 
            this.accountStatusBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.accountStatusBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.accountStatusBox.FormattingEnabled = true;
            resources.ApplyResources(this.accountStatusBox, "accountStatusBox");
            this.accountStatusBox.Name = "accountStatusBox";
            this.accountStatusBox.Validated += new System.EventHandler(this.accountStatusBox_Validated);
            // 
            // lastBookBox
            // 
            this.lastBookBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.lastBookBox, "lastBookBox");
            this.lastBookBox.Name = "lastBookBox";
            this.lastBookBox.ReadOnly = true;
            // 
            // commentBox
            // 
            this.commentBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.commentBox, "commentBox");
            this.commentBox.Name = "commentBox";
            this.commentBox.ReadOnly = true;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.toDateBox, 7, 0);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
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
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
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
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.clientNameBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // clientNameBox
            // 
            this.clientNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.clientNameBox, "clientNameBox");
            this.clientNameBox.Name = "clientNameBox";
            this.clientNameBox.ReadOnly = true;
            this.clientNameBox.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
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
            this.flowLayoutPanel1.Controls.Add(this.updateButton);
            this.flowLayoutPanel1.Controls.Add(this.bookButton);
            this.flowLayoutPanel1.Controls.Add(this.stornoButton);
            this.flowLayoutPanel1.Controls.Add(this.printAccountButton);
            this.flowLayoutPanel1.Controls.Add(this.exportButton);
            this.flowLayoutPanel1.Controls.Add(this.backButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // updateButton
            // 
            resources.ApplyResources(this.updateButton, "updateButton");
            this.updateButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.updateButton.BorderColor = System.Drawing.Color.DimGray;
            this.updateButton.ForeColor = System.Drawing.Color.Black;
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
            this.bookButton.ForeColor = System.Drawing.Color.Black;
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
            this.stornoButton.ForeColor = System.Drawing.Color.Black;
            this.stornoButton.Name = "stornoButton";
            this.stornoButton.Radius = -1F;
            this.stornoButton.UseVisualStyleBackColor = false;
            this.stornoButton.Click += new System.EventHandler(this.stornoButton_Click);
            // 
            // printAccountButton
            // 
            resources.ApplyResources(this.printAccountButton, "printAccountButton");
            this.printAccountButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.printAccountButton.BorderColor = System.Drawing.Color.DimGray;
            this.printAccountButton.ForeColor = System.Drawing.Color.Black;
            this.printAccountButton.Name = "printAccountButton";
            this.printAccountButton.Radius = -1F;
            this.printAccountButton.UseVisualStyleBackColor = false;
            this.printAccountButton.Click += new System.EventHandler(this.printAccountButton_Click);
            // 
            // exportButton
            // 
            resources.ApplyResources(this.exportButton, "exportButton");
            this.exportButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.exportButton.BorderColor = System.Drawing.Color.DimGray;
            this.exportButton.ForeColor = System.Drawing.Color.Black;
            this.exportButton.Name = "exportButton";
            this.exportButton.Radius = -1F;
            this.exportButton.UseVisualStyleBackColor = false;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // backButton
            // 
            resources.ApplyResources(this.backButton, "backButton");
            this.backButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.backButton.BorderColor = System.Drawing.Color.DimGray;
            this.backButton.ForeColor = System.Drawing.Color.Black;
            this.backButton.Name = "backButton";
            this.backButton.Radius = -1F;
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel2.Controls.Add(this.label19, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.bookPanel, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // BookForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "BookForm";
            ((System.ComponentModel.ISupportInitialize)(this.noteBinding)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountBinding)).EndInit();
            this.bookPanel.ResumeLayout(false);
            this.bookPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bookView)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormControls.TableLayoutPanel bookPanel;
        private FormControls.DataGridView bookView;
        private FormControls.TextBox clientNameBox;
        private FormControls.Label label1;
        private FormControls.Button printAccountButton;
        private FormControls.Button stornoButton;
        private FormControls.TableLayoutPanel tableLayoutPanel3;
        private FormControls.Label label5;
        private FormControls.Label label6;
        private FormControls.Label label7;
        private Pflegehaushaltsbuch.FormControls.ComboBox accountStatusBox;
        private FormControls.Button backButton;
        private System.Windows.Forms.BindingSource noteBinding;
        private System.Windows.Forms.BindingSource accountBinding;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.Button bookButton;
        private FormControls.Button updateButton;
        private FormControls.Label label19;
        private FormControls.TableLayoutPanel tableLayoutPanel2;
        private FormControls.TextBox lastBookBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bookTextColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bookCategoryColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bookToColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clientColumn;
        private FormControls.TableLayoutPanel tableLayoutPanel4;
        private FormControls.DateTimeBox toDateBox;
        private FormControls.Label label2;
        private FormControls.TextBox totalAmountBox;
        private FormControls.DateTimeBox fromDateBox;
        private FormControls.Label fromToLabel;
        private FormControls.CheckBox periodCheckBox;
        private FormControls.Button exportButton;
        private FormControls.TextBox commentBox;
    }
}
