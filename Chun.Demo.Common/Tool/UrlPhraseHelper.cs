/*
* ==============================================================================
* Copyright (c) 2019 All Rights Reserved.
* File name: UrlTool
* Machine name: CHUN
* CLR Version: 4.0.30319.42000
* Author: Ocun
* Version: 1.0
* Created: 2019/5/4 20:53:16
* Description: 
* ==============================================================================
*/

using System;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Chun.Work.Common.Helper;

namespace Chun.Demo.Common.Tool
{
    public abstract class UrlPhraseHelper
    {
        /// <summary>
        ///     测试.
        /// </summary>
        public static NameValueCollection Phrase(string url) {
            //或者动态读取前一次的URL
            //string parmStr = Request.UrlReferrer.Query;
            //NameValueCollection col = GetQueryString(parmStr);
            try {
                var uri = new Uri(url);
                var queryString = uri.Query;
                return GetQueryString(queryString);
            }
            catch (Exception e) {
                LogHelper.Error(e);
                return null;
            }

            // var searchKey = col["q"];
            //结果 searchKey = "机械数控"
        }

        /// <summary>
        ///     将查询字符串解析转换为名值集合.
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="encoding"></param>
        /// <param name="isEncoded"></param>
        /// <returns></returns>
        public static NameValueCollection GetQueryString(string queryString, Encoding encoding = null,
            bool isEncoded = true) {
            queryString = queryString.Replace("?", "");
            var result = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(queryString)) {
                var count = queryString.Length;
                for (var i = 0; i < count; i++) {
                    var startIndex = i;
                    var index = -1;
                    while (i < count) {
                        var item = queryString[i];
                        if (item == '=') {
                            if (index < 0)
                                index = i;
                        }
                        else if (item == '&') {
                            break;
                        }

                        i++;
                    }

                    string key = null;
                    string value = null;
                    if (index >= 0) {
                        key = queryString.Substring(startIndex, index - startIndex);
                        value = queryString.Substring(index + 1, i - index - 1);
                    }
                    else {
                        key = queryString.Substring(startIndex, i - startIndex);
                    }

                    if (isEncoded)
                        result[MyUrlDeCode(key, encoding)] = MyUrlDeCode(value, encoding);
                    else
                        result[key] = value;
                    if (i == count - 1 && queryString[i] == '&')
                        result[key] = string.Empty;
                }
            }

            return result;
        }

        /// <summary>
        ///     解码URL.
        /// </summary>
        /// <param name="encoding">null为自动选择编码</param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MyUrlDeCode(string str, Encoding encoding) {
            if (encoding == null) {
                var utf8 = Encoding.UTF8;
                //首先用utf-8进行解码                     
                var code = HttpUtility.UrlDecode(str.ToUpper(), utf8);
                //将已经解码的字符再次进行编码.
                var encode = HttpUtility.UrlEncode(code, utf8).ToUpper();
                if (str == encode)
                    encoding = Encoding.UTF8;
                else
                    encoding = Encoding.GetEncoding("gb2312");
            }

            return HttpUtility.UrlDecode(str, encoding);
        }


        /// <summary>
        ///     获取url字符串参数，返回参数值字符串
        /// </summary>
        /// <param name="url">url字符串</param>
        /// <returns></returns>
        public static NameValueCollection GetQueryParas(string url) {
            var re = new Regex(@"(^|&)?(\w+)=([^&]+)?(&|$)?", RegexOptions.Compiled);
            // var re = new Regex(@"(^|&)?(\w+)=([^&]+)(^&)$", RegexOptions.Compiled);
            var mc = re.Matches(url);
            var result = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            foreach (Match m in mc) {
                var key = m.Result("$2");
                result[key] = m.Result("$3");
            }
            return result;
        }
    }
}