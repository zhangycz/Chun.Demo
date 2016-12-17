/**************Code Info**************************
* Copyright(c) 2012-2013
* CLR 版本  4.0
* 文 件 名：
* 创 建 人： Rongqh
* 创建日期：2012/8/1 14:53:00
* 修 改 人：
* 修改日期：
* 备注描述：序列化操作类
*           
*************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Chun.Demo.Common
{
    public class FileSerializeOper
    {
        #region - 方法 -

        /// <summary>
        /// 根据XML文件名获得该对象
        /// </summary>
        /// <param name="upperFile">文件的上级目录名称</param>
        /// <param name="XmlName">文件名称</param>
        /// <returns></returns>
        public static object GetXmlObject(string upperFile, string XmlName)
        {
            switch (upperFile)
            {
                case "BaseTableSetXML": return BaseTableConfigFile.GetBaseTableConfigFile(XmlName);
                //case "Province": ;
                default: return BaseTableConfigFile.GetBaseTableConfigFile(XmlName);
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="me">序列化的对象</param>
        /// <param name="FILENAME">序列化文件(绝对路径)</param>
        public static void SetSerialize(object me, string file)
        {
            if (File.Exists(file))
                File.Delete(file);
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, me);
            stream.Close();
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="FILENAME">序列化文件(绝对路径)</param>
        /// <returns>反序列化得到的对象</returns>
        public static object Deserialize(string file)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream destream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            object stillme = (object)formatter.Deserialize(destream);
            destream.Close();
            return stillme;
        }

        #endregion

    }
}
