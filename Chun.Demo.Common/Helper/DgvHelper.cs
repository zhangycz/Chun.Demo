///**************Code Info**************************
//* 文 件 名： DgvHelper
//* 创 建 人： Zhengp
//* 创建日期：2012/8/1 14:53:00
//* 修 改 人：
//* 修改日期：
//* 备注描述：DataGridView帮助文档
//*           
//*************************************************/

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows.Forms;
//using System.IO;
//using System.Drawing;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Globalization;
//using System.Xml.Linq;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraGrid.Views.Base;

//namespace Chun.Demo.Common
//{
//    public static class DgvHelper
//    {
//        #region System.Windows.Forms.DataGridView

//        public static void InitDgv(DataGridView dgv, string configXml)
//        {
//            try
//            {
//                string path = AppDomain.CurrentDomain.BaseDirectory + @"\Config\DataGridView\" + configXml + ".xml";
//                if (!File.Exists(path))
//                {
//                    MessageBox.Show("访问该列表的配置文件" + configXml + ".xml不存在!", "信息提示", MessageBoxButtons.OK,
//                        MessageBoxIcon.Warning);
//                    return;
//                }
//                XDocument xml = XDocument.Load(path);
//                string autosizemode = "";
//                if (xml.Root?.Attribute("Autosizemode") != null)
//                    autosizemode = xml.Root.Attribute("Autosizemode").Value;
//                if (xml.Root?.Attribute("HeaderVisible") != null)
//                {
//                }

//                // 使用查询语法获取Person集合
//                if (xml.Root != null)
//                {
//                    var columes = from p in xml.Root.Elements("Colume")
//                        select new //HeaderColume
//                        {
//                            Width = p.Attribute("Width").Value,
//                            Name = p.Attribute("Name").Value,
//                            DataPropertyName = p.Attribute("DataPropertyName").Value,
//                            Visible = bool.Parse(p.Attribute("Visible").Value),
//                            Frozen = bool.Parse(p.Attribute("Frozen").Value),
//                            ReadOnly = bool.Parse(p.Attribute("ReadOnly").Value),
//                            ColumnType = p.Attribute("ColumnType").Value,
//                            // Format = p.Attribute("Format").Value
//                        };
//                    //设定DataGridView单元格样式
//                    string fontFamily = "宋体", fontStyle = "0", fontSize = "11.0";
//                    dgv.Columns.Clear();
//                    dgv.AutoGenerateColumns = false;
//                    dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
//                    dgv.MultiSelect = false;
//                    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
//                    dgv.AllowUserToResizeRows = false;
//                    dgv.RowHeadersVisible = false;
//                    dgv.RowsDefaultCellStyle.Font = new Font(fontFamily, float.Parse(fontSize),
//                        GetFontStyle(int.Parse(fontStyle)));
//                    switch (autosizemode)
//                    {
//                        case "None":
//                            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
//                            break;
//                        case "Fill":
//                            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
//                            break;
//                        case "CellMessage":
//                            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
//                            break;
//                        default:
//                            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
//                            break;
//                    }
//                    // dgv.RowsDefaultCellStyle.Font = new Font("宋体", 11);// FontStyle.Strikeout);  
//                    DataGridViewColumn dgvColumn = null;
//                    foreach (var item in columes)
//                    {
//                        switch (item.ColumnType.ToLower())
//                        {
//                            case "check":
//                                DataGridViewCheckBoxColumn cell = new DataGridViewCheckBoxColumn();
//                                cell.TrueValue = 1;
//                                cell.FalseValue = 0;
//                                dgvColumn = cell;
//                                dgvColumn.CellTemplate = new DataGridViewCheckBoxCell();
//                                break;
//                            default:
//                                dgvColumn = new DataGridViewTextBoxColumn();
//                                break;
//                        }

//                        dgvColumn.HeaderText = item.Name;
//                        dgvColumn.Name = item.DataPropertyName;
//                        dgvColumn.DataPropertyName = item.DataPropertyName;
//                        dgvColumn.Visible = item.Visible;
//                        dgvColumn.ReadOnly = item.ReadOnly;
//                        dgvColumn.Frozen = false;
//                        if (!string.IsNullOrEmpty(item.Width))
//                            dgvColumn.Width = int.Parse(item.Width);
//                        else
//                            dgvColumn.Width = 100;
//                        dgvColumn.MinimumWidth = 50;
//                        //dgvColumn.DefaultCellStyle.Format = string.IsNullOrEmpty(item.Format) ? "" : item.Format;
//                        dgv.Columns.Add(dgvColumn);
//                    }
//                    dgv.Columns[(columes.Count() - 1)].MinimumWidth = 200;
//                    dgv.Columns[(columes.Count() - 1)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
//                }
//            }
//            catch (Exception ex)
//            {
//                Trace.WriteLine(ex.ToString());
//                // MessageBoxEx.Show("搜索结果模板加载失败", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//            }
//        }

//        private static ListSortDirection direction = ListSortDirection.Descending;


//        private static void dgv_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
//        {
//            DataGridView grid = (DataGridView) sender;
//            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
//                e.RowBounds.Location.Y,
//                grid.RowHeadersWidth - 4,
//                e.RowBounds.Height);

//            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
//                grid.RowHeadersDefaultCellStyle.Font,
//                rectangle,
//                grid.RowHeadersDefaultCellStyle.ForeColor,
//                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
//        }

