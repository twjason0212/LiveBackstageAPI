using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ReportWebAPI.ReportModel;
using ReportWebAPI.ReportViewModel;
using System.Net;

namespace ReportWebAPI
{
    public class ReportHelper
    {
        #region 參數
        private string _reporturi = "http://localhost:14813/api/ReportBefore/";
        public string ReportUri { get { return _reporturi; } set { _reporturi = value; } }

        private int _timeOut = 5;
        public int TimeOut { get { return _timeOut; } set { _timeOut = value; } }
        #endregion

        #region 多型建構
        public ReportHelper(string reportUri, int timeout)
        {
            _reporturi = reportUri;
            _timeOut = timeout;
        }

        public ReportHelper(string reportUri)
        {
            _reporturi = reportUri;
        }
        #endregion

        private int CheckHttpCode(HttpStatusCode httpStatusCode)
        {
            return httpStatusCode == HttpStatusCode.OK ? 1 : -1;
        }

        /// <summary>
        /// 代理查詢
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string GetAgentHeader(BeforeParameters postData)
        {
            HttpResponseMessage response = GetReport(postData, "GetAgencyHender");
            string dataStr = response.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<AgencyHeader>(dataStr);

            ResultInfoT<AgencyHeader> result = new ResultInfoT<AgencyHeader>();
            result.BackData = backData;
            result.Code = CheckHttpCode(response.StatusCode);
            result.DataCount = 1;

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 前台下級報表
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string GetAgentReport(BeforeParameters postData)
        {
            HttpResponseMessage response = GetReport(postData, "GetAgentReport");
            string dataStr = response.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<List<AgentReport>>(dataStr);

            ResultInfoT<List<AgentReport>> result = new ResultInfoT<List<AgentReport>>();
            result.Code = CheckHttpCode(response.StatusCode);
            result.DataCount = backData.Count;
            result.BackData = backData;

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 前台今日盈虧
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string GetProfitLossData(BeforeParameters postData)
        {
            HttpResponseMessage response = GetReport(postData, "GetProfitLossData");
            var dataStr = response.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<ProfitLossData>(dataStr);
           
            ResultInfoT<ProfitLossData> result = new ResultInfoT<ProfitLossData>();
            result.DataCount = 1;
            result.BackData = backData;
            result.Code = CheckHttpCode(response.StatusCode);

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 會員歷史中獎總額
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string GetAwardMoneyHistory(BeforeParameters postData)
        {
            HttpResponseMessage response = GetReport(postData, "GetAwardMoneyHistory");
            var dataStr = response.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<AwardMoneyData>(dataStr);

            ResultInfoT<AwardMoneyData> result = new ResultInfoT<AwardMoneyData>();
            result.DataCount = 1;
            result.Code = CheckHttpCode(response.StatusCode);
            result.BackData = backData;

            return JsonConvert.SerializeObject(result);
        }


        /// <summary>
        /// 會員歷史充值總額(成長值)
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string GetRechargeMoneyHistory(BeforeParameters postData)
        {
            HttpResponseMessage response = GetReport(postData, "GetRechargeMoneyHistory");
            var dataStr = response.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<RechargeMoneyHistory>(dataStr);

            ResultInfoT<RechargeMoneyHistory> result = new ResultInfoT<RechargeMoneyHistory>();
            result.DataCount = 1;
            result.Code = CheckHttpCode(response.StatusCode);
            result.BackData = backData;

            return backData.RechargeMoney.ToString();
        }

        /// <summary>
        /// 前台出入款到帳時間
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string GetDealTime(BeforeParameters postData)
        {
            postData.Date = Convert.ToDateTime("2018-08-12");
            HttpResponseMessage response = GetReport(postData, "GetDealTime");
            var dataStr = response.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<List<DealTime>>(dataStr);

            ResultInfoT<List<DealTime>> result = new ResultInfoT<List<DealTime>>();
            result.DataCount = backData.Count;
            result.Code = CheckHttpCode(response.StatusCode);
            result.BackData = backData;

            return JsonConvert.SerializeObject(result);

        }

        /// <summary>
        /// 昨日盈利榜Top10
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string GetTopTenProfitParameters(BeforeParameters postData)
        {
            HttpResponseMessage response = GetReport(postData, "GetTopTenProfitParameters");
            var dataStr = response.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<List<WinMoneyInTop>>(dataStr);

            ResultInfoT<List<WinMoneyInTop>> result = new ResultInfoT<List<WinMoneyInTop>>();
            result.DataCount = backData.Count;
            result.Code = CheckHttpCode(response.StatusCode);
            result.BackData = backData;

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 訪問WebAPI
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private HttpResponseMessage GetReport(BeforeParameters postData, string action)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //Timeout時間5秒
                    client.Timeout = new System.TimeSpan(0, 0, TimeOut);
                    // 指定 authorization header
                    client.DefaultRequestHeaders.Add("authorization", "token {api token}");
                    // 準備寫入的 data
                    //BeforeParameters postData = new BeforeParameters() { IdentityId = "test", UserId = "686026" };
                    // 將 data 轉為 json
                    string json = JsonConvert.SerializeObject(postData);
                    // 將轉為 string 的 json 依編碼並指定 content type 存為 httpcontent
                    HttpContent contentPost = new StringContent(json, Encoding.UTF8, "application/json");
                    // 發出 post 並取得結果
                    HttpResponseMessage response = client.PostAsync(ReportUri + action, contentPost).Result;
                    // 將回應結果內容取出並轉為 string 再透過 linqpad 輸出
                    //result = response.Content.ReadAsStringAsync().Result;
                    //var result = JsonConvert.DeserializeObject<AgencyHeader>(sss.Result);
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

        }

    }


}
