using System;
using System.Net;
using System.Threading.Tasks;
using Chun.Work.Common.Helper;
using HtmlAgilityPack;

namespace Chun.Demo.Common.Helper
{
    public static class HtmlHelper
    {
        public static async Task<HtmlDocument> Start(Uri uri) {
            var htmlDocument = new HtmlDocument();
            try {
                var sc = new SimpleCrawler(new CookieContainer());
                sc.OnCompleted += (sender, data) => {
                    LogHelper.Debug($"Complete Load  {data.Uri.PathAndQuery} ,take time {data.Milliseconds}");
                    var hmtl = data.PageSource;
                    htmlDocument.LoadHtml(hmtl);
                };
                sc.OnError += (sender, data) => {
                    LogHelper.Error($"Load {data.Uri.PathAndQuery} Fail,exception:{data.Exception}");
                };
                sc.OnStart += (sender, data) => { LogHelper.Debug($"Begin Load {data.Uri.PathAndQuery}"); };
                await sc.Start(uri);
            }
            catch (HtmlWebException ex) {
                LogHelper.Error(ex);
            }
            catch (Exception ex) {
                LogHelper.Error(ex);
            }
            return htmlDocument;
        }

        /// <summary>
        ///     载入当前页面的文档
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HtmlDocument LoadUrl(string url) {
            var htmlDocument = new HtmlWeb {
                //   UseCookies = false,
                UserAgent =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.7 Safari/537.36"
            };

            //bool Handler(HttpWebRequest request) {
            //    request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
            //    //  request.Headers[HttpRequestHeader.ContentType] = "application/json";
            //    // request.Headers[HttpRequestHeader.ContentType] = "text/html";
            //    request.ContentType = "text/html";
            //    request.Headers[HttpRequestHeader.Cookie] = "lastupdate=1529507120; visid_incap_1729161=1/tty/3BQF+749gWpBcxHkHwKFsAAAAAQUIPAAAAAABNFMCE54EPtuza53gjslMN; UM_distinctid=16417ea872beb9-0de8e1cb1b26f-7c153a4a-240000-16417ea872da73; aafaf_ol_offset=97; aafaf_skinco=MSN; aafaf_readlog=%2C1197730%2C1205404%2C; aafaf_lastpos=F16; aafaf_threadlog=%2C15%2C5%2C3%2C13%2C16%2C; incap_ses_634_1729161=3BqEO75lWXyybdv/KyzNCPiBKlsAAAAAg7pVWb7jgWGRgn5rwjs4TQ==; aafaf_lastvisit=1430%091529512808%09%2Fpw%2Fthread.php%3Ffid%3D16%26page%3D1%2527%2C; CNZZDATA1261158850=894563901-1529406239-null%7C1529509864";

            //    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            //    request.CookieContainer = new CookieContainer();
            //    //    var cookies = new CookieCollection() {

            //    //        new Cookie("incap_ses_634_1729161", "3BqEO75lWXyybdv/KyzNCPiBKlsAAAAAg7pVWb7jgWGRgn5rwjs4TQ=="),

            //    //};

            //    //  request.CookieContainer.Add(cookies);
            //    return true;
            //}

            //htmlDocument.PreRequest += Handler;


            var hdoc = htmlDocument.Load(url);
            return hdoc;
        }


        /// <summary>
        ///     返回匹配的节点数据
        /// </summary>
        /// <param name="htmlDocument"></param>
        /// <param name="matchNode"></param>
        /// <returns></returns>
        public static HtmlNodeCollection GetNodeCollect(HtmlDocument htmlDocument, string matchNode) {
            return htmlDocument.DocumentNode.SelectNodes(matchNode);
        }
    }
}