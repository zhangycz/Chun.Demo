/*
* ==============================================================================
* Copyright (c) 2019 All Rights Reserved.
* File name: Class1
* Machine name: CHUN
* CLR Version: 4.0.30319.42000
* Author: Ocun
* Version: 1.0
* Created: 2019/4/4 20:12:48
* Description: 
* ==============================================================================
*/


using Chun.Demo.Common.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using static Chun.Demo.TestHelper.AgentState;

namespace Chun.Demo.TestHelper
{
    public enum AgentState
    {
        Busy,
        Ready
    }

    public enum Platform
    {
        Nj,
        Bj,
        Sh
    }

    public class CallerInfo
    {
        public string CallerName { get; set; }
        public int CallerId { get; set; }
        public int CalledId { get; set; }
    }
    public class AgentInfo
    {
        public string AgentName { get; set; }
        public int AgentId { get; set; }
        public AgentState AgentState { get; set; }
    }

    public class AgentLogInfo
    {
        public AgentInfo AgentInfo { get; set; }
        public int Weight { get; set; }
        //public Platform Platform { get; set; }
    }


    public class TestThread
    {
        Dictionary<Platform, Queue<AgentLogInfo>> _platformDictionary = new Dictionary<Platform, Queue<AgentLogInfo>>();
        private readonly Queue<AgentLogInfo> _agentInfos1 = new Queue<AgentLogInfo>();
        private readonly Queue<AgentLogInfo> _agentInfos2 = new Queue<AgentLogInfo>();
        private readonly Queue<AgentLogInfo> _agentInfos3 = new Queue<AgentLogInfo>();
        private readonly Queue<CallerInfo> _callerInfos = new Queue<CallerInfo>();

        public void TestMain()
        {
            Init();
            //var findItem = _agentInfos1.FirstOrDefault(agent => agent.AgentInfo.AgentId ==
            //                                                    110 && agent.AgentInfo.AgentState != Busy);
            //var secondItem = _agentInfos1.FirstOrDefault(agent => agent.AgentInfo.AgentId ==
            //                                                    110 );
            //if (findItem != null) {
            //    findItem.Weight += 10;
            //    var bo = findItem.Equals(secondItem);

            //}

            //return;

            AsyncHleper.RunAsync(CallerComing);
            AsyncHleper.RunAsync(DispatchCaller);
        }


        private void Init()
        {
            _platformDictionary.Add(Platform.Bj, _agentInfos1);
            _platformDictionary.Add(Platform.Nj, _agentInfos2);
            _platformDictionary.Add(Platform.Sh, _agentInfos3);
            AgentLogin(110, "张扬", 10);
            AgentLogin(120, "张三", 10);
            AgentLogin(130, "张四", 10);
        }

        void CallerComing()
        {
            var telNum = 0;
            //var caller
            while (telNum++ < 10)
            {
                lock (_callerInfos)
                {
                    var caller = new CallerInfo()
                    {
                        CallerId = telNum,
                        CallerName = $"李丽{telNum}",
                        CalledId = 110
                    };
                    _callerInfos.Enqueue(caller);
                    Monitor.Pulse(_callerInfos);
                }
            }
        }

        void DispatchCaller()
        {
            while (true)
            {
                lock (_callerInfos)
                {
                    CallerInfo caller = null;
                    if (_callerInfos?.Count > 0)
                    {
                        caller = _callerInfos.Dequeue();
                        if (caller != null)
                        {
                            AsyncHleper.RunAsync(() => ConnectAgent(caller));
                        }
                    }
                    else
                    {
                        Monitor.Wait(_callerInfos);
                    }
                }
            }
        }

        void ConnectAgent(CallerInfo caller)
        {
            LogHelper.Debug(
                $"Begin Connect Caller{caller.CallerName}");
            while (true)
            {
                var findItem = _agentInfos1.FirstOrDefault(agent => agent.AgentInfo.AgentId ==
                     caller.CalledId);
                if (findItem != null)
                {
                    if (findItem.AgentInfo.AgentState != Busy)
                    {
                        lock (findItem)
                        {
                            LogHelper.Debug(
                                $"Connect Caller{caller.CallerName}-->Agent{findItem.AgentInfo.AgentId}--{findItem.AgentInfo.AgentName}");
                            findItem.AgentInfo.AgentState = Busy;

                            Thread.Sleep(1000);
                            LogHelper.Debug(
                                $"DisConnect Caller{caller.CallerName}-->Agent{findItem.AgentInfo.AgentId}--{findItem.AgentInfo.AgentName}");
                            findItem.AgentInfo.AgentState = Ready;
                            findItem.Weight += 10;
                            Monitor.Pulse(findItem);
                            break;
                        }
                    }
                    else
                    {
                        Monitor.Wait(findItem);
                    }

                }


            }
        }

        //void EndConnect(CallerInfo caller,AgentInfo agentInfo) {
        //    agentInfo.AgentState = Ready;
        //    agentInfo.
        //}


        void AgentLogin(int agentId, string agentName, int weight)
        {
            var agentInfo = new AgentInfo
            {
                AgentId = agentId,
                AgentName = agentName,
                AgentState = Ready
            };
            _agentInfos1.Enqueue(new AgentLogInfo
            {
                AgentInfo = agentInfo,
                Weight = weight
            });
            _agentInfos2.Enqueue(new AgentLogInfo
            {
                AgentInfo = agentInfo,
                Weight = weight
            });
            _agentInfos3.Enqueue(new AgentLogInfo
            {
                AgentInfo = agentInfo,
                Weight = weight
            });


        }

    }
}