//        private static void dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
//        {
//            DataGridView dgv = (DataGridView) sender;
//            if (direction.Equals(ListSortDirection.Descending))
//            {
//                dgv.Sort(dgv.Columns[e.ColumnIndex], ListSortDirection.Ascending);
//                direction = ListSortDirection.Ascending;
//            }
//            else
//            {
//                dgv.Sort(dgv.Columns[e.ColumnIndex], ListSortDirection.Descending);
//                direction = ListSortDirection.Descending;
//            }
//        }

//        /// <summary>
//        /// 获取gridview对应的键值对键值对  (FieldName, HeadText)
//        /// </summary>
//        /// <param name="dgv"></param>
//        /// <returns></returns>
//        public static Dictionary<string, string> GetFieldHeadTextKeyValue(DataGridView dgv)
//        {
//            //键值对  FieldName HeadText
//            Dictionary<string, string> dicKeyValue = new Dictionary<string, string>();

//            for (int i = 0; i < dgv.ColumnCount; i++)
//            {
//                if (dgv.Columns[i].Visible && dgv.Columns[i].Width > 0 &&
//                    !string.IsNullOrEmpty(dgv.Columns[i].DataPropertyName))
//                {
//                    dicKeyValue.Add(dgv.Columns[i].DataPropertyName, dgv.Columns[i].HeaderText);
//                }
//            }
//            return dicKeyValue;
//        }

//        #endregion

//        #region DevExpress.XtraGrid.Views.Grid.GridView

//        public static void InitDgv(GridView dgv, string configXml)
//        {
//            InitDgv(dgv, configXml, 1); //默认，显示方式1.
//        }


//        /// <summary>
//        /// 都去序列化文件
//        /// </summary>
//        /// <param name="dgv"></param>
//        /// <param name="configXml"></param>
//        /// <param name="isSerialize">true</param>
//        public static void InitDgv(DevExpress.XtraGrid.Views.Grid.GridView dgv, string configXml, bool isSerialize)
//        {
//            try
//            {
//                if (!isSerialize) return;
//                string path = AppDomain.CurrentDomain.BaseDirectory + @"Config\DataGridView\" + configXml + ".dat";
//                if (!File.Exists(path))
//                {
//                    MessageBox.Show("访问该列表的配置文件" + configXml + ".dat不存在!", "信息提示", MessageBoxButtons.OK,
//                        MessageBoxIcon.Warning);
//                    return;
//                }
//                dgv.Tag = path;
//                DgvXml xml = (DgvXml) FileSerializeOper.Deserialize(path);
//                string autosizemode = "";
//                if (xml.Autosizemode != null)
//                    autosizemode = xml.Autosizemode;
//                bool headerVisible = false;
//                if (xml.HeaderVisible != null)
//                {
//                    headerVisible = bool.Parse(xml.HeaderVisible);
//                }
//                bool showAutoFilterRow = false, multiSelect = false, allowGroup = false, enableColumnMenu = false;

//                showAutoFilterRow = xml.ShowAutoFilterRow;
//                multiSelect = xml.MultiSelect;
//                allowGroup = xml.AllowGroup;
//                enableColumnMenu = xml.EnableColumnMenu;
//                // 使用查询语法获取Person集合
//                var columes = xml.columes.Select(p => new
//                {
//                    p.Width,
//                    p.Name,
//                    p.DataPropertyName,
//                    p.Visible,
//                    Frozen = p.Frozen,
//                    ReadOnly = p.ReadOnly,
//                    ColumnType = p.ColumnType
//                });
//                dgv.Columns.Clear();
//                //列宽true为充满，false为自适应
//                dgv.DragObjectDrop += dgv_DragObjectDrop;

//                dgv.OptionsView.ColumnAutoWidth = false;
//                if (headerVisible)
//                {
//                    dgv.CustomDrawRowIndicator +=
//                        dgv_CustomDrawRowIndicator;
//                    dgv.IndicatorWidth = 30;
//                }
//                //当视图中没有某些数据源中的字段时，在视图中自动创建这些列
//                dgv.OptionsBehavior.AutoPopulateColumns = false;
//                //编辑表
//                dgv.OptionsBehavior.Editable = true;
//                //选择行
//                dgv.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
//                //允许多选行 
//                dgv.OptionsSelection.MultiSelect = multiSelect;
//                dgv.OptionsCustomization.AllowQuickHideColumns = false;
//                dgv.OptionsCustomization.AllowGroup = allowGroup;
//                dgv.OptionsMenu.EnableColumnMenu = enableColumnMenu;
//                //每一行自动根据单元格的内容调整高度
//                dgv.OptionsView.RowAutoHeight = true;
//                //默认第一行显示搜索
//                dgv.OptionsView.ShowAutoFilterRow = showAutoFilterRow;
//                //隐藏groupPanel
//                dgv.OptionsView.ShowGroupPanel = false;
//                dgv.OptionsView.EnableAppearanceOddRow = true;

//                string fontFamily = "微软雅黑",
//                    fontStyle = "0",
//                    fontSize = "11.0",
//                    fontColor = "Black",
//                    oddColor = "WhiteSmoke",
//                    headColor = "Black",
//                    backColor = "White",
//                    selectColor = "SteelBlue"; //设置默认样式;

