using Chun.Demo.ICommon;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Chun.Demo.PhraseHtml
{
    public class GetDirPath : IGetService
    {
        public void GetService()
        {

            string netPath = "http://w1.vt97.biz/pw/thread.php?fid=16&page=";
            GetHtml gt = new GetHtml();
            //获取目录地址
            string dirXpath = "//tr[@class='tr3 t_one']/td/h3/a";
            //string filepath = @"E:\Test\dirpath.txt";
            gt.Match = dirXpath;
            //读取数据库中已经存在的目录地址,过滤已经添加的
            List<string> dirpathList = Tool.ReadPathByMySQL(1,2);

            gt.dirPath = dirpathList;

            Parallel.For(0, 153, item =>
            {
               string  url = netPath + item;
                gt.URL = url;
                if (gt.run("href", 1))
                {
                   // Tool.UpdatefilePath(url, 1);
                    Console.WriteLine("线程 {0} 已经完成了文件 {1} 的获取！", Thread.CurrentThread.ManagedThreadId, url);
                }
                else
                {
                    Console.WriteLine("线程 {0} 对 {1} 的获取失败了！", Thread.CurrentThread.ManagedThreadId, url);
                }
            });

            // gt.writeTxt(filepath);
        }
    }
}
