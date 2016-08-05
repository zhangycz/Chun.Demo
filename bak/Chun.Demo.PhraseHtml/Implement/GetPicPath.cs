using Chun.Demo.ICommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chun.Demo.PhraseHtml
{
    public class GetPicPath : IGetService
    {
        public void GetService()
        {

            GetHtml gt = new GetHtml();
            string picXPath = "//div[@class='tpc_content']/img";
            string basepath = "http://w1.vt97.biz/pw/";
           // string dirpath = @"E:\Test\dirpath.txt";
           // string filepath = @"E:\Test\picpath.txt";
            gt.Match = picXPath;

            // List<string> dirpathList = Tool.ReadTxt(dirpath);
            List<string> dirpathList = Tool.ReadPathByMySQL(1,0);

            //获取数据库中已经有的文件地址，过滤
            List<string> picpathList = Tool.ReadPathByMySQL(2,3);
            gt.dirPath = picpathList;

            if (dirpathList == null)
                return;


            Parallel.ForEach(dirpathList, item =>
            {
                string url = basepath + item;
                gt.URL = url;
                if (gt.run("src", 2))
                {
                    Tool.UpdatefilePath(url, 1,1);
                    Console.WriteLine("线程 {0} 已经完成了文件 {1} 的获取！", Thread.CurrentThread.ManagedThreadId, url);
                }
                else
                {
                    Console.WriteLine("线程 {0} 对 {1} 的获取失败了！", Thread.CurrentThread.ManagedThreadId, url);
                }


            });

            //gt.writeTxt(filepath);
        }
    }
}