//                string values = ConfigerHelper.LoadAppSetting("Gridview");
//                decimal allEdit = 0;
//                decimal oneEdit = 0;
//                if (!string.IsNullOrEmpty(values))
//                {
//                    foreach (string tmp in values.Split('|'))
//                    {
//                        if (tmp.Split(':')[0] == "FontFamily")
//                            fontFamily = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "FontStyle")
//                            fontStyle = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "FontSize")
//                            fontSize = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "FontColor")
//                            fontColor = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "BackColor")
//                            backColor = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "SelectColor")
//                            selectColor = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "OddColor")
//                            oddColor = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "HeadColor")
//                            headColor = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "EditTime")
//                            allEdit = decimal.Parse(tmp.Split(':')[1]);
//                    }
//                }

//                if (xml.EditTime != null)
//                    oneEdit = decimal.Parse(xml.EditTime);

//                if (oneEdit >= allEdit)
//                {
//                    if (xml.FontFamily != null)
//                        fontFamily = xml.FontFamily;
//                    if (xml.FontStyle != null)
//                        fontStyle = xml.FontStyle;
//                    if (xml.FontSize != null)
//                        fontSize = xml.FontSize;
//                    if (xml.FontColor != null)
//                        fontColor = xml.FontColor;
//                    if (xml.BackColor != null)
//                        backColor = xml.BackColor;
//                    if (xml.SelectColor != null)
//                        selectColor = xml.SelectColor;
//                    if (xml.OddColor != null)
//                        oddColor = xml.OddColor;
//                    if (xml.HeadColor != null)
//                        headColor = xml.HeadColor;
//                }
//                //显示居中
//                //水平
//                dgv.Appearance.FocusedRow.TextOptions.HAlignment =
//                    dgv.Appearance.SelectedRow.TextOptions.HAlignment =
//                        dgv.Appearance.OddRow.TextOptions.HAlignment =
//                            dgv.Appearance.Row.TextOptions.HAlignment =
//                                dgv.Appearance.HeaderPanel.TextOptions.HAlignment =
//                                    DevExpress.Utils.HorzAlignment.Center;
//                //上下
//                dgv.Appearance.FocusedRow.TextOptions.VAlignment =
//                    dgv.Appearance.SelectedRow.TextOptions.VAlignment =
//                        dgv.Appearance.OddRow.TextOptions.VAlignment =
//                            dgv.Appearance.Row.TextOptions.VAlignment =
//                                dgv.Appearance.HeaderPanel.TextOptions.VAlignment =
//                                    DevExpress.Utils.VertAlignment.Center;
//                //标题大小
//                dgv.Appearance.HeaderPanel.ForeColor = Color.FromName(headColor);
//                dgv.Appearance.HeaderPanel.Font = new Font("微软雅黑", 11);
//                //显示颜色大小
//                dgv.Appearance.Row.Font = new Font(fontFamily, float.Parse(fontSize), GetFontStyle(int.Parse(fontStyle)));
//                dgv.Appearance.Row.BackColor = Color.FromName(backColor);
//                dgv.Appearance.Row.ForeColor = Color.FromName(fontColor);
//                dgv.Appearance.OddRow.BackColor = Color.FromName(oddColor);

//                //选中时行的背景色和前景色怎么设置
//                dgv.Appearance.HideSelectionRow.BackColor =
//                    dgv.Appearance.FocusedCell.BackColor = dgv.Appearance.SelectedRow.BackColor =
//                        dgv.Appearance.FocusedRow.BackColor =
//                            Color.FromName(selectColor);
//                dgv.Appearance.FocusedRow.ForeColor = Color.FromName(fontColor);
//                dgv.Appearance.FocusedRow.Font = new Font(fontFamily, float.Parse(fontSize),
//                    GetFontStyle(int.Parse(fontStyle)));


//                int width = 0;
//                foreach (var item in columes)
//                {
//                    int i = 0;
//                    var dgvColumn = new DevExpress.XtraGrid.Columns.GridColumn();
//                    switch (item.ColumnType.ToLower())
//                    {
//                        case "check":
//                            DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1 =
//                                new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//                            dgv.GridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.
//                                RepositoryItem[]
//                            {
//                                repositoryItemCheckEdit1
//                            });
//                            repositoryItemCheckEdit1.AutoHeight = false;
//                            repositoryItemCheckEdit1.Name = item.DataPropertyName;
//                            dgvColumn.ColumnEdit = repositoryItemCheckEdit1;
//                            break;
//                    }

                 
//                            dgvColumn.Name = item.DataPropertyName;
//                            dgvColumn.FieldName = item.DataPropertyName;
//                    dgvColumn.Caption = item.Name;


//                    dgvColumn.Width = string.IsNullOrEmpty(item.Width) ? 100 : int.Parse(item.Width);

//                    if (item.ReadOnly)
//                    {
//                        dgvColumn.OptionsColumn.ReadOnly = true;
//                        dgvColumn.OptionsColumn.AllowEdit = false;
//                        dgvColumn.OptionsColumn.AllowFocus = false;
//                    }
//                    else
//                    {
//                        dgvColumn.OptionsColumn.ReadOnly = false;
//                        dgvColumn.OptionsColumn.AllowEdit = true;
//                        dgvColumn.OptionsColumn.AllowFocus = true;
//                    }
//                    if (item.Visible)
//                    {
//                        dgvColumn.Visible = true;
//                        dgvColumn.VisibleIndex = i;
//                        width += dgvColumn.Width;
//                    }
//                    else
//                    {
//                        dgvColumn.Visible = false;
//                        dgvColumn.VisibleIndex = -1;
//                    }
//                    dgv.Columns.Add(dgvColumn);
//                    i++;
//                }
//                if (dgv.GridControl.Width > width)
//                {
//                    dgv.OptionsView.ColumnAutoWidth = true;
//                }

