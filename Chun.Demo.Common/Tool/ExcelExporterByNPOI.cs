//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;
//using NPOI.HSSF.UserModel;
//using NPOI.SS.UserModel;

//namespace Chun.Demo.Common
//{
//    public class ExcelExporterByNPOI
//    {
//        static SaveFileDialog saveFileDialog = new SaveFileDialog();

//        /// <summary>
//        /// NPOI导出到Excel文件
//        /// </summary>
//        /// <param name="strFileName">保存位置</param>
//        public static void DataTableToExcel(MemoryStream ms, string strFileName)
//        {
//            using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
//            {
//                byte[] data = ms.ToArray();
//                fs.Write(data, 0, data.Length);
//                fs.Flush();
//            }

//        }

//        /// <summary>
//        /// dt导出到excel
//        /// </summary>
//        /// <param name="dt">要导出数据的table</param>
//        /// <param name="name">表格名称</param>
//        /// <param name="title">标题</param>
//        /// <param name="staDate">统计起始日期</param>
//        /// <param name="endDate">统计终止日期</param>
//        /// <param name="staColumns">需要统计数据的列</param>
//        /// <param name="pointTxt">指定为文本的列</param>
//        /// <param name="countCoumn">合计行数列</param>
//        /// <param name="formula">自定义公式（"F,实载率,C{0}/D{1}|G,总班次,C{0}+E{1}"）</param>
//        /// <param name="remark">表格底部 要添加的备注</param>
//        public static void ExportByNPOI(DataTable dt, string bookName, string title, string staDate, string endDate, string staColumns, string pointTxt, int countCoumn, string formula, string remark)
//        {
//            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
//            saveFileDialog.FilterIndex = 0;
//            saveFileDialog.RestoreDirectory = true;
//            saveFileDialog.CreatePrompt = true;
//            saveFileDialog.Title = "保存为Excel文件";
//            saveFileDialog.FileName = bookName + ".xls";
//            DialogResult dialogResult = saveFileDialog.ShowDialog();
//            if ((saveFileDialog.FileName.IndexOf(":") < 0) || (dialogResult == DialogResult.Cancel)) return; //被点了"取消"

//            try
//            {
//                if (dt.Rows.Count < 1)
//                {
//                    return;
//                }
//                //创建book
//                HSSFWorkbook workbooks = new HSSFWorkbook();
//                //创建sheet
//                ISheet sheet = workbooks.CreateSheet("Sheet1");
//                //创建单元格样式
//                ICellStyle headercellStyle = workbooks.CreateCellStyle();

//                headercellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
//                headercellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
//                headercellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
//                headercellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
//                headercellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
//                //字体
//                NPOI.SS.UserModel.IFont headerfont = workbooks.CreateFont();
//                //headerfont.Boldweight = (short)FontBoldWeight.Bold;
//                headerfont.FontName = "宋体";
//                headerfont.FontHeightInPoints = 12;
//                headercellStyle.SetFont(headerfont);

//                //第一行 标题
//                IRow titleRow = sheet.CreateRow(0);
//                ICell titleCell = titleRow.CreateCell(0);
//                titleCell.SetCellValue(title);

//                if (staDate.Length > 0 || endDate.Length > 0)
//                {
//                    titleRow = sheet.CreateRow(1);
//                    titleCell = titleRow.CreateCell(0);
//                    titleCell.SetCellValue("统计日期：");
//                    titleCell = titleRow.CreateCell(1);
//                    titleCell.SetCellValue(staDate);
//                    titleCell = titleRow.CreateCell(2);
//                    titleCell.SetCellValue(endDate);
//                }

//                //用column name 作为列名
//                int icolIndex = 0;
//                IRow headerRow = sheet.CreateRow(2);

//                for (int j = 0, jj = 0; j < dt.Columns.Count; j++) //j 与jj不一定相等 只有 所有列的Visible属性全部都为true时才相等
//                {
//                    ICell cell = headerRow.CreateCell(jj);
//                    cell.SetCellValue(dt.Columns[j].Caption.ToString());
//                    // HeadercellStyle.
//                    cell.CellStyle = headercellStyle;
//                    jj++;
//                    icolIndex = jj;
//                }

//                //标题跨行居中
//                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dt.Columns.Count - 1));
//                IRow irow = sheet.GetRow(0);
//                ICell icell = irow.Cells[0];
//                ICellStyle icellStyle = workbooks.CreateCellStyle();
//                icellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
//                NPOI.SS.UserModel.IFont ifont = workbooks.CreateFont();
//                ifont.Boldweight = (short)FontBoldWeight.Bold;
//                ifont.FontName = "宋体";
//                ifont.FontHeightInPoints = 20;
//                icellStyle.SetFont(ifont);
//                icell.CellStyle = icellStyle;

//                //设置正文内容格式
//                ICellStyle cellStyle = workbooks.CreateCellStyle();
//                cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
//                cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
//                cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin; ;
//                cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
//                cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

//                NPOI.SS.UserModel.IFont font = workbooks.CreateFont();
//                font.FontName = "宋体";
//                font.FontHeightInPoints = 12;
//                cellStyle.SetFont(font);


//                //建立内容行

