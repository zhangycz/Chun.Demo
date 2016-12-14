/**************Code Info**************************
* Copyright(c) 2012-2013
* CLR 版本：4.0
* 文 件 名： 
* 创 建 人：Mawx
* 创建日期：2012/8/6 16:20:16
* 修 改 人：
* 修改日期：
* 备注描述：
************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Chun.Demo.Common
{
    public class ExcelExporter
    {
      
        /// <summary>
        /// 从table导出数据到Excel
        /// </summary>
        /// <param name="dt">要导出数据的table</param>
        /// <param name="name">表格名称</param>
        /// <param name="title">标题</param>
        /// <param name="staDate">统计起始日期</param>
        /// <param name="endDate">统计终止日期</param>
        /// <param name="staColumns">需要统计数据的列</param>
        /// <param name="pointTxt">指定为文本的列</param>
        /// <param name="countCoumn">合计行数列</param>
        /// <param name="formula">自定义公式（"F,实载率,C{0}/D{1}|G,总班次,C{0}+E{1}"）</param>
        /// <param name="remark">表格底部 要添加的备注</param>
        public static void Export(DataTable dt, string bookName, string title, string staDate, string endDate, string staColumns, string pointTxt, int countCoumn, string formula, string remark)
        {
            try
            {
                if (dt.Rows.Count < 1)
                {
                    return;
                }
                Excel.Application books = new Excel.Application();

                Excel.Workbook mybook = (Excel.Workbook)books.Workbooks.Add(1);
                ((Excel.Worksheet)mybook.Worksheets[1]).Name = bookName;
                Excel.Worksheet mysheet = (Excel.Worksheet)mybook.Worksheets[1];

                //第一行 标题
                mysheet.Cells[1, 1] = title;
                if (staDate.Length > 0 || endDate.Length > 0)
                {
                    //第二行 统计日期
                    mysheet.Cells[2, 1] = "统计日期：";//+staDate+"  -  "+endDate;
                    mysheet.Cells[2, 2] = staDate;
                    mysheet.Cells[2, 3] = endDate;

                    //mysheet.Cells[2, 10] = "检验员：";
                }

                //填标题
                int columns = 0; //列数
                for (int j = 0, jj = 0; j < dt.Columns.Count; j++) //j 与jj不一定相等 只有 所有列的Visible属性全部都为true时才相等
                {

                    mysheet.Cells[3, jj + 1] = dt.Columns[j].ColumnName.ToString();
                    jj++;
                    columns = jj;
                }
                if (pointTxt.Length > 0)
                {
                    string[] points = pointTxt.Split(',');
                    foreach (string str in points)
                    {
                        int i = int.Parse(str);
                        int ch = 'A' + i - 1;
                        char c = (Char)ch;
                        mysheet.get_Range(c.ToString() + 4.ToString(), c.ToString() + (4 + dt.Rows.Count).ToString()).NumberFormatLocal = "@";
                    }
                }

                int rows = dt.Rows.Count;//行数
                //填  数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0, jj = 0; j < dt.Columns.Count; j++)
                    {
                        string tmp = dt.Rows[i][j].ToString();
                        mysheet.Cells[i + 4, jj + 1] = tmp;
                        jj++;
                    }
                }
                //添加 合计行

                if (rows > 0)
                {
                    mysheet.Cells[rows + 4, 1] = "合计:";
                    if (countCoumn == 1)
                        mysheet.Cells[rows + 4, 1] = "合计:" + rows.ToString();
                    if (countCoumn > 1)
                        mysheet.Cells[rows + 4, countCoumn] = rows.ToString();
                    string[] cloumns = staColumns.Split(',');
                    try
                    {
                        foreach (string str in cloumns)
                        {
                            int i = int.Parse(str);
                            int ch = 'A' + i - 1;
                            char c = (Char)ch;
                            mysheet.Cells[rows + 4, i] = "=SUM(" + c.ToString() + "4:" + c.ToString() + (rows + 3).ToString() + ")";
                        }
                    }
                    catch { }
                }

                //添加备注
                if (remark != "")
                {
                    mysheet.Cells[rows + 5, 1] = remark;
                    mysheet.get_Range(mysheet.Cells[rows + 5, 1], mysheet.Cells[rows + 5, columns]).Select();
                    mysheet.get_Range(mysheet.Cells[rows + 5, 1], mysheet.Cells[rows + 5, columns]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                }

                //自定义公式 格式"F,实载率,C{0}/D{1}|G,总班次,C{0}+E{1}"
                if (formula.Length > 0)
                {
                    string[] formulas = formula.Split('|');
                    try
                    {
                        foreach (string str in formulas)
                        {
                            string[] param = str.Split(',');
                            int i = char.Parse(param[0]);
                            if (param[1].Length > 0)
                                mysheet.Cells[3, i - 64] = param[1];
                            mysheet.Cells[rows + 4, i - 64] = "=" + string.Format(param[2], rows + 4, rows + 4);
                        }
                    }
                    catch { }
                }

                //设置格式
                try
                {
                    //设置 所有文字大小为9
                    mysheet.get_Range(mysheet.Cells[1, 1], mysheet.Cells[rows + 5, columns]).Font.Size = 12;
                }
                catch { }
                try
                {
                    //设置 表格为最适应宽度
                    mysheet.get_Range(mysheet.Cells[2, 1], mysheet.Cells[rows + 4, columns]).Select();
                    mysheet.get_Range(mysheet.Cells[2, 1], mysheet.Cells[rows + 4, columns]).Columns.AutoFit();
                }
                catch { }
                try
                {
                    //设置标题跨列居中
                    mysheet.get_Range(mysheet.Cells[1, 1], mysheet.Cells[1, columns]).Select();
                    mysheet.get_Range(mysheet.Cells[1, 1], mysheet.Cells[1, columns]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenterAcrossSelection;
                }
                catch { }
                try
                {
                    //加边框
                    //内部为虚线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders.LineStyle = Excel.XlLineStyle.xlDash; //设置虚线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders.Weight = Excel.XlBorderWeight.xlHairline;//设为极细
                    //设置左边线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlMedium;
                    //设置上边线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
                    //设置右边线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                    //设置下边线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
                }
                catch { }
                try
                {
                    //设置 纸张为A4 纵向
                    mysheet.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
                    mysheet.PageSetup.Orientation = Excel.XlPageOrientation.xlPortrait;
                }
                catch { }

                books.Visible = true;

                //mybook.SaveCopyAs("行包受理员营收汇总.xls");//.Save();
                mybook = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(books);
                books = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出Excel表出错，可能您的机器未安装Excel!系统报错：" + ex.Message, "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (DialogResult.Yes == MessageBox.Show("您是否想保存Excel?", "保存提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    //ExcelSaver.SaveExcel(dt, bookName, title, staDate, endDate, staColumns, pointTxt, countCoumn, remark);
                }
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// 从datagridview导出数据到Excel
        /// </summary>
        /// <param name="grid">要导出数据的datagridview</param>
        /// <param name="name">表格名称</param>
        /// <param name="title">标题</param>
        /// <param name="staDate">统计起始日期</param>
        /// <param name="endDate">统计终止日期</param>
        /// <param name="staColumns">需要统计数据的列</param>
        /// <param name="pointTxt">指定为文本的列</param>
        /// <param name="countCoumn">合计行数列</param>
        /// <param name="formula">自定义公式（"F,实载率,C{0}/D{1}|G,总班次,C{0}+E{1}"）</param>
        /// <param name="remark">表格底部 要添加的备注</param>
        public static void Export(DataGridView grid, string name, string title, string staDate, string endDate, string staColumns, string pointTxt, int countCoumn, string formula, string remark)
        {
            try
            {
                if (grid.Rows.Count < 1)
                {
                    return;
                }
                Excel.Application books = new Microsoft.Office.Interop.Excel.Application();

                Excel.Workbook mybook = (Excel.Workbook)books.Workbooks.Add(1);
                ((Excel.Worksheet)mybook.Worksheets[1]).Name = name;
                Excel.Worksheet mysheet = (Excel.Worksheet)mybook.Worksheets[1];

                //第一行 标题
                mysheet.Cells[1, 1] = title;
                if (staDate.Length > 0 || endDate.Length > 0)
                {
                    //第二行 统计日期
                    mysheet.Cells[2, 1] = "统计日期：";//+staDate+"  -  "+endDate;
                    mysheet.Cells[2, 2] = staDate;
                    mysheet.Cells[2, 3] = endDate;

                    //mysheet.Cells[2, 10] = "检验员：";
                }

                //填标题
                int columns = 0; //列数
                for (int j = 0, jj = 0; j < grid.Columns.Count; j++) //j 与jj不一定相等 只有 所有列的Visible属性全部都为true时才相等
                {
                    if (grid.Columns[j].Visible == true)
                    {
                        mysheet.Cells[3, jj + 1] = grid.Columns[j].HeaderText.ToString();
                        jj++;
                        columns = jj;
                    }
                }
                if (pointTxt.Length > 0)
                {
                    string[] points = pointTxt.Split(',');
                    foreach (string str in points)
                    {
                        int i = int.Parse(str);
                        int ch = 'A' + i - 1;
                        char c = (Char)ch;
                        mysheet.get_Range(c.ToString() + 4.ToString(), c.ToString() + (4 + grid.Rows.Count).ToString()).NumberFormatLocal = "@";
                    }
                }

                int rows = grid.Rows.Count;//行数
                //填  数据
                for (int i = 0; i < grid.Rows.Count; i++)
                {
                    for (int j = 0, jj = 0; j < grid.Columns.Count; j++)
                    {
                        if (grid.Columns[j].Visible == true)
                        {
                            if (grid.Rows[i].Cells[j].Value != null)
                            {
                                string tmp = grid.Rows[i].Cells[j].Value.ToString();
                                tmp = tmp.ToLower() == "true" ? "√" : tmp;
                                tmp = tmp.ToLower() == "false" ? "" : tmp;
                                mysheet.Cells[i + 4, jj + 1] = tmp;
                            }
                            else
                            {
                                mysheet.Cells[i + 4, jj + 1] = "";
                            }
                            jj++;
                        }
                    }
                }
                //添加 合计行

                if (rows > 0)
                {
                    mysheet.Cells[rows + 4, 1] = "合计:";
                    if (countCoumn == 1)
                        mysheet.Cells[rows + 4, 1] = "合计:" + rows.ToString();
                    if (countCoumn > 1)
                        mysheet.Cells[rows + 4, countCoumn] = rows.ToString();
                    string[] cloumns = staColumns.Split(',');
                    try
                    {
                        foreach (string str in cloumns)
                        {
                            int i = int.Parse(str);
                            int ch = 'A' + i - 1;
                            char c = (Char)ch;
                            mysheet.Cells[rows + 4, i] = "=SUM(" + c.ToString() + "4:" + c.ToString() + (rows + 3).ToString() + ")";
                        }
                    }
                    catch { }
                }

                //添加备注
                if (remark != "")
                {
                    mysheet.Cells[rows + 5, 1] = remark;
                    mysheet.get_Range(mysheet.Cells[rows + 5, 1], mysheet.Cells[rows + 5, columns]).Select();
                    mysheet.get_Range(mysheet.Cells[rows + 5, 1], mysheet.Cells[rows + 5, columns]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                }

                //自定义公式 格式"F,实载率,C{0}/D{1}|G,总班次,C{0}+E{1}"
                if (formula.Length > 0)
                {
                    string[] formulas = formula.Split('|');
                    try
                    {
                        foreach (string str in formulas)
                        {
                            string[] param = str.Split(',');
                            int i = char.Parse(param[0]);
                            mysheet.Cells[3, i - 64] = param[1];
                            mysheet.Cells[rows + 4, i - 64] = "=" + string.Format(param[2], rows + 4, rows + 4);
                        }
                    }
                    catch { }
                }

                //设置格式

                //设置 所有文字大小为9
                try
                {
                    mysheet.get_Range(mysheet.Cells[1, 1], mysheet.Cells[rows + 5, columns]).Font.Size = 9;
                }
                catch { }
                try
                {
                    //设置 表格为最适应宽度                
                    mysheet.get_Range(mysheet.Cells[2, 1], mysheet.Cells[rows + 4, columns]).Select();
                    mysheet.get_Range(mysheet.Cells[2, 1], mysheet.Cells[rows + 4, columns]).Columns.AutoFit();
                }
                catch { }
                try
                {
                    //设置标题跨列居中
                    mysheet.get_Range(mysheet.Cells[1, 1], mysheet.Cells[1, columns]).Select();
                    mysheet.get_Range(mysheet.Cells[1, 1], mysheet.Cells[1, columns]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenterAcrossSelection;
                }
                catch { }
                try
                {
                    //加边框
                    //内部为虚线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders.LineStyle = Excel.XlLineStyle.xlDash; //设置虚线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders.Weight = Excel.XlBorderWeight.xlHairline;//设为极细
                    //设置左边线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlMedium;
                    //设置上边线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
                    //设置右边线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                    //设置下边线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
                }
                catch { }
                try
                {
                    //设置 纸张为A4 纵向
                    mysheet.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
                    mysheet.PageSetup.Orientation = Excel.XlPageOrientation.xlPortrait;
                }
                catch { }
                books.Visible = true;

                //mybook.SaveCopyAs("行包受理员营收汇总.xls");//.Save();
                mybook = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(books);
                books = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出Excel表出错，可能您的机器未安装Excel!系统报错：" + ex.Message, "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (DialogResult.Yes == MessageBox.Show("您是否想保存Excel?", "保存提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    //  ExcelSaver.SaveExcel(grid, name, title, staDate, endDate, staColumns, pointTxt, countCoumn, remark);
                }

            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// 新控件gridview导出数据到Excel
        /// </summary>
        /// <param name="grid">要导出数据的datagridview</param>
        /// <param name="name">表格名称</param>
        /// <param name="title">标题</param>
        /// <param name="staDate">统计起始日期</param>
        /// <param name="endDate">统计终止日期</param>
        /// <param name="staColumns">需要统计数据的列</param>
        /// <param name="pointTxt">指定为文本的列</param>
        /// <param name="countCoumn">合计行数列</param>
        /// <param name="formula">自定义公式（"F,实载率,C{0}/D{1}|G,总班次,C{0}+E{1}"）</param>
        /// <param name="remark">表格底部 要添加的备注</param>
        public static void Export(DevExpress.XtraGrid.GridControl grid, string name, string title, string staDate, string endDate, string staColumns, string pointTxt, int countCoumn, string formula, string remark)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.GridView dgv = (DevExpress.XtraGrid.Views.Grid.GridView)grid.MainView;
                if (dgv.RowCount < 1)
                {
                    return;
                }
                Excel.Application books = new Microsoft.Office.Interop.Excel.Application();

                Excel.Workbook mybook = (Excel.Workbook)books.Workbooks.Add(1);
                ((Excel.Worksheet)mybook.Worksheets[1]).Name = name;
                Excel.Worksheet mysheet = (Excel.Worksheet)mybook.Worksheets[1];
                Excel.Range range;

                //第一行 标题
                mysheet.Cells[1, 1] = title;
                if (staDate.Length > 0 || endDate.Length > 0)
                {
                    //第二行 统计日期
                    mysheet.Cells[2, 1] = "统计日期：";//+staDate+"  -  "+endDate;
                    mysheet.Cells[2, 2] = staDate;
                    mysheet.Cells[2, 3] = endDate;

                    //mysheet.Cells[2, 10] = "检验员：";
                }

                //填标题
                int columns = 0; //列数
                for (int j = 0, jj = 0; j < dgv.Columns.Count; j++) //j 与jj不一定相等 只有 所有列的Visible属性全部都为true时才相等
                {
                    if (dgv.Columns[j].Visible == true)
                    {
                        mysheet.Cells[3, jj + 1] = dgv.Columns[j].Caption.ToString();
                        jj++;
                        columns = jj;
                    }
                }
                if (pointTxt.Length > 0)
                {
                    string[] points = pointTxt.Split(',');
                    foreach (string str in points)
                    {
                        int i = int.Parse(str);
                        int ch = 'A' + i - 1;
                        char c = (Char)ch;
                        mysheet.get_Range(c.ToString() + 4.ToString(), c.ToString() + (4 + dgv.RowCount).ToString()).NumberFormatLocal = "@";
                    }
                }

                int rows = dgv.RowCount;//行数
                //填  数据
                for (int i = 0; i < dgv.RowCount; i++)
                {
                    for (int j = 0, jj = 0; j < dgv.Columns.Count; j++)
                    {
                        if (dgv.Columns[j].Visible == true)
                        {
                            string columnName = dgv.Columns[j].FieldName;
                            DataRow row = dgv.GetDataRow(i);
                            if (row[columnName] != null)
                            {
                                switch (row[columnName].GetType().ToString())
                                {
                                    case "System.String"://字符串类型

                                        break;
                                    case "System.DateTime"://日期类型
                                        System.DateTime dateV;

                                        break;
                                    case "System.Boolean"://布尔型
                                        bool boolV = false;

                                        break;
                                    case "System.Int16"://整型
                                    case "System.Int32":
                                    case "System.Int64":
                                    case "System.Byte":
                                        int intV = 0;

                                        break;
                                    case "System.Decimal"://浮点型
                                    case "System.Double":
                                        range = (Excel.Range)mysheet.Cells[i + 4, jj + 1];
                                        range.NumberFormatLocal = "0";
                                        break;
                                    case "System.DBNull"://空值处理

                                        break;
                                    default:
                                        break;

                                }
                                string tmp = row[columnName].ToString();
                                tmp = tmp.ToLower() == "true" ? "√" : tmp;
                                tmp = tmp.ToLower() == "false" ? "" : tmp;
                                mysheet.Cells[i + 4, jj + 1] = tmp;
                            }
                            else
                            {
                                mysheet.Cells[i + 4, jj + 1] = "";
                            }
                            jj++;
                        }
                    }
                }
                //添加 合计行

                if (rows > 0)
                {
                    mysheet.Cells[rows + 4, 1] = "合计:";
                    if (countCoumn == 1)
                        mysheet.Cells[rows + 4, 1] = "合计:" + rows.ToString();
                    if (countCoumn > 1)
                        mysheet.Cells[rows + 4, countCoumn] = rows.ToString();
                    string[] cloumns = staColumns.Split(',');
                    try
                    {
                        foreach (string str in cloumns)
                        {
                            int i = int.Parse(str);
                            int ch = 'A' + i - 1;
                            char c = (Char)ch;
                            mysheet.Cells[rows + 4, i] = "=SUM(" + c.ToString() + "4:" + c.ToString() + (rows + 3).ToString() + ")";
                        }
                    }
                    catch { }
                }

                //添加备注
                if (remark != "")
                {
                    mysheet.Cells[rows + 5, 1] = remark;
                    mysheet.get_Range(mysheet.Cells[rows + 5, 1], mysheet.Cells[rows + 5, columns]).Select();
                    mysheet.get_Range(mysheet.Cells[rows + 5, 1], mysheet.Cells[rows + 5, columns]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                }

                //自定义公式 格式"F,实载率,C{0}/D{1}|G,总班次,C{0}+E{1}"
                if (formula.Length > 0)
                {
                    string[] formulas = formula.Split('|');
                    try
                    {
                        foreach (string str in formulas)
                        {
                            string[] param = str.Split(',');
                            int i = char.Parse(param[0]);
                            mysheet.Cells[3, i - 64] = param[1];
                            mysheet.Cells[rows + 4, i - 64] = "=" + string.Format(param[2], rows + 4, rows + 4);
                        }
                    }
                    catch { }
                }

                //设置格式

                //设置 所有文字大小为9
                try
                {
                    mysheet.get_Range(mysheet.Cells[1, 1], mysheet.Cells[rows + 5, columns]).Font.Size = 9;
                }
                catch { }
                try
                {
                    //设置 表格为最适应宽度                
                    mysheet.get_Range(mysheet.Cells[2, 1], mysheet.Cells[rows + 4, columns]).Select();
                    mysheet.get_Range(mysheet.Cells[2, 1], mysheet.Cells[rows + 4, columns]).Columns.AutoFit();
                }
                catch { }
                try
                {
                    //设置标题跨列居中
                    mysheet.get_Range(mysheet.Cells[1, 1], mysheet.Cells[1, columns]).Select();
                    mysheet.get_Range(mysheet.Cells[1, 1], mysheet.Cells[1, columns]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenterAcrossSelection;
                }
                catch { }
                try
                {
                    //加边框
                    //内部为虚线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders.LineStyle = Excel.XlLineStyle.xlDash; //设置虚线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders.Weight = Excel.XlBorderWeight.xlHairline;//设为极细
                    //设置左边线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlMedium;
                    //设置上边线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
                    //设置右边线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                    //设置下边线
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    mysheet.get_Range(mysheet.Cells[3, 1], mysheet.Cells[rows + 4, columns]).Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
                }
                catch { }
                try
                {
                    //设置 纸张为A4 纵向
                    mysheet.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
                    mysheet.PageSetup.Orientation = Excel.XlPageOrientation.xlPortrait;
                }
                catch { }
                books.Visible = true;

                //mybook.SaveCopyAs("行包受理员营收汇总.xls");//.Save();
                mybook = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(books);
                books = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出Excel表出错，可能您的机器未安装Excel!系统报错：" + ex.Message, "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (DialogResult.Yes == MessageBox.Show("您是否想保存Excel?", "保存提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    ExcelSaver.SaveExcel(grid, name, title, staDate, endDate, staColumns, pointTxt, countCoumn, remark);
                }

            }
            finally
            {
                GC.Collect();
            }
        }


        #region  Excel行列标字母顺序
        /// <summary>
        /// 自定义字符转换
        /// </summary>
        /// <param name="c">必须大写字母</param>
        /// <param name="step">字符步长</param>
        /// <returns></returns>
        public static string GetCharByStep(char c, int step)
        {
            string str = "";
            char ret = (char)(c + step);
            if (ret > 'Z')
            {
                str = "A" + ((char)('A' + (ret - 'Z') - 1)).ToString();
            }
            else
                str = ret.ToString();

            return str;
        }
        #endregion

        #region 释放资源
        private static void NAR(object o)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }
            catch
            { }
            finally
            {
                o = null;
            }
        }
        #endregion


        /// <summary>
        /// datatable导出到模板
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="templateURL">模板路径</param>
        /// <param name="staDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="insertPoint">第几行开始插入</param>
        /// <param name="remark">表格底部 要添加的备注</param>
        public static void Export(DataTable dt, string templateURL, string staDate, string endDate, int insertPoint, string remark)
        {
            try
            {
                if (dt.Rows.Count < 1)
                {
                    return;
                }
                Object missing = System.Reflection.Missing.Value;

                Excel.Application books = new Microsoft.Office.Interop.Excel.Application();
                //加载模板
                Excel.Workbook mybook = books.Workbooks.Open(templateURL, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);//.Add(missing);
                //获取第一个工作薄
                Excel.Worksheet mysheet = (Excel.Worksheet)mybook.Worksheets[1];

                //第一行 标题
                //mysheet.Cells[1, 1] = title;
                if (staDate.Length > 0 || endDate.Length > 0)
                {
                    //第二行 统计日期
                    mysheet.Cells[2, 1] = "统计日期：";//+staDate+"  -  "+endDate;
                    mysheet.Cells[2, 2] = staDate;
                    mysheet.Cells[2, 3] = endDate;
                    //mysheet.Cells[2, 10] = "检验员：";
                }
                int columns = dt.Columns.Count; //列数
                int rows = dt.Rows.Count;//行数
                //填  数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    mysheet.get_Range(mysheet.Cells[i + insertPoint, 1], mysheet.Cells[i + insertPoint, 1]).EntireRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown, missing);
                    for (int j = 0, jj = 0; j < dt.Columns.Count; j++)
                    {
                        string tmp = dt.Rows[i][j].ToString();
                        mysheet.Cells[i + insertPoint, jj + 1] = tmp;
                        jj++;
                    }
                }
                //删除模板空余行
                mysheet.get_Range(mysheet.Cells[rows + insertPoint, 1], mysheet.Cells[rows + insertPoint, 1]).EntireRow.Delete();

                //添加 合计行
                mysheet.Cells[rows + insertPoint, 1] = "合计:" + rows.ToString();

                //添加备注
                if (remark != "")
                {
                    mysheet.Cells[rows + insertPoint + 1, 1] = remark;
                    mysheet.get_Range(mysheet.Cells[rows + insertPoint + 1, 1], mysheet.Cells[rows + insertPoint + 1, columns]).Select();
                    mysheet.get_Range(mysheet.Cells[rows + insertPoint + 1, 1], mysheet.Cells[rows + insertPoint + 1, columns]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                }

                //保存
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "excel2007|*.xlsx";
                saveFileDialog1.FileName = templateURL.Substring(templateURL.LastIndexOf('\\') + 1);
                saveFileDialog1.DefaultExt = ".xlsx";
                string fileName = "";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileName = saveFileDialog1.FileName;
                    mybook.SaveAs(fileName, missing, missing, missing, missing, missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, missing, missing, missing, missing, missing);
                }

                //按顺序释放资源。
                NAR(mysheet);
                NAR(mybook);
                books.Quit();
                NAR(books);
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出Excel表出错，可能您的机器未安装Excel!系统报错：" + ex.Message, "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                //if (DialogResult.Yes == MessageBox.Show("您是否想保存Excel?", "保存提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                //{
                //    ExcelSaver.SaveExcel(dt, bookName, title, staDate, endDate, staColumns, pointTxt, countCoumn, remark);
                //}
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// gridview导出到模板
        /// </summary>
        /// <param name="grid">数据源</param>
        /// <param name="templateURL">模板路径</param>
        /// <param name="staDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="insertPoint">第几行开始插入</param>
        /// <param name="remark">表格底部 要添加的备注</param>
        public static void Export(DataGridView grid, string templateURL, string staDate, string endDate, int insertPoint, string remark)
        {
            try
            {
                if (grid.Rows.Count < 1)
                {
                    return;
                }
                Object missing = System.Reflection.Missing.Value;

                Excel.Application books = new Microsoft.Office.Interop.Excel.Application();
                //加载模板
                Excel.Workbook mybook = books.Workbooks.Open(templateURL, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);//.Add(missing);
                //获取第一个工作薄
                Excel.Worksheet mysheet = (Excel.Worksheet)mybook.Worksheets[1];

                //第一行 标题
                //mysheet.Cells[1, 1] = title;
                if (staDate.Length > 0 || endDate.Length > 0)
                {
                    //第二行 统计日期
                    mysheet.Cells[2, 1] = "统计日期：";//+staDate+"  -  "+endDate;
                    mysheet.Cells[2, 2] = staDate;
                    mysheet.Cells[2, 3] = endDate;
                    //mysheet.Cells[2, 10] = "检验员：";
                }
                int columns = grid.Columns.Count; //列数
                int rows = grid.Rows.Count;//行数
                //填  数据
                for (int i = 0; i < grid.Rows.Count; i++)
                {
                    mysheet.get_Range(mysheet.Cells[i + insertPoint, 1], mysheet.Cells[i + insertPoint, 1]).EntireRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown, missing);
                    for (int j = 0, jj = 0; j < grid.Columns.Count; j++)
                    {
                        if (grid.Columns[j].Visible == true)
                        {
                            if (grid.Rows[i].Cells[j].Value != null)
                            {
                                string tmp = grid.Rows[i].Cells[j].Value.ToString();
                                tmp = tmp.ToLower() == "true" ? "√" : tmp;
                                tmp = tmp.ToLower() == "false" ? "" : tmp;
                                mysheet.Cells[i + insertPoint, jj + 1] = tmp;
                            }
                            else
                            {
                                mysheet.Cells[i + insertPoint, jj + 1] = "";
                            }
                            jj++;
                        }
                    }
                }

                //删除模板空余行
                mysheet.get_Range(mysheet.Cells[rows + insertPoint, 1], mysheet.Cells[rows + insertPoint, 1]).EntireRow.Delete();

                //添加 合计行
                mysheet.Cells[rows + insertPoint, 1] = "合计:" + rows.ToString();

                //添加备注
                if (remark != "")
                {
                    mysheet.Cells[rows + insertPoint + 1, 1] = remark;
                    mysheet.get_Range(mysheet.Cells[rows + insertPoint + 1, 1], mysheet.Cells[rows + insertPoint + 1, columns]).Select();
                    mysheet.get_Range(mysheet.Cells[rows + insertPoint + 1, 1], mysheet.Cells[rows + insertPoint + 1, columns]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                }

                //保存
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "excel2007|*.xlsx";
                saveFileDialog1.FileName = templateURL.Substring(templateURL.LastIndexOf('\\') + 1);
                saveFileDialog1.DefaultExt = ".xlsx";
                string fileName = "";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileName = saveFileDialog1.FileName;
                    mybook.SaveAs(fileName, missing, missing, missing, missing, missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, missing, missing, missing, missing, missing);
                }

                //按顺序释放资源。
                NAR(mysheet);
                NAR(mybook);
                books.Quit();
                NAR(books);
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出Excel表出错，可能您的机器未安装Excel!系统报错：" + ex.Message, "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                //if (DialogResult.Yes == MessageBox.Show("您是否想保存Excel?", "保存提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                //{
                //    ExcelSaver.SaveExcel(dt, bookName, title, staDate, endDate, staColumns, pointTxt, countCoumn, remark);
                //}
            }
            finally
            {
                GC.Collect();
            }
        }
    
        /// <summary>
        /// 新控件gridcontrol导出到模板
        /// </summary>
        /// <param name="grid">数据源</param>
        /// <param name="templateURL">模板路径</param>
        /// <param name="staDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="insertPoint">第几行开始插入</param>
        /// <param name="remark">表格底部 要添加的备注</param>
        public static void Export(DevExpress.XtraGrid.GridControl grid, string templateURL, string staDate, string endDate, int insertPoint, string remark)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.GridView dgv = (DevExpress.XtraGrid.Views.Grid.GridView)grid.MainView;
                if (dgv.RowCount < 1)
                {
                    return;
                }
                Object missing = System.Reflection.Missing.Value;

                Excel.Application books = new Microsoft.Office.Interop.Excel.Application();
                //加载模板
                Excel.Workbook mybook = books.Workbooks.Open(templateURL, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);//.Add(missing);
                //获取第一个工作薄
                Excel.Worksheet mysheet = (Excel.Worksheet)mybook.Worksheets[1];

                //第一行 标题
                //mysheet.Cells[1, 1] = title;
                if (staDate.Length > 0 || endDate.Length > 0)
                {
                    //第二行 统计日期
                    mysheet.Cells[2, 1] = "统计日期：";//+staDate+"  -  "+endDate;
                    mysheet.Cells[2, 2] = staDate;
                    mysheet.Cells[2, 3] = endDate;
                    //mysheet.Cells[2, 10] = "检验员：";
                }
                int columns = dgv.Columns.Count; //列数
                int rows = dgv.RowCount;//行数
                //填  数据
                for (int i = 0; i < dgv.RowCount; i++)
                {
                    for (int j = 0, jj = 0; j < dgv.Columns.Count; j++)
                    {
                        if (dgv.Columns[j].Visible == true)
                        {
                            string columnName = dgv.Columns[j].FieldName;
                            DataRow row = dgv.GetDataRow(i);
                            if (row[columnName] != null)
                            {
                                string tmp = row[columnName].ToString();
                                tmp = tmp.ToLower() == "true" ? "√" : tmp;
                                tmp = tmp.ToLower() == "false" ? "" : tmp;
                                mysheet.Cells[i + 4, jj + 1] = tmp;
                            }
                            else
                            {
                                mysheet.Cells[i + 4, jj + 1] = "";
                            }
                            jj++;
                        }
                    }
                }
                //删除模板空余行
                mysheet.get_Range(mysheet.Cells[rows + insertPoint, 1], mysheet.Cells[rows + insertPoint, 1]).EntireRow.Delete();

                //添加 合计行
                mysheet.Cells[rows + insertPoint, 1] = "合计:" + rows.ToString();

                //添加备注
                if (remark != "")
                {
                    mysheet.Cells[rows + insertPoint + 1, 1] = remark;
                    mysheet.get_Range(mysheet.Cells[rows + insertPoint + 1, 1], mysheet.Cells[rows + insertPoint + 1, columns]).Select();
                    mysheet.get_Range(mysheet.Cells[rows + insertPoint + 1, 1], mysheet.Cells[rows + insertPoint + 1, columns]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                }

                //保存
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "excel2007|*.xlsx";
                saveFileDialog1.FileName = templateURL.Substring(templateURL.LastIndexOf('\\') + 1);
                saveFileDialog1.DefaultExt = ".xlsx";
                string fileName = "";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileName = saveFileDialog1.FileName;
                    mybook.SaveAs(fileName, missing, missing, missing, missing, missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, missing, missing, missing, missing, missing);
                }

                //按顺序释放资源。
                NAR(mysheet);
                NAR(mybook);
                books.Quit();
                NAR(books);
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出Excel表出错，可能您的机器未安装Excel!系统报错：" + ex.Message, "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                //if (DialogResult.Yes == MessageBox.Show("您是否想保存Excel?", "保存提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                //{
                //    ExcelSaver.SaveExcel(dt, bookName, title, staDate, endDate, staColumns, pointTxt, countCoumn, remark);
                //}
            }
            finally
            {
                GC.Collect();
            }
        }

    }
}