//                //绑定设置菜单
//                BindContextMenu(dgv, BuildSeetingStripMenu(dgv));
//                //绑定导出Excel菜单
//                BindContextMenu(dgv, Export(dgv));
//            }
//            catch (Exception ex)
//            {
//                //  Trace.WriteLine(ex.ToString());
//                MessageBox.Show("搜索结果模板加载失败" + ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//            }
//        }

//        private static void InitDgv(GridView dgv, string configXml, int showType)
//        {
//            try
//            {
//                string path = AppDomain.CurrentDomain.BaseDirectory + @"Config\DataGridView\" + configXml + ".xml";
//                if (!File.Exists(path))
//                {
//                    MessageBox.Show("访问该列表的配置文件" + configXml + ".xml不存在!", "信息提示", MessageBoxButtons.OK,
//                        MessageBoxIcon.Warning);
//                    return;
//                }
//                dgv.Tag = path;
//                XDocument xml = XDocument.Load(path);
//                string autosizemode = "";
//                if (xml.Root.Attribute("Autosizemode") != null)
//                    autosizemode = xml.Root.Attribute("Autosizemode").Value;
//                bool headerVisible = false;
//                if (xml.Root.Attribute("HeaderVisible") != null)
//                {
//                    headerVisible = bool.Parse(xml.Root.Attribute("HeaderVisible").Value);
//                }
//                bool showAutoFilterRow = true, multiSelect = true, allowGroup = false, enableColumnMenu = false;

//                if (xml.Root.Attribute("ShowAutoFilterRow") != null)
//                {
//                    showAutoFilterRow = bool.Parse(xml.Root.Attribute("ShowAutoFilterRow").Value);
//                }
//                if (xml.Root.Attribute("MultiSelect") != null)
//                {
//                    multiSelect = bool.Parse(xml.Root.Attribute("MultiSelect").Value);
//                }
//                if (xml.Root.Attribute("AllowGroup") != null)
//                {
//                    allowGroup = bool.Parse(xml.Root.Attribute("AllowGroup").Value);
//                }
//                if (xml.Root.Attribute("EnableColumnMenu") != null)
//                {
//                    enableColumnMenu = bool.Parse(xml.Root.Attribute("EnableColumnMenu").Value);
//                }
//                // 使用查询语法获取Person集合
//                var Columes = from p in xml.Root.Elements("Colume")
//                    select new
//                    {
//                        ColColor = p.Attribute("Color") == null ? "Black" : p.Attribute("Color").Value,
//                        Width = p.Attribute("Width").Value,
//                        Name = p.Attribute("Name").Value,
//                        DataPropertyName = p.Attribute("DataPropertyName").Value,
//                        Visible = bool.Parse(p.Attribute("Visible").Value),
//                        Frozen = bool.Parse(p.Attribute("Frozen").Value),
//                        ReadOnly = bool.Parse(p.Attribute("ReadOnly").Value),
//                        ColumnType = p.Attribute("ColumnType").Value
//                    };
//                dgv.Columns.Clear();
//                //列宽true为充满，false为自适应
//                dgv.DragObjectDrop += new DevExpress.XtraGrid.Views.Base.DragObjectDropEventHandler(dgv_DragObjectDrop);
//                if (showType == 2)
//                    dgv.CustomDrawCell +=
//                        new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(gridView1_CustomDrawCell);
                
//                dgv.OptionsView.ColumnAutoWidth = false;
//                if (headerVisible)
//                {
//                    dgv.CustomDrawRowIndicator +=
//                        new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(dgv_CustomDrawRowIndicator);
//                    dgv.IndicatorWidth = 70;
//                }
//                //当视图中没有某些数据源中的字段时，在视图中自动创建这些列
//                dgv.OptionsBehavior.AutoPopulateColumns = false;
//                //编辑表
//                dgv.OptionsBehavior.Editable = true;
//                //选择行
//                dgv.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
//                //允许多选行 
//                dgv.OptionsSelection.MultiSelect = multiSelect;
//                dgv.OptionsCustomization.AllowQuickHideColumns = false;
//                dgv.OptionsCustomization.AllowGroup = allowGroup;
//                dgv.OptionsMenu.EnableColumnMenu = enableColumnMenu;
//                //每一行自动根据单元格的内容调整高度
//                dgv.OptionsView.RowAutoHeight = true;
//                //默认第一行显示搜索
//                dgv.OptionsView.ShowAutoFilterRow = showAutoFilterRow;
//                //隐藏groupPanel
//                dgv.OptionsView.ShowGroupPanel = false;
//                ///允许奇数行编辑
//                dgv.OptionsView.EnableAppearanceOddRow = false;

//                string fontFamily = "微软雅黑",
//                    fontStyle = "0",
//                    fontSize = "11.0",
//                    fontColor = "Black",
//                    oddColor = "WhiteSmoke",
//                    headColor = "Black",
//                    backColor = "White",
//                    selectColor = "SteelBlue"; //设置默认样式;

