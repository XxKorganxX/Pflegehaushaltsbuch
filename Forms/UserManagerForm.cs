using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the User Manager Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class UserManagerForm : Form, IUserManagerFormContract
    {
        private readonly UserManagerFormPresenter presenter;

        /// <summary>
        /// Creates a new UserManagerForm view.
        /// </summary>
        public UserManagerForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new UserManagerFormPresenter(this, session);
            view.CellFormatting += CellFormatting;
            view.CellPainting += CellPainting;
            view.AutoGenerateColumns = false;
            Enter += UserRightsForm_Enter;
            Leave += UserRightsForm_Leave;
        }

        /// <summary>
        /// Handles the cell format event.
        /// </summary>
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
        /// <summary>
        /// Handles the cell paint event.
        /// </summary>
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
        /// Handles the user rights form enter enter event.
        /// </summary>
        async void UserRightsForm_Enter(object sender, EventArgs e)
        {
            ApplyCurrentUserRights();
            await presenter.EnterAsync();
        }
        /// <summary>
        /// Handles the user rights form leave leave event.
        /// </summary>
        void UserRightsForm_Leave(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            presenter.Back();
        }
        /// <summary>
        /// Handles the click event for save Button and updates the related state.
        /// </summary>
        private async void saveButton_Click(object sender, EventArgs e)
        {
            await presenter.SaveAsync();
        }
        /// <summary>
        /// Handles the click event for create Button and updates the related state.
        /// </summary>
        private async void createButton_Click(object sender, EventArgs e)
        {
            await presenter.CreateAsync();
        }
        /// <summary>
        /// Handles the click event for update Button and updates the related state.
        /// </summary>
        private async void updateButton_Click(object sender, EventArgs e)
        {
            await presenter.UpdateAsync();
        }
        /// <summary>
        /// Handles the click event for delete Button and updates the related state.
        /// </summary>
        private async void deleteButton_Click(object sender, EventArgs e)
        {
            await presenter.DeleteAsync();
        }

        DataRow IUserManagerFormContract.SelectedUserRow
        {
            get
            {
                if (view.SelectedRows.Count == 0)
                    return null;

                DataGridViewRow rowView = view.SelectedRows[0];
                DataRowView row = rowView.DataBoundItem as DataRowView;
                return row == null ? null : row.Row;
            }
        }

        void IUserManagerFormContract.BindUsers(DataTable table)
        {
            view.DataSource = table;
            clientBox.DisplayMember = Columns.Name;
            clientBox.DataSource = table;
        }

        void IUserManagerFormContract.ClearUsers()
        {
            clientBox.DataSource = null;
            view.DataSource = null;
        }

        /// <summary>
        /// Runs the show administration form view action for the presenter.
        /// </summary>
        void IUserManagerFormContract.ShowAdministrationForm()
        {
            ShowFormEvent(Enums.Forms.Administration);
        }

        /// <summary>
        /// Runs the show create user dialog view action for the presenter.
        /// </summary>
        bool IUserManagerFormContract.ShowCreateUserDialog(SqlSession session)
        {
            using (CreationUserForm userForm = new CreationUserForm(session))
            {
                return userForm.ShowDialog(this) == DialogResult.OK;
            }
        }

        /// <summary>
        /// Runs the show update user dialog view action for the presenter.
        /// </summary>
        bool IUserManagerFormContract.ShowUpdateUserDialog(SqlSession session, DataRow row)
        {
            using (CreationUserForm userForm = new CreationUserForm(session, row))
            {
                return userForm.ShowDialog(this) == DialogResult.OK;
            }
        }

        /// <summary>
        /// Runs the show users missing view action for the presenter.
        /// </summary>
        void IUserManagerFormContract.ShowUsersMissing()
        {
            MessageBox.ShowDialog(this, Messages.usermanagement_users_missing);
        }

        /// <summary>
        /// Runs the confirm last admin delete view action for the presenter.
        /// </summary>
        bool IUserManagerFormContract.ConfirmLastAdminDelete()
        {
            return MessageBox.ShowDialog(this, Messages.usermanagement_last_admin_delete_warning, Messages.usermanagement_user_delete_title, MessageBoxButtons.YesNo)
                == DialogResult.Yes;
        }

        /// <summary>
        /// Runs the confirm user delete view action for the presenter.
        /// </summary>
        bool IUserManagerFormContract.ConfirmUserDelete(string userName)
        {
            return MessageBox.ShowDialog(this, string.Format(Messages.usermanagement_user_delete, userName), Messages.usermanagement_user_delete_title, MessageBoxButtons.YesNo)
                == DialogResult.Yes;
        }

        /// <summary>
        /// Runs the show user deleted view action for the presenter.
        /// </summary>
        void IUserManagerFormContract.ShowUserDeleted()
        {
            MessageBox.ShowDialog(this, Messages.usermanagement_user_deleted);
        }

        /// <summary>
        /// Runs the show user not deleted view action for the presenter.
        /// </summary>
        void IUserManagerFormContract.ShowUserNotDeleted()
        {
            MessageBox.ShowDialog(this, Messages.usermanagement_user_no_deleted);
        }

        /// <summary>
        /// Runs the show data table update failed view action for the presenter.
        /// </summary>
        void IUserManagerFormContract.ShowDataTableUpdateFailed()
        {
            MessageBox.ShowDialog(this, Messages.datatable_update_failed);
        }
    }
}
