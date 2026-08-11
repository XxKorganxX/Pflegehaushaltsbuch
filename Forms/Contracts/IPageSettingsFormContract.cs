using Pflegehaushaltsbuch.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IPageSettingsFormContract
    {
        FontFamily SelectedFontFamily { get; }
        int SelectedFontSize { get; }
        bool BoldChecked { get; }
        bool ItalicChecked { get; }
        bool StrikeChecked { get; }
        bool UnderlineChecked { get; }
        PaperSize SelectedPaperSize { get; }
        void BindFontFamilies(IEnumerable<FontFamily> fontFamilies);
        void BindFontSizes(IEnumerable<int> fontSizes);
        void BindPaperSizes(IEnumerable<PaperSize> paperSizes);
        void BindDocumentSettings(DocumentLayer document);
        void SetSelectedFont(Font font);
        void SetHorizontalSelection(StringAlignment alignment);
        void SetVerticalSelection(StringAlignment alignment);
        void CloseView();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
