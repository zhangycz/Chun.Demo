/*
* ==============================================================================
* Copyright (c) 2019 All Rights Reserved.
* File name: AsyncHleper
* Machine name: CHUN
* CLR Version: 4.0.30319.42000
* Author: Ocun
* Version: 1.0
* Created: 2019/4/4 20:52:29
* Description: 
* ==============================================================================
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chun.Demo.Common.Helper
{
    public static class AsyncHleper
    {
        public static async void RunAsync<T>(Action<T> function) {
            await Task.Run(()=>function);
        }
        public static async void RunAsync(Action function) {
            await Task.Run(function);
        }

        public static async Task<T> RunAsync<T>(Func<T> function) {
           return await Task.Run(function);
        }
        public static async void RunAsync<T>(Func<T> function,Action<T> callBack) {
            callBack.Invoke(await Task.Run(function));
        } 
    }
}
