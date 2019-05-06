/*
* ==============================================================================
* Copyright (c) 2019 All Rights Reserved.
* File name: PhraseHtmlTool
* Machine name: CHUN
* CLR Version: 4.0.30319.42000
* Author: Ocun
* Version: 1.0
* Created: 2019/4/14 12:53:37
* Description: 
* ==============================================================================
*/

using System;
using System.Collections.Generic;
using Chun.Demo.Common.Tool;
using Chun.Demo.ICommon;
using Chun.Demo.Model;
using Chun.Work.Common.Helper;

namespace Chun.Demo.PhraseHtml
{
    public class PhraseHtmlTool
    {
        private int _current;
        public Action OnCompleted;
        private Helper.PhraseHtml PhraseHtml { get; set; }

        public void StartPhraseHtml(PhraseHtmlType phraseHtmlType, FormPars formPars, List<string> filterPath,
            List<string> targetPath) {
            PhraseHtml = new Helper.PhraseHtml {
                MatchNode = formPars.Match,
                AttrName = formPars.AttrName,
                PhraseHtmlType = phraseHtmlType,
                FilterPath = filterPath,
                TargetPath = targetPath
            };
            var targetCount = targetPath.Count;
            PhraseHtml.OnStart += (sender, data) => { LogHelper.Debug($"Begin Phrase {data.Uri.PathAndQuery}"); };
            
            PhraseHtml.OnPhraseUrlCompleted += (sender, data) => {
                var msg = $"Complete Phrase {data.Uri.PathAndQuery},take time {data.Milliseconds}";
                EventHandler(msg, targetCount);
            };

            PhraseHtml.OnError += (sender, data) => {
                var msg = $"The Error Happened form {data.Uri.PathAndQuery},Exception {data.Exception}";
                Tool.UpdatefilePath(data.OrignUrl, Convert.ToInt32(PhraseHtml.PhraseHtmlType) - 1, 2);
                EventHandler(msg, targetCount);
            };
            PhraseHtml.OnCheckTaskCompleted += (sender, e) => {
                if (!_current.Equals(targetCount))
                    return;
                PhraseHtml.StopInsertListener();
                OnCompleted?.Invoke();
            };
            PhraseHtml.OnCompleted += (sender, e) => {
                LogHelper.Debug("Complete Phrase");
                PhraseHtml.StopInsertListener();
                OnCompleted?.Invoke();
            };
            PhraseHtml.Start();
        }

        private void EventHandler(string msg,  int targetCount) {
          
            LogHelper.Debug(msg);

            _current++;
            if (!_current.Equals(targetCount))
                return;
            PhraseHtml.StopInsertListener();
            OnCompleted?.Invoke();
        }
    }
}