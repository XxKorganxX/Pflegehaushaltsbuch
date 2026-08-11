using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Data.Graphics;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using LayoutEnum = Pflegehaushaltsbuch.Data.Printing.LayoutEnum;

namespace Pflegehaushaltsbuch.Forms
{
    public interface ILayoutManagerContract
    {
        event Action<ILayoutSurface> LayoutVisibleChanged;
        event Action<RectangleF> RectangleChanged;
        event Action<GraphicsItem[]> SelectedItemsChanged;
        IEnumerable<ILayoutSurface> Layouts { get; }
        ILayoutSurface GetLayout(LayoutEnum layoutType);
        int SelectedPageIndex { get; set; }
        int SelectedPrintPageIndex { get; set; }
        string SelectedFontFamily { get; set; }
        int SelectedFontSize { get; set; }
        bool BoldChecked { get; set; }
        bool ItalicChecked { get; set; }
        bool UnderlineChecked { get; set; }
        bool StrikeOutChecked { get; set; }
        DashStyle SelectedStyle { get; set; }
        Color SelectedForeColor { get; set; }
        Color SelectedBackColor { get; set; }
        Color SelectedBorderColor { get; set; }
        bool CalculateTextHeightChecked { get; }
        void BindStyles(IEnumerable<DashStyle> styles);
        void BindFontFamilies(IEnumerable<string> fontFamilies);
        void BindFontSizes(IEnumerable<int> fontSizes);
        void SetRectangleValues(RectangleF rectangle);
        void SetBorderWidthText(string value);
        void SetHorizontalAlignment(StringAlignment alignment);
        void SetVerticalAlignment(StringAlignment alignment);
        void SetSaveEnabled(bool enabled);
        void ShowMainForm();
        void ShowAdministrationForm();
        void ShowDatabaseNotAvailable();
        void ShowLayoutSaved();
        void ShowLayoutsExported(string path);
        bool ShowFolderDialog(out string selectedPath);
        void ShowResetLayoutsDialog(SqlSession session);
        bool ShowColorDialog(out Color color);
        bool ShowEditDataTableDialog(DataTable tableDesign, out DataTable updatedTableDesign);
        bool ShowImageFileDialog(out string fileName);
        bool ShowEditTextDialog(SqlSession session, string text, out string content);
        void ShowPageSettings(SqlSession session, ILayoutSurface layout);
        void PrintLayout(SqlSession session, LayoutEnum layoutType);
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }

    public interface ILayoutSurface
    {
        SqlSession Session { get; set; }
        DocumentLayer Document { get; set; }
        IEnumerable<GraphicsItem> SelectedItems { get; }
        Color Fore { get; set; }
        Color Back { get; set; }
        Color Border { get; set; }
        StringAlignment Horizontal { get; set; }
        StringAlignment Vertical { get; set; }
        int PrintOnPage { get; set; }
        float BorderWidth { get; set; }
        bool CalculateTextHeight { get; set; }
        Font CurrentFont { get; set; }
        int CurrentPage { get; set; }
        bool Visible { get; set; }
        void ClearSelectedItems();
        void ChangeDrawMode(LayoutDrawElement drawElement);
        void RefreshDocumentSize();
        void Invalidate();
        void CopyToClipboard();
        void InsertFromClipboard();
    }

    public enum LayoutDrawElement
    {
        Size,
        Image,
        Text,
        Line,
        Arc,
        Join,
        Table
    }
}
