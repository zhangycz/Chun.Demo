using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using System.Data;
/*********************
    CLR 版本:4.0.30319.269
    文 件 名:OperatorInfo
    创建时间:2012/8/6 14:26:11
    命名空间:TTS.ERP.SysInfo
    作    者:SGL
    修 改 人：
    修改日期：
    备注描述:记录系统登录后人员的各种信息
**********************/

namespace Chun.Demo.Common
{
    /// <summary>
    /// 操作员信息
    /// </summary>
    public class OperatorInfo
    {
        private static OperatorInfo operatorInfo = null;

        /// <summary>
        /// 选择的数据库种类名称
        /// </summary>
        public static string DbTypeString ;//= System.Configuration.ConfigurationManager.AppSettings["DbType"].ToString();

        /// <summary>
        /// 所有数据库种类
        /// </summary>
        public static DataBaseType DbType
        {
            get
            {
                DbTypeString = System.Configuration.ConfigurationManager.AppSettings["DbType"].ToString();
                switch (DbTypeString)
                {
                    case "mssql":
                        return DataBaseType.mssql;
                    case "oracle":
                        return DataBaseType.oracle;
                    default:
                        return DataBaseType.mssql;
                }
            }
        }

        /// <summary>
        /// 当前操作员信息
        /// </summary>
        /// <returns></returns>
        public static OperatorInfo GetCurOper()
        {
            if (operatorInfo == null)
                operatorInfo = new OperatorInfo();
            return operatorInfo;
        }

        private OperatorInfo()
        {

        }

        /// <summary>
        /// 当前操作员
        /// </summary>
        //public Operator CurOper { get; set; }

        /// <summary>
        /// 是否注册票段
        /// </summary>
        public bool IsRegTKNum { get; set; }

        /// <summary>
        /// 是否为售票员
        /// </summary>
        public bool IsBookingClerk { get; set; }

        //private BookingClerk _bookingClerk;

        /// <summary>
        /// 售票员
        /// </summary>
        //public BookingClerk BookingClerk 
        //{
        //    get
        //    {
        //        if (IsBookingClerk)
        //        {
        //            return _bookingClerk;
        //        }
        //        else
        //            return null;
        //    }
        //    set
        //    {
        //        _bookingClerk=value;
        //    }
        //}
       
        /// <summary>
        /// 缓存站点
        /// </summary>
        //public List<BusStop> BusStopList { get; set; }

        /// <summary>
        /// 缓存乘车站
        /// </summary>
       // public List<CarryStation> CarryStationList { get; set; }

        /// <summary>
        ///  缓存售票站
        /// </summary>
       // public List<TkStation> TkStationList { get; set; }

        /// <summary>
        /// 是否为行报处理人员
        /// </summary>
        public bool IsLugAcceptClerk { get; set; }

        //private LugAcceptClerk _lugAcceptClerk;

        /// <summary>
        /// 售票员
        /// </summary>
        //public LugAcceptClerk LugAcceptClerk
        //{
        //    get
        //    {
        //        if (IsLugAcceptClerk)
        //        {
        //            return _lugAcceptClerk;
        //        }
        //        else
        //            return null;
        //    }
        //    set
        //    {
        //        _lugAcceptClerk = value;
        //    }
        //}
        /// <summary>
        /// 缓存行包站点
        /// </summary>
       // public List<BusStop> LugBusStopList { get; set; }

        /// <summary>
        /// 缓存行包乘车站
        /// </summary>
        //public List<CarryStation> LugCarryStationList { get; set; }

        /// <summary>
        /// 行包托运站点地址
        /// </summary>
       // public List<LugCheckStopAddress> LugCheckStopAddress { get; set; }

        /// <summary>
        /// 是否注册行包单据
        /// </summary>
        public bool isActiveLugAcceptClerk { get; set; }
        /// <summary>
        /// 是否是行包结算员
        /// </summary>
        public bool IsLugClerkAcctClerk { get; set; }
        /// <summary>
        /// 行包结算单信息
        /// </summary>
        //  public LugClerkAcctTk LugClerkAcctTk { get; set; }
        /// <summary>
        /// 主窗体
        /// </summary>
        public  XtraForm XFMain { get; set; }

        /// <summary>
        /// 退出系统选项，是否重新启动，默认为false （关闭系统）
        /// </summary>
        public bool restartState = false;

//--公共信息 机器、
//--如果是售票员 缓存站点 
//--如果是检票机 获取检口信息 
//--缓存乘车站 售票站
//--根据机器获取 结算单信息 检口信息
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DataBaseType
    {
        /// <summary>
        /// SqlServer数据库
        /// </summary>
        mssql,
        /// <summary>
        /// Oracle数据库
        /// </summary>
        oracle
    }
}
