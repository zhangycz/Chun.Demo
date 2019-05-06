/*
* ==============================================================================
* Copyright (c) 2019 All Rights Reserved.
* File name: ControlTool
* Machine name: CHUN
* CLR Version: 4.0.30319.42000
* Author: Ocun
* Version: 1.0
* Created: 2019/5/4 19:15:12
* Description: 
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chun.Work.Common.Helper;

namespace Chun.Demo.Common.Tool
{
    public abstract class ControlTool
    {
        /// <summary>
        /// 主窗体
        /// </summary>
        public static Form SpiderForm { get; set; }

        public static Form LogForm { get; set; }
        private static SplitContainer _splitContainer { get; set; }

        public static SplitContainer SplitContainer =>
            _splitContainer ??
            (_splitContainer =
                ControlHelper.FindControlByName<SplitContainer>(SpiderForm, "SplitContainer"));

        public void BtnAction<T>(object sender, T e) where T : EventArgs {
           
        }

        public static void SubMaxBtn(object sender, EventArgs e) {
            SplitContainer.Panel2Collapsed = true;
            if ( LogForm.IsDisposed)
            {
                LogForm.Closed += (o, args) => {
                    LogHelper.ChangeTargetControl(SpiderForm, "txtLogger");
                    SplitContainer.Panel2Collapsed = false;
                };
                LogHelper.ChangeTargetControl(LogForm, "LogBox");
                LogForm.Show();
            }
            else
            {
                LogForm.WindowState = FormWindowState.Normal;
                LogForm.Activate();
            }
        }
    }
}
