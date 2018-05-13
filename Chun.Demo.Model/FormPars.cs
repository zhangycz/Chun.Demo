using System;

namespace Chun.Demo.Model {
    public class FormPars {
        public FormPars() {
        }

        public FormPars(string BasePath, string ExtendPath, string Match, string AttrName,string SavePath, string ECode) {
            this.Match = Match;
            this.BasePath = BasePath;
            this.ExtendPath = ExtendPath;
            this.AttrName = AttrName;
            this.ECode = ECode;
            this.SavePath = SavePath;
        }

        public string Match { get; set; }

        public string BasePath { get; set; }
        public string ExtendPath { get; set; }
        public string ECode { get; set; }
        public string AttrName { get; set; }
        public string SavePath { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IgnoreFailed { get; set; }
    }
}