using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainFrom
{
    public class SelectTest
    {
        public List<OpenerWindow> OpenerWindows { get; set; } = new List<OpenerWindow>();

        public void Add(OpenerWindow opener)
        {
            OpenerWindows.Add(opener);
        }
    }

    public class OpenerWindow
    {
        public int QueryId { get; set; }


        public List<string> ReturnExs
        {
            get;
            set; 
        } = new List<string>();
    }
}
