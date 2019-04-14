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
using Chun.Demo.Common.Helper;
using Chun.Demo.Common.Tool;
using Chun.Demo.ICommon;
using Chun.Demo.Model;

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
            var tagartCount = targetPath.Count;
            PhraseHtml.OnStart += (sender, data) => { LogHelper.Debug($"Begin Phrase {data.Uri.PathAndQuery}"); };

            PhraseHtml.OnCompleted += (sender, data) => {
                var msg = $"Complete Phrase {data.Uri.PathAndQuery},take time {data.Milliseconds}";
                EventHandler(data.OrignUrl, msg, 1, tagartCount);
            };

            PhraseHtml.OnError += (sender, data) => {
                var msg = $"The Error Happened form {data.Uri.PathAndQuery},Exception {data.Exception}";
                EventHandler(data.OrignUrl, msg, 2, tagartCount);
            };
            PhraseHtml.OnCheckTaskCompleted += (sender, e) => {
                if (!_current.Equals(tagartCount))
                    return;
                var data = sender as Queue<string>;
                if (data?.Count != 0)
                    return;
                PhraseHtml.StopInsertListener();
                OnCompleted?.Invoke();
            };
            PhraseHtml.Start();
        }

        private void EventHandler(string url, string msg, int type, int tagartCount) {
            Tool.UpdatefilePath(url, Convert.ToInt32(PhraseHtml.PhraseHtmlType) - 1, type);
            LogHelper.Debug(msg);

            _current++;
            if (!_current.Equals(tagartCount))
                return;
            PhraseHtml.StopInsertListener();
            OnCompleted?.Invoke();
        }
    }
}