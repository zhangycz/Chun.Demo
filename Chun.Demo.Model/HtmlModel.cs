using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chun.Demo.Model
{
    public class HtmlModel
    {
        public string Match
        {
            get;
            set;
        }
        public String BasePath { get; set; }
        public string ExtendPath { get; set; }
        public string ECode { get; set; }
        public string AttrName { get; set; }

        public HtmlModel() { }
        public HtmlModel(string BasePath, string ExtendPath, string Match, string AttrName, string ECode)
        {
            this.Match = Match;
            this.BasePath = BasePath;
            this.ExtendPath = ExtendPath;
            this.AttrName = AttrName;
            this.ECode = ECode;
        }
    }
}
