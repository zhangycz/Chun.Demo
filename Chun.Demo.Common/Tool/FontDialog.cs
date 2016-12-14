using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace Chun.Demo.Common
{
    [DefaultProperty("Font")]
    [DefaultEvent("Apply")]
    public class FontDialog : CommonDialog
    {
        protected static readonly object EventApply = new object();

        private Font font;
        private Color color = Color.Black;
        private Color backColor = Color.Transparent;//背景颜色默认
        private Color selectColor = Color.Transparent;//选中颜色默认
        private Color oddColor = Color.Transparent;//背景颜色默认
        private Color headColor = Color.Transparent;//选中颜色默认
        private bool allowSimulations = true;
        private bool allowVectorFonts = true;
        private bool allowVerticalFonts = true;
        private bool allowScriptChange = true;
        private bool fixedPitchOnly = false;
        private int maxSize = 0;
        private int minSize = 0;
        private bool scriptsOnly = false;
        private bool showApply = false;
        private bool showColor = false;//设置颜色框是否显示
        private bool showEffects = true;
        private bool showHelp = false;
        private bool showUnderLined = true;//设置下划线是否显示,默认显示
        private bool showStrikeThrough = true;//设置中间线是否显示,默认显示
        private bool showOddLine = true;//是否显示奇偶行颜色设置菜单

        private bool fontMustExist = false;

        private Panel examplePanel;

        private Button okButton;
        private Button cancelButton;
        private Button applyButton;
        private Button helpButton;
        private CheckBox applyAllCheckBox;

        private TextBox fontTextBox;
        private TextBox fontstyleTextBox;
        private TextBox fontsizeTextBox;

        private ListBox fontListBox;
        private ListBox fontstyleListBox;
        private ListBox fontsizeListBox;

        private GroupBox effectsGroupBox;
        private CheckBox strikethroughCheckBox;
        private CheckBox underlinedCheckBox;
        //private ComboBox scriptComboBox;

        private Label fontLabel;
        private Label fontstyleLabel;
        private Label sizeLabel;
        //private Label scriptLabel;

        private GroupBox exampleGroupBox;
        //头
        private GroupBox headGroupBox;
        private ColorComboBox headComboBox;
        //奇数
        private GroupBox oddGroupBox;
        private ColorComboBox oddComboBox;

        private GroupBox selectGroupBox;//选中色
        private ColorComboBox selectComboBox;//选中背景

        private ColorComboBox colorComboBox;

        private FontFamily[] fontFamilies;

        private GroupBox lineGroupBox;//背景色
        private ColorComboBox lineComboBox;//背景色

        private string currentFontName;

        private float currentSize;

        private FontFamily currentFamily;

        private FontStyle currentFontStyle;

        private bool underlined = false;
        private bool strikethrough = false;

        private Hashtable fontHash = new Hashtable();

        private int[] a_sizes = {
			6, 7, 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72
		};

        // char set stuff is only here to make me happy :-)
        private string[] char_sets_names = {
			"西欧语言",
			"中文(GB2312)"
		};

        private string[] char_sets = {
			"中国AaBYyZz",
			"Symbol",
			"Aa" + (char)0x3042 + (char)0x3041 + (char)0x30a2  + (char)0x30a1 + (char)0x4e9c + (char)0x5b87,
			(char)0xac00 + (char)0xb098 + (char)0xb2e4 + "AaBYyZz",
			new String(new Char [] {(char)0x5fae, (char)0x8f6f, (char)0x4e2d, (char)0x6587, (char)0x8f6f, (char)0x4ef6}),
			new String(new Char [] {(char)0x4e2d, (char)0x6587, (char)0x5b57, (char)0x578b, (char)0x7bc4, (char)0x4f8b}),
			"AaBb" + (char)0x0391 + (char)0x03b1 + (char)0x0392 + (char)0x03b2,
			"AaBb" + (char)0x011e + (char)0x011f + (char)0x015e + (char)0x015f,
			"AaBb" + (char)0x05e0 + (char)0x05e1 + (char)0x05e9 + (char)0x05ea,
			"AaBb" + (char)0x0627 + (char)0x0628 + (char)0x062c + (char)0x062f + (char)0x0647 + (char)0x0648 + (char)0x0632,
			"AaBbYyZz",
			"AaBb" + (char)0x01a0 + (char)0x01a1 + (char)0x01af + (char)0x01b0,
			"AaBb" + (char)0x0411 + (char)0x0431 + (char)0x0424 + (char)0x0444,
			"AaBb" + (char)0xc1 + (char)0xe1 + (char)0xd4 + (char)0xf4,
			"AaBb" + (char)0x0e2d + (char)0x0e31 + (char)0x0e01 + (char)0x0e29 + (char)0x0e23 + (char)0x0e44 + (char)0x0e17 +(char)0x0e22,
			(char)0xac00 + (char)0xb098 + (char)0xb2e4 + "AaBYyZz",
			"AaBbYyZz",
			"AaBb" + (char)0xf8 + (char)0xf1 + (char)0xfd,
			"",
			"",
			"",
			"",
			"",
			"",
			""
		};

        private string example_panel_text;

        private bool internal_change = false;

        #region Public Constructors
        public FontDialog()
        {
            example_panel_text = char_sets[0];

            okButton = new Button();
            cancelButton = new Button();
            applyButton = new Button();
            helpButton = new Button();

            fontTextBox = new TextBox();
            fontstyleTextBox = new TextBox();
            fontsizeTextBox = new TextBox();

            fontListBox = new ListBox();
            fontsizeListBox = new ListBox();

            fontLabel = new Label();
            fontstyleLabel = new Label();
            sizeLabel = new Label();
            //scriptLabel = new Label();

            exampleGroupBox = new GroupBox();
            fontstyleListBox = new ListBox();

            effectsGroupBox = new GroupBox();
            underlinedCheckBox = new CheckBox();
            strikethroughCheckBox = new CheckBox();
            applyAllCheckBox = new CheckBox();
            //scriptComboBox = new ComboBox();

            examplePanel = new Panel();

            colorComboBox = new ColorComboBox(this);

            headGroupBox = new GroupBox();
            headComboBox = new ColorComboBox(this);

            oddGroupBox = new GroupBox();
            oddComboBox = new ColorComboBox(this);

            selectGroupBox = new GroupBox();
            selectComboBox = new ColorComboBox(this);

            lineGroupBox = new GroupBox();
            lineComboBox = new ColorComboBox(this);

            exampleGroupBox.SuspendLayout();
            effectsGroupBox.SuspendLayout();
            form.SuspendLayout();

            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;

            // fontsizeListBox
            fontsizeListBox.Location = new Point(284, 47);
            fontsizeListBox.Size = new Size(52, 95);
            fontsizeListBox.TabIndex = 10;
            fontListBox.Sorted = true;
            // fontTextBox
            fontTextBox.Location = new Point(16, 26);
            fontTextBox.Size = new Size(140, 21);
            fontTextBox.TabIndex = 5;
            fontTextBox.Text = "";
            // fontstyleLabel
            fontstyleLabel.Location = new Point(164, 10);
            fontstyleLabel.Size = new Size(100, 16);
            fontstyleLabel.TabIndex = 1;
            fontstyleLabel.Text = "字形:";
            // typesizeTextBox
            fontsizeTextBox.Location = new Point(284, 26);
            fontsizeTextBox.Size = new Size(52, 21);
            fontsizeTextBox.TabIndex = 7;
            fontsizeTextBox.Text = "";
            fontsizeTextBox.MaxLength = 2;
            // schriftartListBox
            fontListBox.Location = new Point(16, 47);
            fontListBox.Size = new Size(140, 95);
            fontListBox.TabIndex = 8;
            fontListBox.Sorted = true;
            // fontstyleListBox
            fontstyleListBox.Location = new Point(164, 47);
            fontstyleListBox.Size = new Size(112, 95);
            fontstyleListBox.TabIndex = 9;
            // schriftartLabel
            fontLabel.Location = new Point(16, 10);
            fontLabel.Size = new Size(88, 16);
            fontLabel.TabIndex = 0;
            fontLabel.Text = "字体:";
            // effectsGroupBox
            effectsGroupBox.Controls.Add(underlinedCheckBox);
            effectsGroupBox.Controls.Add(strikethroughCheckBox);
            effectsGroupBox.Controls.Add(colorComboBox);
            effectsGroupBox.FlatStyle = FlatStyle.System;
            effectsGroupBox.Location = new Point(16, 158);
            effectsGroupBox.Size = new Size(140, 70);
            effectsGroupBox.TabIndex = 11;
            effectsGroupBox.TabStop = false;
            effectsGroupBox.Text = "效果";
            // strikethroughCheckBox
            strikethroughCheckBox.FlatStyle = FlatStyle.System;
            strikethroughCheckBox.Location = new Point(6, 16);
            strikethroughCheckBox.Size = new Size(60, 16);
            strikethroughCheckBox.TabIndex = 0;
            strikethroughCheckBox.Text = "删除线";
            // underlinedCheckBox
            underlinedCheckBox.FlatStyle = FlatStyle.System;
            underlinedCheckBox.Location = new Point(72, 16);
            underlinedCheckBox.Size = new Size(60, 16);
            underlinedCheckBox.TabIndex = 1;
            underlinedCheckBox.Text = "下划线";
            // colorComboBox
            colorComboBox.Location = new Point(6, 42);
            colorComboBox.Size = new Size(130, 21);
            // sizeLabel
            sizeLabel.Location = new Point(284, 10);
            sizeLabel.Size = new Size(100, 16);
            sizeLabel.TabIndex = 2;
            sizeLabel.Text = "大小:";

            // scriptComboBox
            //scriptComboBox.Location = new Point(164, 253);
            //scriptComboBox.Size = new Size(172, 21);
            //scriptComboBox.TabIndex = 14;
            //scriptComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            // applyAllCheckBox
            applyAllCheckBox.FlatStyle = FlatStyle.System;
            applyAllCheckBox.Location = new Point(352, 26);
            applyAllCheckBox.Size = new Size(70, 16);
            applyAllCheckBox.TabIndex = 0;
            applyAllCheckBox.Checked = false;
            applyAllCheckBox.Text = "应用所有";
            // okButton
            okButton.FlatStyle = FlatStyle.System;
            okButton.Location = new Point(352, 52);
            okButton.Size = new Size(70, 23);
            okButton.TabIndex = 3;
            okButton.Text = "确定";
            // cancelButton
            cancelButton.FlatStyle = FlatStyle.System;
            cancelButton.Location = new Point(352, 78);
            cancelButton.Size = new Size(70, 23);
            cancelButton.TabIndex = 4;
            cancelButton.Text = "取消";
            // applyButton
            applyButton.FlatStyle = FlatStyle.System;
            applyButton.Location = new Point(352, 104);
            applyButton.Size = new Size(70, 23);
            applyButton.TabIndex = 5;
            applyButton.Text = "应用";
            // helpButton
            helpButton.FlatStyle = FlatStyle.System;
            helpButton.Location = new Point(352, 130);
            helpButton.Size = new Size(70, 23);
            helpButton.TabIndex = 6;
            helpButton.Text = "帮助";
            // fontstyleTextBox
            fontstyleTextBox.Location = new Point(164, 26);
            fontstyleTextBox.Size = new Size(112, 21);
            fontstyleTextBox.TabIndex = 6;
            fontstyleTextBox.Text = "";
            // exampleGroupBox
            exampleGroupBox.Controls.Add(examplePanel);
            exampleGroupBox.FlatStyle = FlatStyle.System;
            exampleGroupBox.Location = new Point(164, 158);
            exampleGroupBox.Size = new Size(172, 70);
            exampleGroupBox.TabIndex = 12;
            exampleGroupBox.TabStop = false;
            exampleGroupBox.Text = "示例";
            // examplePanel
            examplePanel.Location = new Point(8, 20);
            examplePanel.TabIndex = 0;
            examplePanel.Size = new Size(156, 40);
            examplePanel.BorderStyle = BorderStyle.Fixed3D;
            //  lineGroupBox
            lineGroupBox.Controls.Add(lineComboBox);
            lineGroupBox.Location = new Point(16, 236);
            lineGroupBox.Size = new Size(140, 50);
            lineGroupBox.TabIndex = 0;
            lineGroupBox.Text = "设置背景色:";
            //  lineComboBox
            lineComboBox.Location = new Point(6, 16);
            lineComboBox.TabIndex = 1;
            lineComboBox.Size = new Size(130, 21);
            //  selectGroupBox
            selectGroupBox.Controls.Add(selectComboBox);
            selectGroupBox.Location = new Point(164, 236);
            selectGroupBox.Size = new Size(173, 50);
            selectGroupBox.TabIndex = 0;
            selectGroupBox.Text = "选中颜色:";

            //  selectComboBox
            selectComboBox.Location = new Point(6, 16);
            selectComboBox.TabIndex = 1;
            selectComboBox.Size = new Size(153, 21);

            //  oddGroupBox
            oddGroupBox.Controls.Add(oddComboBox);
            oddGroupBox.Location = new Point(16, 294);
            oddGroupBox.Size = new Size(140, 50);
            oddGroupBox.TabIndex = 0;
            oddGroupBox.Text = "奇数行颜色:";
            //  oddComboBox
            oddComboBox.Location = new Point(6, 16);
            oddComboBox.TabIndex = 1;
            oddComboBox.Size = new Size(130, 21);

            //  headGroupBox
            headGroupBox.Controls.Add(headComboBox);
            headGroupBox.Location = new Point(164, 294);
            headGroupBox.Size = new Size(173, 50);
            headGroupBox.TabIndex = 0;
            headGroupBox.Text = "标题颜色:";
            //  headComboBox
            headComboBox.Location = new Point(6, 16);
            headComboBox.TabIndex = 1;
            headComboBox.Size = new Size(153, 21);

            form.AcceptButton = okButton;

            //form.Controls.Add(scriptComboBox);
            //form.Controls.Add(scriptLabel);
            form.Controls.Add(exampleGroupBox);
            form.Controls.Add(applyAllCheckBox);
            form.Controls.Add(effectsGroupBox);
            form.Controls.Add(fontsizeListBox);
            form.Controls.Add(fontstyleListBox);
            form.Controls.Add(fontListBox);
            form.Controls.Add(fontsizeTextBox);
            form.Controls.Add(fontstyleTextBox);
            form.Controls.Add(fontTextBox);
            form.Controls.Add(cancelButton);
            form.Controls.Add(okButton);
            form.Controls.Add(sizeLabel);
            form.Controls.Add(fontstyleLabel);
            form.Controls.Add(fontLabel);
            form.Controls.Add(applyButton);
            form.Controls.Add(helpButton);
            form.Controls.Add(lineGroupBox);
            form.Controls.Add(selectGroupBox);
            form.Controls.Add(oddGroupBox);
            form.Controls.Add(headGroupBox);
            exampleGroupBox.ResumeLayout(false);
            effectsGroupBox.ResumeLayout(false);
            /*设置FontDialog对话框大小,设计者可以根据窗体中要添加的内容对该窗体进行修改尺寸.*/
            form.Size = new Size(430, 376);

            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;

            form.Text = "字体";
            /*设置FontDialog的显示位置*/
            form.StartPosition = FormStartPosition.CenterParent;

            form.ResumeLayout(false);

            fontFamilies = FontFamily.Families;

            fontListBox.BeginUpdate();
            foreach (FontFamily ff in fontFamilies)
            {
                if (!fontHash.ContainsKey(ff.Name))
                {
                    fontListBox.Items.Add(ff.Name);
                    fontHash.Add(ff.Name, ff);
                }
            }
            fontListBox.EndUpdate();

            CreateFontSizeListBoxItems();

            //scriptComboBox.BeginUpdate();
            //scriptComboBox.Items.AddRange(char_sets_names);
            //scriptComboBox.SelectedIndex = 0;
            //scriptComboBox.EndUpdate();

            applyButton.Hide();
            helpButton.Hide();
            colorComboBox.Hide();

            cancelButton.Click += new EventHandler(OnClickCancelButton);
            okButton.Click += new EventHandler(OnClickOkButton);
            applyButton.Click += new EventHandler(OnApplyButton);
            examplePanel.Paint += new PaintEventHandler(OnPaintExamplePanel);
            fontListBox.SelectedIndexChanged += new EventHandler(OnSelectedIndexChangedFontListBox);
            fontsizeListBox.SelectedIndexChanged += new EventHandler(OnSelectedIndexChangedSizeListBox);
            fontstyleListBox.SelectedIndexChanged += new EventHandler(OnSelectedIndexChangedFontStyleListBox);
            underlinedCheckBox.CheckedChanged += new EventHandler(OnCheckedChangedUnderlinedCheckBox);
            strikethroughCheckBox.CheckedChanged += new EventHandler(OnCheckedChangedStrikethroughCheckBox);
            //scriptComboBox.SelectedIndexChanged += new EventHandler(OnSelectedIndexChangedScriptComboBox);

            fontTextBox.KeyPress += new KeyPressEventHandler(OnFontTextBoxKeyPress);
            fontstyleTextBox.KeyPress += new KeyPressEventHandler(OnFontStyleTextBoxKeyPress);
            fontsizeTextBox.KeyPress += new KeyPressEventHandler(OnFontSizeTextBoxKeyPress);

            fontTextBox.TextChanged += new EventHandler(OnFontTextBoxTextChanged);
            fontstyleTextBox.TextChanged += new EventHandler(OnFontStyleTextTextChanged);
            fontsizeTextBox.TextChanged += new EventHandler(OnFontSizeTextBoxTextChanged);

            colorComboBox.SelectedIndexChanged += new EventHandler(OnColorComboBoxSelectedIndexChanged);
            lineComboBox.SelectedIndexChanged += new EventHandler(OnLineComboBoxSelectedIndexChanged);
            selectComboBox.SelectedIndexChanged += new EventHandler(OnSelectComboBoxSelectedIndexChanged);
            oddComboBox.SelectedIndexChanged += new EventHandler(OnOddComboBoxSelectedIndexChanged);
            headComboBox.SelectedIndexChanged += new EventHandler(OnHeadComboBoxSelectedIndexChanged);
            Font = form.Font;
        }

        #endregion	// Public Constructors

        #region Public Instance Properties
        /// <summary>
        /// 获取或设置字体,大小,样式
        /// </summary>
        public Font Font
        {
            get
            {
                return font;
            }

            set
            {
                if (value != null)
                {
                    font = new Font(value, value.Style);

                    currentFontStyle = font.Style;
                    currentSize = font.Size;
                    currentFontName = font.Name;

                    int index = fontListBox.FindString(currentFontName);

                    if (index != -1)
                    {
                        fontListBox.SelectedIndex = index;
                    }
                    else
                    {
                        fontListBox.SelectedIndex = 0;
                    }

                    fontListBox.TopIndex = fontListBox.SelectedIndex;
                }
            }
        }
        [DefaultValue(false)]
        public bool FontMustExist
        {
            get
            {
                return fontMustExist;
            }

            set
            {
                fontMustExist = value;
            }
        }
        /// <summary>
        /// 获取或设置字体颜色
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                examplePanel.Invalidate();
                colorComboBox.SelectedIndex = SelectedIndex(colorComboBox, value);
            }
        }
        /// <summary>
        /// 获取背景颜色.
        /// </summary>
        public Color BackColor
        {
            get { return this.backColor; }
            set
            {
                this.backColor = value;
                lineComboBox.SelectedIndex = SelectedIndex(lineComboBox, value);
            }
        }
        /// <summary>
        /// 获取选中颜色
        /// </summary>
        public Color SelectColor
        {
            get { return this.selectColor; }
            set
            {
                this.selectColor = value;
                selectComboBox.SelectedIndex = SelectedIndex(selectComboBox, value);
            }
        }

        /// <summary>
        /// 获取标题颜色
        /// </summary>
        public Color HeadColor
        {
            get { return this.headColor; }
            set
            {
                this.headColor = value;
                headComboBox.SelectedIndex = SelectedIndex(headComboBox, value);
            }
        }

        /// <summary>
        /// 获取奇数行颜色
        /// </summary>
        public Color OddColor
        {
            get { return this.oddColor; }
            set
            {
                this.oddColor = value;
                oddComboBox.SelectedIndex = SelectedIndex(oddComboBox, value);
            }
        }

        [DefaultValue(true)]
        public bool AllowSimulations
        {
            set
            {
                allowSimulations = value;
            }

            get
            {
                return allowSimulations;
            }
        }
        [DefaultValue(true)]
        public bool AllowVectorFonts
        {
            set
            {
                allowVectorFonts = value;
            }

            get
            {
                return allowVectorFonts;
            }
        }

        [DefaultValue(true)]
        public bool AllowVerticalFonts
        {
            set
            {
                allowVerticalFonts = value;
            }

            get
            {
                return allowVerticalFonts;
            }
        }

        [DefaultValue(true)]
        public bool AllowScriptChange
        {
            set
            {
                allowScriptChange = value;
            }

            get
            {
                return allowScriptChange;
            }
        }

        [DefaultValue(false)]
        public bool FixedPitchOnly
        {
            set
            {
                fixedPitchOnly = value;
            }

            get
            {
                return fixedPitchOnly;
            }
        }
        /// <summary>
        /// 设置最小窗体尺寸.
        /// </summary>
        [DefaultValue(0)]
        public int MaxSize
        {
            set
            {
                maxSize = value;

                if (maxSize < 0)
                    maxSize = 0;

                if (maxSize < minSize)
                    minSize = maxSize;

                CreateFontSizeListBoxItems();
            }
            get
            {
                return maxSize;
            }
        }
        /// <summary>
        /// 设置最大窗体尺寸.
        /// </summary>
        [DefaultValue(0)]
        public int MinSize
        {
            set
            {
                minSize = value;

                if (minSize < 0)
                    minSize = 0;

                if (minSize > maxSize)
                    maxSize = minSize;

                CreateFontSizeListBoxItems();

                if (minSize > currentSize)
                    if (font != null)
                    {
                        font.Dispose();

                        currentSize = minSize;

                        font = new Font(currentFamily, currentSize, currentFontStyle);

                        UpdateExamplePanel();

                        fontsizeTextBox.Text = currentSize.ToString();
                    }
            }
            get
            {
                return minSize;
            }
        }
        /// <summary>
        /// 设置是否显示字符集.
        /// </summary>
        [DefaultValue(false)]
        public bool ScriptsOnly
        {
            set
            {
                scriptsOnly = value;
            }

            get
            {
                return scriptsOnly;
            }
        }
        /// <summary>
        /// 设置是否显示应用按钮.
        /// </summary>
        [DefaultValue(false)]
        public bool ShowApply
        {
            set
            {
                if (value != showApply)
                {
                    showApply = value;
                    if (showApply)
                        applyButton.Show();
                    else
                        applyButton.Hide();

                    form.Refresh();
                }

            }

            get
            {
                return showApply;
            }
        }
        /// <summary>
        /// 设置是否显示颜色下拉框.
        /// </summary>
        [DefaultValue(true)]
        public bool ShowColor
        {
            set
            {
                if (value != showColor)
                {
                    showColor = value;
                    if (showColor)
                        colorComboBox.Show();
                    else
                        colorComboBox.Hide();

                    form.Refresh();
                }
            }

            get
            {
                return showColor;
            }
        }
        /// <summary>
        /// 获取或设置下划线CheckBox是否显示.
        /// </summary>
        [DefaultValue(false), Description("获取或设置下划线CheckBox是否显示.")]
        public bool ShowUnderLined
        {
            get { return this.showUnderLined; }
            set
            {
                if (value != showUnderLined)
                {
                    showUnderLined = value;
                    if (showUnderLined)
                        underlinedCheckBox.Show();
                    else
                        underlinedCheckBox.Hide();
                }
            }
        }
        /// <summary>
        /// 获取或设置中间线线CheckBox是否显示.
        /// </summary>
        [DefaultValue(false), Description("获取或设置中间线线CheckBox是否显示.")]
        public bool ShowStrikeThrough
        {
            get { return this.showStrikeThrough; }
            set
            {
                if (value != showStrikeThrough)
                {
                    this.showStrikeThrough = value;
                    if (showStrikeThrough)
                        strikethroughCheckBox.Show();
                    else
                        strikethroughCheckBox.Hide();
                }
            }
        }
        /// <summary>
        /// 获取或设置效果GroupBox控件是否显示出来.
        /// </summary>
        [DefaultValue(true)]
        public bool ShowEffects
        {
            set
            {
                if (value != showEffects)
                {
                    showEffects = value;
                    if (showEffects)
                        effectsGroupBox.Show();
                    else
                        effectsGroupBox.Hide();

                    form.Refresh();
                }
            }

            get
            {
                return showEffects;
            }
        }
        /// <summary>
        /// 获取或设置是否显示帮助按钮
        /// </summary>
        [DefaultValue(false)]
        public bool ShowHelp
        {
            set
            {
                if (value != showHelp)
                {
                    showHelp = value;
                    if (showHelp)
                        helpButton.Show();
                    else
                        helpButton.Hide();

                    form.Refresh();
                }
            }

            get
            {
                return showHelp;
            }
        }
        /// <summary>
        /// 获取或设置是否显示设置奇偶行颜色的菜单.
        /// </summary>
        [DefaultValue(true)]
        public bool ShowOddLine
        {
            get { return this.showOddLine; }
            set
            {
                if (value != showOddLine)
                {
                    showOddLine = value;
                    if (showOddLine)
                        this.lineGroupBox.Show();
                    else
                        this.lineGroupBox.Hide();
                }
            }
        }
        /// <summary>
        /// 是否应用所有
        /// </summary>
        [DefaultValue(false)]
        public bool ApplyAll
        {
            get;
            set;
        }
        #endregion	// Public Instance Properties

        #region Protected Instance Properties
        protected int Options
        {
            get { return 0; }
        }
        #endregion	// Protected Instance Properties

        #region Public Instance Methods
        public override void Reset()
        {
            color = Color.Black;
            allowSimulations = true;
            allowVectorFonts = true;
            allowVerticalFonts = true;
            allowScriptChange = true;
            fixedPitchOnly = false;

            maxSize = 0;
            minSize = 0;
            CreateFontSizeListBoxItems();

            scriptsOnly = false;

            showApply = false;
            applyButton.Hide();

            showColor = false;
            colorComboBox.Hide();

            showEffects = true;
            effectsGroupBox.Show();

            showHelp = false;
            helpButton.Hide();

            form.Refresh();
        }

        public override string ToString()
        {
            if (font == null)
                return base.ToString();
            return String.Concat(base.ToString(), ", Font: ", font.ToString());
        }
        #endregion	// Public Instance Methods

        #region Protected Instance Methods
        protected override IntPtr HookProc(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            return base.HookProc(hWnd, msg, wparam, lparam);
        }

        protected override bool RunDialog(IntPtr hwndOwner)
        {
            form.Refresh();

            return true;
        }

        internal void OnApplyButton(object sender, EventArgs e)
        {
            OnApply(e);
        }

        protected virtual void OnApply(EventArgs e)
        {
            EventHandler apply = (EventHandler)Events[EventApply];
            if (apply != null)
                apply(this, e);
        }
        #endregion	// Protected Instance Methods

        #region Private Instance Methods

        /// <summary>
        /// 获取指定的颜色
        /// </summary>
        private int SelectedIndex(ColorComboBox cmb, Color color)
        {
            int index = 0;
            for (int i = 0; i < cmb.Items.Count; i++)
            {
                ColorComboBox.ColorComboBoxItem item = cmb.Items[i] as ColorComboBox.ColorComboBoxItem;
                if (item.Color.Equals(color))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        void OnClickCancelButton(object sender, EventArgs e)
        {
            form.DialogResult = DialogResult.Cancel;
        }

        void OnClickOkButton(object sender, EventArgs e)
        {
            ApplyAll = applyAllCheckBox.Checked;
            form.DialogResult = DialogResult.OK;
        }

        void OnPaintExamplePanel(object sender, PaintEventArgs e)
        {
            SolidBrush brush = new SolidBrush(color);

            e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), 0, 0, 156, 40);

            SizeF fontSizeF = e.Graphics.MeasureString(example_panel_text, font);

            int text_width = (int)fontSizeF.Width;
            int text_height = (int)fontSizeF.Height;

            int x = (examplePanel.Width / 2) - (text_width / 2);
            if (x < 0) x = 0;

            int y = (examplePanel.Height / 2) - (text_height / 2);

            e.Graphics.DrawString(example_panel_text, font, brush, new Point(x, y));
        }

        void OnSelectedIndexChangedFontListBox(object sender, EventArgs e)
        {
            if (fontListBox.SelectedIndex != -1)
            {
                currentFamily = FindByName(fontListBox.Items[fontListBox.SelectedIndex].ToString());

                fontTextBox.Text = currentFamily.Name;

                internal_change = true;

                UpdateFontStyleListBox();

                UpdateFontSizeListBox();

                UpdateExamplePanel();

                //form.Select(fontTextBox);//change for select()
                form.ActiveControl = fontTextBox;
                fontTextBox.Select();

                internal_change = false;
            }
        }

        void OnSelectedIndexChangedSizeListBox(object sender, EventArgs e)
        {
            if (fontsizeListBox.SelectedIndex != -1)
            {
                currentSize = (float)System.Convert.ToDouble(fontsizeListBox.Items[fontsizeListBox.SelectedIndex]);

                fontsizeTextBox.Text = currentSize.ToString();

                UpdateExamplePanel();

                if (!internal_change)
                {
                    //form.Select(fontsizeTextBox);
                    form.ActiveControl = fontsizeTextBox;
                    fontsizeTextBox.Select();
                }
            }
        }

        void OnSelectedIndexChangedFontStyleListBox(object sender, EventArgs e)
        {
            if (fontstyleListBox.SelectedIndex != -1)
            {
                switch (fontstyleListBox.SelectedIndex)
                {
                    case 0:
                        currentFontStyle = FontStyle.Regular;
                        break;
                    case 1:
                        currentFontStyle = FontStyle.Bold;
                        break;
                    case 2:
                        currentFontStyle = FontStyle.Italic;
                        break;
                    case 3:
                        currentFontStyle = FontStyle.Bold | FontStyle.Italic;
                        break;
                    default:
                        currentFontStyle = FontStyle.Regular;
                        break;
                }

                if (underlined)
                    currentFontStyle = currentFontStyle | FontStyle.Underline;

                if (strikethrough)
                    currentFontStyle = currentFontStyle | FontStyle.Strikeout;

                fontstyleTextBox.Text = fontstyleListBox.Items[fontstyleListBox.SelectedIndex].ToString();

                if (!internal_change)
                {
                    UpdateExamplePanel();

                    //form.Select(fontstyleTextBox);
                    form.ActiveControl = fontstyleTextBox;
                    fontstyleTextBox.Select();
                }
            }
        }

        void OnCheckedChangedUnderlinedCheckBox(object sender, EventArgs e)
        {
            if (underlinedCheckBox.Checked)
            {
                currentFontStyle = currentFontStyle | FontStyle.Underline;
                underlined = true;
            }
            else
            {
                currentFontStyle = currentFontStyle ^ FontStyle.Underline;
                underlined = false;
            }

            UpdateExamplePanel();
        }

        void OnCheckedChangedStrikethroughCheckBox(object sender, EventArgs e)
        {
            if (strikethroughCheckBox.Checked)
            {
                currentFontStyle = currentFontStyle | FontStyle.Strikeout;
                strikethrough = true;
            }
            else
            {
                currentFontStyle = currentFontStyle ^ FontStyle.Strikeout;
                strikethrough = false;
            }

            UpdateExamplePanel();
        }

        bool internal_textbox_change = false;

        void OnFontTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            internal_textbox_change = true;

            if (fontListBox.SelectedIndex > -1)
                fontListBox.SelectedIndex = -1;
        }

        void OnFontStyleTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            internal_textbox_change = true;

            if (fontstyleListBox.SelectedIndex > -1)
                fontstyleListBox.SelectedIndex = -1;
        }

        void OnFontSizeTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar) || Char.IsWhiteSpace(e.KeyChar) || Char.IsPunctuation(e.KeyChar) || e.KeyChar == ',')
            {
                e.Handled = true;
                return;
            }

            internal_textbox_change = true;
        }

        void OnFontTextBoxTextChanged(object sender, EventArgs e)
        {
            if (!internal_textbox_change)
                return;

            internal_textbox_change = false;

            for (int i = 0; i < fontListBox.Items.Count; i++)
            {
                string name = fontListBox.Items[i] as string;

                if (name.StartsWith(fontTextBox.Text))
                {
                    if (name == fontTextBox.Text)
                        fontListBox.SelectedIndex = i;
                    else
                        fontListBox.TopIndex = i;

                    break;
                }
            }
        }

        void OnFontStyleTextTextChanged(object sender, EventArgs e)
        {
            if (!internal_textbox_change)
                return;

            internal_textbox_change = false;

            for (int i = 0; i < fontstyleListBox.Items.Count; i++)
            {
                string name = fontstyleListBox.Items[i] as string;

                if (name.StartsWith(fontstyleTextBox.Text))
                {
                    if (name == fontstyleTextBox.Text)
                        fontstyleListBox.SelectedIndex = i;

                    break;
                }
            }
        }

        void OnFontSizeTextBoxTextChanged(object sender, EventArgs e)
        {
            if (!internal_textbox_change)
                return;

            internal_textbox_change = false;

            if (fontsizeTextBox.Text.Length == 0)
                return;

            for (int i = 0; i < fontsizeListBox.Items.Count; i++)
            {
                string name = fontsizeListBox.Items[i] as string;

                if (name.StartsWith(fontsizeTextBox.Text))
                {
                    if (name == fontsizeTextBox.Text)
                        fontsizeListBox.SelectedIndex = i;
                    else
                        fontsizeListBox.TopIndex = i;

                    break;
                }
            }
        }

        //void OnSelectedIndexChangedScriptComboBox(object sender, EventArgs e)
        //{
        //    string tmp_str = char_sets[scriptComboBox.SelectedIndex];

        //    if (tmp_str.Length > 0)
        //    {
        //        example_panel_text = tmp_str;

        //        UpdateExamplePanel();
        //    }
        //}
        /*新增,由于增加一个ColorComboBox,所以在此处将两个区分开来.
         如果仅有一个ColorComboBox则就不需要进行事件的委托,重载OnSelectedIndex即可.*/
        void OnColorComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ColorComboBox)
            {
                ColorComboBox ccb = sender as ColorComboBox;
                this.Color = (ccb.SelectedItem as ColorComboBox.ColorComboBoxItem).Color;
            }
        }

        void OnLineComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ColorComboBox)
            {
                ColorComboBox ccb = sender as ColorComboBox;
                this.BackColor = (ccb.SelectedItem as ColorComboBox.ColorComboBoxItem).Color;
            }
        }

        void OnSelectComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ColorComboBox)
            {
                ColorComboBox ccb = sender as ColorComboBox;
                this.SelectColor = (ccb.SelectedItem as ColorComboBox.ColorComboBoxItem).Color;
            }
        }
        void OnOddComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ColorComboBox)
            {
                ColorComboBox ccb = sender as ColorComboBox;
                this.OddColor = (ccb.SelectedItem as ColorComboBox.ColorComboBoxItem).Color;
            }
        }
        void OnHeadComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ColorComboBox)
            {
                ColorComboBox ccb = sender as ColorComboBox;
                this.HeadColor = (ccb.SelectedItem as ColorComboBox.ColorComboBoxItem).Color;
            }
        }

        void UpdateExamplePanel()
        {
            if (font != null)
                font.Dispose();

            font = new Font(currentFamily, currentSize, currentFontStyle);

            examplePanel.Invalidate();
        }

        void UpdateFontSizeListBox()
        {
            int index = fontsizeListBox.FindString(currentSize.ToString());

            if (index != -1)
                fontsizeListBox.SelectedIndex = index;
            else
                fontsizeListBox.SelectedIndex = 0;
        }

        void UpdateFontStyleListBox()
        {
            // don't know if that works, IsStyleAvailable returns true for all styles under X

            fontstyleListBox.BeginUpdate();

            fontstyleListBox.Items.Clear();

            int index = -1;
            int to_select = 0;

            if (currentFamily.IsStyleAvailable(FontStyle.Regular))
            {
                index = fontstyleListBox.Items.Add("常规");

                if ((currentFontStyle & FontStyle.Regular) == FontStyle.Regular)
                    to_select = index;
            }

            if (currentFamily.IsStyleAvailable(FontStyle.Bold))
            {
                index = fontstyleListBox.Items.Add("粗体");

                if ((currentFontStyle & FontStyle.Bold) == FontStyle.Bold)
                    to_select = index;
            }

            if (currentFamily.IsStyleAvailable(FontStyle.Italic))
            {
                index = fontstyleListBox.Items.Add("倾斜");

                if ((currentFontStyle & FontStyle.Italic) == FontStyle.Italic)
                    to_select = index;
            }

            if (currentFamily.IsStyleAvailable(FontStyle.Bold) && currentFamily.IsStyleAvailable(FontStyle.Italic))
            {
                index = fontstyleListBox.Items.Add("粗体 倾斜");

                if ((currentFontStyle & (FontStyle.Bold | FontStyle.Italic)) == (FontStyle.Bold | FontStyle.Italic))
                    to_select = index;
            }

            if (fontstyleListBox.Items.Count > 0)
                fontstyleListBox.SelectedIndex = to_select;

            fontstyleListBox.EndUpdate();
        }
        #endregion

        FontFamily FindByName(string name)
        {
            return fontHash[name] as FontFamily;
        }

        void CreateFontSizeListBoxItems()
        {
            fontsizeListBox.BeginUpdate();

            fontsizeListBox.Items.Clear();

            if (minSize == 0 && maxSize == 0)
            {
                foreach (int i in a_sizes)
                    fontsizeListBox.Items.Add(i.ToString());
            }
            else
            {
                foreach (int i in a_sizes)
                {
                    if (i >= minSize && i <= maxSize)
                        fontsizeListBox.Items.Add(i.ToString());
                }
            }

            fontsizeListBox.EndUpdate();
        }

        internal class ColorComboBox : ComboBox
        {
            internal class ColorComboBoxItem
            {
                private Color color;
                private string name;

                public ColorComboBoxItem(Color color, string name)
                {
                    this.color = color;
                    this.name = name;
                }

                public Color Color
                {
                    set
                    {
                        color = value;
                    }
                    get
                    {
                        return color;
                    }
                }

                public string Name
                {
                    set
                    {
                        name = value;
                    }
                    get
                    {
                        return name;
                    }
                }
            }

            private Color selectedColor;

            private FontDialog fontDialog;
            /// <summary>
            /// 绑定颜色菜单
            /// </summary>
            /// <param name="fontDialog"></param>
            public ColorComboBox(FontDialog fontDialog)
            {
                this.fontDialog = fontDialog;

                DropDownStyle = ComboBoxStyle.DropDownList;
                DrawMode = DrawMode.OwnerDrawFixed;

                Items.AddRange(new object[] {
        new ColorComboBoxItem( Color.AliceBlue 				  , "AliceBlue "),
        new ColorComboBoxItem( Color.AntiqueWhite             , "AntiqueWhite "),
        new ColorComboBoxItem( Color.Aqua                     , "Aqua "),
        new ColorComboBoxItem( Color.Aquamarine               , "Aquamarine "),
        new ColorComboBoxItem( Color.Azure                    , "Azure "),
        new ColorComboBoxItem( Color.Beige                    , "Beige "),
        new ColorComboBoxItem( Color.Bisque                   , "Bisque "),
        new ColorComboBoxItem( Color.Black                    , "Black "),
        new ColorComboBoxItem( Color.BlanchedAlmond           , "BlanchedAlmond "),
        new ColorComboBoxItem( Color.Blue                     , "Blue "),
        new ColorComboBoxItem( Color.BlueViolet               , "BlueViolet "),
        new ColorComboBoxItem( Color.Brown                    , "Brown "),
        new ColorComboBoxItem( Color.BurlyWood                , "BurlyWood "),
        new ColorComboBoxItem( Color.CadetBlue                , "CadetBlue "),
        new ColorComboBoxItem( Color.Chartreuse               , "Chartreuse "),
        new ColorComboBoxItem( Color.Chocolate                , "Chocolate "),
        new ColorComboBoxItem( Color.Coral                    , "Coral "),
        new ColorComboBoxItem( Color.CornflowerBlue           , "CornflowerBlue "),
        new ColorComboBoxItem( Color.Cornsilk                 , "Cornsilk "),
        new ColorComboBoxItem( Color.Crimson                  , "Crimson "),
        new ColorComboBoxItem( Color.Cyan                     , "Cyan "),
        new ColorComboBoxItem( Color.DarkBlue                 , "DarkBlue "),
        new ColorComboBoxItem( Color.DarkCyan                 , "DarkCyan "),
        new ColorComboBoxItem( Color.DarkGoldenrod            , "DarkGoldenrod "),
        new ColorComboBoxItem( Color.DarkGray                 , "DarkGray "),
        new ColorComboBoxItem( Color.DarkGreen                , "DarkGreen "),
        new ColorComboBoxItem( Color.DarkKhaki                , "DarkKhaki "),
        new ColorComboBoxItem( Color.DarkMagenta              , "DarkMagenta "),
        new ColorComboBoxItem( Color.DarkOliveGreen           , "DarkOliveGreen "),
        new ColorComboBoxItem( Color.DarkOrange               , "DarkOrange "),
        new ColorComboBoxItem( Color.DarkOrchid               , "DarkOrchid "),
        new ColorComboBoxItem( Color.DarkRed                  , "DarkRed "),
        new ColorComboBoxItem( Color.DarkSalmon               , "DarkSalmon "),
        new ColorComboBoxItem( Color.DarkSeaGreen             , "DarkSeaGreen "),
        new ColorComboBoxItem( Color.DarkSlateBlue            , "DarkSlateBlue "),
        new ColorComboBoxItem( Color.DarkSlateGray            , "DarkSlateGray "),
        new ColorComboBoxItem( Color.DarkTurquoise            , "DarkTurquoise "),
        new ColorComboBoxItem( Color.DarkViolet               , "DarkViolet "),
        new ColorComboBoxItem( Color.DeepPink                 , "DeepPink "),
        new ColorComboBoxItem( Color.DeepSkyBlue              , "DeepSkyBlue "),
        new ColorComboBoxItem( Color.DimGray                  , "DimGray "),
        new ColorComboBoxItem( Color.DodgerBlue               , "DodgerBlue "),
        new ColorComboBoxItem( Color.Firebrick                , "Firebrick "),
        new ColorComboBoxItem( Color.FloralWhite              , "FloralWhite "),
        new ColorComboBoxItem( Color.ForestGreen              , "ForestGreen "),
        new ColorComboBoxItem( Color.Fuchsia                  , "Fuchsia "),
        new ColorComboBoxItem( Color.Gainsboro                , "Gainsboro "),
        new ColorComboBoxItem( Color.GhostWhite               , "GhostWhite "),
        new ColorComboBoxItem( Color.Gold                     , "Gold "),
        new ColorComboBoxItem( Color.Goldenrod                , "Goldenrod "),
        new ColorComboBoxItem( Color.Gray                     , "Gray "),
        new ColorComboBoxItem( Color.Green                    , "Green "),
        new ColorComboBoxItem( Color.GreenYellow              , "GreenYellow "),
        new ColorComboBoxItem( Color.Honeydew                 , "Honeydew "),
        new ColorComboBoxItem( Color.HotPink                  , "HotPink "),
        new ColorComboBoxItem( Color.IndianRed                , "IndianRed "),
        new ColorComboBoxItem( Color.Indigo                   , "Indigo "),
        new ColorComboBoxItem( Color.Ivory                    , "Ivory "),
        new ColorComboBoxItem( Color.Khaki                    , "Khaki "),
        new ColorComboBoxItem( Color.Lavender                 , "Lavender "),
        new ColorComboBoxItem( Color.LavenderBlush            , "LavenderBlush "),
        new ColorComboBoxItem( Color.LawnGreen                , "LawnGreen "),
        new ColorComboBoxItem( Color.LemonChiffon             , "LemonChiffon "),
        new ColorComboBoxItem( Color.LightBlue                , "LightBlue "),
        new ColorComboBoxItem( Color.LightCoral               , "LightCoral "),
        new ColorComboBoxItem( Color.LightCyan                , "LightCyan "),
        new ColorComboBoxItem( Color.LightGoldenrodYellow     , "LightGoldenrodYellow "),
        new ColorComboBoxItem( Color.LightGray                , "LightGray "),
        new ColorComboBoxItem( Color.LightGreen               , "LightGreen "),
        new ColorComboBoxItem( Color.LightPink                , "LightPink "),
        new ColorComboBoxItem( Color.LightSalmon              , "LightSalmon "),
        new ColorComboBoxItem( Color.LightSeaGreen            , "LightSeaGreen "),
        new ColorComboBoxItem( Color.LightSkyBlue             , "LightSkyBlue "),
        new ColorComboBoxItem( Color.LightSlateGray           , "LightSlateGray "),
        new ColorComboBoxItem( Color.LightSteelBlue           , "LightSteelBlue "),
        new ColorComboBoxItem( Color.LightYellow              , "LightYellow "),
        new ColorComboBoxItem( Color.Lime                     , "Lime "),
        new ColorComboBoxItem( Color.LimeGreen                , "LimeGreen "),
        new ColorComboBoxItem( Color.Linen                    , "Linen "),
        new ColorComboBoxItem( Color.Magenta                  , "Magenta "),
        new ColorComboBoxItem( Color.Maroon                   , "Maroon "),
        new ColorComboBoxItem( Color.MediumAquamarine         , "MediumAquamarine "),
        new ColorComboBoxItem( Color.MediumBlue               , "MediumBlue "),
        new ColorComboBoxItem( Color.MediumOrchid             , "MediumOrchid "),
        new ColorComboBoxItem( Color.MediumPurple             , "MediumPurple "),
        new ColorComboBoxItem( Color.MediumSeaGreen           , "MediumSeaGreen "),
        new ColorComboBoxItem( Color.MediumSlateBlue          , "MediumSlateBlue "),
        new ColorComboBoxItem( Color.MediumSpringGreen        , "MediumSpringGreen "),
        new ColorComboBoxItem( Color.MediumTurquoise          , "MediumTurquoise "),
        new ColorComboBoxItem( Color.MediumVioletRed          , "MediumVioletRed "),
        new ColorComboBoxItem( Color.MidnightBlue             , "MidnightBlue "),
        new ColorComboBoxItem( Color.MintCream                , "MintCream "),
        new ColorComboBoxItem( Color.MistyRose                , "MistyRose "),
        new ColorComboBoxItem( Color.Moccasin                 , "Moccasin "),
        new ColorComboBoxItem( Color.Navy                     , "Navy "),
        new ColorComboBoxItem( Color.OldLace                  , "OldLace "),
        new ColorComboBoxItem( Color.Olive                    , "Olive "),
        new ColorComboBoxItem( Color.OliveDrab                , "OliveDrab "),
        new ColorComboBoxItem( Color.Orange                   , "Orange "),
        new ColorComboBoxItem( Color.OrangeRed                , "OrangeRed "),
        new ColorComboBoxItem( Color.Orchid                   , "Orchid "),
        new ColorComboBoxItem( Color.PaleGoldenrod            , "PaleGoldenrod "),
        new ColorComboBoxItem( Color.PaleGreen                , "PaleGreen "),
        new ColorComboBoxItem( Color.PaleTurquoise            , "PaleTurquoise "),
        new ColorComboBoxItem( Color.PaleVioletRed            , "PaleVioletRed "),
        new ColorComboBoxItem( Color.PapayaWhip               , "PapayaWhip "),
        new ColorComboBoxItem( Color.PeachPuff                , "PeachPuff "),
        new ColorComboBoxItem( Color.Peru                     , "Peru "),
        new ColorComboBoxItem( Color.Pink                     , "Pink "),
        new ColorComboBoxItem( Color.Plum                     , "Plum "),
        new ColorComboBoxItem( Color.PowderBlue               , "PowderBlue "),
        new ColorComboBoxItem( Color.Purple                   , "Purple "),
        new ColorComboBoxItem( Color.Red                      , "Red "),
        new ColorComboBoxItem( Color.RosyBrown                , "RosyBrown "),
        new ColorComboBoxItem( Color.RoyalBlue                , "RoyalBlue "),
        new ColorComboBoxItem( Color.SaddleBrown              , "SaddleBrown "),
        new ColorComboBoxItem( Color.Salmon                   , "Salmon "),
        new ColorComboBoxItem( Color.SandyBrown               , "SandyBrown "),
        new ColorComboBoxItem( Color.SeaGreen                 , "SeaGreen "),
        new ColorComboBoxItem( Color.SeaShell                 , "SeaShell "),
        new ColorComboBoxItem( Color.Sienna                   , "Sienna "),
        new ColorComboBoxItem( Color.Silver                   , "Silver "),
        new ColorComboBoxItem( Color.SkyBlue                  , "SkyBlue "),
        new ColorComboBoxItem( Color.SlateBlue                , "SlateBlue "),
        new ColorComboBoxItem( Color.SlateGray                , "SlateGray "),
        new ColorComboBoxItem( Color.Snow                     , "Snow "),
        new ColorComboBoxItem( Color.SpringGreen              , "SpringGreen "),
        new ColorComboBoxItem( Color.SteelBlue                , "SteelBlue "),
        new ColorComboBoxItem( Color.Tan                      , "Tan "),
        new ColorComboBoxItem( Color.Teal                     , "Teal "),
        new ColorComboBoxItem( Color.Thistle                  , "Thistle "),
        new ColorComboBoxItem( Color.Tomato                   , "Tomato "),
        new ColorComboBoxItem( Color.Transparent              , "Transparent "),
        new ColorComboBoxItem( Color.Turquoise                , "Turquoise "),
        new ColorComboBoxItem( Color.Violet                   , "Violet "),
        new ColorComboBoxItem( Color.Wheat                    , "Wheat "),
        new ColorComboBoxItem( Color.White                    , "White "),
        new ColorComboBoxItem( Color.WhiteSmoke               , "WhiteSmoke "),
        new ColorComboBoxItem( Color.Yellow                   , "Yellow "),
        new ColorComboBoxItem( Color.YellowGreen              , "YellowGreen ")
                }
                           );

                SelectedIndex = 0;
            }
            /// <summary>
            /// 对Color框进行重绘
            /// </summary>
            /// <param name="e"></param>
            protected override void OnDrawItem(DrawItemEventArgs e)
            {
                if (e.Index == -1)
                    return;

                ColorComboBoxItem ccbi = Items[e.Index] as ColorComboBoxItem;

                Rectangle r = e.Bounds;
                r.X = r.X + 24;

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.DodgerBlue), e.Bounds); // bot blue
                    //e.Graphics.FillRectangle(new SolidBrush(ccbi.Color), e.Bounds.X + 3, e.Bounds.Y + 3, e.Bounds.X + 16, e.Bounds.Bottom - 3);
                    e.Graphics.FillRectangle(new SolidBrush(ccbi.Color), e.Bounds.X + 3, e.Bounds.Y + 3, e.Bounds.X + 16, e.Bounds.Height - 5);
                    e.Graphics.DrawRectangle(new Pen(Color.Black), e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.X + 17, e.Bounds.Height - 5);
                    e.Graphics.DrawString(ccbi.Name, this.Font, new SolidBrush(Color.White), r);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                    e.Graphics.FillRectangle(new SolidBrush(ccbi.Color), e.Bounds.X + 3, e.Bounds.Y + 3, e.Bounds.X + 16, e.Bounds.Height - 5);
                    e.Graphics.DrawRectangle(new Pen(Color.Black), e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.X + 17, e.Bounds.Height - 5);
                    e.Graphics.DrawString(ccbi.Name, this.Font, new SolidBrush(Color.Black), r);
                }
            }

            //protected override void OnSelectedIndexChanged(EventArgs e)
            //{
            //    ColorComboBoxItem ccbi = Items[SelectedIndex] as ColorComboBoxItem;
            //    selectedColor = ccbi.Color;

            //    fontDialog.Color = selectedColor;
            //}
        }

        public event EventHandler Apply
        {
            add { Events.AddHandler(EventApply, value); }
            remove { Events.RemoveHandler(EventApply, value); }
        }
    }

    [ToolboxItemFilter("System.Windows.Forms")]
    public abstract class CommonDialog : System.ComponentModel.Component
    {
        #region DialogForm
        internal class DialogForm : Form
        {
            #region DialogForm Local Variables
            protected CommonDialog owner;
            #endregion DialogForm Local Variables

            #region DialogForm Constructors
            internal DialogForm(CommonDialog owner)
            {
                this.owner = owner;
                ControlBox = true;
                MinimizeBox = false;
                MaximizeBox = false;
                ShowInTaskbar = false;
                FormBorderStyle = FormBorderStyle.Sizable;
            }
            #endregion DialogForm Constructors

            #region Protected Instance Properties
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp;

                    cp = base.CreateParams;

                    //cp.Style |= (int)(WindowStyles.WS_POPUP | WindowStyles.WS_CAPTION | WindowStyles.WS_SYSMENU);

                    return cp;
                }
            }
            #endregion	// Protected Instance Properties

            #region Internal Methods
            internal DialogResult RunDialog()
            {
                this.StartPosition = FormStartPosition.CenterScreen;

                owner.InitFormsSize(this);

                this.ShowDialog();

                return this.DialogResult;

            }
            #endregion Internal Methods
        }
        #endregion DialogForm

        #region Local Variables
        internal DialogForm form;
        #endregion Local Variables

        #region Public Constructors
        public CommonDialog()
        {
            form = new DialogForm(this);
        }
        #endregion Public Constructors

        #region Internal Methods
        internal virtual void InitFormsSize(Form form)
        {
            form.Width = 200;
            form.Height = 300;
        }
        #endregion Internal Methods

        #region Public Instance Methods
        public abstract void Reset();

        public DialogResult ShowDialog()
        {
            return ShowDialog(null);
        }

        public DialogResult ShowDialog(IWin32Window ownerWin32)
        {
            RunDialog(form.Handle);
            form.ShowDialog(ownerWin32);

            return form.DialogResult;
        }
        #endregion

        #region Protected Instance Methods
        protected virtual IntPtr HookProc(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            return IntPtr.Zero;
        }

        protected virtual void OnHelpRequest(EventArgs e)
        {
            if (HelpRequest != null)
            {
                HelpRequest(this, e);
            }
        }

        protected virtual IntPtr OwnerWndProc(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            return IntPtr.Zero;
        }

        protected abstract bool RunDialog(IntPtr hwndOwner);
        #endregion	// Protected Instance Methods

        #region Events
        public event EventHandler HelpRequest;
        #endregion	// Events
    }
}
