// create By 08628 20180411

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chun.Demo.Common
{
   /// <summary>
   /// 路径辅助类
   /// </summary>
   public  class PathTools {

       /// <summary>
       /// 检查是否为True
       /// </summary>
       /// <param name="str"></param>
       /// <returns></returns>
       public static bool IsTrue(string str) {
           return str != null && str.Trim().ToUpper().Equals("TRUE") ;
       }
       /// <summary>
       /// 检查是否为False
       /// </summary>
       /// <param name="str"></param>
       /// <returns></returns>
       public static bool IsFasle(string str) {
           return str != null && str.Trim().ToUpper().Equals("FALSE") ;
       }

        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static string CombineStr(IEnumerable<string> strs) {
            var sb = new StringBuilder();
            strs.ToList().ForEach(str => sb.Append((str ?? string.Empty).Trim()));
            return sb.ToString();
        }

       /// <summary>
       /// 连接路径
       /// </summary>
       /// <param name="strs"></param>
       /// <returns></returns>
       public static string PathCombine(params string[] strs) {
            var path = string.Empty;
            strs.ToList().ForEach(str => {
                    var pathArg = (str ?? string.Empty).Trim();
                    if (path.Equals(string.Empty)) {
                        path = pathArg;
                    }
                    else if (path.EndsWith(@"\")) {
                        path = $@"{path}{pathArg}";
                    }
                    else {
                        path = $@"{path}\{pathArg}";
                    }
                   
                }
            );
            return path;
        } 

    

        /// <summary>
        /// 检查空字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(object value) {
            var f = value == null;
            if(f) return true;
            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (value is string) {
                f = string.Equals(((string) value).Trim(),string.Empty, StringComparison.Ordinal);
            }
            if (value is Array) {
               f = (value as Array).Length==0;

            }
            return f;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="fileName"></param>
       /// <param name="modelType"></param>
       /// <returns></returns>
       public static string GetSettingPath(string fileName,ModelType modelType) {
            var formPars = MyTools.FormPars;
            var path = string.Empty;
            var mvsToolpath = formPars.AppPath;
            switch (modelType) {
                case ModelType.Xml:
                    path = fileName + ".xml";
                    break;
                case ModelType.Json:
                    path = fileName + ".json";
                    break;
                case ModelType.Binary:
                    path = fileName + ".data";
                    break;
            }
            path = PathCombine(mvsToolpath, "Config", path);
            return path;
        }


       /// <summary>
       ///     打开文件夹
       /// </summary>
       /// <param name="targetDir"></param>
       public static void OpenDir(string targetDir)
       {
           try
           {
               if (Directory.Exists(targetDir))
                   Process.Start(targetDir);
               else
                   MessageBox.Show($"文件夹{targetDir}不存在", "错误",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
           }
           catch (Exception ex)
           {
               LogTools.LogInfo($"OpenDir{targetDir} Error! Detail:{ex.Message}");
           }
       }

    }
}