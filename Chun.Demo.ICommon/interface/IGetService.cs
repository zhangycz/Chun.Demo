using System;

namespace Chun.Demo.ICommon
{
    public interface IGetService
    {
        //本地保存地址
        string SaveFilePath
        {
            get; set;
        }
        //        //网络地址
        //        String NetPath
        //        {
        //            get; set;
        //        }
        //匹配选项
        //         String FileXpath
        //        {
        //            get; set;
        //        }
        //文件基址
        //        String BasePath
        //        {
        //            get; set;
        //        }
        //获取属性值
        //        String PropertyName
        //        {
        //            get;set;
        //        }

        void GetService(int fileTypeId);
    }
}
