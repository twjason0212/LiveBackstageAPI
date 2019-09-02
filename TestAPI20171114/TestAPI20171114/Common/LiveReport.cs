using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using TestAPI20171114.Models;

namespace TestAPI20171114.Common
{
    public static class LiveReport
    {
        public static string ReportUri = System.Configuration.ConfigurationManager.AppSettings["NewReportUri"] ?? "http://3.0.64.219:8001/v1/report/anchorReport";

        public static NewResultInfoT<T> GetAnchorReport<T>()
        {
            var newReport = GetReport();
            return JsonConvert.DeserializeObject<NewResultInfoT<T>>(newReport);
        }

        //測試接口
        public static string GetReport()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //Timeout時間5秒
                    client.Timeout = new System.TimeSpan(0, 0, 1);
                    // 指定 authorization header
                    client.DefaultRequestHeaders.Add("authorization", "token {api token}");
                    // 準備寫入的 data
                    //BeforeParameters postData = new BeforeParameters() { IdentityId = "test", UserId = "686026" };
                    // 將 data 轉為 json
                    string json = JsonConvert.SerializeObject("");
                    // 將轉為 string 的 json 依編碼並指定 content type 存為 httpcontent
                    HttpContent contentPost = new StringContent(json, Encoding.UTF8, "application/json");
                    // 發出 get 並取得結果
                    HttpResponseMessage response = client.GetAsync(ReportUri).Result;
                    // 將回應結果內容取出並轉為 string 再透過 linqpad 輸出
                    return response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                return "";
            }

        }
    }
}