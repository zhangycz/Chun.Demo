/*
* ==============================================================================
* Copyright (c) 2019 All Rights Reserved.
* File name: Models
* Machine name: CHUN
* CLR Version: 4.0.30319.42000
* Author: Ocun
* Version: 1.0
* Created: 2019/4/14 14:08:31
* Description: 
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chun.Demo.ICommon
{
    public enum PhraseHtmlState
    {
        Ready,
        Busy,
        Complete
    }
    public enum PhraseHtmlType
    {
        Root = 10,
        Dir,
        Img
    }
}
