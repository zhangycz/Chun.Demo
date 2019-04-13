using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Chun.Demo.Common
{
    public class TorrentHelper
    {
        public TorrentHelper(string fileName) {
            var torrentFile = new FileStream(fileName, FileMode.Open);
            var buffer = new byte[torrentFile.Length];
            torrentFile.Read(buffer, 0, buffer.Length);
            torrentFile.Close();

            if ((char) buffer[0] != 'd') {
                if (OpenError.Length == 0)
                    OpenError = "错误的Torrent文件，开头第1字节不是100";
                return;
            }
            GetTorrentData(buffer);
        }

        #region 开始读数据

        /// <summary>
        ///     开始读取
        /// </summary>
        /// <param name="buffer"></param>
        private void GetTorrentData(byte[] buffer) {
            var startIndex = 1;
            while (true) {
                var keys = GetKeyText(buffer, ref startIndex);
                if (keys == null) {
                    if (startIndex >= buffer.Length)
                        OpenFile = true;
                    break;
                }

                if (GetValueText(buffer, ref startIndex, keys.ToString().ToUpper()) == false)
                    break;
            }
        }

        #endregion

        /// <summary>
        ///     读取结构
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="starIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool GetValueText(byte[] buffer, ref int starIndex, string key) {
            switch (key) {
                case "ANNOUNCE":
                    Announce = GetKeyText(buffer, ref starIndex).ToString();
                    break;
                case "ANNOUNCE-LIST":
                    var listCount = 0;
                    var tempList = GetKeyData(buffer, ref starIndex, ref listCount);
                    for (var i = 0; i != tempList.Count; i++)
                        AnnounceList.Add(tempList[i].ToString());
                    break;
                case "CREATION DATE":
                    object date = GetKeyNumb(buffer, ref starIndex).ToString();
                    CreateTime = CreateTime.AddTicks(long.Parse(date.ToString()));
                    break;
                case "CODEPAGE":
                    var codePageNumb = GetKeyNumb(buffer, ref starIndex);
                    if (codePageNumb == null) {
                        if (OpenError.Length == 0)
                            OpenError = "CODEPAGE 返回不是数字类型";
                        return false;
                    }
                    CodePage = long.Parse(codePageNumb.ToString());
                    break;
                case "ENCODING":
                    Encoding = GetKeyText(buffer, ref starIndex).ToString();
                    break;
                case "CREATED BY":
                    CreatedBy = GetKeyText(buffer, ref starIndex).ToString();
                    break;
                case "COMMENT":
                    Comment = GetKeyText(buffer, ref starIndex).ToString();
                    break;
                case "COMMENT.UTF-8":
                    CommentUTF8 = GetKeyText(buffer, ref starIndex).ToString();
                    break;
                case "INFO":
                    var fileListCount = 0;
                    GetFileInfo(buffer, ref starIndex, ref fileListCount);
                    break;
                case "NAME":
                    Name = GetKeyText(buffer, ref starIndex).ToString();
                    break;
                case "NAME.UTF-8":
                    NameUTF8 = GetKeyText(buffer, ref starIndex).ToString();
                    break;
                case "PIECE LENGTH":
                    var pieceLengthNumb = GetKeyNumb(buffer, ref starIndex);
                    if (pieceLengthNumb == null) {
                        if (OpenError.Length == 0)
                            OpenError = "PIECE LENGTH 返回不是数字类型";
                        return false;
                    }
                    PieceLength = long.Parse(pieceLengthNumb.ToString());
                    break;
                case "PIECES":
                    Pieces = GetKeyByte(buffer, ref starIndex);
                    break;
                case "PUBLISHER":
                    Publisher = GetKeyText(buffer, ref starIndex).ToString();
                    break;
                case "PUBLISHER.UTF-8":
                    PublisherUTF8 = GetKeyText(buffer, ref starIndex).ToString();
                    break;
                case "PUBLISHER-URL":
                    PublisherUrl = GetKeyText(buffer, ref starIndex).ToString();
                    break;
                case "PUBLISHER-URL.UTF-8":
                    PublisherUrlUTF8 = GetKeyText(buffer, ref starIndex).ToString();
                    break;
                case "NODES":
                    var nodesCount = 0;
                    var _nodesList = GetKeyData(buffer, ref starIndex, ref nodesCount);
                    var ipCount = _nodesList.Count / 2;
                    for (var i = 0; i != ipCount; i++)
                        Notes.Add(_nodesList[i * 2] + ":" + _nodesList[i * 2 + 1]);
                    break;

                default:
                    return false;
            }
            return true;
        }

        /// <summary>
        ///     对应结构 INFO 多个文件时
        /// </summary>
        public class FileInfo
        {
            /// <summary>
            ///     文件路径
            /// </summary>
            public string Path { get; set; } = "";

            /// <summary>
            ///     UTF8的名称
            /// </summary>
            public string PathUtf8 { get; set; } = "";

            /// <summary>
            ///     文件大小
            /// </summary>
            public long Length { get; set; }

            /// <summary>
            ///     MD5验效 （可选）
            /// </summary>
            public string Md5Sum { get; set; } = "";

            /// <summary>
            ///     ED2K 未知
            /// </summary>
            public string De2K { get; set; } = "";

            /// <summary>
            ///     FileHash 未知
            /// </summary>
            public string FileHash { get; set; } = "";
        }

        #region 私有字段

        #endregion

        #region 属性

        /// <summary>
        ///     错误信息
        /// </summary>
        public string OpenError { set; get; } = "";

        /// <summary>
        ///     是否正常打开文件
        /// </summary>
        public bool OpenFile { set; get; }

        /// <summary>
        ///     服务器的URL(字符串)
        /// </summary>
        public string Announce { set; get; } = "";

        /// <summary>
        ///     备用tracker服务器列表(列表)
        /// </summary>
        public IList<string> AnnounceList { set; get; } = new List<string>();

        /// <summary>
        ///     种子创建的时间，Unix标准时间格式，从1970 1月1日 00:00:00到创建时间的秒数(整数)
        /// </summary>
        public DateTime CreateTime { set; get; } = new DateTime(1970, 1, 1, 0, 0, 0);

        /// <summary>
        ///     未知数字CodePage
        /// </summary>
        public long CodePage { set; get; }

        /// <summary>
        ///     种子描述
        /// </summary>
        public string Comment { set; get; } = "";

        /// <summary>
        ///     编码方式
        /// </summary>
        public string CommentUTF8 { set; get; } = "";

        /// <summary>
        ///     创建人
        /// </summary>
        public string CreatedBy { set; get; } = "";

        /// <summary>
        ///     编码方式
        /// </summary>
        public string Encoding { set; get; } = "";

        /// <summary>
        ///     文件信息
        /// </summary>
        public IList<FileInfo> FileList { set; get; } = new List<FileInfo>();

        /// <summary>
        ///     种子名
        /// </summary>
        public string Name { set; get; } = "";

        /// <summary>
        ///     种子名UTF8
        /// </summary>
        public string NameUTF8 { set; get; } = "";

        /// <summary>
        ///     每个块的大小，单位字节(整数)
        /// </summary>
        public long PieceLength { set; get; }

        /// <summary>
        ///     每个块的20个字节的SHA1 Hash的值(二进制格式)
        /// </summary>
        private byte[] Pieces { set; get; }

        /// <summary>
        ///     出版
        /// </summary>
        public string Publisher { set; get; } = "";

        /// <summary>
        ///     出版UTF8
        /// </summary>
        public string PublisherUTF8 { set; get; } = "";

        /// <summary>
        ///     出版地址
        /// </summary>
        public string PublisherUrl { set; get; } = "";

        /// <summary>
        ///     出版地址
        /// </summary>
        public string PublisherUrlUTF8 { set; get; } = "";

        /// <summary>
        ///     NODES
        /// </summary>
        public IList<string> Notes { set; get; } = new List<string>();

        /// <summary>
        ///     包含文件的总长度
        /// </summary>
        public long TotalLength { get; private set; }

        #endregion

        #region 获取数据

        /// <summary>
        ///     获取列表方式 "I1:Xe"="X" 会调用GetKeyText
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="starIndex"></param>
        /// <param name="listCount"></param>
        private ArrayList GetKeyData(byte[] buffer, ref int starIndex, ref int listCount) {
            var tempList = new ArrayList();
            while (true) {
                var textStar = System.Text.Encoding.UTF8.GetString(buffer, starIndex, 1);
                switch (textStar) {
                    case "l":
                        starIndex++;
                        listCount++;
                        break;
                    case "e":
                        listCount--;
                        starIndex++;
                        if (listCount == 0)
                            return tempList;
                        break;
                    case "i":
                        tempList.Add(GetKeyNumb(buffer, ref starIndex).ToString());
                        break;
                    default:
                        var listText = GetKeyText(buffer, ref starIndex);
                        if (listText != null) {
                            tempList.Add(listText.ToString());
                        }
                        else {
                            if (OpenError.Length == 0) {
                                OpenError = "错误的Torrent文件，ANNOUNCE-LIST错误";
                                return tempList;
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        ///     获取字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private object GetKeyText(byte[] buffer, ref int startIndex) {
            var numb = 0;
            var leftNumb = 0;
            for (var i = startIndex; i != buffer.Length; i++) {
                if ((char) buffer[i] == ':')
                    break;
                if ((char) buffer[i] == 'e') {
                    leftNumb++;
                    continue;
                }
                numb++;
            }

            startIndex += leftNumb;
            var textNumb = System.Text.Encoding.UTF8.GetString(buffer, startIndex, numb);
            try {
                var readNumb = int.Parse(textNumb);
                startIndex = startIndex + numb + 1;
                object keyText = System.Text.Encoding.UTF8.GetString(buffer, startIndex, readNumb);
                startIndex += readNumb;
                return keyText;
            }
            catch {
                return null;
            }
        }

        /// <summary>
        ///     获取数字
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        private object GetKeyNumb(byte[] buffer, ref int startIndex) {
            if (System.Text.Encoding.UTF8.GetString(buffer, startIndex, 1) == "i") {
                var numb = 0;
                for (var i = startIndex; i != buffer.Length; i++) {
                    if ((char) buffer[i] == 'e')
                        break;
                    numb++;
                }
                startIndex++;
                try {
                    var retNumb = long.Parse(System.Text.Encoding.UTF8.GetString(buffer, startIndex, numb - 1));
                    startIndex += numb;
                    return retNumb;
                }
                catch {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        ///     获取BYTE数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private byte[] GetKeyByte(byte[] buffer, ref int startIndex) {
            var numb = 0;
            for (var i = startIndex; i != buffer.Length; i++) {
                if ((char) buffer[i] == ':')
                    break;
                numb++;
            }
            var textNumb = System.Text.Encoding.UTF8.GetString(buffer, startIndex, numb);

            try {
                var readNumb = int.Parse(textNumb);
                startIndex = startIndex + numb + 1;
                var keyMemory = new MemoryStream(buffer, startIndex, readNumb);
                var keyBytes = new byte[readNumb];
                keyMemory.Read(keyBytes, 0, readNumb);
                keyMemory.Close();
                startIndex += readNumb;
                return keyBytes;
            }
            catch {
                return null;
            }
        }

        /// <summary>
        ///     对付INFO的结构
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        /// <param name="listCount"></param>
        private void GetFileInfo(byte[] buffer, ref int startIndex, ref int listCount) {
            if ((char) buffer[startIndex] != 'd')
                return;
            startIndex++;

            if (GetKeyText(buffer, ref startIndex).ToString().ToUpper() == "FILES") {
                var info = new FileInfo();

                while (true) {
                    var textStar = System.Text.Encoding.UTF8.GetString(buffer, startIndex, 1);

                    switch (textStar) {
                        case "l":
                            startIndex++;
                            listCount++;
                            break;
                        case "e":
                            listCount--;
                            startIndex++;
                            if (listCount == 1)
                                FileList.Add(info);
                            if (listCount == 0)
                                return;
                            break;
                        case "d":
                            info = new FileInfo();
                            listCount++;
                            startIndex++;
                            break;

                        default:
                            var listText = GetKeyText(buffer, ref startIndex);
                            if (listText == null)
                                return;
                            switch (listText.ToString().ToUpper()) //转换为大写
                            {
                                case "ED2K":
                                    info.De2K = GetKeyText(buffer, ref startIndex).ToString();
                                    break;
                                case "FILEHASH":
                                    info.FileHash = GetKeyText(buffer, ref startIndex).ToString();
                                    break;

                                case "LENGTH":
                                    info.Length = Convert.ToInt64(GetKeyNumb(buffer, ref startIndex));
                                    TotalLength += info.Length;
                                    break;
                                case "PATH":
                                    var pathCount = 0;
                                    var pathList = GetKeyData(buffer, ref startIndex, ref pathCount);
                                    var temp = "";
                                    for (var i = 0; i != pathList.Count; i++) {
                                        if (i < pathList.Count && i != 0)
                                            temp += "\\";
                                        temp += pathList[i].ToString();
                                    }
                                    info.Path = temp;
                                    break;
                                case "PATH.UTF-8":
                                    var pathUtf8Count = 0;
                                    var pathutf8List = GetKeyData(buffer, ref startIndex, ref pathUtf8Count);
                                    var utfTemp = "";
                                    for (var i = 0; i != pathutf8List.Count; i++)
                                        utfTemp += pathutf8List[i].ToString();
                                    info.PathUtf8 = utfTemp;
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        #endregion
    }
}