/**************Code Info**************************
* Copyright(c) 2012-2013
* CLR 版本  4.0
* 文 件 名：
* 创 建 人： Rongqh
* 创建日期：2012/8/1 14:53:00
* 修 改 人：
* 修改日期：
* 备注描述：指定XML文件转换为对象
*           
*************************************************/

using System;
using System.Collections.Generic;
using System.Xml;

namespace Chun.Demo.Common
{
    /// <summary>
    ///     基本表文件类
    /// </summary>
    [Serializable]
    public class BaseTableConfigFile
    {
        #region - 方法 -

        /// <summary>
        ///     基本表的XML文件转化为对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static BaseTableConfigFile GetBaseTableConfigFile(string fileName) {
            try {
                var bc = new BaseTableConfigFile();
                var filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Config\BaseTableSetXML\" + fileName + ".xml";
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                var node = xmlDoc.SelectSingleNode("Header");
                bc.Header = new BaseTableConfigFileHeader {
                    TitleName = node.Attributes["TitleName"].Value,
                    Autosizemode = node.Attributes["Autosizemode"].Value,
                    HeaderVisible = node.Attributes["HeaderVisible"].Value,
                    Index = node.Attributes["Index"].Value,
                    TitleValue = node.Attributes["TitleValue"].Value,
                    Function = node.Attributes["Function"].Value
                };
                var nodeList = xmlDoc.SelectSingleNode("Header").ChildNodes;
                bc.Colume = new List<BaseTableConfigFileColumn>();
                foreach (XmlNode nd in nodeList) {
                    var cf = new BaseTableConfigFileColumn {
                        CanNull = Convert.ToBoolean(nd.Attributes["CanNull"].Value),
                        ColumeIndex = Convert.ToInt32(nd.Attributes["ColumeIndex"].Value),
                        ColumeName = nd.Attributes["ColumeName"].Value,
                        ColumeValue = nd.Attributes["ColumeValue"].Value,
                        ColumnLength = Convert.ToInt32(nd.Attributes["ColumnLength"].Value),
                        ColumnType = Convert.ToInt32(nd.Attributes["ColumnType"].Value),
                        Connect = nd.Attributes["Connect"].Value,
                        Key = Convert.ToBoolean(nd.Attributes["Key"].Value),
                        Visible = Convert.ToBoolean(nd.Attributes["Visible"].Value)
                    };
                    bc.Colume.Add(cf);
                }
                return bc;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        #endregion

        #region - 属性 -

        public BaseTableConfigFileHeader Header { get; set; }

        public List<BaseTableConfigFileColumn> Colume { get; set; }

        #endregion
    }

    #region BaseTableConfigFileHeader类

    /// <summary>
    ///     基本表属性
    /// </summary>
    [Serializable]
    public class BaseTableConfigFileHeader
    {
        /// <summary>
        ///     窗体名称
        /// </summary>
        public string TitleName { get; set; }

        /// <summary>
        ///     窗体表Code
        /// </summary>
        public string TitleValue { get; set; }

        /// <summary>
        ///     显示格式（None，Fill）
        /// </summary>
        public string Autosizemode { get; set; }

        /// <summary>
        ///     是否可见
        /// </summary>
        public string HeaderVisible { get; set; }

        /// <summary>
        ///     基本表Id
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        ///     1-只读|2-可编辑|3-编辑删除修改
        /// </summary>
        public string Function { get; set; }
    }

    #endregion

    #region BaseTableConfigFileColumn类

    /// <summary>
    ///     基本表列属性
    /// </summary>
    [Serializable]
    public class BaseTableConfigFileColumn
    {
        /// <summary>
        ///     序号
        /// </summary>
        public int ColumeIndex { get; set; }

        /// <summary>
        ///     名称
        /// </summary>
        public string ColumeName { get; set; }

        /// <summary>
        ///     列字段
        /// </summary>
        public string ColumeValue { get; set; }

        /// <summary>
        ///     列类型
        /// </summary>
        public int ColumnType { get; set; }

        /// <summary>
        ///     列长
        /// </summary>
        public int ColumnLength { get; set; }

        /// <summary>
        ///     是否主键
        /// </summary>
        public bool Key { get; set; }

        /// <summary>
        ///     是否为空
        /// </summary>
        public bool CanNull { get; set; }

        /// <summary>
        ///     序号
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        ///     下拉框关联
        /// </summary>
        public string Connect { get; set; }
    }

    #endregion


    #region DataGridView

    [Serializable]
    public class Colume
    {
        public string Width { get; set; }
        public string Name { get; set; }
        public string DataPropertyName { get; set; }
        public bool Visible { get; set; }
        public bool Frozen { get; set; }
        public bool ReadOnly { get; set; }
        public string ColumnType { get; set; }
    }

    [Serializable]
    public class DgvXml
    {
        public string Autosizemode { get; set; }
        public string HeaderVisible { get; set; }
        public bool ShowAutoFilterRow { get; set; }
        public bool MultiSelect { get; set; }
        public bool AllowGroup { get; set; }
        public bool EnableColumnMenu { get; set; }
        public List<Colume> columes { get; set; }
        public string EditTime { get; set; }
        public string FontFamily { get; set; }
        public string FontStyle { get; set; }
        public string FontSize { get; set; }
        public string FontColor { get; set; }
        public string BackColor { get; set; }
        public string SelectColor { get; set; }
        public string OddColor { get; set; }
        public string HeadColor { get; set; }
    }

    #endregion
}