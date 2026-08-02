namespace Pflegehaushaltsbuch.Forms
{
    partial class DocumentsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentsForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.tableLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label7 = new Pflegehaushaltsbuch.FormControls.Label();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.view = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.coColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handSignColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel3 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label6 = new Pflegehaushaltsbuch.FormControls.Label();
            this.activeClientsBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.dateCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.dateBox = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            this.clientBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.insertButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.deleteButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.backButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.view)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dataGridViewImageColumn1.DataPropertyName = "!";
            this.dataGridViewImageColumn1.FillWeight = 30.45685F;
            resources.ApplyResources(this.dataGridViewImageColumn1, "dataGridViewImageColumn1");
            this.dataGridViewImageColumn1.Image = global::Pflegehaushaltsbuch.Properties.Resources.Info;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Gradiant = true;
            this.label7.Name = "label7";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel1.Border = true;
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.view, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
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
            this.coColumn,
            this.handSignColumn});
            resources.ApplyResources(this.view, "view");
            this.view.Name = "view";
            this.view.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Empty;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Empty;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.view.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.view.RowHeadersVisible = false;
            this.view.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.view.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.Empty;
            this.view.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Empty;
            this.view.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.view.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.view.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.view.StandardTab = true;
            this.view.VirtualMode = true;
            this.view.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.view_CellDoubleClick);
            // 
            // idColumn
            // 
            this.idColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.idColumn.DataPropertyName = "index";
            dataGridViewCellStyle1.Format = "000";
            dataGridViewCellStyle1.NullValue = null;
            this.idColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.idColumn.FillWeight = 113.9086F;
            resources.ApplyResources(this.idColumn, "idColumn");
            this.idColumn.Name = "idColumn";
            this.idColumn.ReadOnly = true;
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.nameColumn.DataPropertyName = "date";
            this.nameColumn.FillWeight = 113.9086F;
            resources.ApplyResources(this.nameColumn, "nameColumn");
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            // 
            // coColumn
            // 
            this.coColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.coColumn.DataPropertyName = "note";
            resources.ApplyResources(this.coColumn, "coColumn");
            this.coColumn.Name = "coColumn";
            this.coColumn.ReadOnly = true;
            // 
            // handSignColumn
            // 
            this.handSignColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.handSignColumn.DataPropertyName = "handsign";
            resources.ApplyResources(this.handSignColumn, "handSignColumn");
            this.handSignColumn.Name = "handSignColumn";
            this.handSignColumn.ReadOnly = true;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel3.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel3.Controls.Add(this.label6, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.activeClientsBox, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.dateCheckBox, 7, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.dateBox, 8, 0);
            this.tableLayoutPanel3.Controls.Add(this.clientBox, 1, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Name = "label6";
            // 
            // activeClientsBox
            // 
            resources.ApplyResources(this.activeClientsBox, "activeClientsBox");
            this.activeClientsBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.activeClientsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.activeClientsBox.FormattingEnabled = true;
            this.activeClientsBox.Name = "activeClientsBox";
            this.activeClientsBox.SelectedIndexChanged += new System.EventHandler(this.activeClientsBox_SelectedIndexChanged);
            // 
            // dateCheckBox
            // 
            resources.ApplyResources(this.dateCheckBox, "dateCheckBox");
            this.dateCheckBox.BorderColor = System.Drawing.Color.White;
            this.dateCheckBox.CheckedImage = null;
            this.dateCheckBox.ForeColor = System.Drawing.Color.White;
            this.dateCheckBox.Name = "dateCheckBox";
            this.dateCheckBox.UnCheckedImage = null;
            this.dateCheckBox.UseVisualStyleBackColor = true;
            this.dateCheckBox.CheckedChanged += new System.EventHandler(this.dateCheckBox_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // dateBox
            // 
            resources.ApplyResources(this.dateBox, "dateBox");
            this.dateBox.Days = false;
            this.dateBox.Name = "dateBox";
            this.dateBox.ValueChanged += new Pflegehaushaltsbuch.FormControls.DateTimeBox.UpdateDistanceDelegate(this.dateBox_ValueChanged);
            // 
            // clientBox
            // 
            this.clientBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            resources.ApplyResources(this.clientBox, "clientBox");
            this.clientBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.clientBox.FormattingEnabled = true;
            this.clientBox.Name = "clientBox";
            this.clientBox.Sorted = true;
            this.clientBox.SelectedIndexChanged += new System.EventHandler(this.clientBox_SelectedIndexChanged);
            this.clientBox.DropDownClosed += new System.EventHandler(this.clientBox_DropDownClosed);
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.insertButton);
            this.flowLayoutPanel1.Controls.Add(this.deleteButton);
            this.flowLayoutPanel1.Controls.Add(this.backButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // insertButton
            // 
            resources.ApplyResources(this.insertButton, "insertButton");
            this.insertButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.insertButton.BorderColor = System.Drawing.Color.DimGray;
            this.insertButton.Name = "insertButton";
            this.insertButton.Radius = -1F;
            this.insertButton.UseVisualStyleBackColor = false;
            this.insertButton.Click += new System.EventHandler(this.insertButton_Click);
            // 
            // deleteButton
            // 
            resources.ApplyResources(this.deleteButton, "deleteButton");
            this.deleteButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.deleteButton.BorderColor = System.Drawing.Color.DimGray;
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Radius = -1F;
            this.deleteButton.UseVisualStyleBackColor = false;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // backButton
            // 
            resources.ApplyResources(this.backButton, "backButton");
            this.backButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.backButton.BorderColor = System.Drawing.Color.DimGray;
            this.backButton.Name = "backButton";
            this.backButton.Radius = -1F;
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // DocumentsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "DocumentsForm";
            this.Enter += new System.EventHandler(this.clientPanel_Enter);
            this.Leave += new System.EventHandler(this.clientPanel_Leave);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.view)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Pflegehaushaltsbuch.FormControls.DataGridView view;
        private FormControls.Button insertButton;
        private FormControls.Button backButton;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private Pflegehaushaltsbuch.FormControls.ComboBox activeClientsBox;
        private Pflegehaushaltsbuch.FormControls.Label label6;
        private Pflegehaushaltsbuch.FormControls.Label label1;
        private FormControls.DateTimeBox dateBox;
        private FormControls.CheckBox dateCheckBox;
        private FormControls.TableLayoutPanel tableLayoutPanel2;
        private FormControls.TableLayoutPanel tableLayoutPanel3;
        private FormControls.Label label7;
        private FormControls.ComboBox clientBox;
        private FormControls.Button deleteButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn idColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn coColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn handSignColumn;

    }
}