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
using System.Text;
using System.Threading.Tasks;

namespace Chun.Demo.TestHelper
{
    public enum AgentState
    {
        Busy,
        Ready
    }

    public enum PlatForm {
        Nj,Bj,Sh
    }
    public class AgentInfo
    {
        public string AgentName { get; set; }
        public int AgentId { get; set; }
        public AgentState AgentState { get; set; }
    }

    public class AgentLogInfo {
        public AgentInfo AgentInfo { get; set; }
        public int Weight { get; set; }
    }
    public class TestThread
    {
        readonly List<AgentLogInfo> _agentInfos1 = new List<AgentLogInfo>();
        readonly List<AgentLogInfo> _agentInfos2 = new List<AgentLogInfo>();
        readonly List<AgentLogInfo> _agentInfos3 = new List<AgentLogInfo>();

        void InitData() {
            var agentId = 0;
            var agentName = "";
            var agentInfo = new AgentInfo() {
                AgentId = agentId,
                AgentName = agentName,
                AgentState = AgentState.Ready
            };
            var weight = 0;
            _agentInfos1.Add(new AgentLogInfo() {
                AgentInfo = agentInfo,
                Weight = weight
            });
        }

    }
}
