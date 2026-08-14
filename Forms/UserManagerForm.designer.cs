namespace Pflegehaushaltsbuch.Forms
{
    partial class UserManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserManagerForm));
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.view = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.loginColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accessColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.adminColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tableLayoutPanel3 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.userNameTextBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.createButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.updateButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.deleteButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.backButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tableLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label7 = new Pflegehaushaltsbuch.FormControls.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.view)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel1.Border = true;
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
            this.view.BackgroundColor = System.Drawing.Color.White;
            this.view.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.view.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.loginColumn,
            this.nameColumn,
            this.accessColumn,
            this.adminColumn});
            resources.ApplyResources(this.view, "view");
            this.view.MultiSelect = false;
            this.view.Name = "view";
            this.view.ReadOnly = true;
            this.view.RowHeadersVisible = false;
            this.view.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.view.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(177)))), ((int)(((byte)(210)))));
            this.view.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.view.RowTemplate.Height = 24;
            this.view.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.view.StandardTab = true;
            // 
            // loginColumn
            // 
            this.loginColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.loginColumn.DataPropertyName = "login";
            resources.ApplyResources(this.loginColumn, "loginColumn");
            this.loginColumn.Name = "loginColumn";
            this.loginColumn.ReadOnly = true;
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.nameColumn.DataPropertyName = "handsign";
            resources.ApplyResources(this.nameColumn, "nameColumn");
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            // 
            // accessColumn
            // 
            this.accessColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.accessColumn.DataPropertyName = "access";
            resources.ApplyResources(this.accessColumn, "accessColumn");
            this.accessColumn.Name = "accessColumn";
            this.accessColumn.ReadOnly = true;
            this.accessColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // adminColumn
            // 
            this.adminColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.adminColumn.DataPropertyName = "admin";
            resources.ApplyResources(this.adminColumn, "adminColumn");
            this.adminColumn.Name = "adminColumn";
            this.adminColumn.ReadOnly = true;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel3.Controls.Add(this.userNameTextBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // userNameTextBox
            // 
            resources.ApplyResources(this.userNameTextBox, "userNameTextBox");
            this.userNameTextBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.userNameTextBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.userNameTextBox.FormattingEnabled = true;
            this.userNameTextBox.Name = "userNameTextBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.createButton);
            this.flowLayoutPanel1.Controls.Add(this.updateButton);
            this.flowLayoutPanel1.Controls.Add(this.deleteButton);
            this.flowLayoutPanel1.Controls.Add(this.backButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // createButton
            // 
            resources.ApplyResources(this.createButton, "createButton");
            this.createButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.createButton.BorderColor = System.Drawing.Color.DimGray;
            this.createButton.Name = "createButton";
            this.createButton.Radius = -1F;
            this.createButton.UseVisualStyleBackColor = false;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // updateButton
            // 
            resources.ApplyResources(this.updateButton, "updateButton");
            this.updateButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.updateButton.BorderColor = System.Drawing.Color.DimGray;
            this.updateButton.Name = "updateButton";
            this.updateButton.Radius = -1F;
            this.updateButton.UseVisualStyleBackColor = false;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
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
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BorderColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
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
            // UserManagerForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "UserManagerForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.view)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Pflegehaushaltsbuch.FormControls.ComboBox userNameTextBox;
        private Pflegehaushaltsbuch.FormControls.Label label1;
        private FormControls.Button backButton;
        private FormControls.Button createButton;
        private Pflegehaushaltsbuch.FormControls.DataGridView view;
        private FormControls.Button updateButton;
        private FormControls.Button deleteButton;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.TableLayoutPanel tableLayoutPanel3;
        private FormControls.TableLayoutPanel tableLayoutPanel2;
        private FormControls.Label label7;
        private System.Windows.Forms.DataGridViewTextBoxColumn loginColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accessColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn adminColumn;
    }
}
