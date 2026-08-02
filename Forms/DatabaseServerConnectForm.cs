using Pflegehaushaltsbuch.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Database Server Connect Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class DatabaseServerConnectForm : Pflegehaushaltsbuch.FormControls.Form, IDatabaseServerConnectFormContract
    {
        private readonly DatabaseServerConnectFormPresenter presenter;


        XmlConfig config;
        /// <summary>
        /// Creates a new Database Server Connect Form instance and initializes the required state.
        /// </summary>
        public DatabaseServerConnectForm(XmlConfig config)
        {
            this.config = config;
            InitializeComponent();
            presenter = new DatabaseServerConnectFormPresenter(this);
            ConfigureDatabaseTypeButtons();
            hostBox.DataBindings.Add("Text", config, "Host");
            userNameBox.DataBindings.Add("Text", config, "User");
            passwordBox.DataBindings.Add("Text", config, "Keyword");
        }
        /// <summary>
        /// Runs the configure Database Type Buttons operation and updates the related application state.
        /// </summary>
        private void ConfigureDatabaseTypeButtons()
        {
            sqlButton.Image = LoadDatabaseTypeIcon(@"Resources\microsoft-sql-server-logo.png");
            mySqlButton.Image = LoadDatabaseTypeIcon(@"Resources\logo-mysql-170x115.png");
            sqliteButton.Image = LoadDatabaseTypeIcon(@"Resources\sqlite.png");
            UpdateDatabaseTypeButtons();
        }
        /// <summary>
        /// Updates the database Type Buttons data and refreshes the related application state.
        /// </summary>
        private void UpdateDatabaseTypeButtons()
        {
            sqlButton.Checked = config.DBType == XmlConfig.DataBaseTypes.SQL;
            mySqlButton.Checked = config.DBType == XmlConfig.DataBaseTypes.MySQL;
            sqliteButton.Checked = config.DBType == XmlConfig.DataBaseTypes.SQLite;
        }
        /// <summary>
        /// Loads the database Type Icon data required for the current workflow.
        /// </summary>
        private Image LoadDatabaseTypeIcon(string iconPath)
        {
            const int iconHeight = 34;
            using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(iconPath)))
            using (Image image = Image.FromStream(stream))
            {
                float scale = iconHeight / (float)image.Height;
                int width = Math.Max(1, (int)Math.Round(image.Width * scale));
                Bitmap bitmap = new Bitmap(width, iconHeight, PixelFormat.Format32bppPArgb);
                bitmap.SetResolution(96, 96);
                using (Graphics graphics = Graphics.FromImage(bitmap))
                using (ImageAttributes imageAttributes = CreateStrongIconAttributes())
                {
                    graphics.Clear(Color.Transparent);
                    graphics.CompositingMode = CompositingMode.SourceOver;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.DrawImage(image,
                        new Rectangle(0, 0, width, iconHeight),
                        0,
                        0,
                        image.Width,
                        image.Height,
                        GraphicsUnit.Pixel,
                        imageAttributes);
                }
                return bitmap;
            }
        }
        /// <summary>
        /// Creates the strong Icon Attributes data or user interface element for the current workflow.
        /// </summary>
        private ImageAttributes CreateStrongIconAttributes()
        {
            const float contrast = 1.18f;
            const float offset = (1f - contrast) / 2f;
            ColorMatrix matrix = new ColorMatrix(new float[][]
            {
                new float[] { contrast, 0f, 0f, 0f, 0f },
                new float[] { 0f, contrast, 0f, 0f, 0f },
                new float[] { 0f, 0f, contrast, 0f, 0f },
                new float[] { 0f, 0f, 0f, 1f, 0f },
                new float[] { offset, offset, offset, 0f, 1f }
            });
            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            imageAttributes.SetWrapMode(WrapMode.TileFlipXY);
            return imageAttributes;
        }
        /// <summary>
        /// Handles the click event for database Type Button and updates the related state.
        /// </summary>
        private void databaseTypeButton_Click(object sender, EventArgs e)
        {
            XmlConfig.DataBaseTypes type = (XmlConfig.DataBaseTypes)((Control)sender).Tag;
            config.DBType = type;
            UpdateDatabaseTypeButtons();
            hostLabel.Visible = hostBox.Visible = type != XmlConfig.DataBaseTypes.SQLite;
            if (type == XmlConfig.DataBaseTypes.SQL)
            {
                config.Host = @"localhost\SQLEXPRESS";
                config.User = "sa";
            }
            else if (type == XmlConfig.DataBaseTypes.MySQL)
            {
                config.Host = "localhost";
                config.User = "root";
            }
            else if (type == XmlConfig.DataBaseTypes.SQLite)
            {
                config.Host = string.Empty;
            }
        }
        /// <summary>
        /// Handles the click event for connect Button and updates the related state.
        /// </summary>
        private async void connectButton_Click(object sender, EventArgs e)
        {
            SQLBase sql = null;
            if (config.DBType == XmlConfig.DataBaseTypes.SQL)
                sql = new SQL();
            else if (config.DBType == XmlConfig.DataBaseTypes.MySQL)
                sql = new MySQL();
            else if (config.DBType == XmlConfig.DataBaseTypes.SQLite)
                sql = new SQLITE();

            if (sql == null)
            {
                MessageBox.ShowDialog(this, Messages.database_connected_failed);
                return;
            }

            if (sql != null && (config.DBType == XmlConfig.DataBaseTypes.SQLite || await sql.TestConnectionAsync(config.Host, config.Database, config.User, config.Keyword)))
                DialogResult = DialogResult.OK;

            sql.Dispose();
        }
        /// <summary>
        /// Handles the click event for close Button and updates the related state.
        /// </summary>
        private void closeButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
