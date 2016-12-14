using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Chun.Demo.myUserControl
{
    public enum CaptionAlignment
    {
        Left,
        Right,
        Top
    }

    class textBoxWithLabel : TextBox
    {
            private int captionSpace = 5;

            private Label label;

            private Size size;

            private IContainer container = null;

            private CaptionAlignment captionAlignment = CaptionAlignment.Left;

            [CompilerGenerated]
            private string xc91f2d47be9f615e;

            [Category("Caption"), DefaultValue(""), Description("设置/返回当前控件的标题内容"), Localizable(true)]
            public string Caption
            {
                get
                {
                    return this.label.Text;
                }
                set
                {
                    this.label.Text = value;
                    this.setLab();
                }
            }

            [Category("Caption"), DefaultValue(5), Description("设置/返回标题到编辑框的距离")]
            public int CaptionSpace
            {
                get
                {
                    return this.captionSpace;
                }
                set
                {
                    this.captionSpace = value;
                    this.setLab();
                }
            }

            [Category("Caption"), DefaultValue(ContentAlignment.MiddleRight), Description("设置/返回当前控件标题的文本对齐方式")]
            public ContentAlignment CaptionTextAlign
            {
                get
                {
                    return this.label.TextAlign;
                }
                set
                {
                    this.label.TextAlign = value;
                }
            }

            [Category("Caption"), DefaultValue(true), Description("设置/返回当前控件标题是否显示")]
            public bool CaptionVisible
            {
                get
                {
                    return this.label.Visible;
                }
                set
                {
                    this.label.Visible = value;
                }
            }

            [Category("Caption"), DefaultValue(true), Description("设置/返回当前控件标题的是否根据内容自动大小")]
            public bool CaptionAutoSize
            {
                get
                {
                    return this.label.AutoSize;
                }
                set
                {
                    this.label.AutoSize = value;
                    this.setLab();
                }
            }

            [Category("Caption"), DefaultValue(CaptionAlignment.Left), Description("设置/返回当前控件标题对应编辑框的位置")]
            public CaptionAlignment CaptionPosition
            {
                get
                {
                    return this.captionAlignment;
                }
                set
                {
                    this.captionAlignment = value;
                    this.setLab();
                }
            }

            public override RightToLeft RightToLeft
            {
                get
                {
                    return base.RightToLeft;
                }
                set
                {
                    base.RightToLeft = value;
                    this.label.RightToLeft = value;
                }
            }

            [Category("Caption"), Description("设置/返回当前控件的提示文本内容")]
            public string ToolTipText
            {
                get;
                set;
            }

            public textBoxWithLabel()
            {
                this.InitCtrl();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing && this.container != null)
                {
                    this.container.Dispose();
                }
                base.Dispose(disposing);
                if (15 != 0)
                {
                }
            }

            private void InitCtrl()
            {
                this.label = new Label();
                base.SuspendLayout();
                this.label.Text = "Label";
                this.label.TextAlign = ContentAlignment.MiddleRight;
                this.label.AutoSize = true;
                if (!false)
                {
                    this.label.Name = "labTitle" + base.Name;
                    base.Move += new EventHandler(this.MoveHandler);
                    base.Resize += new EventHandler(this.ResizeHandler);
                    base.FontChanged += new EventHandler(this.FontChangedHandler);
                    base.ParentChanged += new EventHandler(this.ParentChangedHandler);
                    base.VisibleChanged += new EventHandler(this.VisibleChangedHandler);
                    base.Size = new Size(181, 21);
                    base.ResumeLayout(false);
                    if (8 == 0 || 4 == 0)
                    {
                        return;
                    }
                }
                base.PerformLayout();
            }

            private void VisibleChangedHandler(object sender, EventArgs e)
            {
                if (this.label != null)
                {
                    this.label.Visible = base.Visible;
                }
            }

            private void ParentChangedHandler(object sender, EventArgs e)
            {
                if (base.Parent != null)
                {
                    this.label.Name = "labTitle" + base.Name;
                    bool flag = this.label != null;
                    if (flag)
                    {
                        while (true)
                        {
                            this.label.Parent = base.Parent;
                            bool flag2 = (flag ? 1u : 0u) - (flag ? 1u : 0u) < 0u;
                            if (!flag2)
                            {
                                break;
                            }
                            if (-2147483648 != 0)
                            {
                                goto IL_17;
                            }
                        }
                        if (!false)
                        {
                        }
                    }
                    else
                    {
                        base.Parent.Controls.Add(this.label);
                    }
                    return;
                }
                IL_17:
                this.label.Parent = base.Parent;
            }

            private void FontChangedHandler(object sender, EventArgs e)
            {
                this.label.Font = this.Font;
                this.setLab();
            }

            private void ResizeHandler(object sender, EventArgs e)
            {
                bool flag = this.label == null;
                while (!flag)
                {
                    this.setLab();
                    if (2147483647 != 0)
                    {
                        break;
                    }
                }
            }

            private void setLab()
            {
                bool flag = base.Parent == null;
                while (true)
                {
                    if (!flag)
                    {
                        base.Parent.SuspendLayout();
                        this.setLabAutoSize();
                        goto IL_68;
                    }
                    if (4 == 0)
                    {
                        continue;
                    }
                    base.SuspendLayout();
                    IL_78:
                    this.setLabAutoSize();
                    if (((flag ? 1u : 0u) | 8u) == 0u)
                    {
                        continue;
                    }
                    base.ResumeLayout(false);
                    base.PerformLayout();
                    if (!false)
                    {
                        return;
                    }
                    IL_68:
                    base.Parent.ResumeLayout(false);
                    if (false)
                    {
                        goto IL_78;
                    }
                    break;
                }
                base.Parent.PerformLayout();
            }

            private void setLabAutoSize()
            {
                this.size = base.CreateGraphics().MeasureString(this.label.Text, this.label.Font).ToSize();
                switch (this.captionAlignment)
                {
                    case CaptionAlignment.Left:
                        break;
                    case CaptionAlignment.Right:
                        this.label.Left = base.Right + this.captionSpace;
                        if (-2 == 0)
                        {
                            return;
                        }
                        this.label.Top = base.Top + (base.Height / 2 - this.size.Height / 2);
                        return;
                    case CaptionAlignment.Top:
                        this.label.Left = base.Left;
                        this.label.Top = base.Top - this.size.Height - this.captionSpace;
                        if (!false)
                        {
                            return;
                        }
                        break;
                    default:
                        this.label.Left = base.Left - this.size.Width - this.captionSpace;
                        this.label.Top = base.Top + (base.Height / 2 - this.size.Height / 2);
                        return;
                }
                this.label.Left = base.Left - this.captionSpace - this.size.Width;
                this.label.Top = base.Top + (base.Height / 2 - this.size.Height / 2);
            }

            private void MoveHandler(object sender, EventArgs e)
            {
                bool flag = this.label == null;
                bool flag2 = ((flag ? 1u : 0u) | 4294967295u) == 0u;
                if (flag2 || !flag)
                {
                    this.setLab();
                }
            }
        
}
}
