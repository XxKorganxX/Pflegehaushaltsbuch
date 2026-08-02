using Pflegehaushaltsbuch;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the User Manager Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class UserManagerForm : Pflegehaushaltsbuch.FormControls.Form, IUserManagerFormContract
    {
        private readonly UserManagerFormPresenter presenter;

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);

        private DataTable table = new DataTable();
        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms formEnum, SQLBase sql);
        public event OnShowForm ShowForm;
        /// <summary>
        /// Creates a new User Manager Form instance and initializes the required state.
        /// </summary>
        public UserManagerForm()
        {
            InitializeComponent();
            presenter = new UserManagerFormPresenter(this);
            view.CellFormatting += CellFormatting;
            view.CellPainting += CellPainting;
            view.AutoGenerateColumns = false;
            Enter += UserRightsForm_Enter;
            Leave += UserRightsForm_Leave;
        }
        void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == accessColumn.Index)
            {
                int access = Int32.Parse(e.Value.ToString());
                string create = ((access & (int)Enums.UserRightEnum.Insert) == (int)Enums.UserRightEnum.Insert) ? Messages.usermanagement_Create : string.Empty;
                string change = ((access & (int)Enums.UserRightEnum.Change) == (int)Enums.UserRightEnum.Change) ? Messages.usermanagement_Change : string.Empty;
                string delete = ((access & (int)Enums.UserRightEnum.Delete) == (int)Enums.UserRightEnum.Delete) ? Messages.usermanagement_Delete : string.Empty;
                string value = "";
                if(!string.IsNullOrEmpty(create))
                    value += create;
                if(!string.IsNullOrEmpty(change))
                    value += "/"+change;
                if(!string.IsNullOrEmpty(delete))
                    value += "/"+delete;
                e.Value = value.Trim(new char[]{'/'});
            }
        }
        void CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == adminColumn.Index && e.RowIndex >= 0)
            {
                SolidBrush backColor = new SolidBrush(view.BackgroundColor);
                e.PaintBackground(e.CellBounds, (e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected);
                bool isActive = (bool)e.Value;
                //if (bool.TryParse(e.Value.ToString(), out isActive))
                //if (isActive)
                {
                    Rectangle rect = e.CellBounds;
                    rect.Height -= 10;
                    rect.X += 5;
                    rect.Y += 5;
                    rect.Width = rect.Height;
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    Rectangle bounds = rect;
                    using (var ellipsePath = new GraphicsPath())
                    {
                        ellipsePath.AddEllipse(bounds);
                        using (var brush = new PathGradientBrush(ellipsePath))
                        {
                            brush.CenterPoint = new PointF(bounds.X + bounds.Width / 3f, bounds.Y + bounds.Height / 3f);
                            brush.CenterColor = Color.White;
                            if (isActive)
                                brush.SurroundColors = new[] { Color.Green };
                            else
                                brush.SurroundColors = new[] { Color.Red };
                            e.Graphics.FillEllipse(brush, rect);
                        }
                    }
                    e.Graphics.DrawEllipse(new Pen(Brushes.Black), rect);
                    e.Graphics.SmoothingMode = SmoothingMode.Default;
                }
                e.Handled = true;
            }
        }
        /// <summary>
        /// Connects the table To Data Base data source or control used by the current workflow.
        /// </summary>
        private async Task ConnectTableToDataBase()
        {
            clientBox.DataSource = null;
            await sql.FillAdapterAsync(SQLBase.SELECT.Users, table);
            table.PrimaryKey = new DataColumn[] { table.Columns[nameColumn.DataPropertyName] };
            table.CaseSensitive = true;
            view.DataSource = table;
            clientBox.DisplayMember = nameColumn.DataPropertyName;
            clientBox.DataSource = table;
        }
        async void UserRightsForm_Enter(object sender, EventArgs e)
        {
            await ConnectTableToDataBase();
        }
        void UserRightsForm_Leave(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Administration, sql);
        }
        /// <summary>
        /// Handles the click event for save Button and updates the related state.
        /// </summary>
        private async void saveButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;
            try
            {
                bool valid = await sql.UpdateAdapterAsync(SQLBase.SELECT.Users, table);
                if (!valid)
                {
                    table.RejectChanges();
                    MessageBox.ShowDialog(this, Messages.datatable_update_failed);
                }
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for create Button and updates the related state.
        /// </summary>
        private async void createButton_Click(object sender, EventArgs e)
        {
            using (CreationUserForm userForm = new CreationUserForm(sql))
            {
                if (userForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                await ConnectTableToDataBase();
            }
        }
        /// <summary>
        /// Handles the click event for update Button and updates the related state.
        /// </summary>
        private async void updateButton_Click(object sender, EventArgs e)
        {
            if (view.SelectedRows.Count == 0)
            {
                MessageBox.ShowDialog(this, Messages.usermanagement_users_missing);
                return;
            }

            DataGridViewRow rowView = view.SelectedRows[0];
            DataRow row = (rowView.DataBoundItem as DataRowView).Row;
            using (CreationUserForm userForm = new CreationUserForm(sql, row))
            {
                if (userForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                await ConnectTableToDataBase();
            }
        }
        /// <summary>
        /// Handles the click event for delete Button and updates the related state.
        /// </summary>
        private async void deleteButton_Click(object sender, EventArgs e)
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {            
                if (view.SelectedRows.Count <= 0)
                {
                    MessageBox.ShowDialog(this, Messages.usermanagement_users_missing);
                    return;
                }

                DataGridViewRow rowView = view.SelectedRows[0];
                DataRow row = (rowView.DataBoundItem as DataRowView).Row;
                if ((bool)row["admin"] && table.Select("admin=true").Count() == 1)
                {
                    if (MessageBox.ShowDialog(this, Messages.usermanagement_last_admin_delete_warning, Messages.usermanagement_user_delete_title, MessageBoxButtons.YesNo)
                        != DialogResult.Yes)
                        return;
                }
                if (MessageBox.ShowDialog(this, string.Format(Messages.usermanagement_user_delete, row[nameColumn.DataPropertyName]), Messages.usermanagement_user_delete_title, MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    return;
                row.Delete();
                bool value = await sql.UpdateAdapterAsync(SQLBase.SELECT.Users, table);
                if (value)
                {
                    table.AcceptChanges();
                    MessageBox.ShowDialog(this, Messages.usermanagement_user_deleted);
                    clientBox.DataSource = table;
                    clientBox.DisplayMember = "name";
                }
                else
                {
                    table.RejectChanges();
                    MessageBox.ShowDialog(this, Messages.usermanagement_user_no_deleted);
                }
            }
            catch
            {
                table.RejectChanges();
                throw;
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }
    }
}