//                string values = ConfigerHelper.LoadAppSetting("Gridview");
//                decimal allEdit = 0;
//                decimal oneEdit = 0;
//                if (!string.IsNullOrEmpty(values))
//                {
//                    foreach (string tmp in values.Split('|'))
//                    {
//                        if (tmp.Split(':')[0] == "FontFamily")
//                            fontFamily = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "FontStyle")
//                            fontStyle = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "FontSize")
//                            fontSize = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "FontColor")
//                            fontColor = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "BackColor")
//                            backColor = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "SelectColor")
//                            selectColor = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "OddColor")
//                            oddColor = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "HeadColor")
//                            headColor = tmp.Split(':')[1];
//                        else if (tmp.Split(':')[0] == "EditTime")
//                            allEdit = decimal.Parse(tmp.Split(':')[1]);
//                    }
//                }
//                if (xml.Element("Header").HasAttributes)
//                {
//                    if (xml.Element("Header").Attribute("EditTime") != null)
//                        oneEdit = decimal.Parse(xml.Element("Header").Attribute("EditTime").Value);
//                }

//                if (xml.Element("Header").HasAttributes && oneEdit >= allEdit)
//                {
//                    if (xml.Element("Header").Attribute("FontFamily") != null)
//                        fontFamily = xml.Element("Header").Attribute("FontFamily").Value;
//                    if (xml.Element("Header").Attribute("FontStyle") != null)
//                        fontStyle = xml.Element("Header").Attribute("FontStyle").Value;
//                    if (xml.Element("Header").Attribute("FontSize") != null)
//                        fontSize = xml.Element("Header").Attribute("FontSize").Value;
//                    if (xml.Element("Header").Attribute("FontColor") != null)
//                        fontColor = xml.Element("Header").Attribute("FontColor").Value;
//                    if (xml.Element("Header").Attribute("BackColor") != null)
//                        backColor = xml.Element("Header").Attribute("BackColor").Value;
//                    if (xml.Element("Header").Attribute("SelectColor") != null)
//                        selectColor = xml.Element("Header").Attribute("SelectColor").Value;
//                    if (xml.Element("Header").Attribute("OddColor") != null)
//                        oddColor = xml.Element("Header").Attribute("OddColor").Value;
//                    if (xml.Element("Header").Attribute("HeadColor") != null)
//                        headColor = xml.Element("Header").Attribute("HeadColor").Value;
//                }
//                //显示居中
//                //水平
//                dgv.Appearance.FocusedRow.TextOptions.HAlignment =
//                    dgv.Appearance.SelectedRow.TextOptions.HAlignment =
//                        dgv.Appearance.OddRow.TextOptions.HAlignment =
//                            dgv.Appearance.Row.TextOptions.HAlignment =
//                                dgv.Appearance.HeaderPanel.TextOptions.HAlignment =
//                                    DevExpress.Utils.HorzAlignment.Center;
//                //上下
//                dgv.Appearance.FocusedRow.TextOptions.VAlignment =
//                    dgv.Appearance.SelectedRow.TextOptions.VAlignment =
//                        dgv.Appearance.OddRow.TextOptions.VAlignment =
//                            dgv.Appearance.Row.TextOptions.VAlignment =
//                                dgv.Appearance.HeaderPanel.TextOptions.VAlignment =
//                                    DevExpress.Utils.VertAlignment.Center;
//                //标题大小
//                dgv.Appearance.HeaderPanel.ForeColor = Color.FromName(headColor);
//                dgv.Appearance.HeaderPanel.Font = new Font("微软雅黑", 11);
//                //显示颜色大小
//                dgv.Appearance.Row.Font = new Font(fontFamily, float.Parse(fontSize), GetFontStyle(int.Parse(fontStyle)));
//                dgv.Appearance.Row.BackColor = Color.FromName(backColor);
//                dgv.Appearance.Row.ForeColor = Color.FromName(fontColor);
//                dgv.Appearance.OddRow.BackColor = Color.FromName(oddColor);

//                //选中时行的背景色和前景色怎么设置
//                dgv.Appearance.HideSelectionRow.BackColor =
//                    dgv.Appearance.SelectedRow.BackColor =
//                        dgv.Appearance.FocusedCell.BackColor =
//                            dgv.Appearance.FocusedRow.BackColor =
//                                (showType == 2 ? Color.Transparent : Color.FromName(selectColor));
//                //   Color.FromName(selectColor);
//                dgv.Appearance.FocusedRow.ForeColor = Color.FromName(fontColor);
//                dgv.Appearance.FocusedRow.Font = new Font(fontFamily, float.Parse(fontSize),
//                    GetFontStyle(int.Parse(fontStyle)));


//                DevExpress.XtraGrid.Columns.GridColumn dgvColumn;
//                int width = 0;
//                int i = 0;
//                foreach (var item in Columes)
//                {
//                    dgvColumn = new DevExpress.XtraGrid.Columns.GridColumn();
//                    switch (item.ColumnType.ToLower())
//                    {
//                        case "check":
//                            DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1 =
//                                new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//                            dgv.GridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.
//                                RepositoryItem[]
//                            {
//                                repositoryItemCheckEdit1
//                            });
//                            repositoryItemCheckEdit1.AutoHeight = false;
//                            repositoryItemCheckEdit1.Name = item.DataPropertyName;
//                            dgvColumn.ColumnEdit = repositoryItemCheckEdit1;
//                            break;
//                    }

                 
//                    dgvColumn.Name = item.DataPropertyName;
//                    dgvColumn.FieldName = item.DataPropertyName;
//                    dgvColumn.Caption = item.Name;


