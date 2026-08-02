namespace Pflegehaushaltsbuch.Forms
{
    public interface ILayoutManagerContract
    {
        void OnUserRights();
        void UpdateLayout();
        void Back();
        void Size();
        void Image();
        void Text();
        void Line();
        void Arc();
        void Join();
        void Seperat();
        void Table();
        void Save();
        void Import();
        void Export();
        void Reset();
        void UpdateFont();
        void UpdateFonSelectedItems();
        void FontButtonsChanged();
        void Horizontal();
        void Vertical();
        void BorderColor();
        void BackColor();
        void ForeColor();
        void Edit();
        void CalculateTextHeight();
        void Copy();
        void Insert();
        void Print();
        void CopyToolStripMenuItem();
        void InsertToolStripMenuItem();
        void EditToolStripMenuItem();
        void EditControl();
        void PageSettings();
        void Cash();
        void Bank();
        void OfficeCash();
        void Statement();
        void Client();
        void Advisor();
        void Assistant();
        void CashAudit();
        void Quittance();
        void LayoutsOff();
    }
}
