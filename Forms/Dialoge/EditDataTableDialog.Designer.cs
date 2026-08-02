namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    partial class EditDataTableDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditDataTableDialog));
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.view = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.okButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.abortButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnAlignColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.textAlignColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.widthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.view)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel1.Controls.Add(this.view, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // view
            // 
            this.view.AllowDrop = true;
            this.view.AllowUserToOrderColumns = true;
            this.view.BackgroundColor = System.Drawing.Color.Silver;
            this.view.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.view.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.view.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameColumn,
            this.idColumn,
            this.columnAlignColumn,
            this.textAlignColumn,
            this.widthColumn});
            resources.ApplyResources(this.view, "view");
            this.view.Name = "view";
            this.view.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(177)))), ((int)(((byte)(210)))));
            this.view.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.view.StandardTab = true;
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.okButton);
            this.flowLayoutPanel1.Controls.Add(this.abortButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
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
            // abortButton
            // 
            resources.ApplyResources(this.abortButton, "abortButton");
            this.abortButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.abortButton.BorderColor = System.Drawing.Color.DimGray;
            this.abortButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.abortButton.Name = "abortButton";
            this.abortButton.Radius = -1F;
            this.abortButton.UseVisualStyleBackColor = false;
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameColumn.DataPropertyName = "name";
            resources.ApplyResources(this.nameColumn, "nameColumn");
            this.nameColumn.Name = "nameColumn";
            // 
            // idColumn
            // 
            this.idColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.idColumn.DataPropertyName = "id";
            this.idColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.idColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.idColumn, "idColumn");
            this.idColumn.Items.AddRange(new object[] {
            "id",
            "document_id",
            "title",
            "name",
            "email",
            "co",
            "street",
            "zipcode",
            "city",
            "date",
            "note",
            "book_cat",
            "amount",
            "credit",
            "debit",
            "amount_payout",
            "amount_payback",
            "amount_payback_type",
            "account",
            "account_transfer",
            "lastbook",
            "active",
            "handsign"});
            this.idColumn.Name = "idColumn";
            this.idColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.idColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // columnAlignColumn
            // 
            this.columnAlignColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnAlignColumn.DataPropertyName = "columnAlign";
            this.columnAlignColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.columnAlignColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.columnAlignColumn, "columnAlignColumn");
            this.columnAlignColumn.Name = "columnAlignColumn";
            // 
            // textAlignColumn
            // 
            this.textAlignColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.textAlignColumn.DataPropertyName = "textAlign";
            this.textAlignColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.textAlignColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.textAlignColumn, "textAlignColumn");
            this.textAlignColumn.Name = "textAlignColumn";
            // 
            // widthColumn
            // 
            this.widthColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.widthColumn.DataPropertyName = "width";
            resources.ApplyResources(this.widthColumn, "widthColumn");
            this.widthColumn.Name = "widthColumn";
            // 
            // EditDataTableDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "EditDataTableDialog";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.view)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Pflegehaushaltsbuch.FormControls.DataGridView view;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.Button okButton;
        private FormControls.Button abortButton;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn idColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnAlignColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn textAlignColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn widthColumn;
    }
}
