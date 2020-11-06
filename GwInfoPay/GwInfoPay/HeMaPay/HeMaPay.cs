using GwInfoPay.Pay.util;
using GwInfoPay.Pay.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace GwInfoPay.Pay.HeMaPay
{
    public class HeMaPay
    {
        private static readonly log4net.ILog log4 = log4net.LogManager.GetLogger("Log");

        private static string api_domain;
        private static string AppId;
        private static string SubAppId;
        private static string PrivateKey;
        private static string StoreId;
        private static string SignType;
        private static string Format;
        private static string Version;
        private static string notify_url;

        /// <summary>
        /// 预支付接口
        /// </summary>
        private static string action_percreate = "trade.percreate";
        /// <summary>
        /// 查询接口
        /// </summary>
        private static string action_query = "trade.query";
        /// <summary>
        /// 退款接口
        /// </summary>
        private static string action_refund = "trade.refund";
        /// <summary>
        /// 退款查询接口
        /// </summary>
        private static string action_refund_query = "trade.refund.query";
        /// <summary>
        /// 撤销订单
        /// </summary>
        private static string action_cancel = "trade.cancel";
        /// <summary>
        /// 关闭订单
        /// </summary>
        private static string action_close = "trade.close";

        private static void getConfig()
        {
            if(api_domain == null)
                api_domain = ConfigAppSettings.GetValue("ApiDomain");
            if (AppId == null)
                AppId = ConfigAppSettings.GetValue("AppId");
            if (SubAppId == null)
                SubAppId = ConfigAppSettings.GetValue("SubAppId");
            if (PrivateKey == null)
                PrivateKey = ConfigAppSettings.GetValue("PrivateKey");
            if (StoreId == null)
                StoreId = ConfigAppSettings.GetValue("StoreId");
            if (SignType == null)
                SignType = ConfigAppSettings.GetValue("SignType");
            if (Format == null)
                Format = ConfigAppSettings.GetValue("Format");
            if (Version == null)
                Version = ConfigAppSettings.GetValue("Version");
            if (notify_url == null)
                notify_url = ConfigAppSettings.GetValue("notify_url");
        }

        /// <summary>
        /// 线下二维码预下单  用于C扫B业务预下单
        /// </summary>        
        /// <param name="payWay">支付类型</param>
        /// <param name="out_order_no">商户订单号，不可重复</param>
        /// <param name="total_amount">订单总金额</param>
        /// <param name="body">交易或商品描述</param>
        /// <param name="create_time">订单创建时间 yyyyMMddHHmmss</param>
        /// <returns>过滤后的参数组</returns>
        public static string Percreate(PayWay payWay, string out_order_no, string total_amount, string body, string create_time)
        {
            string ip = GetLocalIp();
            if(ip == null)
            {
                ip = "127.0.0.1";
            }
            return HeMaPay.Percreate(payWay, out_order_no, total_amount, body, create_time, ip);
        }

        public static string GetLocalIp()
        {
            ///获取本地的IP地址
            string AddressIP = null;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }
        
        //private static string ExpireTime(string create_time)
        //{
        //    if (string.IsNullOrWhiteSpace(create_time))
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        DateTime dt = DateTime.ParseExact(create_time, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
        //        DateTime et = dt.AddSeconds(pay_time_out);
        //        return et.ToString("yyyyMMddHHmmss");
        //    }
        //}

        /// <summary>
        /// 线下二维码预下单  用于C扫B业务预下单
        /// </summary>        
        /// <param name="payWay">支付类型</param>
        /// <param name="out_order_no">商户订单号，不可重复</param>
        /// <param name="total_amount">订单总金额</param>
        /// <param name="body">交易或商品描述</param>
        /// <param name="create_time">订单创建时间 yyyyMMddHHmmss</param>
        /// <param name="create_ip">订单创建IP地址	192.168.0.1</param>
        /// <returns>过滤后的参数组</returns>
        public static string Percreate(PayWay payWay, string out_order_no, string total_amount, string body, string create_time, string create_ip)
        {
            getConfig();

            Result result = new Result();
            SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
            SortedDictionary<string, string> dictComm = new SortedDictionary<string, string>();
            // 业务参数
            dict.Add("pay_way", payWay.ToString());
            dict.Add("body", body);
            dict.Add("total_amount", total_amount);
            dict.Add("create_time", create_time);
            dict.Add("notify_url", notify_url);
            dict.Add("out_order_no", out_order_no);
            dict.Add("store_id", StoreId);
            dict.Add("create_ip", create_ip);
            // expire_time 该参数支付宝精确到分钟,微信建议最少一分钟           
            // dict.Add("expire_time", ExpireTime(create_time));
            // 公共参数封装
            CommParam(action_percreate, dictComm);
            string param = PackageParam(dict, dictComm);
            log4.Info("下单：\r\n" + param);
            string returnResult = HttpUtil.HttpPost(api_domain, param);
            log4.Info("下单返回值：\r\n" + returnResult);
            
            //if (!string.IsNullOrWhiteSpace(returnResult))
            //{
            //    JObject retPayObJ = JObject.Parse(returnResult);
            //    result.returnData = retPayObJ;
            //    if (!retPayObJ["code"].ToString().Equals("200"))
            //    {
            //        result.IsSucceed = "false";
            //        result.Message = "下单失败！";
            //        return result;
            //    }
            //}
            //result.IsSucceed = "true";
            return returnResult;
        }

        public static string VerifyNetwork(String returnData)
        {
            if (string.IsNullOrWhiteSpace(returnData))
            {
                return "网络异常，稍后再试！";
            }else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="out_order_no">订单号</param>
        /// <param name="create_time">时间</param>
        /// <returns></returns>
        public static string Query(string out_order_no, string create_time)
        {
            getConfig();
            Result result = new Result();
            SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
            dict.Add("out_order_no", out_order_no);
            dict.Add("order_create_time", create_time);
            SortedDictionary<string, string> dictComm = new SortedDictionary<string, string>();
            CommParam(action_query, dictComm);
            string param = PackageParam(dict, dictComm);
            log4.Info("查询：\r\n" + param);
            string queryResult = HttpUtil.HttpPost(api_domain, param);
            log4.Info("查询返回值：\r\n" + queryResult);
            //if (string.IsNullOrWhiteSpace(queryResult))
            //{
            //    result.IsSucceed = "false";
            //    result.Message = "网络异常，稍后查询！";
            //    return result;
            //}
            //JObject retQueryObj = JObject.Parse(queryResult);
            //result.returnData = retQueryObj;
            //JObject retQueryDetailObj = JObject.Parse(retQueryObj["data"].ToString());
            //if (retQueryObj["code"].ToString().Equals("200") && (retQueryDetailObj["sub_code"].ToString().Equals("WAITING_PAYMENT") || retQueryDetailObj["sub_code"].ToString().Equals("CREATED")))
            //{
            //    result.IsSucceed = "false";
            //    result.Message = "用户支付中，请稍后！";
            //}
            //else if (retQueryObj["code"].ToString().Equals("200") && retQueryDetailObj["sub_code"].ToString().Equals("SUCCESS"))
            //{
            //    result.IsSucceed = "true";
            //    log4.Info("订单：" + out_order_no + "查询结果为成功\r\n" + queryResult);
            //}
            //else
            //{
            //    result.IsSucceed = "false";
            //    result.Message = "支付失败,请返回购买界面重新购买!";
            //    log4.Info("订单：" + out_order_no + "查询结果为失败\r\n" + queryResult);
            //}
            return queryResult;
        }

        /// <summary>
        /// 取消订单 订单未支付，发起关单，订单已支付，发起全额退款
        /// </summary>
        /// <param name="out_order_no">订单号必填</param>
        /// <param name="plat_trx_no">平台订单号</param>
        /// <returns></returns>
        public static string Close(string out_order_no, string plat_trx_no = "")
        {
            getConfig();
            Result result = new Result();
            SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
            dict.Add("out_order_no", out_order_no);
            dict.Add("plat_trx_no", plat_trx_no);

            SortedDictionary<string, string> dictComm = new SortedDictionary<string, string>();
            CommParam(action_close, dictComm);
            string param = PackageParam(dict, dictComm);
            log4.Info("订单关闭请求：\r\n" + param);
            string returnResult = HttpUtil.HttpPost(api_domain, param);
            log4.Info("订单关闭返回值：\r\n" + returnResult);
            return returnResult;
        }

        /// <summary>
        /// 取消订单 订单未支付，发起关单，订单已支付，发起全额退款
        /// </summary>
        /// <param name="out_order_no">订单号必填</param>
        /// <returns></returns>
        public static string Cancel(string out_order_no, string plat_trx_no = "")
        {
            getConfig();
            Result result = new Result();
            SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
            dict.Add("out_order_no", out_order_no);
            dict.Add("plat_trx_no", plat_trx_no);

            SortedDictionary<string, string> dictComm = new SortedDictionary<string, string>();
            CommParam(action_cancel, dictComm);
            string param = PackageParam(dict, dictComm);
            log4.Info("订单撤销请求：\r\n" + param);
            string returnResult = HttpUtil.HttpPost(api_domain, param);
            log4.Info("订单撤销返回值：\r\n" + returnResult);
            //if (string.IsNullOrWhiteSpace(returnResult))
            //{
            //    result.IsSucceed = "false";
            //    result.Message = "网络异常，请核实是否取消成功！";
            //    log4.Info("订单：" + out_order_no + "取消时网络异常\r\n");
            //    return result;
            //}
            //JObject retQueryObj = JObject.Parse(returnResult);
            //if (retQueryObj["code"].ToString().Equals("200"))
            //{
            //    result.returnData = retQueryObj;
            //    JObject retQueryDetailObj = JObject.Parse(retQueryObj["data"].ToString());
            //    if (retQueryObj["code"].ToString().Equals("200") && retQueryDetailObj["sub_code"].ToString().Equals("CANCEL_SUCCESS"))
            //    {
            //        result.IsSucceed = "true";
            //        log4.Info("订单：" + out_order_no + "取消成功\r\n" + returnResult);
            //    }
            //    else
            //    {
            //        result.Message = retQueryDetailObj["sub_msg"].ToString();
            //        log4.Info(returnResult);
            //    }
            //}else
            //{
            //    result.IsSucceed = "false";
            //    result.returnData = retQueryObj;
            //}
            return returnResult;
        }

        /// <summary>
        /// 退款查询
        /// </summary>
        /// <param name="out_order_no">订单号必填</param>
        /// <param name="refund_request_no">退款号</param>
        /// <returns></returns>
        public static string RefundQuery(string out_order_no, string refund_request_no)
        {
            getConfig();
            Result result = new Result();
            SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
            dict.Add("out_order_no", out_order_no);
            dict.Add("refund_request_no", refund_request_no);

            SortedDictionary<string, string> dictComm = new SortedDictionary<string, string>();
            CommParam(action_refund_query, dictComm);
            string param = PackageParam(dict, dictComm);
            log4.Info("退款查询请求：\r\n" + param);
            string returnResult = HttpUtil.HttpPost(api_domain, param);
            log4.Info("退款查询请求返回值：\r\n" + returnResult);
            return returnResult;
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="out_order_no">订单号必填</param>
        /// <param name="refund_request_no">退款号</param>
        /// <param name="refund_fee">退款金额</param>
        /// <param name="order_create_time">否，当仅上送out_order_no需上送，若不上送，默认当天</param>
        /// <returns></returns>
        public static string Refund(string out_order_no, string refund_request_no, double refund_fee, string order_create_time)
        {
            getConfig();
            Result result = new Result();
            SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
            dict.Add("out_order_no", out_order_no);
            dict.Add("refund_request_no", refund_request_no);
            dict.Add("order_create_time", order_create_time);
            dict.Add("refund_amount", refund_fee.ToString());

            SortedDictionary<string, string> dictComm = new SortedDictionary<string, string>();
            CommParam(action_refund, dictComm);
            string param = PackageParam(dict, dictComm);
            log4.Info("退款请求：\r\n" + param);
            string returnResult = HttpUtil.HttpPost(api_domain, param);
            log4.Info("退款请求返回值：\r\n" + returnResult);
            //if (string.IsNullOrWhiteSpace(returnResult))
            //{
            //    result.IsSucceed = "false";
            //    result.Message = "网络异常，请核实是否退款成功！";
            //    log4.Info("订单：" + out_order_no + "关闭时网络异常\r\n");
            //    return result;
            //}
            //JObject retQueryObj = JObject.Parse(returnResult);
            //result.returnData = retQueryObj;
            //JObject retQueryDetailObj = JObject.Parse(retQueryObj["data"].ToString());
            //if (retQueryObj["code"].ToString().Equals("200") && retQueryDetailObj["sub_code"].ToString().Equals("REFUND_SUCCESS"))
            //{
            //    result.IsSucceed = "true";
            //    log4.Info("订单：" + out_order_no + "退款成功\r\n" + returnResult);
            //}
            //else
            //{
            //    result.Message = retQueryObj["result_message"].ToString();
            //    log4.Info("退款失败" + returnResult);
            //}
            return returnResult;
        }

        private static string PackageParam(SortedDictionary<string, string> dict, SortedDictionary<string, string> dictComm)
        {
            Dictionary<string, string> dictCommParma;
            string sign;
            // 业务参数排序
            Dictionary<string, string> dictParma = FilterPara(dict);
            string biz_content = JObject.FromObject(dictParma).ToString();
            dictComm.Add("biz_content", biz_content);
            // 业务参数排序
            dictCommParma = FilterPara(dictComm);
            sign = GetSign(CreateLinkString(dictCommParma), PrivateKey);
            dictCommParma.Add("sign", sign);
            string param = JObject.FromObject(dictCommParma).ToString();
            return param;
        }


        public static string GuidToLongID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0).ToString();
        }

        private static void CommParam(string action, SortedDictionary<string, string> dictComm)
        {
            string time = DateTime.Now.ToLocalTime().ToString();
            string timestamp = DateTime.Parse(time).ToString("yyyy-MM-dd HH:mm:ss");

            dictComm.Add("app_id", AppId);
            dictComm.Add("sub_app_id", SubAppId);
            dictComm.Add("method", action);
            dictComm.Add("nonce", GuidToLongID());
            dictComm.Add("sign_type", SignType);
            dictComm.Add("timestamp", timestamp);
            dictComm.Add("version", Version);
            dictComm.Add("format", Format);
        }

        #region 签名
        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        private static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key.ToLower() != "sign" && temp.Value != "" && temp.Value != null)
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        private static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>拼接完成以后的字符串</returns>
        private static string CreateLinkStringUrlencode(Dictionary<string, string> dicArray, Encoding code)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + HttpUtility.UrlEncode(temp.Value, code) + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <returns></returns>
        private static string GetSign(string str, string privateKey)
        {
            return RSAFromPkcs8.Sign(str, privateKey, "UTF-8");
        }
        #endregion
    }

    class HttpUtil
    {
        public static readonly log4net.ILog log4 = log4net.LogManager.GetLogger("Log");

        /// <summary>
        /// POST数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="body">参数</param>
        /// <returns></returns>
        public static string HttpPost(string url, string body)
        {
            string lcHtml = "";
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                //发送数据
                HttpWebRequest request = null;
                byte[] byte1 = Encoding.UTF8.GetBytes(body);

                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                }
                else
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                }
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = byte1.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(byte1, 0, byte1.Length);
                stream.Close();

                request.Timeout = 10000;
                //发送成功后接收返回的json信息
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Encoding enc = Encoding.GetEncoding("UTF-8");
                Stream newStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(newStream, enc);
                lcHtml = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                log4.Error(ex);
            }
            return lcHtml;
        }
    }

    public enum PayWay
    {
        WECHAT,
        ALIPAY
    }
}
