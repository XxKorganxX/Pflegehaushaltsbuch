namespace Pflegehaushaltsbuch.Forms
{
    partial class ClientsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientsForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.selectClientButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.scheduleButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.insertButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.changeButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.deleteButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.clientBooksButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.printButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.backButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.label7 = new Pflegehaushaltsbuch.FormControls.Label();
            this.tableLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.clientsView = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.infoColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.handSignColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activeColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label4 = new Pflegehaushaltsbuch.FormControls.Label();
            this.tableLayoutPanel3 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.clientDateBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label5 = new Pflegehaushaltsbuch.FormControls.Label();
            this.bornBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label3 = new Pflegehaushaltsbuch.FormControls.Label();
            this.clientBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.activeClientsBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.label6 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label8 = new Pflegehaushaltsbuch.FormControls.Label();
            this.totalClientsBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.totalAmountBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.deadLineBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientsView)).BeginInit();
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
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.selectClientButton);
            this.flowLayoutPanel1.Controls.Add(this.scheduleButton);
            this.flowLayoutPanel1.Controls.Add(this.insertButton);
            this.flowLayoutPanel1.Controls.Add(this.changeButton);
            this.flowLayoutPanel1.Controls.Add(this.deleteButton);
            this.flowLayoutPanel1.Controls.Add(this.clientBooksButton);
            this.flowLayoutPanel1.Controls.Add(this.printButton);
            this.flowLayoutPanel1.Controls.Add(this.backButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // selectClientButton
            // 
            resources.ApplyResources(this.selectClientButton, "selectClientButton");
            this.selectClientButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.selectClientButton.BorderColor = System.Drawing.Color.DimGray;
            this.selectClientButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.selectClientButton.Name = "selectClientButton";
            this.selectClientButton.Radius = -1F;
            this.selectClientButton.UseVisualStyleBackColor = false;
            this.selectClientButton.Click += new System.EventHandler(this.selectAccountButton_Click);
            // 
            // scheduleButton
            // 
            resources.ApplyResources(this.scheduleButton, "scheduleButton");
            this.scheduleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.scheduleButton.BorderColor = System.Drawing.Color.DimGray;
            this.scheduleButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.scheduleButton.Name = "scheduleButton";
            this.scheduleButton.Radius = -1F;
            this.scheduleButton.UseVisualStyleBackColor = false;
            this.scheduleButton.Click += new System.EventHandler(this.deadLinesButton_Click);
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
            // clientBooksButton
            // 
            resources.ApplyResources(this.clientBooksButton, "clientBooksButton");
            this.clientBooksButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.clientBooksButton.BorderColor = System.Drawing.Color.DimGray;
            this.clientBooksButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.clientBooksButton.Name = "clientBooksButton";
            this.clientBooksButton.Radius = -1F;
            this.clientBooksButton.UseVisualStyleBackColor = false;
            this.clientBooksButton.Click += new System.EventHandler(this.clientBooksButton_Click);
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
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.clientsView, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.totalAmountBox, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.deadLineBox, 2, 3);
            this.tableLayoutPanel2.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.SetColumnSpan(this.label2, 2);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // clientsView
            // 
            this.clientsView.AllowUserToAddRows = false;
            this.clientsView.AllowUserToDeleteRows = false;
            this.clientsView.AllowUserToResizeColumns = false;
            this.clientsView.AllowUserToResizeRows = false;
            this.clientsView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.clientsView.BackgroundColor = System.Drawing.Color.White;
            this.clientsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.clientsView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.infoColumn,
            this.idColumn,
            this.nameColumn,
            this.dateColumn,
            this.amountColumn,
            this.handSignColumn,
            this.activeColumn});
            this.tableLayoutPanel2.SetColumnSpan(this.clientsView, 4);
            resources.ApplyResources(this.clientsView, "clientsView");
            this.clientsView.MultiSelect = false;
            this.clientsView.Name = "clientsView";
            this.clientsView.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.clientsView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.clientsView.RowHeadersVisible = false;
            this.clientsView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.clientsView.RowTemplate.Height = 24;
            this.clientsView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.clientsView.StandardTab = true;
            this.clientsView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.clientsView_KeyUp);
            // 
            // infoColumn
            // 
            this.infoColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.infoColumn.DataPropertyName = "info";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.NullValue = false;
            this.infoColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.infoColumn.FillWeight = 30.45685F;
            resources.ApplyResources(this.infoColumn, "infoColumn");
            this.infoColumn.Name = "infoColumn";
            this.infoColumn.ReadOnly = true;
            this.infoColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.infoColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // idColumn
            // 
            this.idColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.idColumn.DataPropertyName = "id";
            dataGridViewCellStyle2.Format = "000";
            dataGridViewCellStyle2.NullValue = null;
            this.idColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.idColumn.FillWeight = 113.9086F;
            resources.ApplyResources(this.idColumn, "idColumn");
            this.idColumn.Name = "idColumn";
            this.idColumn.ReadOnly = true;
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameColumn.DataPropertyName = "name";
            this.nameColumn.FillWeight = 113.9086F;
            resources.ApplyResources(this.nameColumn, "nameColumn");
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
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
            // amountColumn
            // 
            this.amountColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.amountColumn.DataPropertyName = "amount";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.amountColumn.DefaultCellStyle = dataGridViewCellStyle3;
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
            // activeColumn
            // 
            this.activeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.activeColumn.DataPropertyName = "active";
            this.activeColumn.FillWeight = 113.9086F;
            resources.ApplyResources(this.activeColumn, "activeColumn");
            this.activeColumn.Name = "activeColumn";
            this.activeColumn.ReadOnly = true;
            this.activeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.activeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.SetColumnSpan(this.label4, 2);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Name = "label4";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel2.SetColumnSpan(this.tableLayoutPanel3, 4);
            this.tableLayoutPanel3.Controls.Add(this.clientDateBox, 9, 0);
            this.tableLayoutPanel3.Controls.Add(this.label5, 8, 0);
            this.tableLayoutPanel3.Controls.Add(this.bornBox, 7, 0);
            this.tableLayoutPanel3.Controls.Add(this.label3, 6, 0);
            this.tableLayoutPanel3.Controls.Add(this.clientBox, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.activeClientsBox, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.totalClientsBox, 1, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // clientDateBox
            // 
            this.clientDateBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.clientDateBox, "clientDateBox");
            this.clientDateBox.Name = "clientDateBox";
            this.clientDateBox.ReadOnly = true;
            this.clientDateBox.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Name = "label5";
            // 
            // bornBox
            // 
            this.bornBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.bornBox, "bornBox");
            this.bornBox.Name = "bornBox";
            this.bornBox.ReadOnly = true;
            this.bornBox.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // clientBox
            // 
            this.clientBox.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.clientBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.clientBox.BackColor = System.Drawing.Color.White;
            this.clientBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.clientBox.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.clientBox, "clientBox");
            this.clientBox.Name = "clientBox";
            this.clientBox.DropDownClosed += new System.EventHandler(this.clientBox_DropDownClosed);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
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
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Name = "label6";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // totalClientsBox
            // 
            this.totalClientsBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.totalClientsBox, "totalClientsBox");
            this.totalClientsBox.Name = "totalClientsBox";
            this.totalClientsBox.ReadOnly = true;
            this.totalClientsBox.TabStop = false;
            // 
            // totalAmountBox
            // 
            this.totalAmountBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.totalAmountBox, "totalAmountBox");
            this.totalAmountBox.Name = "totalAmountBox";
            this.totalAmountBox.ReadOnly = true;
            // 
            // deadLineBox
            // 
            this.deadLineBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.deadLineBox, "deadLineBox");
            this.deadLineBox.Name = "deadLineBox";
            this.deadLineBox.ReadOnly = true;
            // 
            // ClientsForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Orange;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ClientsForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientsView)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FormControls.DataGridView clientsView;
        private FormControls.Button insertButton;
        private FormControls.Button selectClientButton;
        private FormControls.Button backButton;
        private FormControls.Label label1;
        private FormControls.ComboBox clientBox;
        private FormControls.Button scheduleButton;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.TableLayoutPanel tableLayoutPanel2;
        private FormControls.Label label2;
        private FormControls.TextBox totalAmountBox;
        private FormControls.Button printButton;
        private FormControls.Button changeButton;
        private FormControls.Label label3;
        private FormControls.Label label4;
        private FormControls.Label label5;
        private FormControls.TextBox bornBox;
        private Pflegehaushaltsbuch.FormControls.ComboBox activeClientsBox;
        private FormControls.Label label6;
        private FormControls.Button clientBooksButton;
        private FormControls.Button deleteButton;
        private FormControls.TableLayoutPanel tableLayoutPanel3;
        private FormControls.TextBox clientDateBox;
        private FormControls.Label label7;
        private FormControls.Label label8;
        private FormControls.TextBox totalClientsBox;
        private FormControls.TextBox deadLineBox;
        private System.Windows.Forms.DataGridViewCheckBoxColumn infoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn handSignColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeColumn;
    }
}
