namespace Pflegehaushaltsbuch.Forms
{
    partial class EmployeesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeesForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel4 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label19 = new Pflegehaushaltsbuch.FormControls.Label();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.tableLayoutPanel3 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label10 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label5 = new Pflegehaushaltsbuch.FormControls.Label();
            this.nameBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.dateBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.tableLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.totalAmountBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.view = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountPayoutColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountPayBackColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paybackTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handSignColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activeColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.updateButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.createButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.changeButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.deleteButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.payOutButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.printButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.buttonImport = new Pflegehaushaltsbuch.FormControls.Button();
            this.exportButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.backButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.view)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BorderColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.label19, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Gradiant = true;
            this.label19.Name = "label19";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel1.Border = true;
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.view, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel3.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label5, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.nameBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.dateBox, 4, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Name = "label10";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Name = "label5";
            // 
            // nameBox
            // 
            this.nameBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.nameBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.nameBox, "nameBox");
            this.nameBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.nameBox.FormattingEnabled = true;
            this.nameBox.Name = "nameBox";
            // 
            // dateBox
            // 
            this.dateBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.dateBox, "dateBox");
            this.dateBox.Name = "dateBox";
            this.dateBox.ReadOnly = true;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.totalAmountBox, 1, 0);
            this.tableLayoutPanel2.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // totalAmountBox
            // 
            this.totalAmountBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.totalAmountBox, "totalAmountBox");
            this.totalAmountBox.Name = "totalAmountBox";
            this.totalAmountBox.ReadOnly = true;
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
            this.nameColumn,
            this.dateColumn,
            this.amountPayoutColumn,
            this.amountPayBackColumn,
            this.paybackTypeColumn,
            this.handSignColumn,
            this.activeColumn});
            resources.ApplyResources(this.view, "view");
            this.view.MultiSelect = false;
            this.view.Name = "view";
            this.view.ReadOnly = true;
            this.view.RowHeadersVisible = false;
            this.view.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.view.StandardTab = true;
            this.view.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.view_CellContentDoubleClick);
            // 
            // idColumn
            // 
            this.idColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.idColumn.DataPropertyName = "id";
            dataGridViewCellStyle1.Format = "000";
            this.idColumn.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.idColumn, "idColumn");
            this.idColumn.Name = "idColumn";
            this.idColumn.ReadOnly = true;
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameColumn.DataPropertyName = "name";
            resources.ApplyResources(this.nameColumn, "nameColumn");
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            // 
            // dateColumn
            // 
            this.dateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dateColumn.DataPropertyName = "date";
            resources.ApplyResources(this.dateColumn, "dateColumn");
            this.dateColumn.Name = "dateColumn";
            this.dateColumn.ReadOnly = true;
            // 
            // amountPayoutColumn
            // 
            this.amountPayoutColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.amountPayoutColumn.DataPropertyName = "amount_payout";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "C";
            this.amountPayoutColumn.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.amountPayoutColumn, "amountPayoutColumn");
            this.amountPayoutColumn.Name = "amountPayoutColumn";
            this.amountPayoutColumn.ReadOnly = true;
            // 
            // amountPayBackColumn
            // 
            this.amountPayBackColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.amountPayBackColumn.DataPropertyName = "amount_payback";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "C";
            this.amountPayBackColumn.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.amountPayBackColumn, "amountPayBackColumn");
            this.amountPayBackColumn.Name = "amountPayBackColumn";
            this.amountPayBackColumn.ReadOnly = true;
            // 
            // paybackTypeColumn
            // 
            this.paybackTypeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.paybackTypeColumn.DataPropertyName = "amount_payback_type";
            resources.ApplyResources(this.paybackTypeColumn, "paybackTypeColumn");
            this.paybackTypeColumn.Name = "paybackTypeColumn";
            this.paybackTypeColumn.ReadOnly = true;
            this.paybackTypeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // handSignColumn
            // 
            this.handSignColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.handSignColumn.DataPropertyName = "handsign";
            resources.ApplyResources(this.handSignColumn, "handSignColumn");
            this.handSignColumn.Name = "handSignColumn";
            this.handSignColumn.ReadOnly = true;
            // 
            // activeColumn
            // 
            this.activeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.activeColumn.DataPropertyName = "active";
            resources.ApplyResources(this.activeColumn, "activeColumn");
            this.activeColumn.Name = "activeColumn";
            this.activeColumn.ReadOnly = true;
            this.activeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.activeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.updateButton);
            this.flowLayoutPanel1.Controls.Add(this.createButton);
            this.flowLayoutPanel1.Controls.Add(this.changeButton);
            this.flowLayoutPanel1.Controls.Add(this.deleteButton);
            this.flowLayoutPanel1.Controls.Add(this.payOutButton);
            this.flowLayoutPanel1.Controls.Add(this.printButton);
            this.flowLayoutPanel1.Controls.Add(this.buttonImport);
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
            // createButton
            // 
            resources.ApplyResources(this.createButton, "createButton");
            this.createButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.createButton.BorderColor = System.Drawing.Color.DimGray;
            this.createButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.createButton.Name = "createButton";
            this.createButton.Radius = -1F;
            this.createButton.UseVisualStyleBackColor = false;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // changeButton
            // 
            resources.ApplyResources(this.changeButton, "changeButton");
            this.changeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.changeButton.BorderColor = System.Drawing.Color.DimGray;
            this.changeButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.changeButton.Name = "changeButton";
            this.changeButton.Radius = -1F;
            this.changeButton.UseVisualStyleBackColor = false;
            this.changeButton.Click += new System.EventHandler(this.changeButton_Click);
            // 
            // deleteButton
            // 
            resources.ApplyResources(this.deleteButton, "deleteButton");
            this.deleteButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.deleteButton.BorderColor = System.Drawing.Color.DimGray;
            this.deleteButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Radius = -1F;
            this.deleteButton.UseVisualStyleBackColor = false;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // payOutButton
            // 
            resources.ApplyResources(this.payOutButton, "payOutButton");
            this.payOutButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.payOutButton.BorderColor = System.Drawing.Color.DimGray;
            this.payOutButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.payOutButton.Name = "payOutButton";
            this.payOutButton.Radius = -1F;
            this.payOutButton.UseVisualStyleBackColor = false;
            this.payOutButton.Click += new System.EventHandler(this.payOutButton_Click);
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
            // buttonImport
            // 
            resources.ApplyResources(this.buttonImport, "buttonImport");
            this.buttonImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.buttonImport.BorderColor = System.Drawing.Color.DimGray;
            this.buttonImport.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Radius = -1F;
            this.buttonImport.UseVisualStyleBackColor = false;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
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
            // EmployeesForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel4);
            this.Name = "EmployeesForm";
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.view)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FormControls.DataGridView view;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.Button createButton;
        private FormControls.Button backButton;
        private FormControls.Button changeButton;
        private FormControls.Button payOutButton;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.TableLayoutPanel tableLayoutPanel2;
        private FormControls.Label label1;
        private FormControls.TextBox totalAmountBox;
        private FormControls.Button updateButton;
        private FormControls.Button printButton;
        private FormControls.Label label5;
        private FormControls.ComboBox nameBox;
        private FormControls.Label label10;
        private FormControls.Button deleteButton;
        private FormControls.TableLayoutPanel tableLayoutPanel3;
        private FormControls.TableLayoutPanel tableLayoutPanel4;
        private FormControls.Label label19;
        private FormControls.TextBox dateBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn idColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountPayoutColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountPayBackColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paybackTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn handSignColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeColumn;
        private FormControls.Button exportButton;
        private FormControls.Button buttonImport;
    }
}
