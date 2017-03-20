// #define BROWER  // ȥ��ע�ͼ�����Brower
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

#if BROWER
using System.Windows.Forms;
using mshtml;
#endif

namespace GvWebCrawler
{
    /// <summary>
    /// ��ҳ������
    /// </summary>
    static class GvCrawler
    {
        #region ˽�б���
        static HttpHeader _header = new HttpHeader();
        static HttpStatusCode _status = HttpStatusCode.OK;
        static CookieContainer _cookies = new CookieContainer();
        #endregion

        #region ��ȡ״̬
        /// <summary>
        /// ��ȡ���ץȡ��ҳ��״̬
        /// </summary>
        /// <returns></returns>
        public static HttpStatusCode GetLastStatus()
        {
            return _status;
        }
        #endregion
        
        #region Get����

        /// <summary>
        /// Get��ʽ��ȡһ����ҳԴ��
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <returns>��ҳԴ��</returns>
        public static string Get(string sWebUrl)
        {
            return Get(sWebUrl, null, true, _cookies, ref _status);
        }

        /// <summary>
        /// Get��ʽ��ȡһ����ҳԴ��
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="sEncoding">ָ����ҳ����</param>
        /// <returns>��ҳԴ��</returns>
        public static string Get(string sWebUrl, string sEncoding)
        {
            return Get(sWebUrl, sEncoding, true, _cookies, ref _status);
        }

        /// <summary>
        /// Get��ʽ��ȡһ����ҳԴ��
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="cookieContainer">��ҳCookies</param>
        /// <returns>��ҳԴ��</returns>
        public static string Get(string sWebUrl, CookieContainer cookieContainer)
        {
            return Get(sWebUrl, null, true, cookieContainer, ref _status);
        }

        /// <summary>
        /// Get��ʽ��ȡһ����ҳԴ��
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="sEncoding">ָ����ҳ����</param>
        /// <param name="bResetSpace">���˿հ׷�</param>
        /// <returns>��ҳԴ��</returns>
        public static string Get(string sWebUrl, string sEncoding, bool bResetSpace)
        {
            return Get(sWebUrl, sEncoding, bResetSpace, _cookies, ref _status);
        }

        /// <summary>
        /// Get��ʽ��ȡһ����ҳԴ��
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="sEncoding">ָ����ҳ����</param>
        /// <param name="cookieContainer">��ҳCookies</param>
        /// <returns>��ҳԴ��</returns>
        public static string Get(string sWebUrl, string sEncoding, CookieContainer cookieContainer)
        {
            return Get(sWebUrl, sEncoding, true, cookieContainer, ref _status);
        }

        /// <summary>
        /// Get��ʽ��ȡһ����ҳԴ��
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="sEncoding">ָ������</param>
        /// <param name="bResetSpace">�Ƿ���˶�հ׷�</param>
        /// <param name="cookieContainer">��ҳ����Cookies</param>
        /// <returns>��ҳԴ��</returns>
        public static string Get(string sWebUrl, string sEncoding, bool bResetSpace, CookieContainer cookieContainer)
        {
            return Get(sWebUrl, sEncoding, bResetSpace, cookieContainer, ref _status);
        }

        /// <summary>
        /// Get��ʽ��ȡһ����ҳԴ��
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="sEncoding">ָ������</param>
        /// <param name="bResetSpace">�Ƿ���˶�հ׷�</param>
        /// <param name="cookieContainer">��ҳ����Cookies</param>
        /// <param name="httpStatus">��ҳ����״̬</param>
        /// <returns>��ҳԴ��</returns>
        public static string Get(string sWebUrl, string sEncoding, bool bResetSpace, CookieContainer cookieContainer, ref HttpStatusCode httpStatus)
        {
            return Crawler(sWebUrl, null, sEncoding, bResetSpace, ref cookieContainer, ref httpStatus);

        }

        #endregion

        #region Post����

        /// <summary>
        /// Post��ʽ��ȡһ����ҳԴ��
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="sData">Post����</param>
        /// <returns>��ҳԴ��</returns>
        public static string Post(string sWebUrl, string sData)
        {
            return Post(sWebUrl, sData, null, true, ref _cookies, ref _status);
        }

        /// <summary>
        /// Post��ʽ��ȡһ����ҳԴ��
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="sData">Post����</param>
        /// <param name="sEncoding">ָ������</param>
        /// <param name="cookieContainer">��ҳ����Cookies</param>
        /// <returns>��ҳԴ��</returns>
        public static string Post(string sWebUrl, string sData, string sEncoding, ref CookieContainer cookieContainer)
        {
            return Post(sWebUrl, sData, sEncoding, true, ref cookieContainer, ref _status);
        }

