/*
* ==============================================================================
* Copyright (c) 2019 All Rights Reserved.
* File name: ThreadHelper
* Machine name: CHUN
* CLR Version: 4.0.30319.42000
* Author: Ocun
* Version: 1.0
* Created: 2019/5/1 15:15:33
* Description: 
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chun.Demo.Common.Helper
{
    public abstract class ThreadHelper
    {
        /// <summary>
        /// 开始一个线程
        /// </summary>
        /// <param name="func"></param>
        /// <param name="listenerThread"></param>
        public static void StartThread(Action func, ref Thread listenerThread) {
            try {
                if (listenerThread != null && listenerThread.IsAlive)
                    LogHelper.Debug(
                        $"the InsertListenerThread {listenerThread.ManagedThreadId} isAlive,ignore start");
                var thread = new Thread(() => func()) {IsBackground = true};
                thread.Start();
                var obj = Interlocked.Exchange(ref listenerThread, thread);

                if (obj != null && obj.IsAlive)
                    obj.Abort();
            }
            catch {
                // ignored
            }
        }

        /// <summary>
        /// 结束一个线程
        /// </summary>
        public static void StopInsertListener(ref Thread listenerThread) {
            try {
                var thread = Interlocked.Exchange(ref listenerThread, null);

                if (thread == null || !thread.IsAlive)
                    return;
                var flag = thread.Join(1000);

                if (flag)
                    return;
                thread.Abort();
            }
            catch (ThreadAbortException ){
                LogHelper.Debug($@"the InsertListenerThread { listenerThread.ManagedThreadId} abort");
                // ignored
            }
            catch (Exception ex)
            {
                LogHelper.Debug("InsertListenerThread error. {0}", ex);
            }
            finally
            {
                LogHelper.Debug("InsertListenerThread  stopped.");
            }
        }
    }
}
