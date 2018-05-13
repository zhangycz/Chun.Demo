/*-----------------------------
 *      create by 08628
 *      create date 20180426
 *----------------------------- 
 */

using System.Collections.Generic;

namespace Chun.Demo.Model
{
    /// <summary>
    /// 硬件信息
    /// </summary>
    public class HardwareEntity
    {
        /// <summary>
        /// CpuInfo
        /// </summary>
        public List<CpuInfo> CpuInfos { get; set; }
        /// <summary>
        /// MainBoardInfo
        /// </summary>
        public List<MainBoardInfo> MainBoardInfos { get; set; }
        /// <summary>
        /// DiskDriveInfo
        /// </summary>
        public List<DiskDriveInfo> DiskDriveInfos { get; set; }
         /// <summary>
         /// NetworkInfo
         /// </summary>
        public List<NetworkInfo> NetworkInfos { get; set; }
        /// <summary>
        /// OsInfo
        /// </summary>
        public List<OsInfo> OsInfo { get; set; }
        /// <summary>
        /// MemoryInfo
        /// </summary>
        public List<MemoryInfo> MemoryInfo { get; set; }
    }

    /// <summary>
    /// CPU信息
    /// </summary>
    public class CpuInfo {
        /// <summary>
        /// 编号
        /// </summary>
        public string ProcessorId { get; set; }
        /// <summary>
        /// CPU型号
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// CPU状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 主机名称
        /// </summary>
        public string SystemName { get; set; }
        /// <summary>
        /// 厂商
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// 最大速率
        /// </summary>
        public string MaxClockSpeed { get; set; }
    }

    /// <summary>
    /// 主板信息
    /// </summary>
    public class MainBoardInfo {
        /// <summary>
        /// 主板ID
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string Product { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

    }

    /// <summary>
    /// 硬盘信息
    /// </summary>
    public class DiskDriveInfo {
        /// <summary>
        /// 硬盘SN
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 可用空间
        /// </summary>
        public string FreeSpace { get; set; }
        /// <summary>
        /// 已用空间
        /// </summary>
        public string UsedSpace { get; set; }

    }

    /// <summary>
    /// 网络信息
    /// </summary>
    public class NetworkInfo
    {
        /// <summary>
        /// MAC地址
        /// </summary>
        public string MacAddress { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 主机名
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Caption { get; set; }
        /// <summary>
        /// 子网掩码
        /// </summary>
        public string IPSubnet { get; set; }
        /// <summary>
        /// 网关
        /// </summary>
        public string DefaultIPGateway { get; set; }
        /// <summary>
        /// DNS
        /// </summary>
        public string DNSServerSearchOrder { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 操作系统信息
    /// </summary>
    public class OsInfo
    {
        /// <summary>
        /// 操作系统
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 系统目录
        /// </summary>
        public string SystemDirectory { get; set; }
    }
    /// <summary>
    /// 内存信息
    /// </summary>
    public class MemoryInfo
    {
        /// <summary>
        /// 总大小
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 可用
        /// </summary>
        public string AvailableSize { get; set; }

        /// <summary>
        /// 生产商
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 内存类型
        /// </summary>
        public string MemoryType { get; set; }
        /// <summary>
        /// 内存类型
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 速率
        /// </summary>
        public string Speed { get; set; }
        /// <summary>
        /// 版本信息
        /// </summary>
        public string Version { get; set; }
    }

}
