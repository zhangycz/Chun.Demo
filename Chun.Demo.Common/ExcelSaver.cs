using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;

namespace Chun.Demo.Common
{
    public class ExcelSaver
    {
        static SaveFileDialog saveFileDialog = new SaveFileDialog();
        /// <summary>
        /// 从table导出数据  保存为Excel
        /// </summary>
        /// <param name="dt">要导出数据的table</param>
        /// <param name="name">表格名称</param>
        /// <param name="title">标题</param>
        /// <param name="staDate">统计起始日期</param>
        /// <param name="endDate">统计终止日期</param>
        /// <param name="staColumns">需要统计数据的列</param>
        /// <param name="pointTxt">指定为文本的列</param>
        /// <param name="countCoumn">合计行数列</param>
        /// <param name="remark">表格底部 要添加的备注</param>
        public static void SaveExcel(DataTable dt, string name, string title, string staDate, string endDate, string staColumns, string pointTxt, int countCoumn, string remark)
        {
            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "保存为Excel文件";
            saveFileDialog.FileName = name + ".xls";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName.IndexOf(":") < 0) return; //被点了"取消"

            Stream myStream;
            myStream = saveFileDialog.OpenFile();
            StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));

            //第一行 标题
            string dt_title = "";
            dt_title = title + "\t";
            sw.WriteLine(dt_title);
            //第二行 统计日期
            string sta_date = "";
            sta_date = "统计日期：\t" + staDate + "\t" + endDate;
            sw.WriteLine(sta_date);

            string columnTitle = "";
            try
            {
                //写入列标题
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i >= 0)
                    {
                        if (columnTitle.Length == 0)
                        {
                            columnTitle += dt.Columns[i].ColumnName;
                        }
                        else
                        {

                            columnTitle += "\t" + dt.Columns[i].ColumnName;
                        }
                    }
                }
                sw.WriteLine(columnTitle);

                string[] points = pointTxt.Split(',');

                //写入列内容
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string columnValue = "";
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        if (k >= 0)
                        {
                            if (columnValue.Length > 0)
                                columnValue += "\t";
                        }
                        else
                        {
                            continue;
                        }
                        if (dt.Rows[j][k].ToString().Length == 0)
                            columnValue += "";
                        else
                        {
                            bool isTxt = false;
                            foreach (string str in points)
                            {
                                if (str == (k + 1).ToString())
                                {
                                    isTxt = true; break;
                                }
                            }
                            if (isTxt)
                                columnValue += "'" + dt.Rows[j][k].ToString().Trim();
                            else
                                columnValue += dt.Rows[j][k].ToString().Trim();
                        }
                    }
                    sw.WriteLine(columnValue);
                }

                //添加 合计行

                if (dt.Rows.Count > 0)
                {
                    string sta_info = "";
                    if (countCoumn > 0)
                    {
                        if (countCoumn == 1)
                            sta_info = "合计:" + dt.Rows.Count.ToString();
                        else
                        {
                            sta_info = "合计:\t" + dt.Rows.Count.ToString();
                        }
                    }
                    else
                    {
                        sta_info = "合计:\t";
                    }

                    string[] cloumns = staColumns.Split(',');
                    try
                    {
                        for (int k = 1; k <= dt.Columns.Count; k++)
                        {

                            if (k == 1 && countCoumn == 1)
                                continue;
                            foreach (string str in cloumns)
                            {
                                int i = int.Parse(str);
                                if (countCoumn == 1)
                                {
                                    if (i == k)
                                    {
                                        int ch = 'A' + i - 1;
                                        char c = (Char)ch;
                                        sta_info += "=SUM(" + c.ToString() + "4:" + c.ToString() + (dt.Rows.Count + 3).ToString() + ")";
                                    }
                                }
                                else
                                {
                                    if (i == k + 1)
                                    {
                                        int ch = 'A' + i - 1;
                                        char c = (Char)ch;
                                        sta_info += "=SUM(" + c.ToString() + "4:" + c.ToString() + (dt.Rows.Count + 3).ToString() + ")";
                                    }
                                }
                            }
                            sta_info += "\t";
                        }

                    }
                    catch { }
                    sw.WriteLine(sta_info);

                    if (remark.Length > 0)
                    {
                        sw.WriteLine(remark);
                    }
                }

                sw.Close();
                myStream.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("保存EXCEL出错！系统报错：" + e.ToString());
            }
            finally
            {
                sw.Close();
                myStream.Close();
            }

        }
        
        private static DialogResult result;

        private static void InvokeMethod()
        {
            result = saveFileDialog.ShowDialog();
        }

        private static Thread invokeThread;

        /// <summary>
        /// 从datagridview导出数据 保存为Excel
        /// </summary>
        /// <param name="grid">要导出数据的datagridview</param>
        /// <param name="name">表格名称</param>
        /// <param name="title">标题</param>
        /// <param name="staDate">统计起始日期</param>
        /// <param name="endDate">统计终止日期</param>
        /// <param name="staColumns">需要统计数据的列</param>
        /// <param name="pointTxt">指定为文本的列</param>
        /// <param name="countCoumn">合计行数列</param>
        /// <param name="remark">表格底部 要添加的备注</param>
        public static void SaveExcel(DataGridView grid, string name, string title, string staDate, string endDate, string staColumns, string pointTxt, int countCoumn, string remark)
        {
            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "保存为Excel文件";
            saveFileDialog.FileName = name + ".xls";

            invokeThread = new Thread(new ThreadStart(InvokeMethod));
            invokeThread.SetApartmentState(ApartmentState.STA);
            invokeThread.Start();
            invokeThread.Join();

            if (result != DialogResult.OK)
            {
                return;
            }
            //saveFileDialog.ShowDialog();

            //if (saveFileDialog.FileName.IndexOf(":") < 0) return; //被点了"取消"
             Stream myStream;
             myStream = saveFileDialog.OpenFile();
            StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));

            //第一行 标题
            string dt_title = "";
            dt_title = title + "\t";
            sw.WriteLine(dt_title);
            //第二行 统计日期
            string sta_date = "";
            sta_date = "统计日期：\t" + staDate + "\t" + endDate;
            sw.WriteLine(sta_date);


            string columnTitle = "";
            try
            {
                //写入列标题
                for (int i = 0; i < grid.ColumnCount; i++)
                {
                    if (i >= 0 && grid.Columns[i].Visible == true)
                    {
                        if (columnTitle.Length == 0)
                        {
                            columnTitle += grid.Columns[i].HeaderText;
                        }
                        else
                        {

                            columnTitle += "\t" + grid.Columns[i].HeaderText;
                        }
                    }
                }
                sw.WriteLine(columnTitle);

                string[] points = pointTxt.Split(',');

                //写入列内容
                for (int j = 0; j < grid.Rows.Count; j++)
                {
                    string columnValue = "";
                    for (int k = 0; k < grid.Columns.Count; k++)
                    {
                        if (k >= 0 && grid.Columns[k].Visible == true)
                        {
                            if (columnValue.Length > 0)
                                columnValue += "\t";
                        }
                        else
                        {
                            continue;
                        }
                        if (grid.Rows[j].Cells[k].Value == null)
                            columnValue += "";
                        else
                        {
                            bool isTxt = false;
                            foreach (string str in points)
                            {
                                if (str == (k + 1).ToString())
                                {
                                    isTxt = true; break;
                                }
                            }
                            if (isTxt)
                                columnValue += "'" + grid.Rows[j].Cells[k].Value.ToString().Trim();
                            else
                            {
                                string tmp = grid.Rows[j].Cells[k].Value.ToString().Trim();
                                tmp = tmp.ToLower() == "true" ? "√" : tmp;
                                tmp = tmp.ToLower() == "false" ? "" : tmp;
                                columnValue += tmp;
                            }
                        }
                    }
                    sw.WriteLine(columnValue);
                }

                //添加 合计行

                if (grid.Rows.Count > 0)
                {
                    string sta_info = "";
                    if (countCoumn > 0)
                    {
                        if (countCoumn == 1)
                            sta_info = "合计:" + grid.Rows.Count.ToString();
                        else
                        {
                            sta_info = "合计:\t" + grid.Rows.Count.ToString();
                        }
                    }
                    else
                    {
                        sta_info = "合计:\t";
                    }

                    string[] cloumns = staColumns.Split(',');
                    try
                    {
                        for (int k = 1; k <= grid.Columns.Count; k++)
                        {

                            if (k == 1 && countCoumn == 1)
                            {
                                sta_info += "\t";
                                continue;
                            }
                            foreach (string str in cloumns)
                            {
                                int i = int.Parse(str);
                                if (countCoumn == 1)
                                {
                                    if (i == k)
                                    {
                                        int ch = 'A' + i - 1;
                                        char c = (Char)ch;
                                        sta_info += "=SUM(" + c.ToString() + "4:" + c.ToString() + (grid.Rows.Count + 3).ToString() + ")";
                                    }
                                }
                                else
                                {
                                    if (i == k + 1)
                                    {
                                        int ch = 'A' + i - 1;
                                        char c = (Char)ch;
                                        sta_info += "=SUM(" + c.ToString() + "4:" + c.ToString() + (grid.Rows.Count + 3).ToString() + ")";
                                    }
                                }
                            }
                            sta_info += "\t";
                        }

                    }
                    catch { }
                    sw.WriteLine(sta_info);

                    if (remark.Length > 0)
                    {
                        sw.WriteLine(remark);
                    }
                }

                sw.Close();
                myStream.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("保存EXCEL出错！系统报错：" + e.ToString());
            }
            finally
            {
                sw.Close();
                myStream.Close();
            }
        }

        /// <summary>
        /// 从datagridview导出数据 保存为Excel
        /// </summary>
        /// <param name="grid">要导出数据的datagridview</param>
        /// <param name="name">表格名称</param>
        /// <param name="title">标题</param>
        /// <param name="staDate">统计起始日期</param>
        /// <param name="endDate">统计终止日期</param>
        /// <param name="staColumns">需要统计数据的列</param>
        /// <param name="pointTxt">指定为文本的列</param>
        /// <param name="countCoumn">合计行数列</param>
        /// <param name="remark">表格底部 要添加的备注</param>
        public static void SaveExcel(DevExpress.XtraGrid.GridControl grid, string name, string title, string staDate, string endDate, string staColumns, string pointTxt, int countCoumn, string remark)
        {
            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "保存为Excel文件";
            saveFileDialog.FileName = name + ".xls";

            invokeThread = new Thread(new ThreadStart(InvokeMethod));
            invokeThread.SetApartmentState(ApartmentState.STA);
            invokeThread.Start();
            invokeThread.Join();

            if (result != DialogResult.OK)
            {
                return;
            }
            //saveFileDialog.ShowDialog();

            //if (saveFileDialog.FileName.IndexOf(":") < 0) return; //被点了"取消"
            Stream myStream;
            myStream = saveFileDialog.OpenFile();
            StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));

            //第一行 标题
            string dt_title = "";
            dt_title = title + "\t";
            sw.WriteLine(dt_title);
            //第二行 统计日期
            string sta_date = "";
            sta_date = "统计日期：\t" + staDate + "\t" + endDate;
            sw.WriteLine(sta_date);

            DevExpress.XtraGrid.Views.Grid.GridView dgv = (DevExpress.XtraGrid.Views.Grid.GridView)grid.MainView;
            string columnTitle = "";
            try
            {
                //写入列标题
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    if (i >= 0 && dgv.Columns[i].Visible == true)
                    {
                        if (columnTitle.Length == 0)
                        {
                            columnTitle += dgv.Columns[i].Caption;
                        }
                        else
                        {

                            columnTitle += "\t" + dgv.Columns[i].Caption;
                        }
                    }
                }
                sw.WriteLine(columnTitle);

                string[] points = pointTxt.Split(',');

                //写入列内容
                for (int j = 0; j < dgv.RowCount; j++)
                {
                    string columnValue = "";
                    for (int k = 0; k < dgv.Columns.Count; k++)
                    {
                        string columnName = dgv.Columns[k].FieldName;
                        DataRow row = dgv.GetDataRow(j);
                        if (k >= 0 && dgv.Columns[columnName].Visible == true)
                        {
                            if (columnValue.Length > 0)
                                columnValue += "\t";
                        }
                        else
                        {
                            continue;
                        }
                        if (row[columnName].ToString() == "")
                            columnValue += "";
                        else
                        {
                            bool isTxt = false;
                            foreach (string str in points)
                            {
                                if (str == (k + 1).ToString())
                                {
                                    isTxt = true; break;
                                }
                            }
                            if (isTxt)
                                columnValue += "'" + row[columnName].ToString().Trim();
                            else
                            {
                                string tmp = row[columnName].ToString().Trim();
                                tmp = tmp.ToLower() == "true" ? "√" : tmp;
                                tmp = tmp.ToLower() == "false" ? "" : tmp;
                                columnValue += tmp;
                            }
                        }
                    }
                    sw.WriteLine(columnValue);
                }

                //添加 合计行

                if (dgv.RowCount > 0)
                {
                    string sta_info = "";
                    if (countCoumn > 0)
                    {
                        if (countCoumn == 1)
                            sta_info = "合计:" + dgv.RowCount.ToString();
                        else
                        {
                            sta_info = "合计:\t" + dgv.RowCount.ToString();
                        }
                    }
                    else
                    {
                        sta_info = "合计:\t";
                    }

                    string[] cloumns = staColumns.Split(',');
                    try
                    {
                        for (int k = 1; k <= dgv.Columns.Count; k++)
                        {

                            if (k == 1 && countCoumn == 1)
                            {
                                sta_info += "\t";
                                continue;
                            }
                            foreach (string str in cloumns)
                            {
                                int i = int.Parse(str);
                                if (countCoumn == 1)
                                {
                                    if (i == k)
                                    {
                                        int ch = 'A' + i - 1;
                                        char c = (Char)ch;
                                        sta_info += "=SUM(" + c.ToString() + "4:" + c.ToString() + (dgv.RowCount + 3).ToString() + ")";
                                    }
                                }
                                else
                                {
                                    if (i == k + 1)
                                    {
                                        int ch = 'A' + i - 1;
                                        char c = (Char)ch;
                                        sta_info += "=SUM(" + c.ToString() + "4:" + c.ToString() + (dgv.RowCount + 3).ToString() + ")";
                                    }
                                }
                            }
                            sta_info += "\t";
                        }

                    }
                    catch { }
                    sw.WriteLine(sta_info);

                    if (remark.Length > 0)
                    {
                        sw.WriteLine(remark);
                    }
                }

                sw.Close();
                myStream.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("保存EXCEL出错！系统报错：" + e.ToString());
            }
            finally
            {
                sw.Close();
                myStream.Close();
            }
        }

    }
}
