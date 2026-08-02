using Pflegehaushaltsbuch.Data.Graphics;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.FormControls;
using Pflegehaushaltsbuch.Forms.Dialoge;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Layout Manager window and coordinates its user interface behavior.
    /// </summary>
    public partial class LayoutManager : Pflegehaushaltsbuch.FormControls.Form, ILayoutManagerContract
    {
        private readonly LayoutManagerPresenter presenter;

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms formEnum, SQLBase sql);
        public event OnShowForm ShowForm;
        /// <summary>
        /// Handles the change Draw Mode Delegate lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnChangeDrawModeDelegate(Layout.DrawElement drawElement);
        public static event OnChangeDrawModeDelegate OnChangeDrawMode;
        private GraphicsItem[] SelectedItems = new GraphicsItem[0];
        Layout CurrentLayout = null;
        Pflegehaushaltsbuch.Data.Printing.LayoutEnum CurrentLayoutType = Data.Printing.LayoutEnum.cash;
        /// <summary>
        /// Handles the user Rights lifecycle step and applies the related control behavior.
        /// </summary>
        public void OnUserRights(int access, bool admin, bool supervisor)
        {
            saveButton.Enabled = admin | supervisor;
        }
        /// <summary>
        /// Creates a new Layout Manager instance and initializes the required state.
        /// </summary>
        public LayoutManager()
        {
            InitializeComponent();
            presenter = new LayoutManagerPresenter(this);
            styleBox.DataSource = Enum.GetValues(typeof(DashStyle));
            fontFamilyBox.Items.AddRange(FontFamily.Families.Select(a => a.Name).ToArray());
            List<object> fontSizes = new List<object>();
            for (int i = 1; i < 200; i++)
                fontSizes.Add(i);
            fontSizeBox.Items.AddRange(fontSizes.ToArray());
        }
        /// <summary>
        /// Handles the create Control lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (Program.DesignMode)
                return;
            if (!initialized)
            {
                initialized = true;
                cash.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.cash];
                CurrentLayout = cash;
                bank.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.bank];
                clients.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.clients];
                account.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.accounts];
                advisor.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.advisors];
                assistants.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.assistants];
                cashaudit.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.cashaudit];
                quittance.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.quittance];
                officeCash.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.officecash];
                pageBox.SelectedIndex = 0;
                pageBox1.SelectedIndex = 0;
                fontFamilyBox.SelectedItem = "Arial";
                fontSizeBox.SelectedItem = 10;
                Pflegehaushaltsbuch.FormControls.Layout.OnChangeRectangle += OnChangeRectangle;
                Pflegehaushaltsbuch.FormControls.Layout.OnUpdateSelectedItem += OnUpdateSelectedItem;
                /*
                PrinterSettings printer = new PrinterSettings();
                var rect = printer.DefaultPageSettings.PrintableArea;
                Size size = new Size((int)(rect.Width - rect.X), (int)(rect.Height-rect.Y));
                */
                
                foreach (Control c in tabbedPane.Controls)
                {
                    Layout layer = c as Layout;
                    if (layer == null)
                        continue;
                    layer.sql = sql;
                    OnChangeDrawMode += layer.ChangeDrawMode;
                    layer.VisibleChanged += layout_VisibleChanged;
                }
            }
        }
        void Layout_OnUpdateSelectedItem(GraphicsItem[] items)
        {
        }
        private bool initialized = false;
        void layout_VisibleChanged(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            Layout layout = sender as Layout;
            if (layout == null)
                return;
            SelectedItems = new GraphicsItem[0];
            layout.SelectedItems.Clear();
            if (CurrentLayout != null)
            {
                layout.Back = CurrentLayout.Back;
                layout.Fore = CurrentLayout.Fore;
                layout.Border = CurrentLayout.Border;
                layout.Font = CurrentLayout.CurrentFont;
                layout.Horizontal = CurrentLayout.Horizontal;
                layout.Vertical = CurrentLayout.Vertical;
                layout.BorderWidth = CurrentLayout.BorderWidth;
                layout.PrintOnPage = CurrentLayout.PrintOnPage;
                layout.CurrentPage = CurrentLayout.CurrentPage;
            }
            else
            {
                layout.CurrentPage = pageBox.SelectedIndex;
                layout.Back = backColorButton.BackColor;
                layout.Fore = foreColorButton.BackColor;
                layout.Border = borderColorButton.BackColor;
            }
            //this.KeyUp -= new System.Windows.Forms.KeyEventHandler(CurrentLayout.OnKey);
            CurrentLayout = layout;
            //this.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.LayoutManager_KeyUp);
            UpdateLayout();
        }
        /// <summary>
        /// Updates the layout data and refreshes the related application state.
        /// </summary>
        private void UpdateLayout()
        {
            CurrentLayout.Invalidate();
        }
        void OnUpdateSelectedItem(GraphicsItem[] items)
        {
            SelectedItems = items;
            if (items == null || SelectedItems.Length == 0)
                return;
            GraphicsItem item = items.Last();
            foreColorButton.BackColor = CurrentLayout.Fore = item.ForeColor;
            backColorButton.BackColor = CurrentLayout.Back = item.BackColor;
            borderColorButton.BackColor = CurrentLayout.Border = item.BorderColor;
            borderBox.Text = item.BorderWidth.ToString("f2");
            styleBox.SelectedItem = item.Style;
            CurrentLayout.BorderWidth = item.BorderWidth;
            RectangleF rect = item.GetRectangle(CurrentLayout.CurrentPage);
            xText.Text = rect.X.ToString();
            yText.Text = rect.Y.ToString();
            widthBox.Text = rect.Width.ToString();
            heightBox.Text = rect.Height.ToString();
            CurrentLayout.PrintOnPage = pageBox1.SelectedIndex = (int)item.PrintOn;

            if (item is FontItem)
            {
                FontItem fontItem = item as FontItem;
                Font font = fontItem.Font;
                CurrentLayout.CurrentFont = font;
                fontFamilyBox.SelectedItem = font.Name;
                fontSizeBox.SelectedItem = (int)font.Size;
                boldButton.Checked = font.Bold;
                italicButton.Checked = font.Italic;
                underlineButton.Checked = font.Underline;
                strikeOutButton.Checked = font.Strikeout;
            }
            if (item is TextItem)
            {
                TextItem textItem = item as TextItem;
                CurrentLayout.Horizontal = textItem.HorizontalAlignment;
                CurrentLayout.Vertical = textItem.VerticalAlignment;
                CurrentLayout.CalculateTextHeight = textItem.CalculateTextHeight;
                calculateTextHeightBox.Checked = CurrentLayout.CalculateTextHeight;
                leftText.Checked = false;
                centerText.Checked = false;
                rightText.Checked = false;
                if (textItem.HorizontalAlignment == StringAlignment.Near)
                    leftText.Checked = true;
                else if (textItem.HorizontalAlignment == StringAlignment.Center)
                    centerText.Checked = true;
                else if (textItem.HorizontalAlignment == StringAlignment.Far)
                    rightText.Checked = true;
                topText.Checked = false;
                verticallCenter.Checked = false;
                bottomText.Checked = false;
                if (textItem.VerticalAlignment == StringAlignment.Near)
                    topText.Checked = true;
                else if (textItem.VerticalAlignment == StringAlignment.Center)
                    verticallCenter.Checked = true;
                else if (textItem.VerticalAlignment == StringAlignment.Far)
                    bottomText.Checked = true;
            }
        }
        void OnChangeRectangle(RectangleF p)
        {
            xText.Text = p.X.ToString();
            yText.Text = p.Y.ToString();
            widthBox.Text = p.Width.ToString();
            heightBox.Text = p.Height.ToString();
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Administration, sql);
        }
        /// <summary>
        /// Handles the click event for size Button and updates the related state.
        /// </summary>
        private void sizeButton_Click(object sender, EventArgs e)
        {
            if (OnChangeDrawMode != null)
                OnChangeDrawMode(FormControls.Layout.DrawElement.Size);
        }
        /// <summary>
        /// Handles the click event for image Button and updates the related state.
        /// </summary>
        private void imageButton_Click(object sender, EventArgs e)
        {
            if (OnChangeDrawMode != null)
                OnChangeDrawMode(FormControls.Layout.DrawElement.Image);
        }
        /// <summary>
        /// Handles the click event for text Button and updates the related state.
        /// </summary>
        private void textButton_Click(object sender, EventArgs e)
        {
            if (OnChangeDrawMode != null)
                OnChangeDrawMode(FormControls.Layout.DrawElement.Text);
        }
        /// <summary>
        /// Handles the click event for line Button and updates the related state.
        /// </summary>
        private void lineButton_Click(object sender, EventArgs e)
        {
            if (OnChangeDrawMode != null)
                OnChangeDrawMode(FormControls.Layout.DrawElement.Line);
        }
        /// <summary>
        /// Handles the click event for arc Button and updates the related state.
        /// </summary>
        private void arcButton_Click(object sender, EventArgs e)
        {
            if (OnChangeDrawMode != null)
                OnChangeDrawMode(FormControls.Layout.DrawElement.Arc);
        }
        /// <summary>
        /// Handles the click event for join Button and updates the related state.
        /// </summary>
        private void joinButton_Click(object sender, EventArgs e)
        {
            if (OnChangeDrawMode != null)
                OnChangeDrawMode(FormControls.Layout.DrawElement.Join);
        }
        /// <summary>
        /// Handles the click event for seperat Button and updates the related state.
        /// </summary>
        private void seperatButton_Click(object sender, EventArgs e)
        {
            foreach (GraphicsItem item in SelectedItems)
                item.Disconnect();
        }
        /// <summary>
        /// Handles the click event for table Button and updates the related state.
        /// </summary>
        private void tableButton_Click(object sender, EventArgs e)
        {
            if (OnChangeDrawMode != null)
                OnChangeDrawMode(FormControls.Layout.DrawElement.Table);
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
                await sql.Printing.SaveDocuments(sql);
                MessageBox.ShowDialog(this, Messages.layout_saved);
            }
            finally 
            { 
                databaseOperationLock.Release();
            }
        }
        /// <summary>
        /// Handles the click event for import Button and updates the related state.
        /// </summary>
        private void importButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    sql.Printing.ImportDocuments(dialog.SelectedPath);
                    Reset();
                }
            }
        }
        /// <summary>
        /// Handles the click event for export Button and updates the related state.
        /// </summary>
        private void exportButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    sql.Printing.ExportDocuments(dialog.SelectedPath);
                    MessageBox.ShowDialog(this, string.Format(Messages.layouts_exported, dialog.SelectedPath));
                }
            }
        }
        /// <summary>
        /// Handles the activated event for layout Manager and updates the related state.
        /// </summary>
        private void LayoutManager_Activated(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the click event for reset Button and updates the related state.
        /// </summary>
        private void resetButton_Click(object sender, EventArgs e)
        {
            ResetLayoutsDialog resetLayouts = new ResetLayoutsDialog(sql);
            resetLayouts.ShowDialog(this);
            
            Reset();
        }
        /// <summary>
        /// Runs the reset operation and updates the related application state.
        /// </summary>
        private void Reset()
        {
            foreach (Control c in tabbedPane.Controls)
            {
                //foreach (Control panel in tab.Controls)
                {
                    //foreach (Control c in panel.Controls)
                    {
                        Layout layer = c as Layout;
                        layer.SelectedItems.Clear();
                    }
                }
            }
            cash.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.cash];
            bank.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.bank];
            clients.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.clients];
            account.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.accounts];
            advisor.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.advisors];
            assistants.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.assistants];
            cashaudit.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.cashaudit];
            quittance.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.quittance];
            officeCash.Document = sql.Printing.Layouts[Data.Printing.LayoutEnum.officecash];
            CurrentLayout = cash;
            SelectedItems = new GraphicsItem[0];
            UpdateLayout();
        }
        /// <summary>
        /// Updates the font data and refreshes the related application state.
        /// </summary>
        private void UpdateFont()
        {
            if (CurrentLayout == null)
                return;
            FontStyle fontStyle = FontStyle.Regular;
            if (boldButton.Checked)
                fontStyle |= FontStyle.Bold;
            if (italicButton.Checked)
                fontStyle |= FontStyle.Italic;
            if (underlineButton.Checked)
                fontStyle |= FontStyle.Underline;
            if (strikeOutButton.Checked)
                fontStyle |= FontStyle.Strikeout;
            Font font = new Font(fontFamilyBox.SelectedItem.ToString(), (float)Int32.Parse(fontSizeBox.SelectedItem.ToString()), fontStyle, GraphicsUnit.World);
            CurrentLayout.CurrentFont = font;
        }
        /// <summary>
        /// Updates the fon Selected Items data and refreshes the related application state.
        /// </summary>
        private void UpdateFonSelectedItems()
        {
            foreach (FontItem item in SelectedItems.OfType<FontItem>())
                item.Font = CurrentLayout.CurrentFont;
            UpdateLayout();
        }
        /// <summary>
        /// Handles the selected Index Changed event for font Combo Box Changed and updates the related state.
        /// </summary>
        private void fontComboBoxChanged_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender as Pflegehaushaltsbuch.FormControls.ComboBox).Focused)
                return;
            UpdateFont();
            UpdateFonSelectedItems();
        }
        /// <summary>
        /// Handles the click event for font Buttons Changed and updates the related state.
        /// </summary>
        private void fontButtonsChanged_Click(object sender, EventArgs e)
        {
            UpdateFont();
            UpdateFonSelectedItems();
        }
        /// <summary>
        /// Handles the click event for horizontal and updates the related state.
        /// </summary>
        private void horizontal_Click(object sender, EventArgs e)
        {
            leftText.Checked = false;
            centerText.Checked = false;
            rightText.Checked = false;
            (sender as FormControls.Button).Checked = true;
            StringAlignment align = StringAlignment.Near;
            if (leftText.Checked)
                align = StringAlignment.Near;
            else if (centerText.Checked)
                align = StringAlignment.Center;
            else if (rightText.Checked)
                align = StringAlignment.Far;
            foreach (TextItem item in SelectedItems.OfType<TextItem>())
                item.HorizontalAlignment = align;
            CurrentLayout.Horizontal = align;
            UpdateLayout();
        }
        /// <summary>
        /// Handles the click event for vertical and updates the related state.
        /// </summary>
        private void vertical_Click(object sender, EventArgs e)
        {
            topText.Checked = false;
            verticallCenter.Checked = false;
            bottomText.Checked = false;
            (sender as FormControls.Button).Checked = true;
            StringAlignment align = StringAlignment.Near;
            if (topText.Checked)
                align = StringAlignment.Near;
            else if (verticallCenter.Checked)
                align = StringAlignment.Center;
            else if (bottomText.Checked)
                align = StringAlignment.Far;
            foreach (TextItem item in SelectedItems.OfType<TextItem>())
                item.VerticalAlignment = align;
            CurrentLayout.Vertical = align;
            UpdateLayout();
        }
        /// <summary>
        /// Handles the selected Index Changed event for style Box and updates the related state.
        /// </summary>
        private void styleBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!styleBox.Focused)
                return;
            foreach (GraphicsItem item in SelectedItems)
                item.Style = (DashStyle)styleBox.SelectedItem;
            UpdateLayout();
        }
        /// <summary>
        /// Handles the text Changed event for border Box and updates the related state.
        /// </summary>
        private void borderBox_TextChanged(object sender, EventArgs e)
        {
            if (!borderBox.Focused)
                return;
            if (SelectedItems == null)
                return;
            float value;
            if (!float.TryParse((sender as Pflegehaushaltsbuch.FormControls.TextBox).Text, out value))
                return;
            foreach (GraphicsItem item in SelectedItems)
                item.BorderWidth = value;
            CurrentLayout.BorderWidth = value;
            UpdateLayout();
        }
        /// <summary>
        /// Handles the text Changed event for x Text and updates the related state.
        /// </summary>
        private void xText_TextChanged(object sender, EventArgs e)
        {
            if (!xText.Focused)
                return;
            if (SelectedItems == null)
                return;
            int value;
            if (!int.TryParse((sender as Pflegehaushaltsbuch.FormControls.TextBox).Text, out value))
                return;
            foreach (GraphicsItem item in SelectedItems)
            {
                RectangleF rect = item.GetRectangle(CurrentLayout.CurrentPage);
                rect.X = value;
                item.SetRectangle(CurrentLayout.CurrentPage, rect);
            }
            UpdateLayout();
        }
        /// <summary>
        /// Handles the text Changed event for y Text and updates the related state.
        /// </summary>
        private void yText_TextChanged(object sender, EventArgs e)
        {
            if (!yText.Focused)
                return;
            if (SelectedItems == null)
                return;
            int value;
            if (!int.TryParse((sender as Pflegehaushaltsbuch.FormControls.TextBox).Text, out value))
                return;
            foreach (GraphicsItem item in SelectedItems)
            {
                RectangleF rect = item.GetRectangle(CurrentLayout.CurrentPage);
                rect.Y = value;
                item.SetRectangle(CurrentLayout.CurrentPage, rect);
            }
            UpdateLayout();
        }
        /// <summary>
        /// Handles the text Changed event for width Box and updates the related state.
        /// </summary>
        private void widthBox_TextChanged(object sender, EventArgs e)
        {
            if (!widthBox.Focused)
                return;
            if (SelectedItems == null)
                return;
            int value;
            if (!int.TryParse((sender as Pflegehaushaltsbuch.FormControls.TextBox).Text, out value))
                return;
            foreach (GraphicsItem item in SelectedItems)
            {
                RectangleF rect = item.GetRectangle(CurrentLayout.CurrentPage);
                rect.Width = value;
                item.SetRectangle(CurrentLayout.CurrentPage, rect);
            }
            UpdateLayout();
        }
        /// <summary>
        /// Handles the text Changed event for height Box and updates the related state.
        /// </summary>
        private void heightBox_TextChanged(object sender, EventArgs e)
        {
            if (!heightBox.Focused)
                return;
            if (SelectedItems == null)
                return;
            int value;
            if (!int.TryParse((sender as Pflegehaushaltsbuch.FormControls.TextBox).Text, out value))
                return;
            foreach (GraphicsItem item in SelectedItems)
            {
                RectangleF rect = item.GetRectangle(CurrentLayout.CurrentPage);
                rect.Height = value;
                item.SetRectangle(CurrentLayout.CurrentPage, rect);
            }
            UpdateLayout();
        }
        /// <summary>
        /// Handles the click event for border Color Button and updates the related state.
        /// </summary>
        private void borderColorButton_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    (sender as Control).BackColor = colorDialog.Color;
                else
                    return;
            }
            if (SelectedItems == null)
                return;
            foreach (GraphicsItem item in SelectedItems)
                item.BorderColor = borderColorButton.BackColor;
            CurrentLayout.Border = borderColorButton.BackColor; ;
            UpdateLayout();
        }
        /// <summary>
        /// Handles the click event for back Color Button and updates the related state.
        /// </summary>
        private void backColorButton_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    (sender as Control).BackColor = colorDialog.Color;
                else
                    return;
            }
            if (SelectedItems == null)
                return;
            foreach (GraphicsItem item in SelectedItems)
                item.BackColor = backColorButton.BackColor;
            CurrentLayout.Back = backColorButton.BackColor;
            UpdateLayout();
        }
        /// <summary>
        /// Handles the click event for fore Color Button and updates the related state.
        /// </summary>
        private void foreColorButton_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    (sender as Control).BackColor = colorDialog.Color;
                else
                    return;
            }
            if (SelectedItems == null)
                return;
            foreach (GraphicsItem item in SelectedItems)
                item.ForeColor = foreColorButton.BackColor;
            CurrentLayout.Fore = foreColorButton.BackColor;
            UpdateLayout();
        }
        /// <summary>
        /// Handles the click event for edit Button and updates the related state.
        /// </summary>
        private void editButton_Click(object sender, EventArgs e)
        {
            EditControl();
        }
        /// <summary>
        /// Handles the selected Index Changed event for page Box1 and updates the related state.
        /// </summary>
        private void pageBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!pageBox1.Focused)
                return;
            int selected = pageBox1.SelectedIndex;
            CurrentLayout.PrintOnPage = selected;
            foreach (GraphicsItem item in SelectedItems)
                item.PrintOn = (GraphicsItem.PrintPage)selected;
            UpdateLayout();
        }
        /// <summary>
        /// Handles the selected Index Changed event for page Box and updates the related state.
        /// </summary>
        private void pageBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentLayout == null)
                return;
            CurrentLayout.CurrentPage = pageBox.SelectedIndex;
            SelectedItems = new GraphicsItem[0];
            CurrentLayout.SelectedItems.Clear();
            UpdateLayout();
        }
        /// <summary>
        /// Handles the click event for calculate Text Height Box and updates the related state.
        /// </summary>
        private void calculateTextHeightBox_Click(object sender, EventArgs e)
        {
            bool value = calculateTextHeightBox.Checked;
            foreach (TextItem item in SelectedItems.OfType<TextItem>())
                item.CalculateTextHeight = value;
        }
        /// <summary>
        /// Handles the click event for copy Button and updates the related state.
        /// </summary>
        private void copyButton_Click(object sender, EventArgs e)
        {
            if (CurrentLayout == null)
                return;
            CurrentLayout.Copy2ClipBoard();
        }
        /// <summary>
        /// Handles the click event for insert Button and updates the related state.
        /// </summary>
        private void insertButton_Click(object sender, EventArgs e)
        {
            if (CurrentLayout == null)
                return;
            CurrentLayout.InsertByClipBoard();
        }
        /// <summary>
        /// Handles the click event for print Button and updates the related state.
        /// </summary>
        private void printButton_Click(object sender, EventArgs e)
        {
            try
            {
                PrintBase printing = new PrintBase(sql, CurrentLayoutType);
                 printing.Print("Layout", "Layout", this);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Handles the click event for copy Tool Strip Menu Item and updates the related state.
        /// </summary>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentLayout == null)
                return;
            CurrentLayout.Copy2ClipBoard();
        }
        /// <summary>
        /// Handles the click event for insert Tool Strip Menu Item and updates the related state.
        /// </summary>
        private void InsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentLayout == null)
                return;
            CurrentLayout.InsertByClipBoard();
        }
        /// <summary>
        /// Handles the click event for edit Tool Strip Menu Item and updates the related state.
        /// </summary>
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditControl();
        }
        /// <summary>
        /// Runs the edit Control operation and updates the related application state.
        /// </summary>
        public void EditControl()
        {
            foreach (DataTableItem item in SelectedItems.OfType<DataTableItem>())
            {
                using (EditDataTableDialog editDataTableForm = new EditDataTableDialog(item.TableDesign))
                {
                    if (editDataTableForm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        item.TableDesign = editDataTableForm.TableDesign;
                }
                return;
            }
            ImageItem[] imageItems = SelectedItems.OfType<ImageItem>().ToArray();
            if (imageItems.Length > 0)
            {
                using (OpenFileDialog fileDialog = new OpenFileDialog())
                {
                    if (fileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        Image image = Image.FromFile(fileDialog.FileName);
                        foreach (ImageItem item in imageItems)
                        {
                            item.Image = image;
                        }
                        UpdateLayout();
                    }
                }
                return;
            }
            TextItem[] textItems = SelectedItems.OfType<TextItem>().ToArray();
            if (textItems.Length > 0)
            {
                string text = textItems[0].Text;
                using (EditTextDialog form = new EditTextDialog(sql, text))
                {
                    if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        text = form.Content;
                    else
                        return;
                }
                foreach (TextItem textItem in textItems)
                {
                    textItem.Text = text;
                }
                UpdateLayout();
                return;
            }
        }
        /// <summary>
        /// Handles the click event for page Settings Button and updates the related state.
        /// </summary>
        private void pageSettingsButton_Click(object sender, EventArgs e)
        {
            PageSettingsForm pageSettingsForm = new PageSettingsForm(CurrentLayout);
            pageSettingsForm.ShowDialog(this);
        }
        /// <summary>
        /// Handles the click event for cash Button and updates the related state.
        /// </summary>
        private void cashButton_Click(object sender, EventArgs e)
        {
            LayoutsOff();
            CurrentLayout = cash;
            CurrentLayoutType = Data.Printing.LayoutEnum.cash;
            CurrentLayout.Visible = true;
        }
        /// <summary>
        /// Handles the click event for bank Button and updates the related state.
        /// </summary>
        private void bankButton_Click(object sender, EventArgs e)
        {
            LayoutsOff();
            CurrentLayout = bank;
            CurrentLayoutType = Data.Printing.LayoutEnum.bank;
            CurrentLayout.Visible = true;
        }
        /// <summary>
        /// Handles the click event for office Cash Button and updates the related state.
        /// </summary>
        private void officeCashButton_Click(object sender, EventArgs e)
        {
            LayoutsOff();
            CurrentLayout = officeCash;
            CurrentLayoutType = Data.Printing.LayoutEnum.officecash;
            CurrentLayout.Visible = true;
        }
        /// <summary>
        /// Handles the click event for statement Button and updates the related state.
        /// </summary>
        private void statementButton_Click(object sender, EventArgs e)
        {
            LayoutsOff();
            CurrentLayout = account;
            CurrentLayoutType = Data.Printing.LayoutEnum.accounts;
            CurrentLayout.Visible = true;
        }
        /// <summary>
        /// Handles the click event for client Button and updates the related state.
        /// </summary>
        private void clientButton_Click(object sender, EventArgs e)
        {
            LayoutsOff();
            CurrentLayout = clients;
            CurrentLayoutType = Data.Printing.LayoutEnum.clients;
            CurrentLayout.Visible = true;
        }
        /// <summary>
        /// Handles the click event for advisor Button and updates the related state.
        /// </summary>
        private void advisorButton_Click(object sender, EventArgs e)
        {
            LayoutsOff();
            CurrentLayout = advisor;
            CurrentLayoutType = Data.Printing.LayoutEnum.advisors;
            CurrentLayout.Visible = true;
        }
        /// <summary>
        /// Handles the click event for assistant Button and updates the related state.
        /// </summary>
        private void assistantButton_Click(object sender, EventArgs e)
        {
            LayoutsOff();
            CurrentLayout = assistants;
            CurrentLayoutType = Data.Printing.LayoutEnum.assistants;
            CurrentLayout.Visible = true;
        }
        /// <summary>
        /// Handles the click event for cash Audit Button and updates the related state.
        /// </summary>
        private void cashAuditButton_Click(object sender, EventArgs e)
        {
            LayoutsOff();
            CurrentLayout = cashaudit;
            CurrentLayoutType = Data.Printing.LayoutEnum.cashaudit;
            CurrentLayout.Visible = true;
        }
        /// <summary>
        /// Handles the click event for quittance Button and updates the related state.
        /// </summary>
        private void quittanceButton_Click(object sender, EventArgs e)
        {
            LayoutsOff();
            CurrentLayout = quittance;
            CurrentLayoutType = Data.Printing.LayoutEnum.quittance;
            CurrentLayout.Visible = true;
        }
        /// <summary>
        /// Runs the layouts Off operation and updates the related application state.
        /// </summary>
        private void LayoutsOff()
        {
            cash.Visible = false;
            bank.Visible = false;
            clients.Visible = false;
            account.Visible = false;
            advisor.Visible = false;
            assistants.Visible = false;
            cashaudit.Visible = false;
            quittance.Visible = false;
            officeCash.Visible = false;
        }
    }
}
