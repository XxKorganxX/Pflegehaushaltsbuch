using Pflegehaushaltsbuch.Databases;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class DatabaseConnectionFormPresenter
    {
        private readonly SqlSession session;
        private readonly XmlConfig config;

        public DatabaseConnectionFormPresenter(IDatabaseConnectionFormContract view, SqlSession session, XmlConfig config)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
            this.session = session;
            this.config = config;
        }

        protected IDatabaseConnectionFormContract View { get; private set; }

        public virtual void Initialize()
        {
            View.BindConfig(config);
            ConfigureDatabaseTypeButtons();
        }

        public virtual void ConfigureDatabaseTypeButtons()
        {
            View.SetDatabaseTypeIcons(
                LoadDatabaseTypeIcon(@"Resources\microsoft-sql-server-logo.png"),
                LoadDatabaseTypeIcon(@"Resources\logo-mysql-170x115.png"),
                LoadDatabaseTypeIcon(@"Resources\sqlite.png"));
            UpdateDatabaseTypeButtons();
        }

        public virtual void UpdateDatabaseTypeButtons()
        {
            View.SetDatabaseTypeButtons(
                config.DBType == XmlConfig.DataBaseTypes.SQL,
                config.DBType == XmlConfig.DataBaseTypes.MySQL,
                config.DBType == XmlConfig.DataBaseTypes.SQLite);
            View.SetHostVisible(config.DBType != XmlConfig.DataBaseTypes.SQLite);
            View.SetTrustServerCertificateVisible(config.DBType == XmlConfig.DataBaseTypes.SQL);
        }

        public virtual void DatabaseType(XmlConfig.DataBaseTypes type)
        {
            config.DBType = type;
            UpdateDatabaseTypeButtons();

            if (type == XmlConfig.DataBaseTypes.SQL)
            {
                config.Host = @"localhost\SQLEXPRESS";
                config.User = "";
                config.Keyword = "";
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

        public virtual async Task ConnectAsync()
        {
            SQLBase sql = CreateSqlProviderOrNull(config);
            if (sql == null)
            {
                View.ShowConnectionFailed();
                return;
            }

            using (sql)
            {
                if (config.DBType == XmlConfig.DataBaseTypes.SQLite ||
                    await sql.TestConnectionAsync(config.Host, config.Database, config.User, config.Keyword))
                {
                    View.AcceptDialog();
                }
            }
        }

        public virtual void Close()
        {
            View.CancelDialog();
        }

        private static SQLBase CreateSqlProviderOrNull(XmlConfig config)
        {
            XmlConfig.DataBaseTypes dbType = config.DBType;
            if (dbType == XmlConfig.DataBaseTypes.SQL)
                return new SQL { TrustServerCertificate = config.TrustServerCertificate };
            if (dbType == XmlConfig.DataBaseTypes.MySQL)
                return new MySQL();
            if (dbType == XmlConfig.DataBaseTypes.SQLite)
                return new SQLITE();

            return null;
        }

        private static Image LoadDatabaseTypeIcon(string iconPath)
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

        private static ImageAttributes CreateStrongIconAttributes()
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
    }
}