//                    dgvColumn.Width = string.IsNullOrEmpty(item.Width) ? 100 : int.Parse(item.Width);


//                    if (item.ReadOnly)
//                    {
//                        dgvColumn.OptionsColumn.ReadOnly = true;
//                        dgvColumn.OptionsColumn.AllowEdit = false;
//                        dgvColumn.OptionsColumn.AllowFocus = false;
//                    }
//                    else
//                    {
//                        dgvColumn.OptionsColumn.ReadOnly = false;
//                        dgvColumn.OptionsColumn.AllowEdit = true;
//                        dgvColumn.OptionsColumn.AllowFocus = true;
//                    }
//                    switch (item.ColumnType)
//                    {
//                        case "date":
//                            dgvColumn.DisplayFormat.FormatString = "yyyy-MM-dd";
//                            dgvColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
//                            break;
//                            ;
//                        case "time":
//                            dgvColumn.DisplayFormat.FormatString = "HH:mm";
//                            dgvColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
//                            break;
//                        case "datetime":
//                            dgvColumn.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
//                            dgvColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
//                            break;
//                    }

//                    if (item.Visible)
//                    {
//                        dgvColumn.Visible = true;
//                        dgvColumn.VisibleIndex = i;
//                        width += dgvColumn.Width;
//                    }

//                    else
//                    {
//                        dgvColumn.Visible = false;
//                        dgvColumn.VisibleIndex = -1;
//                    }
//                    if (item.Frozen)
//                        dgvColumn.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

//                    dgv.Columns.Add(dgvColumn);
//                    i++;
//                }
//                if (dgv.GridControl.Width > width)
//                {
//                    dgv.OptionsView.ColumnAutoWidth = true;
//                }
//                //绑定设置菜单
//                BindContextMenu(dgv, BuildSeetingStripMenu(dgv));
//                //绑定导出Excel菜单
//                BindContextMenu(dgv, Export(dgv));
//                // dgv.Columns[(Columes.Count() - 1)].MinimumWidth = 200;
//                //  dgv.Columns[(Columes.Count() - 1)].OptionsFilter =DevExpress.XtraGrid.Views.Grid.;
//            }
//            catch (Exception ex)
//            {
//                //  Trace.WriteLine(ex.ToString());
//                MessageBox.Show("搜索结果模板加载失败" + ex.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//            }
//        }

//        private static void dgv_CustomDrawRowIndicator(object sender,
            
//            RowIndicatorCustomDrawEventArgs e)
//        {
//            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
//            if (e.Info.IsRowIndicator)
//            {
//                if (e.RowHandle >= 0)
//                {
//                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
//                }
//                else if (e.RowHandle < 0 && e.RowHandle > -1000)
//                {
//                    e.Info.Appearance.BackColor = Color.AntiqueWhite;
//                    e.Info.DisplayText = "G" + e.RowHandle.ToString();
//                }
//            }
//        }

//        private static void dgv_DragObjectDrop(object sender, DragObjectDropEventArgs e)
//        {
//            if (e != null)
//            {
//                GridView dgv = (GridView) sender;
//                string path = dgv.Tag.ToString();
//                if (path.Remove(0, path.Length - 4) == ".xml")
//                {
//                    XDocument xml = XDocument.Load(path);
//                    DevExpress.XtraGrid.Columns.GridColumn column =
//                        (DevExpress.XtraGrid.Columns.GridColumn) (e.DragObject);
//                    int i = 0;
//                    XElement moveNode = null;
//                    if (xml.Root != null)
//                    {
//                        foreach (XElement node in xml.Root.Elements("Colume"))
//                        {
//                            if (node.Attribute("DataPropertyName").Value == column.Name)
//                            {
//                                moveNode = node;
//                                node.Remove();
//                            }
//                        }
//                        foreach (XElement node in xml.Root.Elements("Colume"))
//                        {
//                            if (bool.Parse(node.Attribute("Visible").Value) && column.VisibleIndex == i && moveNode != null)
//                            {
//                                node.AddBeforeSelf(moveNode);
//                            }
//                            if (bool.Parse(node.Attribute("Visible").Value))
//                            {
//                                i++;
//                            }
//                        }
//                    }
//                    xml.Save(path);
//                }
//                else
//                {
//                    DgvXml xml = (DgvXml) FileSerializeOper.Deserialize(path);
//                    DevExpress.XtraGrid.Columns.GridColumn column =
//                        (DevExpress.XtraGrid.Columns.GridColumn) (e.DragObject);
//                    int i = 0;
//                    int index = 0;
//                    foreach (Colume node in xml.columes)
//                    {
//                        if (node.Visible && column.VisibleIndex == i)
//                        {
//                            index = xml.columes.IndexOf(node);
//                            break;
//                        }
//                        if (node.Visible)
//                        {
//                            i++;
//                        }
//                    }
//                    foreach (Colume node in xml.columes)
//                    {
//                        if (node.DataPropertyName == column.Name)
//                        {
//                            xml.columes.Remove(node);
//                            xml.columes.Insert(index, node);
//                            break;
//                        }
//                    }

