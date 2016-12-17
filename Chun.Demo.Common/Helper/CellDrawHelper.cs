using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid.Drawing;
namespace Chun.Demo.Common
{
  public static class CellDrawHelper {
      
        public static void DrawCellBorder(DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e) {
            Brush brush = Brushes.YellowGreen;
            
            Pen pen = new Pen(Color.Black,3);
                            e.Graphics.DrawRectangle(pen, new Rectangle(e.Bounds.X - 1, e.Bounds.Y - 1, e.Bounds.Width+ 2, 2));

                            e.Graphics.DrawRectangle(pen,new Rectangle(e.Bounds.X - 1, e.Bounds.Bottom - 1,e.Bounds.Width + 2, 2));

                         //   e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.X - 1, e.Bounds.Y - 1, e.Bounds.Width+ 2, 2));                           
                             //e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.Right - 1, e.Bounds.Y - 1, 2,
                             //      e.Bounds.Height + 2));
                                              
                          //  e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.X - 1, e.Bounds.Bottom - 1,e.Bounds.Width + 2, 2));

                         
                                //e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.X - 1, e.Bounds.Y - 1, 2,
                                //             e.Bounds.Height + 2));
                         
                        }

        public static void DoDefaultDrawCell(GridView view, RowCellCustomDrawEventArgs e) {
            PropertyInfo pi;
            GridControl grid;
            GridViewInfo info;
            GridCellInfo cell;
            GridEditorContainerHelper helper;
            GridViewDrawArgs args;

            info = view.GetViewInfo() as GridViewInfo;
            cell = e.Cell as GridCellInfo;
            grid = view.GridControl;
            pi = grid.GetType().GetProperty("EditorHelper", BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.DeclaredOnly);
            helper = pi.GetValue(grid, null) as GridEditorContainerHelper;
            args = new GridViewDrawArgs(e.Cache, info, e.Bounds);
            e.Appearance.FillRectangle(e.Cache, e.Bounds);
            helper.DrawCellEdit(args, cell.Editor, cell.ViewInfo, e.Appearance,
            cell.CellValueRect.Location);

        }
    }

}
