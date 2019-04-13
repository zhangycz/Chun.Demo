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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Chun.Demo.Common.Helper;
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

    /// <summary>
    ///     呼叫者信息
    /// </summary>
    public class CallerInfo
    {
        public string CallerName { get; set; }
        public int CallerId { get; set; }
        public int CalledId { get; set; }
    }

    /// <summary>
    ///     坐席信息
    /// </summary>
    public class AgentInfo
    {
        public string AgentName { get; set; }
        public int AgentId { get; set; }
        public AgentState AgentState { get; set; }

        /// <summary>
        ///     加入的平台信息
        /// </summary>
        public List<PlatformInfo> PlatformListInfo { get; set; } = new List<PlatformInfo>();

        public void JoinPlatform(Platform platform)
        {
            var joined = PlatformListInfo?.FirstOrDefault(pf => pf.Platform.Equals(platform));
            if (joined == null)
                PlatformListInfo?.Add(new PlatformInfo
                {
                    Platform = platform
                });
            else
                LogHelper.Debug($"{AgentName} has joined {platform}");
        }
    }

    /// <summary>
    ///     平台信息
    /// </summary>
    public class PlatformInfo
    {
        public Platform Platform { get; set; }
        public string PlatformName => Platform == Platform.Nj ? "南京" : (Platform == Platform.Bj ? "背景" : "上海");
        public int Weight { get; set; }
    }

    public class TestThread
    {
        private readonly Queue<AgentInfo> _agentInfos1 = new Queue<AgentInfo>();
        private readonly Queue<AgentInfo> _agentInfos2 = new Queue<AgentInfo>();
        private readonly Queue<AgentInfo> _agentInfos3 = new Queue<AgentInfo>();

        private readonly Dictionary<AgentInfo, List<Platform>> _agentPlatformInfos =
            new Dictionary<AgentInfo, List<Platform>>();

        private readonly Queue<CallerInfo> _callerInfos = new Queue<CallerInfo>();

        private readonly Dictionary<Platform, Queue<AgentInfo>> _platformQueue =
            new Dictionary<Platform, Queue<AgentInfo>>();


        public void TestMain()
        {
            Init();
            for (var i=0; i < 5; i++) {
                AsyncHleper.RunAsync(() => GetCaller(110));
                AsyncHleper.RunAsync(() => GetCaller(120));
                AsyncHleper.RunAsync(() => GetCaller(130));
            }
         
            AsyncHleper.RunAsync(DispatchCaller);
        }


        void GetCaller(int calledId ) {
            string[] callers = { "lili", "wangping", "junu", "hoh", "golad", "mimi" };
            var r = new Random(unchecked((int)DateTime.Now.Ticks));
            var call = callers[r.Next(0, 5)];
            AsyncHleper.RunAsync(() => CallerComing(call, calledId));
        }

        private void Init()
        {
            _platformQueue.Add(Platform.Bj, _agentInfos1);
            _platformQueue.Add(Platform.Nj, _agentInfos2);
            _platformQueue.Add(Platform.Sh, _agentInfos3);

            AgentLogin(110, "张扬", Platform.Bj);
            AgentLogin(110, "张扬", Platform.Nj);
            AgentLogin(110, "张扬", Platform.Sh);
            AgentLogin(120, "张三", Platform.Bj);
            AgentLogin(120, "张三", Platform.Nj);
            AgentLogin(120, "张三", Platform.Sh);
            AgentLogin(130, "张四", Platform.Bj);
            AgentLogin(130, "张四", Platform.Nj);
            AgentLogin(130, "张四", Platform.Sh);
        }

        private void JoinPlatform(Platform platform, AgentInfo agent)
        {
            if (!_agentPlatformInfos.ContainsKey(agent))
            {
                _agentPlatformInfos.Add(agent, new List<Platform> { platform });
                agent.JoinPlatform(platform);
                _platformQueue.TryGetValue(platform, out var platfromQueue);
                platfromQueue?.Enqueue(agent);
            }
            else
            {
                _agentPlatformInfos.TryGetValue(agent, out var platforms);
                platforms?.Add(platform);
            }
            LogHelper.Debug($"{agent.AgentName}加入{platform}");
        }

        private void CallerComing(string name, int calledId)
        {
            //var caller
            lock (_callerInfos)
            {
                var caller = new CallerInfo
                {
                    CallerId = 1,
                    CallerName = name,
                    CalledId = calledId
                };
                LogHelper.Debug($"{name} 呼叫 {calledId}");
                _callerInfos.Enqueue(caller);
                Monitor.Pulse(_callerInfos);
            }
         

        }

        private void DispatchCaller()
        {
            while (true)
                lock (_callerInfos)
                {
                    CallerInfo caller = null;
                    if (_callerInfos?.Count > 0)
                    {
                        caller = _callerInfos.Dequeue();
                        if (caller != null)
                            AsyncHleper.RunAsync(() => ConnectAgent(caller));
                    }
                    else
                    {
                        Monitor.Wait(_callerInfos);
                    }
                }
        }

        private void ConnectAgent(CallerInfo caller)
        {
            //LogHelper.Debug(
            //    $"Begin Connect Caller:{caller.CallerName}，Agent:{caller.CalledId}");
            while (true)
            {
                var agentInfo = _agentPlatformInfos.Keys.FirstOrDefault(agent => agent.AgentId ==
                                                                                 caller.CalledId);

                if (agentInfo != null)
                    lock (agentInfo)
                    {
                        var minPlatformInfo = agentInfo.PlatformListInfo.OrderByDescending(item => item.Weight)
                            .FirstOrDefault();

                        if (agentInfo.AgentState != Busy)
                        {
                            LogHelper.Debug(
                                $"Connect Caller:{caller.CallerName}-->Agent:{agentInfo.AgentId}--{agentInfo.AgentName}");
                            agentInfo.AgentState = Busy;

                            Thread.Sleep(1000);

                            LogHelper.Error(
                                $"DisConnect Caller:{caller.CallerName}-->Agent:{agentInfo.AgentId}--{agentInfo.AgentName}");
                            agentInfo.AgentState = Ready;
                            if (minPlatformInfo != null)
                                minPlatformInfo.Weight += 10;
                            // agentInfo..Weight += 10;
                            Monitor.Pulse(agentInfo);
                            break;
                        }
                        Monitor.Wait(agentInfo);
                    }
            }
        }

        //void EndConnect(CallerInfo caller,AgentInfo agentInfo) {
        //    agentInfo.AgentState = Ready;
        //    agentInfo.
        //}


        private void AgentLogin(int agentId, string agentName, Platform platform)
        {
            var findAgent = _agentPlatformInfos.Keys.FirstOrDefault(agent => agent.AgentId == agentId);
            AgentInfo agentInfo;
            if (findAgent == null)
            {
                agentInfo = new AgentInfo
                {
                    AgentId = agentId,
                    AgentName = agentName,
                    AgentState = Ready
                };
            }
            else
            {
                _agentPlatformInfos.TryGetValue(findAgent, out var platforms);
                if (platforms != null && platforms.Any(item => item.Equals(platform)))
                {
                    LogHelper.Debug($"{agentName} has joined {platform}");
                    return;
                }
                agentInfo = findAgent;
            }

            JoinPlatform(platform, agentInfo);
        }
    }
}