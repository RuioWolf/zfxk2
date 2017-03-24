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
        static string[] _classes;
        static string _login, _inedx, _vstate;

        static void Main(string[] args)
        {
            //if (args.Length < 3)
            //{
            //Console.WriteLine("�����е��÷�������������Ϊ ѧ�� ���� �κ��б����Ÿ�����");
            //Console.WriteLine("����: xsxk.exe 1401260241 123456 1,5,6,7");
            _login = ConfigurationManager.AppSettings.Get("login");
            _inedx = ConfigurationManager.AppSettings.Get("index");
            _vstate = ConfigurationManager.AppSettings.Get("vstate");
            if (args.Length < 3)
            {
                Console.Write("������ѧ��: ");
                _user = Console.ReadLine();
                Console.Write("���������ϵͳ����: ");
                //_pwd = Console.ReadLine();
                _pwd = EnterPasswd();
                Console.Write("������ѡ�ογ̴���: ");
                _classes = Console.ReadLine().Split(',');
            }
            else
            {
                _user = args[0];
                _pwd = args[1];
                _classes = args[2].Split(',');
            }
            if (string.IsNullOrEmpty(_user) || string.IsNullOrEmpty(_pwd) || string.IsNullOrEmpty(_classes[0]))
            {
                Console.WriteLine("��һ���������ϲ���Ϊ�գ�");
                Thread.Sleep(1000);
                System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Environment.Exit(0);
            }
            if (string.IsNullOrEmpty(_login))
            {
#if (DEBUG)
                Console.WriteLine("_login null");
#endif
                _login = "http://jw1.gdsdxy.cn:81/(zs0xzqifbnov3l55guj3ai55)/default2.aspx";
            }
            if (string.IsNullOrEmpty(_inedx))
            {
#if (DEBUG)
                Console.WriteLine("_index null");
#endif
                _inedx = "http://jw1.gdsdxy.cn:81/xf_xsqxxxk.aspx?xh=";
            }
            if (string.IsNullOrEmpty(_vstate))
            {
#if (DEBUG)
                Console.WriteLine("__VIEWSTATE null");
#endif
                _vstate = definition.LoginViewState;
            }
#if (DEBUG)
            Console.WriteLine(_user);
            Console.WriteLine(_pwd);
            Console.WriteLine(_login);
            Console.WriteLine(_inedx);
            Console.WriteLine(_vstate);
            foreach (var item in _classes)
            {
                Console.WriteLine(item);
            }
            Console.Read();
#endif
            //}
            //else
            //{
            //    _user = args[0];
            //    _pwd = args[1];
            //    _classes = args[2].Split(',');
            //}
            Console.WriteLine("###### ���ο�ʼ��######");
            string sLogin = "";
            int n = 1;
            while (sLogin == "" || sLogin.IndexOf("ϵͳ��æ") >= 0)
            {
                //Thread.Sleep(1000);
                string _Post = "__VIEWSTATE=" + _vstate + "&TextBox1=" + _user + "&TextBox2=" + _pwd + "&RadioButtonList1=%E5%AD%A6%E7%94%9F&Button1=";
                //string _Post = "__VIEWSTATE=" + _vstate + "&tbYHM=" + _user + "&tbPSW=" + _pwd + "&ddlSF=%D1%A7%C9%FA&imgDL.x=25&imgDL.y=12";
#if (DEBUG)
                Console.WriteLine(_Post);
                //Console.Read();
#endif
                sLogin = GvCrawler.Post(_login, _Post, _cookies);
#if (DEBUG)
                //Console.WriteLine(sLogin);
                //Console.Read();
#endif
                if (sLogin.IndexOf("�������") > 0)
                {
                    Console.WriteLine("######## ѧ�Ż��������########");
                    Console.ReadLine();
                    return;
                }

                if (sLogin.IndexOf("�û���������") > 0)
                {
                    Console.WriteLine("######## �û��������ڣ� ########");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("���Ե�¼��" + (n++) + "��");
                //string stjkbcx = _inedx + _user + "&lb=1";
                string stjkbcx = _inedx + _user;
                if ((sLogin != "" && sLogin.IndexOf("ϵͳ��æ") < 0))
                {
                    Console.WriteLine("��¼�ɹ�");
                    sLogin = GvCrawler.Get(stjkbcx, _cookies);

                    //while (sLogin.IndexOf("window.parent.location='';") < 0)
                    {
                        if ((sLogin != "" && sLogin.IndexOf("ϵͳ��æ") < 0))
                        {
                            List<CLASS_INFO> lstClass = GetClassList(sLogin);
                            Console.WriteLine("��ȡѡ���б�");

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
                                //sLogin = GvCrawler.Post(_inedx + _user + "&lb=1", _Post, _cookies);
                                sLogin = GvCrawler.Post(_inedx + _user, _Post, _cookies);
                                if ((sLogin != "" && sLogin.IndexOf("ϵͳ��æ") < 0))
                                {
                                    Console.WriteLine("#####ѡ�γɹ�#####");
                                    SaveClassInfo(lstClass);
                                    SaveToFile(sLogin, "result.html");
                                    Console.WriteLine(GetMessgae(sLogin));
                                    Console.Read();
                                }
                                else
                                {
                                    Console.WriteLine("�ύʧ��");
                                    //Thread.Sleep(1000);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("��ȡѡ���б�ʧ��");
                            //Thread.Sleep(1000);
                        }
                    }
                }
            }
            Console.Read();
        }

        static string EnterPasswd() //�˶�Դ�������ļ����� https://blog.skitisu.com/csharp-console-password-asterisk-backspace
        {
            while (true)
            {
                //Console.Write("����������>");
                string key = string.Empty;
                while (true)
                {
                    ConsoleKeyInfo keyinfo = Console.ReadKey(true);
                    if (keyinfo.Key == ConsoleKey.Enter) //���»س�������
                    {
                        Console.WriteLine();
                        break;
                    }
                    else if (keyinfo.Key == ConsoleKey.Backspace && key.Length > 0) //������˸�������ַ�û��ɾ��
                    {
                        Console.Write("\b \b"); //���һ���˸񣨴�ʱ�����������һλ����Ȼ�����һ���ո�ȡ�����һ���Ǻţ�Ȼ������ǰ��һλ��Ҳ����˵��ʵ������һ���ո����㿴����= =
                        key = key.Substring(0, key.Length - 1);
                    }

                    else if (!char.IsControl(keyinfo.KeyChar)) //���˵����ܰ�����
                    {
                        key += keyinfo.KeyChar.ToString();
                        Console.Write("*");
                    }
                }
                return key;
            }
        }

        /// <summary>
        /// ��ȡViewState
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
        /// ��ȡ��ʾ
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
        /// ��ȡ�γ��б�
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
        /// ������������ı�
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
        /// ����γ��б�
        /// </summary>
        /// <param name="lstInfo"></param>
        static void SaveClassInfo(List<CLASS_INFO> lstInfo)
        {
            StringBuilder sList = new StringBuilder();
            sList.AppendLine("����,ʱ��,��ʦ,�κ�,����");
            for (int i = 0; i < lstInfo.Count; i++)
            {
                sList.AppendLine(lstInfo[i].sName + ",'" + lstInfo[i].sTime + "'," + lstInfo[i].sTeacher + "," + lstInfo[i].sId + "," + lstInfo[i].sCheck);
            }
            //����α�class.csv ˮԺ����ϵͳ��Ч
            //SaveToFile(sList.ToString(), "class.csv");
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
