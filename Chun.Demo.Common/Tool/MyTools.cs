using Chun.Demo.Model;

namespace Chun.Demo.Common
{
    public static class MyTools
    {
        public static FormPars FormPars { get; set; }
        /// <summary>
        /// 硬件信息
        /// </summary>
        public static HardwareEntity HardwareInfo { get; } = HardwareTools.GetHardwareInfo();
    }
}