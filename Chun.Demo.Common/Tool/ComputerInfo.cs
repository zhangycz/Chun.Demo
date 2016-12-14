using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
/*********************
    CLR 版本:4.0.30319.269
    文 件 名:ComputerInfo
    创建时间:2012/8/8 9:37:41
    命名空间:TTS.ERP.SysInfo
    作    者:SGL
    修 改 人：
    修改日期：
    备注描述:登陆机器信息
**********************/

namespace Chun.Demo.Common
{
    /// <summary>
    /// 登陆机器信息
    /// </summary>
    public class ComputerInfo
    {
        private ComputerInfo()
        {
            //IsCheckTkPC = true;
        }

        private static ComputerInfo computerInfo;

        public static ComputerInfo GetCurComputer()
        {
            if (computerInfo == null)
            {
                computerInfo = new ComputerInfo();
            }
            return computerInfo;
        }

        /// <summary>
        /// PC名
        /// </summary>
        public static string ComputerName
        {
            get
            {
                return Dns.GetHostName();
            }
        }

        /// <summary>
        /// 机器Ip
        /// </summary>
        public static string ComputerIp
        {
            get
            {
                return Dns.GetHostAddresses(Dns.GetHostName())[0].ToString();
            }
        }

        /// <summary>
        /// 是否为检票机
        /// </summary>
        //public bool IsCheckTkPC { get; set; }

        ///// <summary>
        ///// 检票机信息
        ///// </summary>
        //public ChkTkComputer ChkTkComputer { get; set; }

        //--根据机器获取 结算单信息 检口信息
    }
}