//                    File.Delete(path);
//                    FileSerializeOper.SetSerialize(xml, path);
//                }
//            }
//        }

//        #endregion

//        #region 公共方法

//        /// <summary>
//        /// 根据参数设置字体样式
//        /// </summary>
//        /// <param name="item"></param>
//        /// <returns></returns>
//        private static FontStyle GetFontStyle(int item)
//        {
//            switch (item)
//            {
//                case 0:
//                    return FontStyle.Regular;
//                case 1:
//                    return FontStyle.Bold;
//                case 2:
//                    return FontStyle.Italic;
//                case 4:
//                    return FontStyle.Underline;
//                case 8:
//                    return FontStyle.Strikeout;
//                default:
//                    return FontStyle.Regular;
//            }
//        }
        
//        /// <summary>
//        /// 根据字体样式返回一个Int值
//        /// </summary>
//        /// <param name="style"></param>
//        /// <returns></returns>
//        private static int SetFontStyle(FontStyle style)
//        {
//            switch (style)
//            {
//                case FontStyle.Regular:
//                    return 0;
//                case FontStyle.Bold:
//                    return 1;
//                case FontStyle.Italic:
//                    return 2;
//                case FontStyle.Underline:
//                    return 4;
//                case FontStyle.Strikeout:
//                    return 8;
//                default:
//                    return 0;
//            }
//        }

//        /// <summary>
//        /// 给xml节点增加属性值,保存实现在外部实现
//        /// </summary>
//        /// <param name="xml">xml实体</param>
//        /// <param name="nodeName">节点名称</param>
//        /// <param name="name">节点属性名称</param>
//        /// <param name="value">节点属性值</param>
//        private static void AddAttribute(XDocument xml, string nodeName, string name, string value)
//        {
//            var xElement = xml.Element(nodeName);
//            if (xElement != null && xElement.HasAttributes)
//            {
//                var element = xml.Element(nodeName);
//                if (element != null && element.Attribute(name) == null)
//                {
//                    var o = xml.Element(nodeName);
//                    o?.SetAttributeValue(name, value);
//                }
//                else
//                {
//                    var xElement1 = xml.Element(nodeName);
//                    xElement1?.Attribute(name).SetValue(value);
//                }
//            }
//            else
//            {
//                var element = xml.Element(nodeName);
//                element?.SetAttributeValue(name, value);
//            }
//        }

//        #region 绑定菜单

//        /// <summary>
//        /// 绑定菜单
//        /// </summary>
//        /// <param name="dgv"></param>
//        /// <param name="item"></param>
//        private static void BindContextMenu(BaseView dgv, ToolStripItem item)
//        {
//            if (dgv.GridControl.ContextMenuStrip != null)
//            {
//                bool canAdd = true;
//                dgv.GridControl.ContextMenuStrip.AllowMerge = true;
//                foreach (ToolStripItem tsItem in dgv.GridControl.ContextMenuStrip.Items)
//                {
//                    if (tsItem.Name == item.Name)
//                    {
//                        canAdd = false;
//                        break;
//                    }
//                }
//                if (canAdd)
//                    dgv.GridControl.ContextMenuStrip.Items.Add(item);
//            }
//            else
//            {
//                ContextMenuStrip menu = new ContextMenuStrip();
//                //menu.AllowMerge = true;

//                menu.Items.Add(item);
//                dgv.GridControl.ContextMenuStrip = menu;
//                //dgv.ContextMenuStrip.AllowMerge = true;
//            }
//        }

//        /// <summary>
//        /// 设置按钮
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private static void item_Click(object sender, EventArgs e)
//        {
//            ToolStripMenuItem ob = (ToolStripMenuItem) sender;

//            GridView ownerdgv = (GridView) ob?.Tag;
//            if (ownerdgv != null)
//            {
//                string xmlFilePath = ownerdgv.Tag.ToString();


