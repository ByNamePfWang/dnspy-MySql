using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GwInfoPay.Pay.Util
{
    public class Result
    {
        public String Message;
        public String IsSucceed;
        public JObject returnData;

        public override string ToString()
        {
            return $"IsSucceed:{IsSucceed}***Message:{Message}***returnData:{returnData.ToString()}";
        }
    }
}
