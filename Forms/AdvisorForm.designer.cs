namespace Pflegehaushaltsbuch.Forms
{
    partial class AdvisorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvisorForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.tableLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label5 = new Pflegehaushaltsbuch.FormControls.Label();
            this.advisorsBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.dateBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.view = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.titleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emailColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.coColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.streetColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zipCodeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handSignColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.updateButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.insertButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.changeButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.deleteButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.printButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.backButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tableLayoutPanel3 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label19 = new Pflegehaushaltsbuch.FormControls.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.view)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel1.Border = true;
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.view, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.advisorsBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.dateBox, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Name = "label5";
            // 
            // advisorsBox
            // 
            this.advisorsBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.advisorsBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.advisorsBox, "advisorsBox");
            this.advisorsBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.advisorsBox.FormattingEnabled = true;
            this.advisorsBox.Name = "advisorsBox";
            // 
            // dateBox
            // 
            resources.ApplyResources(this.dateBox, "dateBox");
            this.dateBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dateBox.Name = "dateBox";
            this.dateBox.ReadOnly = true;
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
            this.titleColumn,
            this.nameColumn,
            this.emailColumn,
            this.coColumn,
            this.streetColumn,
            this.zipCodeColumn,
            this.cityColumn,
            this.dateColumn,
            this.handSignColumn});
            resources.ApplyResources(this.view, "view");
            this.view.MultiSelect = false;
            this.view.Name = "view";
            this.view.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.view.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.view.RowHeadersVisible = false;
            this.view.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.view.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.view.StandardTab = true;
            this.view.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.view_CellDoubleClick);
            // 
            // idColumn
            // 
            this.idColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.idColumn.DataPropertyName = "id";
            dataGridViewCellStyle1.Format = "000";
            dataGridViewCellStyle1.NullValue = null;
            this.idColumn.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.idColumn, "idColumn");
            this.idColumn.Name = "idColumn";
            this.idColumn.ReadOnly = true;
            // 
            // titleColumn
            // 
            this.titleColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.titleColumn.DataPropertyName = "title";
            resources.ApplyResources(this.titleColumn, "titleColumn");
            this.titleColumn.Name = "titleColumn";
            this.titleColumn.ReadOnly = true;
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.nameColumn.DataPropertyName = "name";
            resources.ApplyResources(this.nameColumn, "nameColumn");
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            // 
            // emailColumn
            // 
            this.emailColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.emailColumn.DataPropertyName = "email";
            resources.ApplyResources(this.emailColumn, "emailColumn");
            this.emailColumn.Name = "emailColumn";
            this.emailColumn.ReadOnly = true;
            // 
            // coColumn
            // 
            this.coColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.coColumn.DataPropertyName = "co";
            resources.ApplyResources(this.coColumn, "coColumn");
            this.coColumn.Name = "coColumn";
            this.coColumn.ReadOnly = true;
            // 
            // streetColumn
            // 
            this.streetColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.streetColumn.DataPropertyName = "street";
            resources.ApplyResources(this.streetColumn, "streetColumn");
            this.streetColumn.Name = "streetColumn";
            this.streetColumn.ReadOnly = true;
            // 
            // zipCodeColumn
            // 
            this.zipCodeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.zipCodeColumn.DataPropertyName = "zipcode";
            resources.ApplyResources(this.zipCodeColumn, "zipCodeColumn");
            this.zipCodeColumn.Name = "zipCodeColumn";
            this.zipCodeColumn.ReadOnly = true;
            // 
            // cityColumn
            // 
            this.cityColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cityColumn.DataPropertyName = "city";
            resources.ApplyResources(this.cityColumn, "cityColumn");
            this.cityColumn.Name = "cityColumn";
            this.cityColumn.ReadOnly = true;
            // 
            // dateColumn
            // 
            this.dateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dateColumn.DataPropertyName = "date";
            resources.ApplyResources(this.dateColumn, "dateColumn");
            this.dateColumn.Name = "dateColumn";
            this.dateColumn.ReadOnly = true;
            // 
            // handSignColumn
            // 
            this.handSignColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.handSignColumn.DataPropertyName = "handsign";
            resources.ApplyResources(this.handSignColumn, "handSignColumn");
            this.handSignColumn.Name = "handSignColumn";
            this.handSignColumn.ReadOnly = true;
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.updateButton);
            this.flowLayoutPanel1.Controls.Add(this.insertButton);
            this.flowLayoutPanel1.Controls.Add(this.changeButton);
            this.flowLayoutPanel1.Controls.Add(this.deleteButton);
            this.flowLayoutPanel1.Controls.Add(this.printButton);
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
            // insertButton
            // 
            resources.ApplyResources(this.insertButton, "insertButton");
            this.insertButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.insertButton.BorderColor = System.Drawing.Color.DimGray;
            this.insertButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.insertButton.Name = "insertButton";
            this.insertButton.Radius = -1F;
            this.insertButton.UseVisualStyleBackColor = false;
            this.insertButton.Click += new System.EventHandler(this.createAccountButton_Click);
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
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BorderColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.label19, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Gradiant = true;
            this.label19.Name = "label19";
            // 
            // AdvisorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "AdvisorForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.view)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Pflegehaushaltsbuch.FormControls.DataGridView view;
        private FormControls.Button insertButton;
        private FormControls.Button backButton;
        private Pflegehaushaltsbuch.FormControls.Label label1;
        private FormControls.ComboBox advisorsBox;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.Button printButton;
        private FormControls.Button changeButton;
        private Pflegehaushaltsbuch.FormControls.Label label5;
        private FormControls.Button deleteButton;
        private FormControls.TableLayoutPanel tableLayoutPanel2;
        private FormControls.TableLayoutPanel tableLayoutPanel3;
        private FormControls.Label label19;
        private FormControls.TextBox dateBox;
        private FormControls.Button updateButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn handSignColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cityColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn zipCodeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn streetColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn coColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn emailColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idColumn;
    }
}
