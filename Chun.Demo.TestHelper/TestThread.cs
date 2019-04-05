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

namespace Chun.Demo.TestHelper {
    public enum AgentState {
        Busy,
        Ready
    }

    public enum Platform {
        Nj,
        Bj,
        Sh
    }

    public class CallerInfo {
        public string AgentName { get; set; }
        public int CallerId { get; set; }
        public int CalledId { get; set; }
    }
    public class AgentInfo {
        public string AgentName { get; set; }
        public int AgentId { get; set; }
        public AgentState AgentState { get; set; }
    }

    public class AgentLogInfo {
        public AgentInfo AgentInfo { get; set; }
        public int Weight { get; set; }
        //public Platform Platform { get; set; }
    }


    public class TestThread {
        Dictionary<Platform , List<AgentLogInfo>> _platformDictionary = new Dictionary<Platform, List<AgentLogInfo>>();
        private readonly List<AgentLogInfo> _agentInfos1 = new List<AgentLogInfo>();
        private readonly List<AgentLogInfo> _agentInfos2 = new List<AgentLogInfo>();
        private readonly List<AgentLogInfo> _agentInfos3 = new List<AgentLogInfo>();
        private readonly List<CallerInfo> _callerInfos = new List<CallerInfo>();

        public void TestMain() {
            Init();
            AsyncHleper.RunAsync(CallerComing);
        }

        private void Init() {
            _platformDictionary.Add(Platform.Bj, _agentInfos1);
            _platformDictionary.Add(Platform.Nj, _agentInfos2);
            _platformDictionary.Add(Platform.Sh, _agentInfos3);
            AgentLogin(110, "张扬", 10);
            AgentLogin(120, "张三", 10);
            AgentLogin(130, "张四", 10);
        }

        void CallerComing() {
            var telNum = 0;
            while (telNum<100) {
                lock (_callerInfos) {
                    
                }
            }
        }

        void AgentLogin(int agentId, string agentName,int weight) {
            var agentInfo = new AgentInfo
            {
                AgentId = agentId,
                AgentName = agentName,
                AgentState = AgentState.Ready
            };
            _agentInfos1.Add(new AgentLogInfo
            {
                AgentInfo = agentInfo,
                Weight = weight
            });
            _agentInfos2.Add(new AgentLogInfo
            {
                AgentInfo = agentInfo,
                Weight = weight
            });
            _agentInfos3.Add(new AgentLogInfo
            {
                AgentInfo = agentInfo,
                Weight = weight
            });

       
        }

    }
}