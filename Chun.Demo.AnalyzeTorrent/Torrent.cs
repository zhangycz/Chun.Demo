using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chun.Demo.AnalyzeTorrent
{
        public class Torrent
        {
            #region 私有字段

            private string _openError = "";
            private bool _openFile = false;

            private string _announce = "";
            private IList<string> _announceList = new List<string>();
            private DateTime _createTime = new DateTime(1970, 1, 1, 0, 0, 0);
            private long _codePage = 0;
            private string _comment = "";
            private string _createdBy = "";
            private string _encoding = "";
            private string _commentUTF8 = "";
            private IList<FileInfo> _fileList = new List<FileInfo>();
            private string _name = "";
            private string _nameUTF8 = "";
            private long _pieceLength = 0;
            private byte[] _pieces;
            private string _publisher = "";
            private string _publisherUTF8 = "";
            private string _publisherUrl = "";
            private string _publisherUrlUTF8 = "";
            private IList<string> _notes = new List<string>();
            private long _totalLength;

            #endregion

            #region 属性

            /// <summary>
            /// 错误信息
            /// </summary>
            public string OpenError { set { _openError = value; } get { return _openError; } }

            /// <summary>
            /// 是否正常打开文件
            /// </summary>
            public bool OpenFile { set { _openFile = value; } get { return _openFile; } }

            /// <summary>
            /// 服务器的URL(字符串)
            /// </summary>
            public string Announce { set { _announce = value; } get { return _announce; } }

            /// <summary>
            /// 备用tracker服务器列表(列表)
            /// </summary>
            public IList<string> AnnounceList { set { _announceList = value; } get { return _announceList; } }

            /// <summary>
            /// 种子创建的时间，Unix标准时间格式，从1970 1月1日 00:00:00到创建时间的秒数(整数)
            /// </summary>
            public DateTime CreateTime { set { _createTime = value; } get { return _createTime; } }

            /// <summary>
            /// 未知数字CodePage
            /// </summary>
            public long CodePage { set { _codePage = value; } get { return _codePage; } }

            /// <summary>
            /// 种子描述
            /// </summary>
            public string Comment { set { _comment = value; } get { return _comment; } }

            /// <summary>
            /// 编码方式
            /// </summary>
            public string CommentUTF8 { set { _commentUTF8 = value; } get { return _commentUTF8; } }

            /// <summary>
            /// 创建人
            /// </summary>
            public string CreatedBy { set { _createdBy = value; } get { return _createdBy; } }

            /// <summary>
            /// 编码方式
            /// </summary>
            public string Encoding { set { _encoding = value; } get { return _encoding; } }
            /// <summary>
            /// 文件信息
            /// </summary>
            public IList<FileInfo> FileList { set { _fileList = value; } get { return _fileList; } }

            /// <summary>
            /// 种子名
            /// </summary>
            public string Name { set { _name = value; } get { return _name; } }
            /// <summary>
            /// 种子名UTF8
            /// </summary>
            public string NameUTF8 { set { _nameUTF8 = value; } get { return _nameUTF8; } }

            /// <summary>
            /// 每个块的大小，单位字节(整数)
            /// </summary>
            public long PieceLength { set { _pieceLength = value; } get { return _pieceLength; } }

            /// <summary>
            /// 每个块的20个字节的SHA1 Hash的值(二进制格式)
            /// </summary>
            private byte[] Pieces { set { _pieces = value; } get { return _pieces; } }

            /// <summary>
            /// 出版
            /// </summary>
            public string Publisher { set { _publisher = value; } get { return _publisher; } }

            /// <summary>
            /// 出版UTF8
            /// </summary>
            public string PublisherUTF8 { set { _publisherUTF8 = value; } get { return _publisherUTF8; } }

            /// <summary>
            /// 出版地址
            /// </summary>
            public string PublisherUrl { set { _publisherUrl = value; } get { return _publisherUrl; } }

            /// <summary>
            /// 出版地址
            /// </summary>
            public string PublisherUrlUTF8 { set { _publisherUrlUTF8 = value; } get { return _publisherUrlUTF8; } }

            /// <summary>
            /// NODES
            /// </summary>
            public IList<string> Notes { set { _notes = value; } get { return _notes; } }

            /// <summary>
            /// 包含文件的总长度
            /// </summary>
            public long TotalLength
            {
                get
                {
                    return _totalLength;
                }
            }

            #endregion

            public Torrent(string fileName)
            {
                System.IO.FileStream torrentFile = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
                byte[] buffer = new byte[torrentFile.Length];
                torrentFile.Read(buffer, 0, buffer.Length);
                torrentFile.Close();

                if ((char)buffer[0] != 'd')
                {
                    if (OpenError.Length == 0) OpenError = "错误的Torrent文件，开头第1字节不是100";
                    return;
                }
                GetTorrentData(buffer);
            }

            #region 开始读数据

            /// <summary>
            /// 开始读取
            /// </summary>
            /// <param name="buffer"></param>
            private void GetTorrentData(byte[] buffer)
            {
                int startIndex = 1;
                while (true)
                {
                    object Keys = GetKeyText(buffer, ref startIndex);
                    if (Keys == null)
                    {
                        if (startIndex >= buffer.Length) OpenFile = true;
                        break;
                    }

                    if (GetValueText(buffer, ref startIndex, Keys.ToString().ToUpper()) == false) break;
                }
            }

            #endregion

            /// <summary>
            /// 读取结构
            /// </summary>
            /// <param name="buffer"></param>
            /// <param name="starIndex"></param>
            /// <param name="key"></param>
            /// <returns></returns>
            private bool GetValueText(byte[] buffer, ref int starIndex, string key)
            {
                switch (key)
                {
                    case "ANNOUNCE":
                        Announce = GetKeyText(buffer, ref starIndex).ToString();
                        break;
                    case "ANNOUNCE-LIST":
                        int listCount = 0;
                        ArrayList _tempList = GetKeyData(buffer, ref starIndex, ref listCount);
                        for (int i = 0; i != _tempList.Count; i++)
                        {
                            AnnounceList.Add(_tempList[i].ToString());
                        }
                        break;
                    case "CREATION DATE":
                        object date = GetKeyNumb(buffer, ref starIndex).ToString();
                        if (date == null)
                        {
                            if (OpenError.Length == 0) OpenError = "CREATION DATE 返回不是数字类型";
                            return false;
                        }
                        CreateTime = CreateTime.AddTicks(long.Parse(date.ToString()));
                        break;
                    case "CODEPAGE":
                        object codePageNumb = GetKeyNumb(buffer, ref starIndex);
                        if (codePageNumb == null)
                        {
                            if (OpenError.Length == 0) OpenError = "CODEPAGE 返回不是数字类型";
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
                        int fileListCount = 0;
                        GetFileInfo(buffer, ref starIndex, ref fileListCount);
                        break;
                    case "NAME":
                        Name = GetKeyText(buffer, ref starIndex).ToString();
                        break;
                    case "NAME.UTF-8":
                        NameUTF8 = GetKeyText(buffer, ref starIndex).ToString();
                        break;
                    case "PIECE LENGTH":
                        object pieceLengthNumb = GetKeyNumb(buffer, ref starIndex);
                        if (pieceLengthNumb == null)
                        {
                            if (OpenError.Length == 0) OpenError = "PIECE LENGTH 返回不是数字类型";
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
                        int nodesCount = 0;
                        ArrayList _nodesList = GetKeyData(buffer, ref starIndex, ref nodesCount);
                        int ipCount = _nodesList.Count / 2;
                        for (int i = 0; i != ipCount; i++)
                        {
                            Notes.Add(_nodesList[i * 2] + ":" + _nodesList[(i * 2) + 1]);
                        }
                        break;

                    default:
                        return false;
                }
                return true;
            }

            #region 获取数据

            /// <summary>
            /// 获取列表方式 "I1:Xe"="X" 会调用GetKeyText
            /// </summary>
            /// <param name="buffer"></param>
            /// <param name="starIndex"></param>
            /// <param name="listCount"></param>
            private ArrayList GetKeyData(byte[] buffer, ref int starIndex, ref int listCount)
            {
                ArrayList _tempList = new ArrayList();
                while (true)
                {
                    string textStar = System.Text.Encoding.UTF8.GetString(buffer, starIndex, 1);
                    switch (textStar)
                    {
                        case "l":
                            starIndex++;
                            listCount++;
                            break;
                        case "e":
                            listCount--;
                            starIndex++;
                            if (listCount == 0) return _tempList;
                            break;
                        case "i":
                            _tempList.Add(GetKeyNumb(buffer, ref starIndex).ToString());
                            break;
                        default:
                            object listText = GetKeyText(buffer, ref starIndex);
                            if (listText != null)
                            {
                                _tempList.Add(listText.ToString());
                            }
                            else
                            {
                                if (OpenError.Length == 0)
                                {
                                    OpenError = "错误的Torrent文件，ANNOUNCE-LIST错误";
                                    return _tempList;
                                }
                            }
                            break;
                    }
                }
            }

            /// <summary>
            /// 获取字符串
            /// </summary>
            /// <param name="buffer"></param>
            /// <param name="startIndex"></param>
            /// <returns></returns>
            private object GetKeyText(byte[] buffer, ref int startIndex)
            {
                int numb = 0;
                int leftNumb = 0;
                for (int i = startIndex; i != buffer.Length; i++)
                {
                    if ((char)buffer[i] == ':') break;
                    if ((char)buffer[i] == 'e')
                    {
                        leftNumb++;
                        continue;
                    }
                    numb++;
                }

                startIndex += leftNumb;
                string textNumb = System.Text.Encoding.UTF8.GetString(buffer, startIndex, numb);
                try
                {
                    int readNumb = Int32.Parse(textNumb);
                    startIndex = startIndex + numb + 1;
                    object keyText = System.Text.Encoding.UTF8.GetString(buffer, startIndex, readNumb);
                    startIndex += readNumb;
                    return keyText;
                }
                catch
                {
                    return null;
                }

            }

            /// <summary>
            /// 获取数字
            /// </summary>
            /// <param name="buffer"></param>
            /// <param name="startIndex"></param>
            private object GetKeyNumb(byte[] buffer, ref int startIndex)
            {
                if (System.Text.Encoding.UTF8.GetString(buffer, startIndex, 1) == "i")
                {
                    int numb = 0;
                    for (int i = startIndex; i != buffer.Length; i++)
                    {
                        if ((char)buffer[i] == 'e') break;
                        numb++;
                    }
                    startIndex++;
                    long retNumb = 0;
                    try
                    {
                        retNumb = long.Parse(System.Text.Encoding.UTF8.GetString(buffer, startIndex, numb - 1));
                        startIndex += numb;
                        return retNumb;
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

            }

            /// <summary>
            /// 获取BYTE数据
            /// </summary>
            /// <param name="buffer"></param>
            /// <param name="startIndex"></param>
            /// <returns></returns>
            private byte[] GetKeyByte(byte[] buffer, ref int startIndex)
            {
                int numb = 0;
                for (int i = startIndex; i != buffer.Length; i++)
                {
                    if ((char)buffer[i] == ':') break;
                    numb++;
                }
                string textNumb = System.Text.Encoding.UTF8.GetString(buffer, startIndex, numb);

                try
                {
                    int readNumb = Int32.Parse(textNumb);
                    startIndex = startIndex + numb + 1;
                    System.IO.MemoryStream keyMemory = new System.IO.MemoryStream(buffer, startIndex, readNumb);
                    byte[] keyBytes = new byte[readNumb];
                    keyMemory.Read(keyBytes, 0, readNumb);
                    keyMemory.Close();
                    startIndex += readNumb;
                    return keyBytes;
                }
                catch
                {
                    return null;
                }
            }

            /// <summary>
            /// 对付INFO的结构
            /// </summary>
            /// <param name="buffer"></param>
            /// <param name="startIndex"></param>
            /// <param name="listCount"></param>
            private void GetFileInfo(byte[] buffer, ref int startIndex, ref int listCount)
            {
                if ((char)buffer[startIndex] != 'd') return;
                startIndex++;

                if (GetKeyText(buffer, ref startIndex).ToString().ToUpper() == "FILES")
                {
                    FileInfo info = new FileInfo();

                    while (true)
                    {
                        string TextStar = System.Text.Encoding.UTF8.GetString(buffer, startIndex, 1);

                        switch (TextStar)
                        {
                            case "l":
                                startIndex++;
                                listCount++;
                                break;
                            case "e":
                                listCount--;
                                startIndex++;
                                if (listCount == 1) FileList.Add(info);
                                if (listCount == 0) return;
                                break;
                            case "d":
                                info = new FileInfo();
                                listCount++;
                                startIndex++;
                                break;

                            default:
                                object listText = GetKeyText(buffer, ref startIndex);
                                if (listText == null) return;
                                switch (listText.ToString().ToUpper())   //转换为大写
                                {
                                    case "ED2K":
                                        info.De2K = GetKeyText(buffer, ref startIndex).ToString();
                                        break;
                                    case "FILEHASH":
                                        info.FileHash = GetKeyText(buffer, ref startIndex).ToString();
                                        break;

                                    case "LENGTH":
                                        info.Length = Convert.ToInt64(GetKeyNumb(buffer, ref startIndex));
                                        _totalLength += info.Length;
                                        break;
                                    case "PATH":
                                        int PathCount = 0;
                                        ArrayList PathList = GetKeyData(buffer, ref startIndex, ref PathCount);
                                        string Temp = "";
                                        for (int i = 0; i != PathList.Count; i++)
                                        {
                                            if (i < PathList.Count && i != 0)
                                                Temp += "\\";
                                            Temp += PathList[i].ToString();
                                        }
                                        info.Path = Temp;
                                        break;
                                    case "PATH.UTF-8":
                                        int pathUtf8Count = 0;
                                        ArrayList pathutf8List = GetKeyData(buffer, ref startIndex, ref pathUtf8Count);
                                        string utfTemp = "";
                                        for (int i = 0; i != pathutf8List.Count; i++)
                                        {
                                            utfTemp += pathutf8List[i].ToString();
                                        }
                                        info.PathUTF8 = utfTemp;
                                        break;
                                }
                                break;
                        }

                    }

                }
            }

            #endregion

            /// <summary>
            /// 对应结构 INFO 多个文件时
            /// </summary>
            public class FileInfo
            {
                private string path = "";
                private string pathutf8 = "";
                private long length = 0;
                private string md5sum = "";
                private string de2k = "";
                private string filehash = "";

                /// <summary>
                /// 文件路径
                /// </summary>
                public string Path { get { return path; } set { path = value; } }

                /// <summary>
                /// UTF8的名称
                /// </summary>
                public string PathUTF8 { get { return pathutf8; } set { pathutf8 = value; } }

                /// <summary>
                /// 文件大小
                /// </summary>
                public long Length { get { return length; } set { length = value; } }

                /// <summary>
                /// MD5验效 （可选）
                /// </summary>
                public string MD5Sum { get { return md5sum; } set { md5sum = value; } }

                /// <summary>
                /// ED2K 未知
                /// </summary>
                public string De2K { get { return de2k; } set { de2k = value; } }

                /// <summary>
                /// FileHash 未知
                /// </summary>
                public string FileHash { get { return filehash; } set { filehash = value; } }
            }
        
    


}
}