//                //可以针对该FontDialog进行重写,进而达到可以在此处设置奇偶行颜色的接口
//                FontDialog font = new FontDialog
//                {
//                    ShowApply = false,
//                    ShowHelp = false,
//                    ShowColor = true,
//                    ShowOddLine = true,
//                    Color = ownerdgv.Appearance.Row.ForeColor,
//                    Font = ownerdgv.Appearance.Row.Font,
//                    BackColor = ownerdgv.Appearance.Row.BackColor,
//                    SelectColor = ownerdgv.Appearance.FocusedRow.BackColor,
//                    HeadColor = ownerdgv.Appearance.HeaderPanel.ForeColor,
//                    OddColor = ownerdgv.Appearance.OddRow.BackColor
//                };
//                if (font.ShowDialog() == DialogResult.OK)
//                {
//                    ownerdgv.Appearance.FocusedRow.Font = ownerdgv.Appearance.Row.Font = font.Font;
//                    ownerdgv.Appearance.Row.BackColor = font.BackColor;
//                    ownerdgv.Appearance.FocusedRow.ForeColor = ownerdgv.Appearance.Row.ForeColor = font.Color;
//                    //标题大小
//                    ownerdgv.Appearance.HideSelectionRow.BackColor =
//                        ownerdgv.Appearance.FocusedCell.BackColor = ownerdgv.Appearance.SelectedRow.BackColor =
//                            ownerdgv.Appearance.FocusedRow.BackColor = font.SelectColor;
//                    ownerdgv.Appearance.OddRow.BackColor = font.OddColor;
//                    ownerdgv.Appearance.HeaderPanel.ForeColor = font.HeadColor;
//                    //修改当前xml文件,将字体,颜色等信息保存
//                    if (font.ApplyAll)
//                    {
//                        string value = "FontFamily:" + font.Font.FontFamily.Name +
//                                       "|FontStyle:" + SetFontStyle(font.Font.Style).ToString() +
//                                       "|FontSize:" + font.Font.Size.ToString(CultureInfo.InvariantCulture) +
//                                       "|FontColor:" + font.Color.Name +
//                                       "|BackColor:" + font.BackColor.Name +
//                                       "|SelectColor:" + font.SelectColor.Name +
//                                       "|OddColor:" + font.OddColor.Name +
//                                       "|HeadColor:" + font.HeadColor.Name +
//                                       "|EditTime:" + DateTime.Now.ToString("yyyyMMddHHmm");
//                        ConfigerHelper.WriteAppSettingKey("Gridview", value);
//                    }
//                    else
//                    {
//                        if (xmlFilePath.Remove(0, xmlFilePath.Length - 4) == ".xml")
//                        {
//                            XDocument xml = XDocument.Load(xmlFilePath);
//                            AddAttribute(xml, "Header", "FontFamily", font.Font.FontFamily.Name);
//                            AddAttribute(xml, "Header", "FontStyle", SetFontStyle(font.Font.Style).ToString());
//                            AddAttribute(xml, "Header", "FontSize", font.Font.Size.ToString());
//                            AddAttribute(xml, "Header", "FontColor", font.Color.Name);
//                            AddAttribute(xml, "Header", "BackColor", font.BackColor.Name);
//                            AddAttribute(xml, "Header", "SelectColor", font.SelectColor.Name);
//                            AddAttribute(xml, "Header", "OddColor", font.OddColor.Name);
//                            AddAttribute(xml, "Header", "HeadColor", font.HeadColor.Name);
//                            AddAttribute(xml, "Header", "EditTime", DateTime.Now.ToString("yyyyMMddHHmm"));
//                            xml.Save(xmlFilePath);
//                        }
//                        else
//                        {
//                            DgvXml xml = (DgvXml) FileSerializeOper.Deserialize(xmlFilePath);
//                            xml.FontFamily = font.Font.FontFamily.Name;
//                            xml.FontStyle = SetFontStyle(font.Font.Style).ToString();
//                            xml.FontSize = font.Font.Size.ToString(CultureInfo.InvariantCulture);
//                            xml.FontColor = font.Color.Name;
//                            xml.BackColor = font.BackColor.Name;
//                            xml.SelectColor = font.SelectColor.Name;
//                            xml.OddColor = font.OddColor.Name;
//                            xml.HeadColor = font.HeadColor.Name;
//                            xml.EditTime = DateTime.Now.ToString("yyyyMMddHHmm");
//                            File.Delete(xmlFilePath);
//                            FileSerializeOper.SetSerialize(xml, xmlFilePath);
//                        }
//                    }

//                    ownerdgv.Invalidate();
//                }
//            }
//        }

//        static ToolStripMenuItem BuildSeetingStripMenu(GridView dgv)
//        {
//            ToolStripMenuItem item = new ToolStripMenuItem
//            {
//                Tag = dgv,
//                Text = "基本设置",
//                Name = "SettingMenu"
//            };
//            //   item.Click += new EventHandler(item_Click);

//            return item;
//        }

//        static ToolStripMenuItem Export(GridView dgv)
//        {
//            ToolStripMenuItem item = new ToolStripMenuItem
//            {
//                Tag = dgv,
//                Text = "导出Excel",
//                Name = "Export"
//            };
//            item.Click += Export_ItemClick;

//            return item;
//        }
        
//        /// <summary>
//        /// 导出excel按钮
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        static void Export_ItemClick(object sender, EventArgs e)
//        {
//            ToolStripMenuItem ob = (ToolStripMenuItem) sender;

//            GridView ownerdgv = (GridView) ob?.Tag;
//            if (ownerdgv != null)
//            {
//                SaveFileDialog saveFileDialog = new SaveFileDialog
//                {
//                    Title = "导出Excel",
//                    Filter = "Excel文件(*.xls)|*.xls"
//                };
//                Control form = ownerdgv.GridControl.TopLevelControl;
//                if (form != null) saveFileDialog.FileName = form.Text;
//                DialogResult dialogResult = saveFileDialog.ShowDialog();
//                if (dialogResult == DialogResult.OK)
//                {
//                    // XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
//                    ownerdgv.ExportToXls(saveFileDialog.FileName);
//                    DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK,
//                        MessageBoxIcon.Information);
//                }
//            }
//        }

//        #endregion

//        #endregion

//        private static void gridView1_CustomDrawCell(object sender,
//            RowCellCustomDrawEventArgs e)
//        {
//            GridView view = sender as GridView;
//            if (view != null && e.RowHandle == view.FocusedRowHandle) //e.Column == view.FocusedColumn && 
//            {
//                CellDrawHelper.DoDefaultDrawCell(view, e);
//                //   GridCell[] cells = gridView1.GetSelectedCells();
//                CellDrawHelper.DrawCellBorder(e);
//                e.Handled = true;
//            }
//        }
        
//    }
//    }