using System;
using System.Windows.Forms;

namespace Chun.Demo.Common
{
    public partial class TextBoxHelper
    {
        /// <summary>
        /// 只允许文本框 输入数字
        /// </summary>
        /// <param name="txt"></param>
        public void TextBoxInputOnlyNum(TextBox txt)
        {
            txt.KeyPress += new KeyPressEventHandler(txt_KeyPress);
        }

        public void TextBoxInputOnlyNumWithOutTab(TextBox txt)
        {
            txt.KeyPress += new KeyPressEventHandler(txt_KeyPress1);
        }
        /// <summary>
        /// 只允许输入浮点型数值
        /// 如果输入整数,默认转化为0.00格式
        /// </summary>
        /// <param name="txt"></param>
        public void TextBoxInputOnlyFloatNum(TextBox txt)
        {
            txt.KeyPress += new KeyPressEventHandler(txtFloatNum_KeyPress);
        }

        void txt_KeyPress1(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || e.KeyChar == (char)8 || e.KeyChar == '.')
            {
                (sender as TextBox).Tag = (sender as TextBox).Text;
            }
        }
        void txtFloatNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Trim().Length > 0 && e.KeyChar == (Char)Keys.Enter)
            {
                SendKeys.Send("{TAB}"); return;
            }
            if (!(e.KeyChar == (Char)Keys.Back || (e.KeyChar <= '9' && e.KeyChar >= '0') || e.KeyChar == '.' || e.KeyChar == '-'))
            {
                e.Handled = true;
                return;
            }
        }
        public void TextBoxOnlyNumWithOutEnter(TextBox txt)
        {
            txt.KeyPress += new KeyPressEventHandler(txt_KeyPressOnlyNumWithOutEnter);
        }

        /// <summary>
        /// 文本框 只接受数字 空格 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_KeyPressOnlyNumWithOutEnter(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == (Char)Keys.Back || e.KeyChar == (Char)Keys.Space))
            {
                e.Handled = true;
                return;
            }
        }

        public void TextBoxOnlyNumWithoutSpace(TextBox txt)
        {
            txt.KeyPress += new KeyPressEventHandler(txt_KeyPressOnlyNumWithoutSpace);
        }

        /// <summary>
        /// 文本框 只接受数字 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_KeyPressOnlyNumWithoutSpace(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == (Char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// 只允许文本框 输入数字
        /// </summary>
        /// <param name="txt">文本框</param>
        /// <param name="allowNull">是否允许空</param>
        public void TextBoxInputOnlyNum(TextBox txt, bool allowNull)
        {
            if (allowNull)
                txt.KeyPress += new KeyPressEventHandler(txt_KeyPress_allowNull);
            else
                txt.KeyPress += new KeyPressEventHandler(txt_KeyPress);
        }

        public void TextBoxInput(TextBox txt, bool allNUll)
        {
            if (allNUll)
                txt.KeyPress += new KeyPressEventHandler(txt_KeyPressAny);
            else
                txt.KeyPress += new KeyPressEventHandler(txt_KeyPressAny_allowNull);
        }

        /// <summary>
        /// 文本框 只接受数字 回车 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Trim().Length > 0 && e.KeyChar == (Char)Keys.Enter)
            {
                SendKeys.Send("{TAB}"); return;
            }
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == (Char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// 接受数字和文本  可为空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_KeyPressAny(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                SendKeys.Send("{TAB}"); return;
            }
        }
        /// <summary>
        /// 接受数字和文本   不可为空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_KeyPressAny_allowNull(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Trim().Length > 0 && e.KeyChar == (Char)Keys.Enter)
            {
                SendKeys.Send("{TAB}"); return;
            }
        }
        /// <summary>
        /// 文本框 只接受数字 回车 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_KeyPress_allowNull(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Trim().Length > 0 && e.KeyChar == (Char)Keys.Enter)
            {
                SendKeys.Send("{TAB}"); return;
            }
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == (Char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// 判断文本框是否为空
        /// </summary>
        /// <param name="txt">文本框</param>
        /// <param name="txtName">文本框名称</param>
        /// <returns></returns>
        public bool TextBoxIsNull(TextBox txt, string txtName)
        {
            if (txt.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入" + txtName + "!", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt.Focus();
                return true;
            }
            else
                return false;
        }


        /// <summary>
        /// 只允许文本框 输入字母
        /// </summary>
        /// <param name="txtAbbr"></param>
        public void TextBoxInputOnlyAbbr(TextBox txtAbbr)
        {
            txtAbbr.KeyPress += new KeyPressEventHandler(txtAbbr_KeyPress);
        }

        /// <summary>
        /// 只允许文本框 输入字母
        /// </summary>
        /// <param name="txtAbbr"></param>
        public void TextBoxInputOnlyAbbr(TextBox txtAbbr, bool allowNull)
        {
            if (allowNull)
                txtAbbr.KeyPress += new KeyPressEventHandler(txtAbbr_KeyPress_allowNull);
            else
                txtAbbr.KeyPress += new KeyPressEventHandler(txtAbbr_KeyPress);
        }

        /// <summary>
        /// 文本框 只接受字母 回车 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtAbbr_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Trim().Length > 0 && e.KeyChar == (Char)Keys.Enter)
            {
                SendKeys.Send("{TAB}"); return;
            }
            if (!((e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= 'A' && e.KeyChar <= 'Z') || e.KeyChar == (Char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// 文本框 只接受字母 回车 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtAbbr_KeyPress_allowNull(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                SendKeys.Send("{TAB}"); return;
            }
            if (!((e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= 'A' && e.KeyChar <= 'Z') || e.KeyChar == (Char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// 仅接受整型数据，可以输入负号
        /// </summary>
        /// <param name="tb"></param>
        public void TextBoxInputOnlyInt(TextBox tb)
        {
            tb.KeyPress += new KeyPressEventHandler(txt_KeyPressInt);
        }

        /// <summary>
        /// 仅接受整型数据，可以输入负号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_KeyPressInt(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)sender).Text.Trim().Length > 0 && e.KeyChar == (Char)Keys.Enter)
            {
                SendKeys.Send("{TAB}"); return;
            }
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == (Char)Keys.Back) || e.KeyChar == (Char)Keys.Subtract)
            {
                e.Handled = true;
                return;
            }
        }
    }
}
