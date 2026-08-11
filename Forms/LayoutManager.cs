using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using LayoutEnum = Pflegehaushaltsbuch.Data.Printing.LayoutEnum;
using Pflegehaushaltsbuch.FormControls;
using Pflegehaushaltsbuch.Forms.Dialoge;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Data.Graphics;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using LayoutControl = Pflegehaushaltsbuch.FormControls.Layout;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Layout Manager window and coordinates its user interface behavior.
    /// </summary>
    public partial class LayoutManager : Form, ILayoutManagerContract
    {
        private readonly LayoutManagerPresenter presenter;
        private readonly Dictionary<LayoutEnum, ILayoutSurface> layoutSurfaces;
        private event Action<ILayoutSurface> layoutVisibleChanged;
        private event Action<RectangleF> rectangleChanged;
        private event Action<GraphicsItem[]> selectedItemsChanged;

        /// <summary>
        /// Creates a new LayoutManager view.
        /// </summary>
        public LayoutManager(SqlSession session)
        {
            InitializeComponent();
            layoutSurfaces = CreateLayoutSurfaces();
            WireLayoutEvents();
            Session = session;
            presenter = new LayoutManagerPresenter(this, session);
            presenter.InitializeControls();
        }

        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            if (Program.DesignMode)
                return;

            ApplyCurrentUserRights();
            presenter.Enter();
        }

        /// <summary>
        /// Runs the layout manager_activated action.
        /// </summary>
        private void LayoutManager_Activated(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Runs the back button_click action.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            presenter.Back();
        }

        /// <summary>
        /// Runs the size button_click action.
        /// </summary>
        private void sizeButton_Click(object sender, EventArgs e)
        {
            presenter.Size();
        }

        /// <summary>
        /// Runs the image button_click action.
        /// </summary>
        private void imageButton_Click(object sender, EventArgs e)
        {
            presenter.Image();
        }

        /// <summary>
        /// Runs the text button_click action.
        /// </summary>
        private void textButton_Click(object sender, EventArgs e)
        {
            presenter.Text();
        }

        /// <summary>
        /// Runs the line button_click action.
        /// </summary>
        private void lineButton_Click(object sender, EventArgs e)
        {
            presenter.Line();
        }

        /// <summary>
        /// Runs the arc button_click action.
        /// </summary>
        private void arcButton_Click(object sender, EventArgs e)
        {
            presenter.Arc();
        }

        /// <summary>
        /// Runs the join button_click action.
        /// </summary>
        private void joinButton_Click(object sender, EventArgs e)
        {
            presenter.Join();
        }

        /// <summary>
        /// Runs the seperat button_click action.
        /// </summary>
        private void seperatButton_Click(object sender, EventArgs e)
        {
            presenter.Seperat();
        }

        /// <summary>
        /// Runs the table button_click action.
        /// </summary>
        private void tableButton_Click(object sender, EventArgs e)
        {
            presenter.Table();
        }

        /// <summary>
        /// Runs the save button_click action.
        /// </summary>
        private async void saveButton_Click(object sender, EventArgs e)
        {
            await presenter.SaveAsync();
        }

        /// <summary>
        /// Runs the import button_click action.
        /// </summary>
        private void importButton_Click(object sender, EventArgs e)
        {
            presenter.Import();
        }

        /// <summary>
        /// Runs the export button_click action.
        /// </summary>
        private void exportButton_Click(object sender, EventArgs e)
        {
            presenter.Export();
        }

        /// <summary>
        /// Runs the reset button_click action.
        /// </summary>
        private void resetButton_Click(object sender, EventArgs e)
        {
            presenter.ResetWithDialog();
        }

        /// <summary>
        /// Runs the font combo box changed_selected index changed action.
        /// </summary>
        private void fontComboBoxChanged_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            presenter.FontSelectionChanged(control != null && control.Focused);
        }

        /// <summary>
        /// Runs the font buttons changed_click action.
        /// </summary>
        private void fontButtonsChanged_Click(object sender, EventArgs e)
        {
            presenter.FontStyleChanged();
        }

        /// <summary>
        /// Runs the horizontal_click action.
        /// </summary>
        private void horizontal_Click(object sender, EventArgs e)
        {
            if (sender == leftText)
                presenter.Horizontal(StringAlignment.Near);
            else if (sender == centerText)
                presenter.Horizontal(StringAlignment.Center);
            else if (sender == rightText)
                presenter.Horizontal(StringAlignment.Far);
        }

        /// <summary>
        /// Runs the vertical_click action.
        /// </summary>
        private void vertical_Click(object sender, EventArgs e)
        {
            if (sender == topText)
                presenter.Vertical(StringAlignment.Near);
            else if (sender == verticallCenter)
                presenter.Vertical(StringAlignment.Center);
            else if (sender == bottomText)
                presenter.Vertical(StringAlignment.Far);
        }

        /// <summary>
        /// Runs the style box_selected index changed action.
        /// </summary>
        private void styleBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            presenter.StyleChanged(styleBox.Focused);
        }

        /// <summary>
        /// Runs the border box_text changed action.
        /// </summary>
        private void borderBox_TextChanged(object sender, EventArgs e)
        {
            presenter.BorderWidthChanged(borderBox.Text, borderBox.Focused);
        }

        /// <summary>
        /// Runs the x text_text changed action.
        /// </summary>
        private void xText_TextChanged(object sender, EventArgs e)
        {
            presenter.RectangleValueChanged(RectangleProperty.X, xText.Text, xText.Focused);
        }

        /// <summary>
        /// Runs the y text_text changed action.
        /// </summary>
        private void yText_TextChanged(object sender, EventArgs e)
        {
            presenter.RectangleValueChanged(RectangleProperty.Y, yText.Text, yText.Focused);
        }

        /// <summary>
        /// Runs the width box_text changed action.
        /// </summary>
        private void widthBox_TextChanged(object sender, EventArgs e)
        {
            presenter.RectangleValueChanged(RectangleProperty.Width, widthBox.Text, widthBox.Focused);
        }

        /// <summary>
        /// Runs the height box_text changed action.
        /// </summary>
        private void heightBox_TextChanged(object sender, EventArgs e)
        {
            presenter.RectangleValueChanged(RectangleProperty.Height, heightBox.Text, heightBox.Focused);
        }

        /// <summary>
        /// Runs the border color button_click action.
        /// </summary>
        private void borderColorButton_Click(object sender, EventArgs e)
        {
            presenter.BorderColor();
        }

        /// <summary>
        /// Runs the back color button_click action.
        /// </summary>
        private void backColorButton_Click(object sender, EventArgs e)
        {
            presenter.BackColor();
        }

        /// <summary>
        /// Runs the fore color button_click action.
        /// </summary>
        private void foreColorButton_Click(object sender, EventArgs e)
        {
            presenter.ForeColor();
        }

        /// <summary>
        /// Runs the edit button_click action.
        /// </summary>
        private void editButton_Click(object sender, EventArgs e)
        {
            presenter.Edit();
        }

        /// <summary>
        /// Runs the page box1_selected index changed action.
        /// </summary>
        private void pageBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            presenter.PrintPageChanged(pageBox1.Focused);
        }

        /// <summary>
        /// Runs the page box_selected index changed action.
        /// </summary>
        private void pageBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            presenter.PageChanged();
        }

        /// <summary>
        /// Runs the calculate text height box_click action.
        /// </summary>
        private void calculateTextHeightBox_Click(object sender, EventArgs e)
        {
            presenter.CalculateTextHeight();
        }

        /// <summary>
        /// Runs the copy button_click action.
        /// </summary>
        private void copyButton_Click(object sender, EventArgs e)
        {
            presenter.Copy();
        }

        /// <summary>
        /// Runs the insert button_click action.
        /// </summary>
        private void insertButton_Click(object sender, EventArgs e)
        {
            presenter.Insert();
        }

        /// <summary>
        /// Runs the print button_click action.
        /// </summary>
        private void printButton_Click(object sender, EventArgs e)
        {
            presenter.Print();
        }

        /// <summary>
        /// Runs the copy tool strip menu item_click action.
        /// </summary>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.Copy();
        }

        /// <summary>
        /// Runs the insert tool strip menu item_click action.
        /// </summary>
        private void InsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.Insert();
        }

        /// <summary>
        /// Runs the edit tool strip menu item_click action.
        /// </summary>
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.EditControl();
        }

        /// <summary>
        /// Runs the edit control action.
        /// </summary>
        public void EditControl()
        {
            presenter.EditControl();
        }

        /// <summary>
        /// Runs the page settings button_click action.
        /// </summary>
        private void pageSettingsButton_Click(object sender, EventArgs e)
        {
            presenter.PageSettings();
        }

        /// <summary>
        /// Runs the cash button_click action.
        /// </summary>
        private void cashButton_Click(object sender, EventArgs e)
        {
            presenter.SelectLayout(LayoutEnum.cash);
        }

        /// <summary>
        /// Runs the bank button_click action.
        /// </summary>
        private void bankButton_Click(object sender, EventArgs e)
        {
            presenter.SelectLayout(LayoutEnum.bank);
        }

        /// <summary>
        /// Runs the office cash button_click action.
        /// </summary>
        private void officeCashButton_Click(object sender, EventArgs e)
        {
            presenter.SelectLayout(LayoutEnum.officecash);
        }

        /// <summary>
        /// Runs the statement button_click action.
        /// </summary>
        private void statementButton_Click(object sender, EventArgs e)
        {
            presenter.SelectLayout(LayoutEnum.accounts);
        }

        /// <summary>
        /// Runs the client button_click action.
        /// </summary>
        private void clientButton_Click(object sender, EventArgs e)
        {
            presenter.SelectLayout(LayoutEnum.clients);
        }

        /// <summary>
        /// Runs the advisor button_click action.
        /// </summary>
        private void advisorButton_Click(object sender, EventArgs e)
        {
            presenter.SelectLayout(LayoutEnum.advisors);
        }

        /// <summary>
        /// Runs the assistant button_click action.
        /// </summary>
        private void assistantButton_Click(object sender, EventArgs e)
        {
            presenter.SelectLayout(LayoutEnum.employees);
        }

        /// <summary>
        /// Runs the cash audit button_click action.
        /// </summary>
        private void cashAuditButton_Click(object sender, EventArgs e)
        {
            presenter.SelectLayout(LayoutEnum.cashaudit);
        }

        /// <summary>
        /// Runs the quittance button_click action.
        /// </summary>
        private void quittanceButton_Click(object sender, EventArgs e)
        {
            presenter.SelectLayout(LayoutEnum.quittance);
        }

        /// <summary>
        /// Provides the layouts value for the presenter.
        /// </summary>
        event Action<ILayoutSurface> ILayoutManagerContract.LayoutVisibleChanged
        {
            add { layoutVisibleChanged += value; }
            remove { layoutVisibleChanged -= value; }
        }

        event Action<RectangleF> ILayoutManagerContract.RectangleChanged
        {
            add { rectangleChanged += value; }
            remove { rectangleChanged -= value; }
        }

        event Action<GraphicsItem[]> ILayoutManagerContract.SelectedItemsChanged
        {
            add { selectedItemsChanged += value; }
            remove { selectedItemsChanged -= value; }
        }

        IEnumerable<ILayoutSurface> ILayoutManagerContract.Layouts
        {
            get { return layoutSurfaces.Values; }
        }

        ILayoutSurface ILayoutManagerContract.GetLayout(LayoutEnum layoutType)
        {
            return layoutSurfaces[layoutType];
        }

        int ILayoutManagerContract.SelectedPageIndex
        {
            get { return pageBox.SelectedIndex; }
            set { pageBox.SelectedIndex = value; }
        }

        int ILayoutManagerContract.SelectedPrintPageIndex
        {
            get { return pageBox1.SelectedIndex; }
            set { pageBox1.SelectedIndex = value; }
        }

        string ILayoutManagerContract.SelectedFontFamily
        {
            get { return Convert.ToString(fontFamilyBox.SelectedItem); }
            set { fontFamilyBox.SelectedItem = value; }
        }

        int ILayoutManagerContract.SelectedFontSize
        {
            get { return Convert.ToInt32(fontSizeBox.SelectedItem); }
            set { fontSizeBox.SelectedItem = value; }
        }

        bool ILayoutManagerContract.BoldChecked
        {
            get { return boldButton.Checked; }
            set { boldButton.Checked = value; }
        }

        bool ILayoutManagerContract.ItalicChecked
        {
            get { return italicButton.Checked; }
            set { italicButton.Checked = value; }
        }

        bool ILayoutManagerContract.UnderlineChecked
        {
            get { return underlineButton.Checked; }
            set { underlineButton.Checked = value; }
        }

        bool ILayoutManagerContract.StrikeOutChecked
        {
            get { return strikeOutButton.Checked; }
            set { strikeOutButton.Checked = value; }
        }

        DashStyle ILayoutManagerContract.SelectedStyle
        {
            get { return (DashStyle)styleBox.SelectedItem; }
            set { styleBox.SelectedItem = value; }
        }

        Color ILayoutManagerContract.SelectedForeColor
        {
            get { return foreColorButton.BackColor; }
            set { foreColorButton.BackColor = value; }
        }

        Color ILayoutManagerContract.SelectedBackColor
        {
            get { return backColorButton.BackColor; }
            set { backColorButton.BackColor = value; }
        }

        Color ILayoutManagerContract.SelectedBorderColor
        {
            get { return borderColorButton.BackColor; }
            set { borderColorButton.BackColor = value; }
        }

        /// <summary>
        /// Provides the calculate text height checked value for the presenter.
        /// </summary>
        bool ILayoutManagerContract.CalculateTextHeightChecked
        {
            get { return calculateTextHeightBox.Checked; }
        }

        void ILayoutManagerContract.BindStyles(IEnumerable<DashStyle> styles)
        {
            styleBox.DataSource = styles.ToArray();
        }

        void ILayoutManagerContract.BindFontFamilies(IEnumerable<string> fontFamilies)
        {
            fontFamilyBox.Items.Clear();
            fontFamilyBox.Items.AddRange(fontFamilies.Cast<object>().ToArray());
        }

        void ILayoutManagerContract.BindFontSizes(IEnumerable<int> fontSizes)
        {
            fontSizeBox.Items.Clear();
            fontSizeBox.Items.AddRange(fontSizes.Cast<object>().ToArray());
        }

        void ILayoutManagerContract.SetRectangleValues(RectangleF rectangle)
        {
            xText.Text = rectangle.X.ToString();
            yText.Text = rectangle.Y.ToString();
            widthBox.Text = rectangle.Width.ToString();
            heightBox.Text = rectangle.Height.ToString();
        }

        void ILayoutManagerContract.SetBorderWidthText(string value)
        {
            borderBox.Text = value;
        }

        void ILayoutManagerContract.SetHorizontalAlignment(StringAlignment alignment)
        {
            leftText.Checked = alignment == StringAlignment.Near;
            centerText.Checked = alignment == StringAlignment.Center;
            rightText.Checked = alignment == StringAlignment.Far;
        }

        void ILayoutManagerContract.SetVerticalAlignment(StringAlignment alignment)
        {
            topText.Checked = alignment == StringAlignment.Near;
            verticallCenter.Checked = alignment == StringAlignment.Center;
            bottomText.Checked = alignment == StringAlignment.Far;
        }

        /// <summary>
        /// Runs the set save enabled view action for the presenter.
        /// </summary>
        void ILayoutManagerContract.SetSaveEnabled(bool enabled)
        {
            saveButton.Enabled = enabled;
        }

        /// <summary>
        /// Runs the show main form view action for the presenter.
        /// </summary>
        void ILayoutManagerContract.ShowMainForm()
        {
            ShowFormEvent(Enums.Forms.Main);
        }

        /// <summary>
        /// Runs the show administration form view action for the presenter.
        /// </summary>
        void ILayoutManagerContract.ShowAdministrationForm()
        {
            ShowFormEvent(Enums.Forms.Administration);
        }

        /// <summary>
        /// Runs the show database not available view action for the presenter.
        /// </summary>
        void ILayoutManagerContract.ShowDatabaseNotAvailable()
        {
            MessageBox.ShowDialog(this, Messages.database_not_available);
        }

        /// <summary>
        /// Runs the show layout saved view action for the presenter.
        /// </summary>
        void ILayoutManagerContract.ShowLayoutSaved()
        {
            MessageBox.ShowDialog(this, Messages.layout_saved);
        }

        /// <summary>
        /// Runs the show layouts exported view action for the presenter.
        /// </summary>
        void ILayoutManagerContract.ShowLayoutsExported(string path)
        {
            MessageBox.ShowDialog(this, string.Format(Messages.layouts_exported, path));
        }

        /// <summary>
        /// Runs the show folder dialog view action for the presenter.
        /// </summary>
        bool ILayoutManagerContract.ShowFolderDialog(out string selectedPath)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    selectedPath = string.Empty;
                    return false;
                }

                selectedPath = dialog.SelectedPath;
                return true;
            }
        }

        /// <summary>
        /// Runs the show reset layouts dialog view action for the presenter.
        /// </summary>
        void ILayoutManagerContract.ShowResetLayoutsDialog(SqlSession session)
        {
            using (ResetLayoutsDialog resetLayouts = new ResetLayoutsDialog(session))
            {
                resetLayouts.ShowDialog(this);
            }
        }

        /// <summary>
        /// Runs the show color dialog view action for the presenter.
        /// </summary>
        bool ILayoutManagerContract.ShowColorDialog(out Color color)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog(this) != DialogResult.OK)
                {
                    color = Color.Empty;
                    return false;
                }

                color = colorDialog.Color;
                return true;
            }
        }

        /// <summary>
        /// Runs the show edit data table dialog view action for the presenter.
        /// </summary>
        bool ILayoutManagerContract.ShowEditDataTableDialog(DataTable tableDesign, out DataTable updatedTableDesign)
        {
            using (EditDataTableDialog editDataTableForm = new EditDataTableDialog(tableDesign))
            {
                if (editDataTableForm.ShowDialog(this) != DialogResult.OK)
                {
                    updatedTableDesign = null;
                    return false;
                }

                updatedTableDesign = editDataTableForm.TableDesign;
                return true;
            }
        }

        /// <summary>
        /// Runs the show image file dialog view action for the presenter.
        /// </summary>
        bool ILayoutManagerContract.ShowImageFileDialog(out string fileName)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                if (fileDialog.ShowDialog(this) != DialogResult.OK)
                {
                    fileName = string.Empty;
                    return false;
                }

                fileName = fileDialog.FileName;
                return true;
            }
        }

        /// <summary>
        /// Runs the show edit text dialog view action for the presenter.
        /// </summary>
        bool ILayoutManagerContract.ShowEditTextDialog(SqlSession session, string text, out string content)
        {
            using (EditTextDialog form = new EditTextDialog(session, text))
            {
                if (form.ShowDialog(this) != DialogResult.OK)
                {
                    content = string.Empty;
                    return false;
                }

                content = form.Content;
                return true;
            }
        }

        /// <summary>
        /// Runs the show page settings view action for the presenter.
        /// </summary>
        void ILayoutManagerContract.ShowPageSettings(SqlSession session, ILayoutSurface layout)
        {
            LayoutSurface surface = layout as LayoutSurface;
            if (surface == null)
                return;

            using (PageSettingsForm pageSettingsForm = new PageSettingsForm(session, layout))
            {
                pageSettingsForm.ShowDialog(this);
            }
        }

        /// <summary>
        /// Runs the print layout view action for the presenter.
        /// </summary>
        void ILayoutManagerContract.PrintLayout(SqlSession session, Data.Printing.LayoutEnum layoutType)
        {
            Data.Print.PrintBase printing = new Data.Print.PrintBase(session, layoutType);
            printing.Print("Layout", "Layout", this);
        }

        private Dictionary<LayoutEnum, ILayoutSurface> CreateLayoutSurfaces()
        {
            return new Dictionary<LayoutEnum, ILayoutSurface>
            {
                { LayoutEnum.cash, new LayoutSurface(cash) },
                { LayoutEnum.bank, new LayoutSurface(bank) },
                { LayoutEnum.clients, new LayoutSurface(clients) },
                { LayoutEnum.accounts, new LayoutSurface(account) },
                { LayoutEnum.advisors, new LayoutSurface(advisor) },
                { LayoutEnum.employees, new LayoutSurface(assistants) },
                { LayoutEnum.cashaudit, new LayoutSurface(cashaudit) },
                { LayoutEnum.quittance, new LayoutSurface(quittance) },
                { LayoutEnum.officecash, new LayoutSurface(officeCash) }
            };
        }

        private void WireLayoutEvents()
        {
            foreach (KeyValuePair<LayoutEnum, ILayoutSurface> pair in layoutSurfaces)
            {
                LayoutSurface surface = (LayoutSurface)pair.Value;
                surface.Layout.VisibleChanged += delegate { layoutVisibleChanged?.Invoke(surface); };
            }

            LayoutControl.OnChangeRectangle += rectangle => rectangleChanged?.Invoke(rectangle);
            LayoutControl.OnUpdateSelectedItem += items => selectedItemsChanged?.Invoke(items);
        }

        private sealed class LayoutSurface : ILayoutSurface
        {
            public LayoutSurface(LayoutControl layout)
            {
                Layout = layout;
            }

            public LayoutControl Layout { get; private set; }
            public SqlSession Session { get { return Layout.Session; } set { Layout.Session = value; } }
            public DocumentLayer Document { get { return Layout.Document; } set { Layout.Document = value; } }
            public IEnumerable<GraphicsItem> SelectedItems { get { return Layout.SelectedItems; } }
            public Color Fore { get { return Layout.Fore; } set { Layout.Fore = value; } }
            public Color Back { get { return Layout.Back; } set { Layout.Back = value; } }
            public Color Border { get { return Layout.Border; } set { Layout.Border = value; } }
            public StringAlignment Horizontal { get { return Layout.Horizontal; } set { Layout.Horizontal = value; } }
            public StringAlignment Vertical { get { return Layout.Vertical; } set { Layout.Vertical = value; } }
            public int PrintOnPage { get { return Layout.PrintOnPage; } set { Layout.PrintOnPage = value; } }
            public float BorderWidth { get { return Layout.BorderWidth; } set { Layout.BorderWidth = value; } }
            public bool CalculateTextHeight { get { return Layout.CalculateTextHeight; } set { Layout.CalculateTextHeight = value; } }
            public Font CurrentFont { get { return Layout.CurrentFont; } set { Layout.CurrentFont = value; } }
            public int CurrentPage { get { return Layout.CurrentPage; } set { Layout.CurrentPage = value; } }
            public bool Visible { get { return Layout.Visible; } set { Layout.Visible = value; } }

            public void ClearSelectedItems()
            {
                Layout.SelectedItems.Clear();
            }

            public void ChangeDrawMode(LayoutDrawElement drawElement)
            {
                Layout.ChangeDrawMode(MapDrawElement(drawElement));
            }

            public void RefreshDocumentSize()
            {
                Layout.Size = Layout.Document.GetSize();
            }

            public void Invalidate()
            {
                Layout.Invalidate();
            }

            public void CopyToClipboard()
            {
                Layout.Copy2ClipBoard();
            }

            public void InsertFromClipboard()
            {
                Layout.InsertByClipBoard();
            }

            private static LayoutControl.DrawElement MapDrawElement(LayoutDrawElement drawElement)
            {
                if (drawElement == LayoutDrawElement.Size)
                    return LayoutControl.DrawElement.Size;
                if (drawElement == LayoutDrawElement.Image)
                    return LayoutControl.DrawElement.Image;
                if (drawElement == LayoutDrawElement.Text)
                    return LayoutControl.DrawElement.Text;
                if (drawElement == LayoutDrawElement.Line)
                    return LayoutControl.DrawElement.Line;
                if (drawElement == LayoutDrawElement.Arc)
                    return LayoutControl.DrawElement.Arc;
                if (drawElement == LayoutDrawElement.Join)
                    return LayoutControl.DrawElement.Join;
                if (drawElement == LayoutDrawElement.Table)
                    return LayoutControl.DrawElement.Table;

                return LayoutControl.DrawElement.Select;
            }
        }
    }
}