        /// <summary>
        /// Post��ʽ��ȡһ����ҳԴ��
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="sData">Post����</param>
        /// <param name="cookieContainer">��ҳ����Cookies</param>
        /// <returns>��ҳԴ��</returns>
        public static string Post(string sWebUrl, string sData, ref CookieContainer cookieContainer)
        {
            return Post(sWebUrl, sData, null, true, ref cookieContainer, ref _status);
        }

        /// <summary>
        /// Post��ʽ��ȡһ����ҳԴ��
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="sData">Post����</param>
        /// <param name="cookieContainer">��ҳ����Cookies</param>
        /// <returns>��ҳԴ��</returns>
        public static string Post(string sWebUrl, string sData, CookieContainer cookieContainer)
        {
            return Post(sWebUrl, sData, null, true, ref cookieContainer, ref _status);
        }

        /// <summary>
        /// Post��ʽ��ȡһ����ҳԴ��
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="sData">Post����</param>
        /// <param name="sEncoding">ָ������</param>
        /// <returns>��ҳԴ��</returns>
        public static string Post(string sWebUrl, string sData, string sEncoding)
        {
            return Post(sWebUrl, sData, sEncoding, true, ref _cookies, ref _status);
        }

        /// <summary>
        /// Post��ʽ��ȡһ����ҳԴ��
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="sData">Post����</param>
        /// <param name="sEncoding">ָ������</param>
        /// <param name="bResetSpace">�Ƿ���˶�հ׷�</param>
        /// <returns>��ҳԴ��</returns>
        public static string Post(string sWebUrl, string sData, string sEncoding, bool bResetSpace)
        {
            return Post(sWebUrl, sData, sEncoding, bResetSpace, ref _cookies, ref _status);
        }

        /// <summary>
        /// Post��ʽ��ȡһ����ҳԴ��
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="sData">Post����</param>
        /// <param name="sEncoding">ָ������</param>
        /// <param name="bResetSpace">�Ƿ���˶�հ׷�</param>
        /// <param name="cookieContainer">��ҳ����Cookies</param>
        /// <returns>��ҳԴ��</returns>
        public static string Post(string sWebUrl, string sData, string sEncoding, bool bResetSpace, ref CookieContainer cookieContainer)
        {
            return Post(sWebUrl, sData, sEncoding, bResetSpace, ref cookieContainer, ref _status);
        }

        /// <summary>
        /// Post��ʽ��ȡһ����ҳԴ��
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="sData">Post����</param>
        /// <param name="sEncoding">ָ������</param>
        /// <param name="bResetSpace">�Ƿ���˶�հ׷�</param>
        /// <param name="cookieContainer">��ҳ����Cookies</param>
        /// <param name="httpStatus">��ҳ����״̬</param>
        /// <returns>��ҳԴ��</returns>
        public static string Post(string sWebUrl, string sData, string sEncoding, bool bResetSpace, ref CookieContainer cookieContainer, ref HttpStatusCode httpStatus)
        {
            return Crawler(sWebUrl, sData, sEncoding, bResetSpace, ref cookieContainer, ref httpStatus);
        }

        #endregion

        #region �������ʽ������ҳ��Ĭ�Ϲر�

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *\
         *                                                           *
         *  �ٶ����ȶ��Խ��� ������Ϊ���ѡ�񣨽���App����
         *  
         *  ������ȥ����һ�� #define BROWER ��ע��
         * 
         *  ͬʱ������� System.Windows.Forms �� Microsoft.mshtml
         *                                                           *
        \* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

#if BROWER
        /// <summary>
        /// ģ���������ʽ������ҳ
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="sJavascript">Ҫִ�е�Javascript����</param>
        /// <param name="nMillisecond">�ȴ�Javascriptִ�к�����</param>
        /// <returns></returns>
        public static string Brower(string sWebUrl, string sJavascript, int nMillisecond)
        {
            WebBrowser wb = new WebBrowser();
            wb.ScriptErrorsSuppressed = true;
            string sHtml = string.Empty;
            wb.Navigate(sWebUrl);
            while (true)
            {
                Delay(50);
                // �ж��ĵ��Ƿ�������
                if (wb.ReadyState == WebBrowserReadyState.Complete)
                {
                    if (!wb.IsBusy)
                    {
                        HtmlElement headElement = wb.Document.GetElementsByTagName("head")[0];
                        HtmlElement scriptElement = wb.Document.CreateElement("script");
                        IHTMLScriptElement element = (IHTMLScriptElement)scriptElement.DomElement;
                        element.text = "function fn_GvCrawler() { " + sJavascript + " }";
                        headElement.AppendChild(scriptElement);
                        wb.Document.InvokeScript("fn_GvCrawler");
                        Delay(nMillisecond);
                        sHtml = wb.Document.Body.InnerHtml;
                        break;
                    }
                }
            }

            return sHtml;
        }
#endif
        #endregion

