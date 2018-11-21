using System;
using System.IO;
using System.Net;
using System.Text;

namespace AssistTool
{
    public class HttpHelper
    {
        public static string Post(string url, string data)
        {
            CookieContainer cookies = new CookieContainer();
            return Post(url,data,null, ECookie.none,ref cookies);
        }
        public static string Post(string url, string data, ECookie ecookie, ref CookieContainer cookie)
        {
            return Post(url, data, null, ecookie, ref cookie);
        }
        public static string Post(string url,string data, Header header, ECookie ecookie,ref CookieContainer cookie)
        {
            string result = null;
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] dataByte = encoding.GetBytes(data);
            //向服务端请求
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "POST";
            if (header != null)
            {
                if (!string.IsNullOrWhiteSpace(header.ContentType))
                {
                    myRequest.ContentType = header.ContentType;//"application/x-www-form-urlencoded";
                }
                else
                {
                    myRequest.ContentType = "text/html; charset=UTF-8";//"application/x-www-form-urlencoded";
                }
                if (!string.IsNullOrWhiteSpace(header.Accept))
                {
                    myRequest.Accept = header.Accept;//"application/json, text/javascript, */*; q=0.01";
                }
            }
            myRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0";
            myRequest.ContentLength = data.Length;
            myRequest.CookieContainer = cookie == null ? new CookieContainer() : cookie;
            Stream newStream = myRequest.GetRequestStream();
            newStream.Write(dataByte, 0, data.Length);
            newStream.Close();
            //将请求的结果发送给客户端(界面、应用)
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            if (ecookie == ECookie.get)
            {
                cookie.Add(myResponse.Cookies);
            }
            else if (ecookie == ECookie.use)
            {
                myResponse.Cookies = cookie.GetCookies(new Uri(url));
            }
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            result = reader.ReadToEnd();
            if (reader != null)
            {
                reader.Close();
                reader.Close();
            }
            return result;
        }
        //private static string PostLogin(string postData, string requestUrlString, ref CookieContainer cookie)
        //{
        //    ASCIIEncoding encoding = new ASCIIEncoding();
        //    byte[] data = encoding.GetBytes(postData);
        //    //向服务端请求
        //    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(requestUrlString);
        //    myRequest.Method = "POST";
        //    myRequest.ContentType = "application/x-www-form-urlencoded";
        //    myRequest.Accept = "application/json, text/javascript, */*; q=0.01";
        //    myRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0";
        //    myRequest.ContentLength = data.Length;
        //    myRequest.CookieContainer = new CookieContainer();
        //    Stream newStream = myRequest.GetRequestStream();
        //    newStream.Write(data, 0, data.Length);
        //    newStream.Close();
        //    //将请求的结果发送给客户端(界面、应用)
        //    HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
        //    cookie.Add(myResponse.Cookies);
        //    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
        //    return reader.ReadToEnd();
        //}
        public class Header
        {
            public string ContentType { get; set; }
            public string Accept { get; set; }
        }
        public enum ECookie
        {
            none,
            use,
            get
        }
    }
}
