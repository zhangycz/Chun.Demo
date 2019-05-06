/*
* ==============================================================================
* Copyright (c) 2019 All Rights Reserved.
* File name: TextBoxEx
* Machine name: CHUN
* CLR Version: 4.0.30319.42000
* Author: Ocun
* Version: 1.0
* Created: 2019/5/4 20:07:38
* Description: 
* ==============================================================================
*/

using System.Drawing;
using System.Windows.Forms;

namespace Chun.Demo.VIEW
{
    public class RichTextBoxEx : RichTextBox
    {
        public string PlaceHolderStr { get; set; }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
            if (m.Msg == 0xF || m.Msg == 0x133)
                WmPaint(ref m);
        }

        private void WmPaint(ref Message m) {
            var g = Graphics.FromHwnd(Handle);
            if (!string.IsNullOrEmpty(PlaceHolderStr) && string.IsNullOrEmpty(Text))
                g.DrawString(PlaceHolderStr, Font, new SolidBrush(Color.LightGray), 0.5f, 0.5f);
        }
    }
}