        #region ˽�к���

        /// <summary>
        /// ץȡ��ҳ����
        /// </summary>
        /// <param name="sWebUrl">��ҳ��ַ</param>
        /// <param name="sData">Post����</param>
        /// <param name="sEncoding">ָ������</param>
        /// <param name="bResetSpace">�Ƿ���˶�հ׷�</param>
        /// <param name="cookieContainer">��ҳ����Cookies</param>
        /// <param name="httpStatus">��ҳ����״̬</param>
        /// <returns>��ҳԴ��</returns>
        private static string Crawler(string sWebUrl, string sData, string sEncoding, bool bResetSpace, ref CookieContainer cookieContainer, ref HttpStatusCode httpStatus)
        {
            string sWebHtml = "";

            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (!string.IsNullOrEmpty(sWebUrl))
            {
                try
                {
                    httpRequest = (HttpWebRequest)HttpWebRequest.Create(sWebUrl);
                    httpRequest.CookieContainer = cookieContainer;
                    httpRequest.ContentType = _header.contentType;
                    httpRequest.ServicePoint.ConnectionLimit = _header.maxTry;
                    httpRequest.Referer = sWebUrl;
                    httpRequest.Accept = _header.accept;
                    httpRequest.UserAgent = _header.userAgent;

                    if (!string.IsNullOrEmpty(sData)) // Post
                    {
                        byte[] btDataArray = Encoding.UTF8.GetBytes(sData);
                        httpRequest.Method = "POST";
                        httpRequest.ContentLength = btDataArray.Length;

                        Stream dataStream = null;
                        dataStream = httpRequest.GetRequestStream();
                        dataStream.Write(btDataArray, 0, btDataArray.Length);
                        dataStream.Close();

                        httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    }
                    else    //Get
                    {
                        httpRequest.Method = "GET";
                        httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    }

                    httpStatus = httpResponse.StatusCode;

                    Stream resStream = httpResponse.GetResponseStream();
                    byte[] resByte = StreamToBytes(resStream);
                    sWebHtml = string.IsNullOrEmpty(sEncoding) ?
                        CorrectEncode(resByte) : // У������
                        Encoding.GetEncoding(sEncoding).GetString(resByte);
                    if (bResetSpace)
                        sWebHtml = ResetSpace(sWebHtml);
                    httpResponse.Close();
                }
                catch (Exception ex)
                {
                    //throw ex;
                    Console.WriteLine(ex.Message);
                    return "";
                }
            }
            return sWebHtml;
        }

        /// <summary>
        /// У��Դ���ַ�����
        /// </summary>
        /// <param name="sReader">������</param>
        /// <returns>ת������ַ���</returns>
        private static string CorrectEncode(byte[] btWebHtml)
        {
            string sWebHtml = Encoding.UTF8.GetString(btWebHtml);
            string pattern = @"(?i)\bcharset=(?<charset>[-a-zA-Z_0-9]+)";
            string sCharset = Regex.Match(sWebHtml, pattern).Groups["charset"].Value;
            if (sCharset == "") sCharset = "gbk";
            return Encoding.GetEncoding(sCharset).GetString(btWebHtml);
        }

        /// <summary>
        /// ���˶���ո�
        /// </summary>
        /// <param name="sWebHtml">��ҳԴ��</param>
        /// <returns></returns>
        private static string ResetSpace(string sWebHtml)
        {
            Regex r = new Regex(@"\s+");
            return r.Replace(sWebHtml, " ");
        }

        /// <summary>
        /// ת��StreamΪByte����
        /// </summary>
        /// <param name="stream">Stream����</param>
        /// <returns></returns>
        private static byte[] StreamToBytes(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

#if BROWER
        /// <summary>
        /// �ȴ�
        /// </summary>
        /// <param name="nMillisecond">������</param>
        private static void Delay(int nMillisecond)
        {
            DateTime current = DateTime.Now;
            while (current.AddMilliseconds(nMillisecond) > DateTime.Now)
            {
                Application.DoEvents();
            }
            return;
        }
#endif
        #endregion

    }

    #region �ļ�ͷ
    class HttpHeader
    {
        public string contentType;

        public string accept;

        public string userAgent;

        public string method;

        public int maxTry;

        public HttpHeader()
        {
            accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
            contentType = "application/x-www-form-urlencoded";
            userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            maxTry = 300;
            method = "GET";
        }
    }
    #endregion

}
