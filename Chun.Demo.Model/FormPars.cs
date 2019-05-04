using System;

namespace Chun.Demo.Model {
    public class FormPars {
        public FormPars() {
        }

        public FormPars(string basePath, string extendPath, string match, string attrName,string savePath, string eCode, string picType) {
            this.Match = match;
            this.BasePath = basePath;
            this.ExtendPath = extendPath;
            this.AttrName = attrName;
            this.ECode = eCode;
            this.SavePath = savePath;
            this.PicType = picType;
        }

        public string AppPath { get; set; } = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        public string Match { get; set; }

        public string BasePath { get; set; }
        public string ExtendPath { get; set; }
        public string ECode { get; set; }
        public string AttrName { get; set; }
        public string SavePath { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IgnoreFailed { get; set; }
        public string PicType { get; set; }
    }
}