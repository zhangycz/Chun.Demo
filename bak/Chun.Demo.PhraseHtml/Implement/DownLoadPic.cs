using Chun.Demo.ICommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Chun.Demo.PhraseHtml
{
    public class DownLoadPic : IGetService
    {
        string picFilePath = @"J:\Picture\";
        public void GetService()
        {
           
            #region 废弃
            //string picPath = @"E:\Test\picpath.txt";
            //string downPath = @"E:\Test\picDone.txt";
            //读取未下载的图片地址
            // List<string> picPathList = Tool.ReadTxt(picPath);

            //读取已经下载完成的地址
            //List<string> DoneList = Tool.ReadTxt(downPath);
            //移除已经下载完成的
            //foreach (string doneItem in DoneList)
            //{
            //    if (picPathList.Contains(doneItem))
            //        picPathList.Remove(doneItem);
            //}
            #endregion
            List<string> picPathList = Tool.ReadPathByMySQL(2, 0);
            
            Parallel.ForEach(picPathList, path =>
            {

                dosomething(path);
            });
        }

        void dosomething(String path)
        {
            if (string.IsNullOrEmpty(path))
                return;
            try
            {
                string[] dirstact = path.Split('/');
                if (dirstact.Length == 0)
                    return;
                string fileName = dirstact[dirstact.Length - 1];

                string createDir = picFilePath
                    + dirstact[dirstact.Length - 3] + @"\"
                    + dirstact[dirstact.Length - 2] + @"\";

                if (!Directory.Exists(createDir))
                {
                    Directory.CreateDirectory(createDir);
                }

                fileName = createDir + fileName;

                Tool.DownLoad(path, fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data + "\n" + e.Message);
                return;
            }
        }
    }
}