//                for (int i = 0; i < dt.Rows.Count; i++)
//                {
//                    IRow DataRow = sheet.CreateRow(i + 3);
//                    for (int j = 0, jj = 0; j < dt.Columns.Count; j++)
//                    {
//                        ICell cell = DataRow.CreateCell(jj);
//                        string columnName = dt.Columns[j].ColumnName;
//                        DataRow row = dt.Rows[i];
//                        string drValue = row[columnName].ToString();
//                        if (row[columnName] != null)
//                        {
//                            switch (row[columnName].GetType().ToString())
//                            {
//                                case "System.String"://字符串类型   
//                                    cell.SetCellValue(drValue);
//                                    break;
//                                case "System.DateTime"://日期类型   
//                                    DateTime dateV;
//                                    DateTime.TryParse(drValue, out dateV);
//                                    cell.SetCellValue(dateV);
//                                    break;
//                                case "System.Boolean"://布尔型   
//                                    //  bool boolV = false;
//                                    //   bool.TryParse(drValue, out boolV);
//                                    cell.SetCellValue(drValue);
//                                    break;
//                                case "System.Int16"://整型   
//                                case "System.Int32":
//                                case "System.Int64":
//                                case "System.Byte":
//                                    int intV = 0;
//                                    int.TryParse(drValue, out intV);
//                                    cell.SetCellValue(intV);
//                                    break;
//                                case "System.Decimal"://浮点型   
//                                case "System.Double":
//                                    if (drValue.Length < 10)
//                                    {
//                                        double doubV = 0;
//                                        double.TryParse(drValue, out doubV);
//                                        cell.SetCellValue(doubV);
//                                    }
//                                    else
//                                        cell.SetCellValue(drValue);
//                                    break;
//                                case "System.DBNull"://空值处理   
//                                    cell.SetCellValue("");
//                                    break;
//                                default:
//                                    cell.SetCellValue("");
//                                    break;
//                            }
//                            cell.CellStyle = cellStyle;
//                        }
//                        else
//                        {
//                            cell.SetCellValue("");
//                        }
//                        jj++;
//                    }
//                }


//                //添加合计
//                int rows = dt.Rows.Count;//行数
//                if (rows > 0)
//                {
//                    IRow drow = sheet.CreateRow(rows + 3);
//                    ICell cell;
//                    for (int i = 0; i < dt.Columns.Count; i++)
//                    {
//                        if (countCoumn != 0 || !String.IsNullOrEmpty(staColumns))
//                        {
//                            cell = drow.CreateCell(i);
//                            cell.CellStyle = cellStyle;
//                        }

//                    }
//                    if (countCoumn == 1)
//                    {
//                        cell = drow.Cells[0];
//                        cell.SetCellValue("合计:" + rows.ToString());

//                    }

//                    if (countCoumn > 1)
//                    {
//                        cell = drow.Cells[countCoumn - 1];
//                        cell.SetCellValue(rows.ToString());
//                    }

//                    string[] cloumns = staColumns.Split(',');
//                    try
//                    {
//                        foreach (string str in cloumns)
//                        {
//                            int i = int.Parse(str);
//                            int ch = 'A' + i - 1;
//                            char c = (Char)ch;
//                            cell = drow.Cells[i - 1];
//                            cell.CellFormula = "SUM(" + c.ToString() + "4:" + c.ToString() + (rows + 3).ToString() + ")";
//                        }

//                    }
//                    catch
//                    {

//                    }

//                }

//                //自定义公式 格式"F,实载率,C{0}/D{1}|G,总班次,C{0}+E{1}"
//                if (formula.Length > 0)
//                {
//                    string[] formulas = formula.Split('|');
//                    try
//                    {
//                        foreach (string str in formulas)
//                        {
//                            string[] param = str.Split(',');
//                            int i = char.Parse(param[0]);
//                            IRow drow = sheet.CreateRow(3);
//                            ICell cell;
//                            for (int j = 0; j < dt.Columns.Count; j++)
//                            {
//                                cell = drow.CreateCell(j);
//                                cell.CellStyle = cellStyle;
//                            }
//                            cell = drow.Cells[i - 64];
//                            cell.SetCellValue(param[1]);
//                            drow = sheet.CreateRow(rows + 4);
//                            for (int j = 0; j < dt.Columns.Count; j++)
//                            {
//                                cell = drow.CreateCell(j);
//                                cell.CellStyle = cellStyle;
//                            }
//                            cell = drow.Cells[i - 64];
//                            cell.CellFormula = string.Format(param[2], rows + 4, rows + 4);
//                        }
//                    }
//                    catch { }

//                }
//                //添加备注
//                if (remark != "")
//                {
//                    IRow drow = sheet.CreateRow(rows + 4);
//                    ICell cell;
//                    for (int j = 0; j < dt.Columns.Count; j++)
//                    {
//                        cell = drow.CreateCell(j);
//                        cell.CellStyle = cellStyle;
//                    }
//                    cell = drow.Cells[0];
//                    cell.SetCellValue(remark);
//                }

//                //自适应列宽度
//                for (int i = 0; i < icolIndex; i++)
//                {
//                    sheet.AutoSizeColumn(i);
//                }

