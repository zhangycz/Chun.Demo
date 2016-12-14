using System;
using System.Configuration;
using System.Windows.Forms;
using System.Xml;

namespace Chun.Demo.Common
{
    public  static  class ConfigerHelper
    {
        ///<summary>
        ///依据连接串名字connectionName返回数据连接字符串
        ///</summary>
        ///<param name="connectionName"></param>
        ///<returns></returns>
        public static string GetConnectionStringsConfig(string connectionName)
        {
            string connectionString =
                    ConfigurationManager.ConnectionStrings[connectionName].ConnectionString.ToString();
            Console.WriteLine(connectionString);
            return connectionString;
        }

        ///<summary>
        ///更新连接字符串
        ///</summary>
        ///<param name="newName">连接字符串名称</param>
        ///<param name="newConString">连接字符串内容</param>
        ///<param name="newProviderName">数据提供程序名称</param>
        public static void UpdateConnectionStringsConfig(string newName,
            string newConString,
            string newProviderName)
        {
            bool isModified = ConfigurationManager.ConnectionStrings[newName] != null;
            //新建一个连接字符串实例
            ConnectionStringSettings mySettings =
                new ConnectionStringSettings(newName, newConString, newProviderName);
            // 打开可执行的配置文件*.exe.config
            Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // 如果连接串已存在，首先删除它
            if (isModified)
            {
                config.ConnectionStrings.ConnectionStrings.Remove(newName);
            }
            // 将新的连接串添加到配置文件中.
            config.ConnectionStrings.ConnectionStrings.Add(mySettings);
            // 保存对配置文件所作的更改
            config.Save(ConfigurationSaveMode.Modified);
            // 强制重新载入配置文件的ConnectionStrings配置节
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }

        ///<summary>
        ///返回＊.exe.config文件中appSettings配置节的value项
        ///</summary>
        ///<param name="strKey"></param>
        ///<returns></returns>
        public static string GetAppConfig(string strKey)
        {
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == strKey)
                {
                    return ConfigurationManager.AppSettings[strKey];
                }
            }
            return null;
        }

        ///<summary>
        ///在＊.exe.config文件中appSettings配置节增加一对键、值对
        ///</summary>
        ///<param name="newKey"></param>
        ///<param name="newValue"></param>
        public static void UpdateAppConfig(string newKey, string newValue)
        {
            bool isModified = false;
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == newKey)
                {
                    isModified = true;
                }
            }

            // Open App.Config of executable
            Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // You need to remove the old settings object before you can replace it
            if (isModified)
            {
                config.AppSettings.Settings.Remove(newKey);
            }
            // Add an Application Setting.
            config.AppSettings.Settings.Add(newKey, newValue);
            // Save the changes in App.config file.
            config.Save(ConfigurationSaveMode.Modified);
            // Force a reload of a changed section.
            ConfigurationManager.RefreshSection("appSettings");
        }

        private static string LOCAL_PATH = Application.ExecutablePath + ".config";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="port"></param>
        public static void WriteAppSettingKey(string key, string value)
        {
            try
            {
                XmlDocument myXmlDocument = new XmlDocument();
                myXmlDocument.Load(LOCAL_PATH);

                // search the appSetting Node
                foreach (XmlNode myNode in myXmlDocument["configuration"]["appSettings"])
                {
                    if (myNode.Name == "add")
                    {
                        // rewrite the Web.Config file
                        if (myNode.Attributes.GetNamedItem("key").Value == key)
                        {
                            myNode.Attributes.GetNamedItem("value").Value = value;
                        }
                    }
                }
                myXmlDocument.Save(LOCAL_PATH);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "系统异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 读取AppSetting信息
        /// </summary>
        /// <param name="protocol"></param>
        /// <returns></returns>
        public static string LoadAppSetting(string key)
        {
            try
            {
                XmlDocument myXmlDocument = new XmlDocument();
                myXmlDocument.Load(LOCAL_PATH);

                // search the appSetting Node
                foreach (XmlNode myNode in myXmlDocument["configuration"]["appSettings"])
                {
                    if (myNode.Name == "add")
                    {
                        // rewrite the Web.Config file
                        if (myNode.Attributes.GetNamedItem("key").Value == key)
                        {
                            return myNode.Attributes.GetNamedItem("value").Value;
                        }

                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "系统异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }
    }
}
