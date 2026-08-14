using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
namespace Pflegehaushaltsbuch
{
    /// <summary>
    /// Represents the xml Config component used by the application.
    /// </summary>
    [Serializable]
    [XmlRoot("Config")]
    public class XmlConfig : INotifyPropertyChanged, ICloneable
    {
        public string keyword = string.Empty;
        /// <summary>
        /// Defines the available data Base Types values used by the application.
        /// </summary>
        public enum DataBaseTypes
        {
            None,
            SQL,
            MySQL,
            SQLite
        }
        private string host, port, user, database;
        private bool trustServerCertificate;
        /// <summary>
        /// Creates a new Xml Config instance and initializes the required state.
        /// </summary>
        protected XmlConfig()
        {
            DBType = DataBaseTypes.None;
            Host = "localhost";
            User = "root";
            Database = "Verwahrgeld";
            port = "3306";
            keyword = string.Empty;
            trustServerCertificate = true;
        }
        [XmlIgnore]
        public string Keyword
        {
            get
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return keyword;
                if (CredentialProtector.TryUnprotect(keyword, out string plainKeyword))
                    return plainKeyword;
                keyword = string.Empty;
                return keyword;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    keyword = value;
                    return;
                }
                this.keyword = CredentialProtector.Protect(value);
                FirePropertyChanged("Keyword");
            }
        }
        public DataBaseTypes DBType { get; set; }
        public bool TrustServerCertificate
        {
            get { return trustServerCertificate; }
            set { trustServerCertificate = value; FirePropertyChanged("TrustServerCertificate"); }
        }
        public string User { get { return user; } set { user = value; FirePropertyChanged("User"); } }
        public string Host { get { return host; } set { host = value; FirePropertyChanged("Host"); } }
        public string Database { get { return database; } set { database = value; FirePropertyChanged("Database"); } }
        /// <summary>
        /// Checks whether the modify operation is allowed in the current state.
        /// </summary>
        public bool CanModify()
        {
            try
            {
                using (FileStream fs = File.OpenWrite(GetXmlFilename()))
                {
                    fs.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Gets the xml Filename value from the current application state.
        /// </summary>
        private static string GetXmlFilename()
        {
            if (!Directory.Exists(
                Path.Combine(
                    System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    Application.CompanyName
                )
            ))
            {
                return Path.Combine(
                    System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    Application.ProductName,
                    "config.xml"
                );
            }
            else
            {
                return Path.Combine(
                    System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    Application.ProductName,
                    "config.xml"
                );
            }
        }
        /// <summary>
        /// Loads the xml data required for the current workflow.
        /// </summary>
        public static XmlConfig LoadXml()
        {
            string filename = GetXmlFilename();
            if (File.Exists(filename))
            {
                using (TextReader stream = new StreamReader(filename))
                {
                    XmlSerializer serialize = new XmlSerializer(typeof(XmlConfig));
                    return serialize.Deserialize(stream) as XmlConfig;
                }
            }
            return new XmlConfig();
        }
        /// <summary>
        /// Saves the save data for the current workflow.
        /// </summary>
        public void Save()
        {
            string filename = GetXmlFilename();
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (TextWriter stream = new StreamWriter(filename))
            {
                XmlSerializer serialize = new XmlSerializer(typeof(XmlConfig));
                serialize.Serialize(stream, this);
            }
        }
        /// <summary>
        /// Disconnects the disconnect data source or control from the current workflow.
        /// </summary>
        public static void Disconnect()
        {
            var config = XmlConfig.LoadXml();
            config.DBType = DataBaseTypes.None;
            config.Save();
        }
        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Runs the fire Property Changed operation and updates the related application state.
        /// </summary>
        protected void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Runs the clone operation and updates the related application state.
        /// </summary>
        public object Clone()
        {
            return new XmlConfig
            {
                DBType = DBType,
                Host = Host,
                User = User,
                Database = Database,
                TrustServerCertificate = TrustServerCertificate,
                keyword = keyword
            };
        }
    }
}
