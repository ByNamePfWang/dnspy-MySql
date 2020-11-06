using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HeatMeterPrePaySelfHelp.Util
{
    class ApiHelper
    {

        private static string getPrefix()
        {
            string Domain = "0.0.0.0";
            int Port = 80;
            String sd = System.Configuration.ConfigurationManager.AppSettings["Domain"];
            if (null != sd && "" != sd)
            {
                Domain = sd;
            }
            String sp = System.Configuration.ConfigurationManager.AppSettings["port"];
            if (null != sp && "" != sp)
            {
                Port = Convert.ToInt32(sp);
            }
            return $"http://{Domain}:{Port}/";
        }

        public static string HttpPost(string Url, string postDataStr)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getPrefix() + Url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            //request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        public static string HttpGet(string Url, string p)
        {
            return ApiHelper.HttpGet(Url, null, p);
        }

        public static string HttpGet(string Url, string s, string p)
        {
            String httpUrl = getPrefix() + Url + "?p=" + p;
            if (null != s)
            {
                httpUrl = httpUrl + "&s=" + s;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(httpUrl);
            request.Method = "GET";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;

        }

        public static JObject str2Data(string str)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(str);
            string dr = jo.GetValue("Data").ToString();
            if (dr == "")
                return null;
            return (JObject)jo.GetValue("Data");
        }

        /**
         * JObject 为 json 格式的DataRow数据
         * 
         * keyType true 返回的key为index  false返回的为列名
         * 
         * */
        public static JObject GetHttpData(string prefix, string route, string sql, String p)
        {
            if (null == prefix)
            {
                prefix = "query";
            }
            string dataStr = ApiHelper.HttpGet($"api/{prefix}/{route}", sql, p);
            return ApiHelper.str2Data(dataStr);
        }

        /**
         * JObject 为 json 格式的DataRow数据
         * 
         * keyType true 返回的key为index  false返回的为列名
         * 
         * */
        public static JObject GetHttpData(string route, string sql, String p)
        {
            return ApiHelper.GetHttpData(null, route, sql, p);
        }

        public static JObject GetHttpData(string route, string sql)
        {
            return ApiHelper.GetHttpData(null, route, sql, null);
        }
    }
}
