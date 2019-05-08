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
using System.Runtime.Remoting.Channels;
using Chun.Demo.Common.Tool;
using Chun.Demo.ICommon;
using Chun.Demo.Model;
using Chun.Demo.Model.Entity;
using Chun.Demo.PhraseHtml.Interface;
using Chun.Work.Common.Helper;

namespace Chun.Demo.PhraseHtml
{

    public class PhraseHtmlTool
    {
        public PhraseHtmlTool() {
            _completedAction =  new CompletedAction();
            _completedAction.Action += ()=>PhraseHtml.StopInsertListener();
        }

        private readonly CompletedAction _completedAction;
      
        public Action OnCompleted;
        private Helper.PhraseHtml PhraseHtml { get; set; }

        public void StartPhraseHtml(PhraseHtmlType phraseHtmlType, SiteInfo siteInfo, List<filepath> filterPath,
            List<filepath> targetPath) {
            PhraseHtml = new Helper.PhraseHtml {
                SiteInfo = siteInfo,
                PhraseHtmlType = phraseHtmlType,
                FilterPath = filterPath,
                TargetPath = targetPath
            };
            var targetCount = targetPath.Count;
            _completedAction.TargetCount = targetCount;
            PhraseHtml.OnStart += (sender, data) => {
                LogHelper.Debug($"Begin Phrase {data.Uri.PathAndQuery}");
            };
            
            PhraseHtml.OnPhraseUrlCompleted += (sender, data) => {
                LogHelper.Debug( $"Complete Phrase {data.Uri.PathAndQuery},take time {data.Milliseconds}");
                _completedAction.EventHandler();
            };

            PhraseHtml.OnError += (sender, data) => {
                LogHelper.Debug($"The Error Happened form {data.Uri.PathAndQuery},Exception {data.Exception}");
                //Tool.UpdateFilePath(data.OrignUrl,??, Convert.ToInt32(PhraseHtml.PhraseHtmlType) - 1, 2);
                Tool.UpdateFilePath((int)sender, 2);
                _completedAction.EventHandler();
            };
            PhraseHtml.OnCheckTaskCompleted += (sender, e) => {
                if (!_completedAction.Current.Equals(targetCount))
                    return;
                PhraseHtml.StopInsertListener();
            };
            PhraseHtml.OnCompleted += (sender, e) => {
                LogHelper.Debug("Completed All PhraseHtml");
                OnCompleted?.Invoke();
            };
            PhraseHtml.Start();
        }
    }

    public class CompletedAction
    {
        /// <summary>
        /// 当前访问数
        /// </summary>
        private int _current;
        /// <summary>
        /// 总数量
        /// </summary>
        public int TargetCount;
        public CompletedAction() {
            _current = 0;
        }
        public int Current => _current;
        private readonly object _locker = new object();
        public Action Action;
        public void EventHandler()
        {
            lock (_locker)
            {
                if(_current< TargetCount)
                _current++;
            }
            if (!_current.Equals(TargetCount))
                return;
            Action?.Invoke();
        }
    }
}