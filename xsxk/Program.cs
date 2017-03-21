using System;
using System.Collections.Generic;
using System.Text;
using GvWebCrawler;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.IO;
using System.Configuration;

namespace xsxk
{
    class Program
    {
        static CookieContainer _cookies = new CookieContainer();

        static string _user = "";
        static string _pwd = "";
        static string[] _classes ;
        static string _login, _inedx;

        static void Main(string[] args)
        {
            //if (args.Length < 3)
            //{
                //Console.WriteLine("命令行调用方法，参数依次为 学号 密码 课号列表（逗号隔开）");
                //Console.WriteLine("例如: xsxk.exe 1401260241 123456 1,5,6,7");
                _login = ConfigurationManager.AppSettings.Get("login");
                _inedx = ConfigurationManager.AppSettings.Get("index");
                Console.Write("请输入学号: ");
                _user = Console.ReadLine();
                Console.Write("请输入教务管理系统密码: ");
                //_pwd = Console.ReadLine();
                _pwd = EnterPasswd();
                Console.Write("请输入选课课程代码: ");
                _classes = Console.ReadLine().Split(',');
                if (string.IsNullOrEmpty(_user) || string.IsNullOrEmpty(_pwd) || string.IsNullOrEmpty(_classes[0]))
                {
                    Console.WriteLine("有一个或多个以上参数为空！");
                    Thread.Sleep(1000);
                    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    Environment.Exit(0);
                }
                if (string.IsNullOrEmpty(_login))
                {
#if (DEBUG)
                    Console.WriteLine("_login is null");
#endif
                    _login = "http://jwnew.gdsdxy.cn/default2.aspx";
                }
                if (string.IsNullOrEmpty(_inedx))
                {
#if (DEBUG)
                    Console.WriteLine("_index is null");
#endif
                    _inedx = "http://jwnew.gdsdxy.cn/xs_main.aspx?xh=";
                }
#if (DEBUG)
                Console.WriteLine(_user);
                Console.WriteLine(_pwd);
                Console.WriteLine(_login);
                Console.WriteLine(_inedx);
                foreach (var item in _classes)
                {
                    Console.WriteLine(item);
                }
                Console.ReadLine();
#endif
            //}
            //else
            //{
            //    _user = args[0];
            //    _pwd = args[1];
            //    _classes = args[2].Split(',');
            //}
            Console.WriteLine("###### 抢课开始！######");
            string sLogin = "";
            int n = 1;
            while (sLogin == "" || sLogin.IndexOf("系统繁忙") >= 0)
            {
                Thread.Sleep(1000);
                string _Post = "__VIEWSTATE=" + definition.LoginViewState + "&tbYHM=" + _user + "&tbPSW=" + _pwd + "&ddlSF=%D1%A7%C9%FA&imgDL.x=25&imgDL.y=12"; 
                sLogin = GvCrawler.Post(_login, _Post, _cookies);
                if (sLogin.IndexOf("密码不正确") > 0)
                {
                    Console.WriteLine("######## 学号或密码错误！########");
                    Console.Read();
                    return;
                }

                Console.WriteLine("尝试登录第" + (n++) + "次");
                string stjkbcx = _inedx + _user + "&lb=1";
                if ((sLogin != "" && sLogin.IndexOf("系统繁忙") < 0))
                {
                    Console.WriteLine("登录成功");
                    sLogin = GvCrawler.Get(stjkbcx, _cookies);

                    //while (sLogin.IndexOf("window.parent.location='';") < 0)
                    {
                        if ((sLogin != "" && sLogin.IndexOf("系统繁忙") < 0))
                        {
                            List<CLASS_INFO> lstClass = GetClassList(sLogin);
                            Console.WriteLine("获取选课列表");

                            _Post = "__VIEWSTATE=" + GetVIEWSTATE(sLogin) + "&Button1=%D1%A1++%B6%A8";

                            for (int i = 0; i < lstClass.Count; i++)
                            {
                                for (int j = 0; j < _classes.Length; j++)
                                {
                                    if (lstClass[i].sId == _classes[j])
                                    {
                                        _Post += "&" + lstClass[i].sCheck + "=no";
                                    }
                                }
                            }

                            while (sLogin.IndexOf("window.parent.location='';") < 0)
                            {
                                sLogin = GvCrawler.Post(_inedx + _user + "&lb=1", _Post, _cookies);
                                if ((sLogin != "" && sLogin.IndexOf("系统繁忙") < 0))
                                {
                                    Console.WriteLine("#####选课成功#####");
                                    SaveClassInfo(lstClass);
                                    SaveToFile(sLogin, "result.html");
                                    Console.WriteLine(GetMessgae(sLogin));
                                    Console.Read();
                                }
                                else
                                {
                                    Console.WriteLine("提交失败");
                                    Thread.Sleep(1000);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("获取选课列表失败");
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
            Console.Read();
        }

        static string EnterPasswd() //源码来自四季天书 https://blog.skitisu.com/csharp-console-password-asterisk-backspace
        {
            while (true)
            {
                //Console.Write("请输入密码>");
                string key = string.Empty;
                while (true)
                {
                    ConsoleKeyInfo keyinfo = Console.ReadKey(true);
                    if (keyinfo.Key == ConsoleKey.Enter) //按下回车，结束
                    {
                        Console.WriteLine();
                        break;
                    }
                    else if (keyinfo.Key == ConsoleKey.Backspace && key.Length > 0) //如果是退格键并且字符没有删光
                    {
                        Console.Write("\b \b"); //输出一个退格（此时光标向左走了一位），然后输出一个空格取代最后一个星号，然后再往前走一位，也就是说其实后面有一个空格但是你看不见= =
                        key = key.Substring(0, key.Length - 1);
                    }

                    else if (!char.IsControl(keyinfo.KeyChar)) //过滤掉功能按键等
                    {
                        key += keyinfo.KeyChar.ToString();
                        Console.Write("*");
                    }
                }
                return key;
            }
        }

        /// <summary>
        /// 获取ViewState
        /// </summary>
        /// <param name="sHtml"></param>
        /// <returns></returns>
        static string GetVIEWSTATE(string sHtml)
        {
            string sRegex = "<input type=\"hidden\" name=\"__VIEWSTATE\" value=\"(.*?)\" />";
            MatchCollection Matches = Analtytic(sRegex, sHtml);
            if (Matches.Count <= 0) return "";
            return HttpUtility.UrlEncode(Matches[0].Groups[1].Value, Encoding.GetEncoding("GB2312"));
        }

        /// <summary>
        /// 获取提示
        /// </summary>
        /// <param name="sHtml"></param>
        /// <returns></returns>
        static string GetMessgae(string sHtml)
        {
            string sRegex = @"alert\('(.*?)'\)";

            MatchCollection Matches = Analtytic(sRegex, sHtml);
            if (Matches.Count <= 0) return "";

            string sContent = Matches[0].Groups[1].Value;

            return sContent.Replace(@"\r", "\n");
        }

        /// <summary>
        /// 获取课程列表
        /// </summary>
        /// <param name="sHtml"></param>
        /// <returns></returns>
        static List<CLASS_INFO> GetClassList(string sHtml)
        {
            string sRegex = @"<tr[^<]*?<td><a [^>]*?>([^<]*?)</a></td><td>[\d-]*?</td><td>([^<]*?)</td>.*?</td><td>(?:<a href=""[^""]*?"">([^<]*?)</a>)+</td><td> <input id=""DataGrid1__ctl\d*_zhj1"" type=""checkbox"" name=""(DataGrid1:_ctl\d*:zhj1)"" /> </td><td>(\d*)</td><td>";

            MatchCollection Matches = Analtytic(sRegex, sHtml);
            List<CLASS_INFO> checks = new List<CLASS_INFO>();

            foreach (Match match in Matches)
            {
                CLASS_INFO item = new CLASS_INFO();
                item.sName = match.Groups[1].Value;
                item.sTime = match.Groups[2].Value;
                item.sTeacher = match.Groups[3].Value;
                item.sCheck = match.Groups[4].Value;
                item.sId = match.Groups[5].Value;
                checks.Add(item);
            }

            return checks;
        }
        
        /// <summary>
        /// 根据正则解析文本
        /// </summary>
        /// <param name="sRegEx"></param>
        /// <param name="sHtml"></param>
        /// <returns></returns>
        static public MatchCollection Analtytic(string sRegEx, string sHtml)
        {
            System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(sHtml, sRegEx, RegexOptions.IgnoreCase);
            //Console.WriteLine("Analtytic RegEx: " + sRegEx + " Count:" + matches.Count);
            return matches;
        }

        /// <summary>
        /// 保存课程列表
        /// </summary>
        /// <param name="lstInfo"></param>
        static void SaveClassInfo(List<CLASS_INFO> lstInfo)
        {
            StringBuilder sList = new StringBuilder();
            sList.AppendLine("名称,时间,老师,课号,请求");
            for (int i = 0; i < lstInfo.Count; i++)
            {
                sList.AppendLine(lstInfo[i].sName + ",'" + lstInfo[i].sTime + "'," + lstInfo[i].sTeacher + "," + lstInfo[i].sId + "," + lstInfo[i].sCheck);
            }
            SaveToFile(sList.ToString(), "class.csv");
        }

        static void SaveToFile(string sContent, string sFileName)
        {
            using (StreamWriter wr = new StreamWriter(sFileName, false, Encoding.Default))
            {
                wr.Write(sContent);
            }
        }
    }
}