//                using (MemoryStream ms = new MemoryStream())
//                {
//                    workbooks.Write(ms);
//                    ms.Flush();
//                    ms.Position = 0;
//                    //workbooks.Close();
//                    DataTableToExcel(ms, saveFileDialog.FileName);
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("导出Excel表出错：" + ex.Message, "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
//                if (DialogResult.Yes == MessageBox.Show("您是否想保存Excel?", "保存提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
//                {
//                    ExcelSaver.SaveExcel(dt, bookName, title, staDate, endDate, staColumns, pointTxt, countCoumn, remark);
//                }

//            }
//            finally
//            {
//                GC.Collect();
//            }
//        }

//        /// <summary>
//        /// 从datagridview导出数据到Excel
//        /// </summary>
//        /// <param name="grid">要导出数据的datagridview</param>
//        /// <param name="name">表格名称</param>
//        /// <param name="title">标题</param>
//        /// <param name="staDate">统计起始日期</param>
//        /// <param name="endDate">统计终止日期</param>
//        /// <param name="staColumns">需要统计数据的列</param>
//        /// <param name="pointTxt">指定为文本的列</param>
//        /// <param name="countCoumn">合计行数列</param>
//        /// <param name="formula">自定义公式（"F,实载率,C{0}/D{1}|G,总班次,C{0}+E{1}"）</param>
//        /// <param name="remark">表格底部 要添加的备注</param>
//        public static void ExportByNPOI(DataGridView dgv, string name, string title, string staDate, string endDate, string staColumns, string pointTxt, int countCoumn, string formula, string remark)
//        {
//            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
//            saveFileDialog.FilterIndex = 0;
//            saveFileDialog.RestoreDirectory = true;
//            saveFileDialog.CreatePrompt = true;
//            saveFileDialog.Title = "保存为Excel文件";
//            saveFileDialog.FileName = name + ".xls";
//            DialogResult dialogResult = saveFileDialog.ShowDialog();
//            if ((saveFileDialog.FileName.IndexOf(":") < 0) || (dialogResult == DialogResult.Cancel)) return; //被点了"取消"

//            try
//            {

//                if (dgv.RowCount < 1)
//                {
//                    return;
//                }

//                //创建book
//                HSSFWorkbook workbooks = new HSSFWorkbook();
//                //创建sheet
//                ISheet sheet = workbooks.CreateSheet("Sheet1");
//                //创建单元格样式
//                ICellStyle HeadercellStyle = workbooks.CreateCellStyle();

//                HeadercellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
//                HeadercellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
//                HeadercellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
//                HeadercellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
//                HeadercellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
//                //字体
//                NPOI.SS.UserModel.IFont headerfont = workbooks.CreateFont();
//                headerfont.Boldweight = (short)FontBoldWeight.Bold;
//                HeadercellStyle.SetFont(headerfont);

//                //第一行 标题
//                IRow titleRow = sheet.CreateRow(0);
//                ICell titleCell = titleRow.CreateCell(0);
//                titleCell.SetCellValue(title);
//                if (staDate.Length > 0 || endDate.Length > 0)
//                {
//                    titleRow = sheet.CreateRow(1);
//                    titleCell = titleRow.CreateCell(0);
//                    titleCell.SetCellValue("统计日期：");
//                    titleCell = titleRow.CreateCell(1);
//                    titleCell.SetCellValue(staDate);
//                    titleCell = titleRow.CreateCell(2);
//                    titleCell.SetCellValue(endDate);
//                }

//                //用column name 作为列名
//                int icolIndex = 0; //有效列数
//                IRow headerRow = sheet.CreateRow(2);

//                for (int j = 0, jj = 0; j < dgv.Columns.Count; j++) //j 与jj不一定相等 只有 所有列的Visible属性全部都为true时才相等
//                {
//                    if (dgv.Columns[j].Visible == true)
//                    {
//                        ICell cell = headerRow.CreateCell(jj);
//                        cell.SetCellValue(dgv.Columns[j].Name.ToString());
//                        cell.CellStyle = HeadercellStyle;
//                        jj++;
//                        icolIndex = jj;
//                    }
//                }

//                //跨行居中
//                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, icolIndex - 1));
//                IRow irow = sheet.GetRow(0);
//                ICell icell = irow.Cells[0];
//                ICellStyle icellStyle = workbooks.CreateCellStyle();
//                icellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
//                NPOI.SS.UserModel.IFont font = workbooks.CreateFont();
//                font.Boldweight = (short)FontBoldWeight.Bold;
//                font.FontName = "宋体";
//                font.FontHeightInPoints = 20;
//                icellStyle.SetFont(font);
//                icell.CellStyle = icellStyle;

//                //正文格式
//                ICellStyle cellStyle = workbooks.CreateCellStyle();

//                cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
//                cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
//                cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
//                cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
//                cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

//                NPOI.SS.UserModel.IFont cellfont = workbooks.CreateFont();
//                cellfont.Boldweight = (short)FontBoldWeight.Normal;
//                cellfont.FontName = "宋体";
//                cellfont.FontHeightInPoints = 12;
//                cellStyle.SetFont(cellfont);

//                //建立内容行

//                for (int i = 0; i < dgv.RowCount; i++)
//                {
//                    IRow DataRow = sheet.CreateRow(i + 3);
//                    for (int j = 0, jj = 0; j < dgv.Columns.Count; j++)
//                    {
//                        if (dgv.Columns[j].Visible == true)
//                        {
//                            ICell cell = DataRow.CreateCell(jj);
//                            string drValue = dgv.Rows[i].Cells[j].Value.ToString();
//                            object obj = dgv.Rows[i].Cells[j].Value;
//                            cell.SetCellValue(drValue);
//                            cell.CellStyle = cellStyle;
//                            jj++;
//                        }
//                    }
//                }

//                //添加合计
//                int rows = dgv.Rows.Count;//行数
//                if (rows > 0)
//                {
//                    IRow drow = sheet.CreateRow(rows + 3);
//                    ICell cell;
//                    for (int i = 0; i < dgv.Columns.Count; i++)
//                    {
//                        if (countCoumn != 0 || !String.IsNullOrEmpty(staColumns))
//                        {
//                            cell = drow.CreateCell(i);
//                            cell.CellStyle = cellStyle;
//                        }
//                    }
//                    if (countCoumn == 1)
//                    {
//                        cell = drow.CreateCell(0);
//                        cell.SetCellValue("合计:" + rows.ToString());

//                    }

//                    if (countCoumn > 1)
//                    {
//                        cell = drow.CreateCell(countCoumn - 1);
//                        cell.SetCellValue(rows.ToString());
//                    }

//                    string[] cloumns = staColumns.Split(',');
//                    try
//                    {
//                        foreach (string str in cloumns)
//                        {
//                            int i = int.Parse(str);
//                            int ch = 'A' + i - 1;
//                            char c = (Char)ch;
//                            cell = drow.CreateCell(i - 1);
//                            cell.CellStyle = cellStyle;
//                            cell.CellFormula = "SUM(" + c.ToString() + "4:" + c.ToString() + (rows + 3).ToString() + ")";
//                        }

//                    }
//                    catch
//                    {

//                    }

//                }

//                //自定义公式 格式"F,实载率,C{0}/D{1}|G,总班次,C{0}+E{1}"
//                if (formula.Length > 0)
//                {
//                    string[] formulas = formula.Split('|');
//                    try
//                    {
//                        foreach (string str in formulas)
//                        {
//                            string[] param = str.Split(',');
//                            int i = char.Parse(param[0]);
//                            IRow drow = sheet.CreateRow(3);
//                            ICell cell;
//                            for (int j = 0; j < icolIndex; j++)
//                            {
//                                cell = drow.CreateCell(j);
//                                cell.CellStyle = cellStyle;
//                            }
//                            cell = drow.CreateCell(i - 64);
//                            cell.SetCellValue(param[1]);
//                            drow = sheet.CreateRow(rows + 4);
//                            for (int j = 0; j < icolIndex; j++)
//                            {
//                                cell = drow.CreateCell(j);
//                                cell.CellStyle = cellStyle;
//                            }
//                            cell = drow.CreateCell(i - 64);
//                            cell.CellFormula = string.Format(param[2], rows + 4, rows + 4);
//                        }
//                    }
//                    catch { }

//                }
//                //添加备注
//                if (remark != "")
//                {
//                    IRow drow = sheet.CreateRow(rows + 4);
//                    ICell cell;
//                    for (int j = 0; j < icolIndex; j++)
//                    {
//                        cell = drow.CreateCell(j);
//                        cell.CellStyle = cellStyle;
//                    }
//                    cell = drow.Cells[0];
//                    cell.SetCellValue(remark);
//                }

//                //自适应列宽度
//                for (int i = 0; i < icolIndex; i++)
//                {
//                    sheet.AutoSizeColumn(i);
//                }

//                using (MemoryStream ms = new MemoryStream())
//                {
//                    workbooks.Write(ms);
//                    ms.Flush();
//                    ms.Position = 0;
//                   // workbooks.Close();
//                    DataTableToExcel(ms, saveFileDialog.FileName);
//                }

//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("导出Excel表出错：" + ex.Message, "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
//                if (DialogResult.Yes == MessageBox.Show("您是否想保存Excel?", "保存提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
//                {
//                    // ExcelSaver.SaveExcel(grid, name, title, staDate, endDate, staColumns, pointTxt, countCoumn, remark);
//                }

//            }
//            finally
//            {
//                GC.Collect();
//            }
//        }

//        /// <summary>
//        /// gridview导出数据到Excel
//        /// </summary>
//        /// <param name="grid">要导出数据的datagridview</param>
//        /// <param name="name">表格名称</param>
//        /// <param name="title">标题</param>
//        /// <param name="staDate">统计起始日期</param>
//        /// <param name="endDate">统计终止日期</param>
//        /// <param name="staColumns">需要统计数据的列</param>
//        /// <param name="pointTxt">指定为文本的列</param>
//        /// <param name="countCoumn">合计行数列</param>
//        /// <param name="formula">自定义公式（"F,实载率,C{0}/D{1}|G,总班次,C{0}+E{1}"）</param>
//        /// <param name="remark">表格底部 要添加的备注</param>
//        public static void ExportByNPOI(DevExpress.XtraGrid.GridControl grid, string name, string title, string staDate, string endDate, string staColumns, string pointTxt, int countCoumn, string formula, string remark)
//        {
//            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
//            saveFileDialog.FilterIndex = 0;
//            saveFileDialog.RestoreDirectory = true;
//            saveFileDialog.CreatePrompt = true;
//            saveFileDialog.Title = "保存为Excel文件";
//            saveFileDialog.FileName = name + ".xls";
//            DialogResult dialogResult = saveFileDialog.ShowDialog();
//            if ((saveFileDialog.FileName.IndexOf(":") < 0) || (dialogResult == DialogResult.Cancel)) return; //被点了"取消"

//            try
//            {
//                DevExpress.XtraGrid.Views.Grid.GridView dgv = (DevExpress.XtraGrid.Views.Grid.GridView)grid.MainView;

//                //创建book
//                HSSFWorkbook workbooks = new HSSFWorkbook();
//                //创建sheet
//                ISheet sheet = workbooks.CreateSheet("Sheet1");
//                //创建单元格样式
//                ICellStyle HeadercellStyle = workbooks.CreateCellStyle();

//                HeadercellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
//                HeadercellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
//                HeadercellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
//                HeadercellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
//                HeadercellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
//                //字体
//                NPOI.SS.UserModel.IFont headerfont = workbooks.CreateFont();
//                headerfont.Boldweight = (short)FontBoldWeight.Bold;
//                HeadercellStyle.SetFont(headerfont);

//                //第一行 标题
//                IRow titleRow = sheet.CreateRow(0);
//                ICell titleCell = titleRow.CreateCell(0);
//                titleCell.SetCellValue(title);
//                if (staDate.Length > 0 || endDate.Length > 0)
//                {
//                    titleRow = sheet.CreateRow(1);
//                    titleCell = titleRow.CreateCell(0);
//                    titleCell.SetCellValue("统计日期：");
//                    titleCell = titleRow.CreateCell(1);
//                    titleCell.SetCellValue(staDate);
//                    titleCell = titleRow.CreateCell(2);
//                    titleCell.SetCellValue(endDate);
//                }


//                //用column name 作为列名
//                int icolIndex = 0;
//                IRow headerRow = sheet.CreateRow(2);

//                for (int j = 0, jj = 0; j < dgv.Columns.Count; j++) //j 与jj不一定相等 只有 所有列的Visible属性全部都为true时才相等
//                {
//                    if (dgv.Columns[j].Visible == true)
//                    {
//                        ICell cell = headerRow.CreateCell(jj);
//                        cell.SetCellValue(dgv.Columns[j].Caption.ToString());
//                        cell.CellStyle = HeadercellStyle;
//                        jj++;
//                        icolIndex = jj;
//                    }
//                }

//                //跨行居中
//                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, icolIndex - 1));
//                IRow irow = sheet.GetRow(0);
//                ICell icell = irow.Cells[0];
//                ICellStyle icellStyle = workbooks.CreateCellStyle();
//                icellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
//                NPOI.SS.UserModel.IFont font = workbooks.CreateFont();
//                font.Boldweight = (short)FontBoldWeight.Bold;
//                font.FontName = "宋体";
//                font.FontHeightInPoints = 20;
//                icellStyle.SetFont(font);
//                icell.CellStyle = icellStyle;

//                ICellStyle cellStyle = workbooks.CreateCellStyle();

//                cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
//                cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
//                cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
//                cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
//                cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

//                NPOI.SS.UserModel.IFont cellfont = workbooks.CreateFont();
//                cellfont.Boldweight = (short)FontBoldWeight.Normal;
//                cellfont.FontName = "宋体";
//                cellfont.FontHeightInPoints = 12;
//                cellStyle.SetFont(cellfont);

//                //建立内容行

//                for (int i = 0; i < dgv.RowCount; i++)
//                {
//                    IRow DataRow = sheet.CreateRow(i + 3);
//                    for (int j = 0, jj = 0; j < dgv.Columns.Count; j++)
//                    {
//                        if (dgv.Columns[j].Visible == true)
//                        {
//                            ICell cell = DataRow.CreateCell(jj);
//                            string columnName = dgv.Columns[j].FieldName;
//                            DataRow row = dgv.GetDataRow(i);
//                            string drValue = row[columnName].ToString();
//                            if (row[columnName] != null)
//                            {

//                                switch (row[columnName].GetType().ToString())
//                                {
//                                    case "System.String"://字符串类型   
//                                        cell.SetCellValue(drValue);
//                                        break;
//                                    case "System.DateTime"://日期类型   
//                                        //DateTime dateV;
//                                        //   DateTime.TryParse(drValue, out dateV);
//                                        cell.SetCellValue(drValue);
//                                        break;
//                                    case "System.Boolean"://布尔型   
//                                        bool boolV = false;
//                                        bool.TryParse(drValue, out boolV);
//                                        cell.SetCellValue(boolV);
//                                        break;
//                                    case "System.Int16"://整型   
//                                    case "System.Int32":
//                                    case "System.Int64":
//                                    case "System.Byte":
//                                        int intV = 0;
//                                        int.TryParse(drValue, out intV);
//                                        cell.SetCellValue(intV);
//                                        break;
//                                    case "System.Decimal"://浮点型   
//                                    case "System.Double":
//                                        if (drValue.Length < 10)
//                                        {
//                                            double doubV = 0;
//                                            double.TryParse(drValue, out doubV);
//                                            cell.SetCellValue(doubV);
//                                        }
//                                        else
//                                            cell.SetCellValue(drValue);
//                                        break;
//                                    case "System.DBNull"://空值处理   
//                                        cell.SetCellValue("");
//                                        break;
//                                    default:
//                                        cell.SetCellValue("");
//                                        break;
//                                }
//                                cell.CellStyle = cellStyle;

//                            }
//                            else
//                            {
//                                cell.SetCellValue("");
//                            }
//                            jj++;
//                        }
//                    }
//                }

//                //添加合计
//                int rows = dgv.RowCount;//行数
//                if (rows > 0)
//                {
//                    IRow drow = sheet.CreateRow(rows + 3);
//                    ICell cell;
//                    for (int i = 0; i < icolIndex; i++)
//                    {
//                        if (countCoumn != 0 || !String.IsNullOrEmpty(staColumns))
//                        {
//                            cell = drow.CreateCell(i);
//                            cell.CellStyle = cellStyle;
//                        }
//                    }
//                    if (countCoumn == 1)
//                    {
//                        cell = drow.CreateCell(0);
//                        cell.SetCellValue("合计:" + rows.ToString());

//                    }

//                    if (countCoumn > 1)
//                    {
//                        cell = drow.CreateCell(countCoumn - 1);
//                        cell.SetCellValue(rows.ToString());
//                    }

//                    string[] cloumns = staColumns.Split(',');
//                    try
//                    {
//                        foreach (string str in cloumns)
//                        {
//                            int i = int.Parse(str);
//                            int ch = 'A' + i - 1;
//                            char c = (Char)ch;
//                            cell = drow.CreateCell(i - 1);
//                            cell.CellStyle = cellStyle;
//                            cell.CellFormula = "SUM(" + c.ToString() + "4:" + c.ToString() + (rows + 3).ToString() + ")";
//                        }

//                    }
//                    catch
//                    {

//                    }

//                }

//                //自定义公式 格式"F,实载率,C{0}/D{1}|G,总班次,C{0}+E{1}"
//                if (formula.Length > 0)
//                {
//                    string[] formulas = formula.Split('|');
//                    try
//                    {
//                        foreach (string str in formulas)
//                        {
//                            string[] param = str.Split(',');
//                            int i = char.Parse(param[0]);
//                            IRow drow = sheet.CreateRow(3);
//                            ICell cell;
//                            for (int j = 0; j < icolIndex; j++)
//                            {
//                                cell = drow.CreateCell(j);
//                                cell.CellStyle = cellStyle;
//                            }
//                            cell = drow.CreateCell(i - 64);
//                            cell.SetCellValue(param[1]);
//                            drow = sheet.CreateRow(rows + 4);
//                            for (int j = 0; j < icolIndex; j++)
//                            {
//                                cell = drow.CreateCell(j);
//                                cell.CellStyle = cellStyle;
//                            }
//                            cell = drow.CreateCell(i - 64);
//                            cell.CellFormula = string.Format(param[2], rows + 4, rows + 4);
//                        }
//                    }
//                    catch { }

//                }
//                //添加备注
//                if (remark != "")
//                {
//                    IRow drow = sheet.CreateRow(rows + 4);
//                    ICell cell;
//                    for (int j = 0; j < icolIndex; j++)
//                    {
//                        cell = drow.CreateCell(j);
//                        cell.CellStyle = cellStyle;
//                    }
//                    cell = drow.Cells[0];
//                    cell.SetCellValue(remark);
//                }

//                //自适应列宽度
//                for (int i = 0; i < icolIndex; i++)
//                {
//                    sheet.AutoSizeColumn(i);
//                }

//                using (MemoryStream ms = new MemoryStream())
//                {
//                    workbooks.Write(ms);
//                    ms.Flush();
//                    ms.Position = 0;
//                   // workbooks.Close();
//                    DataTableToExcel(ms, saveFileDialog.FileName);
//                }

//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("导出Excel表出错：" + ex.Message, "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
//                if (DialogResult.Yes == MessageBox.Show("您是否想保存Excel?", "保存提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
//                {
//                    ExcelSaver.SaveExcel(grid, name, title, staDate, endDate, staColumns, pointTxt, countCoumn, remark);
//                }

//            }
//            finally
//            {
//                GC.Collect();
//            }
//        }

//        /// <summary>
//        /// datatable导出到模板
//        /// </summary>
//        /// <param name="dt">数据源</param>
//        /// <param name="modelExlPath">模板路径</param>
//        /// <param name="staDate">起始日期</param>
//        /// <param name="endDate">结束日期</param>
//        /// <param name="insertPoint">第几行开始插入</param>
//        /// <param name="remark">表格底部 要添加的备注</param>
//        public static void ExportByNPOI(DataTable dt, string modelExlPath, string staDate, string endDate, int insertPoint, string remark)
//        {
//            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
//            saveFileDialog.FilterIndex = 0;
//            saveFileDialog.RestoreDirectory = true;
//            saveFileDialog.CreatePrompt = true;
//            saveFileDialog.Title = "保存为Excel文件";
//            saveFileDialog.FileName = "";
//            DialogResult dialogResult = saveFileDialog.ShowDialog();
//            if ((saveFileDialog.FileName.IndexOf(":") < 0) || (dialogResult == DialogResult.Cancel)) return; //被点了"取消"

//            try
//            {
//                if (dt.Rows.Count < 1)
//                {
//                    return;
//                }
//                //创建book
//                HSSFWorkbook workbooks;
//                using (FileStream file = new FileStream(modelExlPath, FileMode.Open, FileAccess.Read))
//                {
//                    workbooks = new HSSFWorkbook(file);
//                    file.Close();
//                }
//                //创建sheet
//                ISheet sheet = workbooks.CreateSheet("Sheet1");

//                //第一行 标题
//                IRow titleRow;
//                //= sheet.CreateRow(0);
//                ICell titleCell;
//                //    = titleRow.CreateCell(0);
//                //titleCell.SetCellValue(title);

//                if (staDate.Length > 0 || endDate.Length > 0)
//                {
//                    titleRow = sheet.CreateRow(1);
//                    titleCell = titleRow.CreateCell(0);
//                    titleCell.SetCellValue("统计日期：");
//                    titleCell = titleRow.CreateCell(1);
//                    titleCell.SetCellValue(staDate);
//                    titleCell = titleRow.CreateCell(2);
//                    titleCell.SetCellValue(endDate);
//                }

//                //用column name 作为列名
//                int icolIndex = dt.Columns.Count; //列数
//                int rows = dt.Rows.Count;//行数
//                IRow headerRow = sheet.CreateRow(2);

//                //建立内容行

//                for (int i = 0; i < dt.Rows.Count; i++)
//                {
//                    IRow DataRow = sheet.CreateRow(i + insertPoint);
//                    for (int j = 0, jj = 0; j < dt.Columns.Count; j++)
//                    {
//                        ICell cell = DataRow.CreateCell(jj);
//                        string columnName = dt.Columns[j].ColumnName;
//                        DataRow row = dt.Rows[i];
//                        string drValue = row[columnName].ToString();
//                        if (row[columnName] != null)
//                        {
//                            switch (row[columnName].GetType().ToString())
//                            {
//                                case "System.String"://字符串类型   
//                                    cell.SetCellValue(drValue);
//                                    break;
//                                case "System.DateTime"://日期类型   
//                                    DateTime dateV;
//                                    DateTime.TryParse(drValue, out dateV);
//                                    cell.SetCellValue(dateV);
//                                    break;
//                                case "System.Boolean"://布尔型   
//                                    //bool boolV = false;
//                                    //bool.TryParse(drValue, out boolV);
//                                    cell.SetCellValue(drValue);
//                                    break;
//                                case "System.Int16"://整型   
//                                case "System.Int32":
//                                case "System.Int64":
//                                case "System.Byte":
//                                    int intV = 0;
//                                    int.TryParse(drValue, out intV);
//                                    cell.SetCellValue(intV);
//                                    break;
//                                case "System.Decimal"://浮点型   
//                                case "System.Double":
//                                    if (drValue.Length < 10)
//                                    {
//                                        double doubV = 0;
//                                        double.TryParse(drValue, out doubV);
//                                        cell.SetCellValue(doubV);
//                                    }
//                                    else
//                                        cell.SetCellValue(drValue);
//                                    break;
//                                case "System.DBNull"://空值处理   
//                                    cell.SetCellValue("");
//                                    break;
//                                default:
//                                    cell.SetCellValue("");
//                                    break;
//                            }
//                        }
//                        else
//                        {
//                            cell.SetCellValue("");
//                        }
//                        jj++;
//                    }
//                }


//                //添加合计

//                if (rows > 0)
//                {
//                    IRow drow = sheet.CreateRow(rows + insertPoint);
//                    ICell cell;
//                    for (int i = 0; i < dt.Columns.Count; i++)
//                    {
//                        cell = drow.CreateCell(i);
//                    }
//                    cell = drow.Cells[0];
//                    cell.SetCellValue("合计:" + rows.ToString());

//                }

//                //添加备注
//                if (remark != "")
//                {
//                    IRow drow = sheet.CreateRow(rows + insertPoint + 1);
//                    ICell cell;
//                    for (int j = 0; j < dt.Columns.Count; j++)
//                    {
//                        cell = drow.CreateCell(j);
//                    }
//                    cell = drow.Cells[0];
//                    cell.SetCellValue(remark);
//                }

//                //自适应列宽度
//                for (int i = 0; i < icolIndex; i++)
//                {
//                    sheet.AutoSizeColumn(i);
//                }

//                using (MemoryStream ms = new MemoryStream())
//                {
//                    workbooks.Write(ms);
//                    ms.Flush();
//                    ms.Position = 0;
//                   // workbooks.Close();
//                    DataTableToExcel(ms, saveFileDialog.FileName);
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("导出Excel表出错：" + ex.Message, "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
//                if (DialogResult.Yes == MessageBox.Show("您是否想保存Excel?", "保存提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
//                {
//                    ExcelSaver.SaveExcel(dt, saveFileDialog.FileName, saveFileDialog.FileName, staDate, endDate, String.Empty, String.Empty, 0, remark);
//                }

//            }
//            finally
//            {
//                GC.Collect();
//            }
//        }

//        /// <summary>
//        /// gridview导出到模板
//        /// </summary>
//        /// <param name="dgv">数据源</param>
//        /// <param name="modelExlPath">模板路径</param>
//        /// <param name="staDate">起始日期</param>
//        /// <param name="endDate">结束日期</param>
//        /// <param name="insertPoint">第几行开始插入</param>
//        /// <param name="remark">备注</param>
//        public static void ExportByNPOI(DataGridView dgv, string modelExlPath, string staDate, string endDate, int insertPoint, string remark)
//        {
//            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
//            saveFileDialog.FilterIndex = 0;
//            saveFileDialog.RestoreDirectory = true;
//            saveFileDialog.CreatePrompt = true;
//            saveFileDialog.Title = "保存为Excel文件";
//            saveFileDialog.FileName = "";
//            DialogResult dialogResult = saveFileDialog.ShowDialog();
//            if ((saveFileDialog.FileName.IndexOf(":") < 0) || (dialogResult == DialogResult.Cancel)) return; //被点了"取消"

//            try
//            {

//                if (dgv.RowCount < 1)
//                {
//                    return;
//                }

//                //创建book
//                HSSFWorkbook workbooks;
//                //= new HSSFWorkbook();
//                using (FileStream file = new FileStream(modelExlPath, FileMode.Open, FileAccess.Read))
//                {
//                    workbooks = new HSSFWorkbook(file);
//                    file.Close();
//                }
//                //创建sheet
//                ISheet sheet = workbooks.CreateSheet("Sheet1");

//                //第一行 标题
//                IRow titleRow;
//                //= sheet.CreateRow(0);
//                ICell titleCell;
//                //    = titleRow.CreateCell(0);
//                //titleCell.SetCellValue(title);
//                if (staDate.Length > 0 || endDate.Length > 0)
//                {
//                    titleRow = sheet.CreateRow(1);
//                    titleCell = titleRow.CreateCell(0);
//                    titleCell.SetCellValue("统计日期：");
//                    titleCell = titleRow.CreateCell(1);
//                    titleCell.SetCellValue(staDate);
//                    titleCell = titleRow.CreateCell(2);
//                    titleCell.SetCellValue(endDate);
//                }

//                //用column name 作为列名
//                //用column name 作为列名
//                int icolIndex = dgv.Columns.Count; //列数
//                int rows = dgv.Rows.Count;//行数
//                IRow headerRow = sheet.CreateRow(2);

//                //建立内容行

//                for (int i = 0; i < dgv.RowCount; i++)
//                {
//                    IRow DataRow = sheet.CreateRow(i + insertPoint);
//                    for (int j = 0, jj = 0; j < dgv.Columns.Count; j++)
//                    {
//                        if (dgv.Columns[j].Visible == true)
//                        {
//                            ICell cell = DataRow.CreateCell(jj);
//                            string drValue = dgv.Rows[i].Cells[j].Value.ToString();
//                            object obj = dgv.Rows[i].Cells[j].Value;
//                            cell.SetCellValue(drValue);
//                            jj++;
//                        }
//                    }
//                }

//                //添加合计
//                if (rows > 0)
//                {
//                    IRow drow = sheet.CreateRow(rows + insertPoint);
//                    ICell cell;
//                    for (int i = 0; i < dgv.Columns.Count; i++)
//                    {
//                        if (dgv.Columns[i].Visible == true)
//                            cell = drow.CreateCell(i);
//                    }
//                    cell = drow.Cells[0];
//                    cell.SetCellValue("合计:" + rows.ToString());

//                }

//                //添加备注
//                if (remark != "")
//                {
//                    IRow drow = sheet.CreateRow(rows + insertPoint + 1);
//                    ICell cell;
//                    for (int j = 0; j < icolIndex; j++)
//                    {
//                        if (dgv.Columns[j].Visible == true)
//                            cell = drow.CreateCell(j);
//                    }
//                    cell = drow.Cells[0];
//                    cell.SetCellValue(remark);
//                }

//                //自适应列宽度
//                for (int i = 0; i < icolIndex; i++)
//                {
//                    sheet.AutoSizeColumn(i);
//                }

//                using (MemoryStream ms = new MemoryStream())
//                {
//                    workbooks.Write(ms);
//                    ms.Flush();
//                    ms.Position = 0;
//                   // workbooks.Close();
//                    DataTableToExcel(ms, saveFileDialog.FileName);
//                }

//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("导出Excel表出错：" + ex.Message, "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
//                if (DialogResult.Yes == MessageBox.Show("您是否想保存Excel?", "保存提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
//                {
//                    // ExcelSaver.SaveExcel(grid, name, title, staDate, endDate, staColumns, pointTxt, countCoumn, remark);
//                }

//            }
//            finally
//            {
//                GC.Collect();
//            }
//        }

//        /// <summary>
//        /// 新控件gridcontrol导出到模板
//        /// </summary>
//        /// <param name="grid">数据源</param>
//        /// <param name="templateURL">模板路径</param>
//        /// <param name="staDate">起始日期</param>
//        /// <param name="endDate">结束日期</param>
//        /// <param name="insertPoint">第几行开始插入</param>
//        /// <param name="remark">表格底部 要添加的备注</param>
//        public static void ExportByNPOI(DevExpress.XtraGrid.GridControl grid, string modelExlPath, string staDate, string endDate, int insertPoint, string remark)
//        {
//            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
//            saveFileDialog.FilterIndex = 0;
//            saveFileDialog.RestoreDirectory = true;
//            saveFileDialog.CreatePrompt = true;
//            saveFileDialog.Title = "保存为Excel文件";
//            saveFileDialog.FileName = "";
//            DialogResult dialogResult = saveFileDialog.ShowDialog();
//            if ((saveFileDialog.FileName.IndexOf(":") < 0) || (dialogResult == DialogResult.Cancel)) return; //被点了"取消"

//            try
//            {
//                DevExpress.XtraGrid.Views.Grid.GridView dgv = (DevExpress.XtraGrid.Views.Grid.GridView)grid.MainView;

//                //创建book
//                HSSFWorkbook workbooks;
//                //= new HSSFWorkbook();
//                using (FileStream file = new FileStream(modelExlPath, FileMode.Open, FileAccess.Read))
//                {
//                    workbooks = new HSSFWorkbook(file);
//                    file.Close();
//                }
//                //创建sheet
//                ISheet sheet = workbooks.CreateSheet("Sheet1");

//                //第一行 标题
//                IRow titleRow;
//                //= sheet.CreateRow(0);
//                ICell titleCell;
//                //= titleRow.CreateCell(0);
//                //titleCell.SetCellValue(title);
//                if (staDate.Length > 0 || endDate.Length > 0)
//                {
//                    titleRow = sheet.CreateRow(1);
//                    titleCell = titleRow.CreateCell(0);
//                    titleCell.SetCellValue("统计日期：");
//                    titleCell = titleRow.CreateCell(1);
//                    titleCell.SetCellValue(staDate);
//                    titleCell = titleRow.CreateCell(2);
//                    titleCell.SetCellValue(endDate);
//                }


//                //用column name 作为列名
//                int icolIndex = dgv.Columns.Count; //列数
//                int rows = dgv.RowCount;//行数

//                //建立内容行

//                for (int i = 0; i < dgv.RowCount; i++)
//                {
//                    IRow DataRow = sheet.CreateRow(i + 3);
//                    for (int j = 0, jj = 0; j < dgv.Columns.Count; j++)
//                    {
//                        if (dgv.Columns[j].Visible == true)
//                        {
//                            ICell cell = DataRow.CreateCell(jj);
//                            string columnName = dgv.Columns[j].FieldName;
//                            DataRow row = dgv.GetDataRow(i);
//                            string drValue = row[columnName].ToString();
//                            if (row[columnName] != null)
//                            {

//                                switch (row[columnName].GetType().ToString())
//                                {
//                                    case "System.String"://字符串类型   
//                                        cell.SetCellValue(drValue);
//                                        break;
//                                    case "System.DateTime"://日期类型   
//                                        //DateTime dateV;
//                                        //DateTime.TryParse(drValue, out dateV);
//                                        cell.SetCellValue(drValue);
//                                        break;
//                                    case "System.Boolean"://布尔型   
//                                        bool boolV = false;
//                                        bool.TryParse(drValue, out boolV);
//                                        cell.SetCellValue(boolV);
//                                        break;
//                                    case "System.Int16"://整型   
//                                    case "System.Int32":
//                                    case "System.Int64":
//                                    case "System.Byte":
//                                        int intV = 0;
//                                        int.TryParse(drValue, out intV);
//                                        cell.SetCellValue(intV);
//                                        break;
//                                    case "System.Decimal"://浮点型   
//                                    case "System.Double":
//                                        if (drValue.Length < 10)
//                                        {
//                                            double doubV = 0;
//                                            double.TryParse(drValue, out doubV);
//                                            cell.SetCellValue(doubV);
//                                        }
//                                        else
//                                            cell.SetCellValue(drValue);
//                                        break;
//                                    case "System.DBNull"://空值处理   
//                                        cell.SetCellValue("");
//                                        break;
//                                    default:
//                                        cell.SetCellValue("");
//                                        break;
//                                }
//                            }
//                            else
//                            {
//                                cell.SetCellValue("");
//                            }
//                            jj++;
//                        }
//                    }
//                }

//                //添加合计
//                if (rows > 0)
//                {
//                    IRow drow = sheet.CreateRow(rows + insertPoint);
//                    ICell cell;
//                    for (int i = 0; i < dgv.Columns.Count; i++)
//                    {
//                        if (dgv.Columns[i].Visible == true)
//                        {
//                            cell = drow.CreateCell(i);
//                        }
//                    }
//                    cell = drow.Cells[0];
//                    cell.SetCellValue("合计:" + rows.ToString());

//                }

//                //添加备注
//                if (remark != "")
//                {
//                    IRow drow = sheet.CreateRow(rows + insertPoint + 1);
//                    ICell cell;
//                    for (int j = 0; j < dgv.Columns.Count; j++)
//                    {
//                        cell = drow.CreateCell(j);
//                    }
//                    cell = drow.Cells[0];
//                    cell.SetCellValue(remark);
//                }

//                //自适应列宽度
//                for (int i = 0; i < icolIndex; i++)
//                {
//                    sheet.AutoSizeColumn(i);
//                }

//                using (MemoryStream ms = new MemoryStream())
//                {
//                    workbooks.Write(ms);
//                    ms.Flush();
//                    ms.Position = 0;
//                    //workbooks.Close();
//                    DataTableToExcel(ms, saveFileDialog.FileName);
//                }

//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("导出Excel表出错：" + ex.Message, "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
//                if (DialogResult.Yes == MessageBox.Show("您是否想保存Excel?", "保存提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
//                {
//                    ExcelSaver.SaveExcel(grid, saveFileDialog.FileName, saveFileDialog.FileName, staDate, endDate, String.Empty, String.Empty, 0, remark);
//                }

//            }
//            finally
//            {
//                GC.Collect();
//            }
//        }

//    }
//}
