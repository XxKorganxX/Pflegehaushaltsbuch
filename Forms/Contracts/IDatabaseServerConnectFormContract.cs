using System.Drawing;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IDatabaseServerConnectFormContract
    {
        void BindConfig(XmlConfig config);
        void SetDatabaseTypeIcons(Image sqlIcon, Image mySqlIcon, Image sqliteIcon);
        void SetDatabaseTypeButtons(bool sqlChecked, bool mySqlChecked, bool sqliteChecked);
        void SetHostVisible(bool visible);
        void AcceptDialog();
        void CancelDialog();
        void ShowConnectionFailed();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
