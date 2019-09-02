using Newtonsoft.Json;
using ReportWebAPI.ReportModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Data;
using ReportWebAPI.ReportCommon;
using ReportWebAPI.ReportViewModel;
using ReportWebAPI.ReportChangeModel;
using System.Linq;
using ReportWebAPI.ReportConvert;
using System.Threading.Tasks;

namespace ReportWebAPI
{
    public class ReportAfterHelper
    {
        #region 參數
        private string _reporturi = "http://localhost:14813/api/ReportBefore/";
        public string ReportUri { get { return _reporturi; } set { _reporturi = value; } }

        private int _timeOut = 5;
        public int TimeOut { get { return _timeOut; } set { _timeOut = value; } }
        #endregion

        #region 多型建構
        public ReportAfterHelper(string reportUri, int timeout)
        {
            _reporturi = reportUri;
            _timeOut = timeout;
        }

        public ReportAfterHelper(string reportUri)
        {
            _reporturi = reportUri;
        }
        #endregion

        private int CheckHttpCode(HttpStatusCode httpStatusCode)
        {
            return httpStatusCode == HttpStatusCode.OK ? 1 : -1;
        }

        /// <summary>
        /// 等級報表
        /// </summary>
        /// <param name="afterParameters"></param>
        /// <returns></returns>
        public DataSet GetGradeReport(AfterParameters afterParameters)
        {
            var response = GetReportAsync(afterParameters, "GetGradeReport");
            var dataStr = response.Result.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<List<GradeReport>>(dataStr);
            //轉換前端Model
            List<ReportGroup> reportGroups = ConvertReport.ConvertReportGroupModel(backData);

            DataTable totalDataTable = new DataTable();
            totalDataTable.Columns.Add(new DataColumn("totalCount", typeof(Int32)));
            var row = totalDataTable.NewRow();
            row["totalCount"] = backData.Count;
            totalDataTable.Rows.Add(row);

            //分頁排序&分頁
            reportGroups = reportGroups.OrderBy(o => o.group_name).ToList();
            reportGroups = reportGroups.Skip((afterParameters.PageNum - 1) * afterParameters.PageSize).Take(afterParameters.PageSize).ToList();

            DataSet ds = new DataSet();
            ds.Tables.Add(totalDataTable);
            ds.Tables.Add(reportGroups.ToDataTable<ReportGroup>());

            return ds;
        }

        /// <summary>
        /// 後台終端報表
        /// </summary>
        /// <param name="afterParameters"></param>
        /// <returns></returns>
        public DataSet GetSourceReport(AfterParameters afterParameters)
        {
            var response = GetReportAsync(afterParameters, "GetSourceReport");
            var dataStr = response.Result.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<List<SourceReport>>(dataStr);
            //轉換前端Model
            List<ReportSource> reportSources = ConvertReport.ConvertReportSourceModel(backData);

            DataTable totalDataTable = new DataTable();
            totalDataTable.Columns.Add(new DataColumn("totalCount", typeof(Int32)));
            var row = totalDataTable.NewRow();
            row["totalCount"] = backData.Count;
            totalDataTable.Rows.Add(row);

            //分頁排序&分頁
            reportSources = reportSources.OrderBy(o => o.group_name).ToList();
            reportSources = reportSources.Skip((afterParameters.PageNum - 1) * afterParameters.PageSize).Take(afterParameters.PageSize).ToList();

            DataSet ds = new DataSet();
            ds.Tables.Add(totalDataTable);
            ds.Tables.Add(reportSources.ToDataTable<ReportSource>());

            return ds;
        }

        /// <summary>
        /// 後台彩種報表
        /// </summary>
        /// <param name="afterParameters"></param>
        /// <returns></returns>
        public DataSet GetLotteryReport(AfterParameters afterParameters)
        {
            var response = GetReportAsync(afterParameters, "GetLotteryReport");
            var dataStr = response.Result.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<List<LotteryReport>>(dataStr);
            //轉換前端Model
            List<ReportLottery> reportLottery = ConvertReport.ConvertReportLotteryModel(backData);

            DataTable totalDataTable = new DataTable();
            totalDataTable.Columns.Add(new DataColumn("totalCount", typeof(Int32)));
            var row = totalDataTable.NewRow();
            row["totalCount"] = backData.Count;
            totalDataTable.Rows.Add(row);

            //分頁排序&分頁
            switch (afterParameters.OrderBy)
            {
                case "profits_money desc":
                    reportLottery = reportLottery.OrderByDescending(o => o.ProfitMoney).ToList();
                    break;
                case "profits_money asc":
                    reportLottery = reportLottery.OrderBy(o => o.ProfitMoney).ToList();
                    break;
                case "profits_rates desc":
                    reportLottery = reportLottery.OrderByDescending(o => o.ProfitRate).ToList();
                    break;
                //盈率递增
                case "profits_rates asc":
                    reportLottery = reportLottery.OrderBy(o => o.ProfitRate).ToList();
                    break;
                default:
                    reportLottery = reportLottery.OrderByDescending(o => o.BetMoney).ToList();
                    break;

            }
            reportLottery = reportLottery.Skip((afterParameters.PageNum - 1) * afterParameters.PageSize).Take(afterParameters.PageSize).ToList();

            DataSet ds = new DataSet();
            ds.Tables.Add(totalDataTable);
            ds.Tables.Add(reportLottery.ToDataTable<ReportLottery>());

            return ds;
        }

        /// <summary>
        /// 後台首充報表
        /// </summary>
        /// <param name="afterParameters"></param>
        /// <returns></returns>
        public DataSet GetNewPayReport(AfterParameters afterParameters)
        {
            var response = GetReportAsync(afterParameters, "GetNewPayReport");
            var dataStr = response.Result.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<List<NewPaysReport>>(dataStr);
            //轉換前端Model
            List<ReportNewPays> reportNewPays = ConvertReport.ConvertReportNewPayModel(backData);

            DataTable totalDataTable = new DataTable();
            totalDataTable.Columns.Add(new DataColumn("totalCount", typeof(Int32)));
            var row = totalDataTable.NewRow();
            row["totalCount"] = backData.Count;
            totalDataTable.Rows.Add(row);

            //分頁排序&分頁
            switch (afterParameters.OrderBy)
            {
                case "money desc":
                    reportNewPays = reportNewPays.OrderByDescending(o => o.RechargeMongy).ToList();
                    break;
                default:
                    reportNewPays = reportNewPays.OrderByDescending(o => o.Time).ToList();
                    break;

            }
            reportNewPays = reportNewPays.Skip((afterParameters.PageNum - 1) * afterParameters.PageSize).Take(afterParameters.PageSize).ToList();

            DataSet ds = new DataSet();
            ds.Tables.Add(totalDataTable);
            ds.Tables.Add(reportNewPays.ToDataTable<ReportNewPays>());

            return ds;
        }

        /// <summary>
        /// 後台會員報表
        /// </summary>
        /// <param name="afterParameters"></param>
        /// <returns></returns>
        public DataSet GetMemberReport(AfterParameters afterParameters)
        {
            var response = GetReportAsync(afterParameters, "GetMemberReport");
            var dataStr = response.Result.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<List<MembersReport>>(dataStr);
            //轉換前端Model
            List<ReportMember> reportMembers = ConvertReport.ConvertReportMemberModel(backData);

            DataTable totalDataTable = new DataTable();
            totalDataTable.Columns.Add(new DataColumn("totalCount", typeof(Int32)));
            var row = totalDataTable.NewRow();
            row["totalCount"] = backData.Count;
            totalDataTable.Rows.Add(row);

            //分頁排序&分頁
            switch (afterParameters.OrderBy)
            {
                case "BetMoney desc":
                    reportMembers = reportMembers.OrderByDescending(o => o.BetMoney).ToList();
                    break;
                case "ProfitLoss desc":
                    reportMembers = reportMembers.OrderByDescending(o => o.ProfitLoss).ToList();
                    break;
                case "ProfitLoss asc":
                    reportMembers = reportMembers.OrderBy(o => o.ProfitLoss).ToList();
                    break;
                case "ProfitRate desc":
                    reportMembers = reportMembers.OrderByDescending(o => o.ProfitRate).ToList();
                    break;
                case "ProfitRate asc":
                    reportMembers = reportMembers.OrderBy(o => o.ProfitRate).ToList();
                    break;
                case "InMoney desc":
                    reportMembers = reportMembers.OrderByDescending(o => o.InMoney).ToList();
                    break;
                case "OutMoney desc":
                    reportMembers = reportMembers.OrderByDescending(o => o.OutMoney).ToList();
                    break;
                case "RebateMoney desc":
                    reportMembers = reportMembers.OrderByDescending(o => o.RebateMoney).ToList();
                    break;
                case "DiscountMoney desc":
                    reportMembers = reportMembers.OrderByDescending(o => o.DiscountMoney).ToList();
                    break;
                default:
                    reportMembers = reportMembers.OrderByDescending(o => o.BetMoney).ToList();
                    break;

            }
            reportMembers = reportMembers.Skip((afterParameters.PageNum - 1) * afterParameters.PageSize).Take(afterParameters.PageSize).ToList();

            DataSet ds = new DataSet();
            ds.Tables.Add(totalDataTable);
            ds.Tables.Add(reportMembers.ToDataTable<ReportMember>());

            return ds;
        }

        /// <summary>
        /// 會員報表-會員報表歷史
        /// </summary>
        /// <param name="afterParameters"></param>
        /// <returns></returns>
        public DataSet GetUsersHistory(AfterParameters afterParameters)
        {
            var response = GetReportAsync(afterParameters, "GetUsersHistory");
            var dataStr = response.Result.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<List<UsersHistoryReport>>(dataStr);
            //轉換前端Model
            List<ReportHistory> reportMembers = ConvertReport.ConvertReportHistoryMemberModel(backData);

            DataTable totalDataTable = new DataTable();
            totalDataTable.Columns.Add(new DataColumn("totalCount", typeof(Int32)));
            var row = totalDataTable.NewRow();
            row["totalCount"] = backData.Count;
            totalDataTable.Rows.Add(row);

            DataSet ds = new DataSet();
            ds.Tables.Add(totalDataTable);
            ds.Tables.Add(reportMembers.ToDataTable<ReportHistory>());

            return ds;
        }

        /// <summary>
        /// 後台代理報表
        /// </summary>
        /// <param name="afterParameters"></param>
        /// <returns></returns>
        public DataSet GetAgencyHender(AfterParameters afterParameters)
        {
            var response = GetReportAsync(afterParameters, "GetAgencyHender");
            var dataStr = response.Result.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<List<AgentsReport>>(dataStr);
            //轉換前端Model
            List<ReportAgent> reportAgents = ConvertReport.ConvertReportAgentModel(backData);

            DataTable totalDataTable = new DataTable();
            totalDataTable.Columns.Add(new DataColumn("totalCount", typeof(Int32)));
            var row = totalDataTable.NewRow();
            row["totalCount"] = backData.Count;
            totalDataTable.Rows.Add(row);

            //分頁排序&分頁

            reportAgents = reportAgents.OrderByDescending(o => o.TotalBettingAccount).ToList();



            DataSet ds = new DataSet();
            ds.Tables.Add(totalDataTable);
            ds.Tables.Add(reportAgents.ToDataTable<ReportAgent>());

            return ds;
        }

        /// <summary>
        /// 後台代理報表
        /// </summary>
        /// <param name="afterParameters"></param>
        /// <returns></returns>
        public DataSet GetIntegratedReport(AfterParameters afterParameters)
        {
            var response = GetReportAsync(afterParameters, "GetIntegratedReport");
            var dataStr = response.Result.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<List<IntegratedReport>>(dataStr);
            //轉換前端Model
            List<ReportIntegrated> reportIntegrateds = ConvertReport.ConvertReportIntegratedModel(backData);

            //DataTable totalDataTable = new DataTable();
            //totalDataTable.Columns.Add(new DataColumn("totalCount", typeof(Int32)));
            //var row = totalDataTable.NewRow();
            //row["totalCount"] = backData.Count;
            //totalDataTable.Rows.Add(row);
            DataSet ds = new DataSet();
            if (reportIntegrateds.Count==0)
            {
                reportIntegrateds.Add(new ReportIntegrated());
            }
           
            ds.Tables.Add(reportIntegrateds.ToDataTable<ReportIntegrated>());


            return ds;
        }

        /// <summary>
        /// 平台彩種報表
        /// </summary>
        /// <param name="afterParameters"></param>
        /// <returns></returns>
        public DataSet GetPlatformLotteryReport(AfterParameters afterParameters)
        {
            var response = GetReportAsync(afterParameters, "GetPlatformLotteryReport");
            var dataStr = response.Result.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<List<LotteryReport>>(dataStr);
            //轉換前端Model
            List<ReportLottery> reportLotterys = ConvertReport.ConvertReportLotteryModel(backData);

            DataTable totalDataTable = new DataTable();
            totalDataTable.Columns.Add(new DataColumn("totalCount", typeof(Int32)));
            var row = totalDataTable.NewRow();
            row["totalCount"] = backData.Count;
            totalDataTable.Rows.Add(row);

            //分頁排序&分頁
            switch (afterParameters.OrderBy)
            {
                case "profits_money desc":
                    reportLotterys = reportLotterys.OrderByDescending(o => o.ProfitMoney).ToList();
                    break;
                case "profits_money asc":
                    reportLotterys = reportLotterys.OrderBy(o => o.ProfitMoney).ToList();
                    break;
                case "profits_rates desc":
                    reportLotterys = reportLotterys.OrderByDescending(o => o.ProfitRate).ToList();
                    break;
                //盈率递增
                case "profits_rates asc":
                    reportLotterys = reportLotterys.OrderBy(o => o.ProfitRate).ToList();
                    break;
                default:
                    reportLotterys = reportLotterys.OrderByDescending(o => o.BetMoney).ToList();
                    break;

            }
            reportLotterys = reportLotterys.Skip((afterParameters.PageNum - 1) * afterParameters.PageSize).Take(afterParameters.PageSize).ToList();

            DataSet ds = new DataSet();
            ds.Tables.Add(totalDataTable);
            ds.Tables.Add(reportLotterys.ToDataTable<ReportLottery>());

            return ds;
        }

        /// <summary>
        /// 平台月度報表
        /// </summary>
        /// <param name="afterParameters"></param>
        /// <returns></returns>
        public DataSet GetPlatformMonthReport(AfterParameters afterParameters)
        {
            var response = GetReportAsync(afterParameters, "GetPlatformMonthReport");
            var dataStr = response.Result.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<List<PlatformReport>>(dataStr);
            //轉換前端Model
            List<ReportPlatform> reportLotterys = ConvertReport.ConvertReportPlatformModel(backData);

            DataTable totalDataTable = new DataTable();
            totalDataTable.Columns.Add(new DataColumn("totalCount", typeof(Int32)));
            var row = totalDataTable.NewRow();
            row["totalCount"] = backData.Count;
            totalDataTable.Rows.Add(row);

            reportLotterys = reportLotterys.OrderBy(o => o.identityid).ToList();
            reportLotterys = reportLotterys.Skip((afterParameters.PageNum - 1) * afterParameters.PageSize).Take(afterParameters.PageSize).ToList();

            DataSet ds = new DataSet();
            ds.Tables.Add(totalDataTable);
            ds.Tables.Add(reportLotterys.ToDataTable<ReportPlatform>());

            return ds;
        }

        /// <summary>
        /// 平台站長報表
        /// </summary>
        /// <param name="afterParameters"></param>
        /// <returns></returns>
        public DataSet GetPlatformIdentityIdReport(AfterParameters afterParameters)
        {
            var response = GetReportAsync(afterParameters, "GetPlatformTenantReport");
            var dataStr = response.Result.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<List<PlatformReport>>(dataStr);
            //轉換前端Model
            List<ReportPlatform> reportLotterys = ConvertReport.ConvertReportPlatformModel(backData);

            DataTable totalDataTable = new DataTable();
            totalDataTable.Columns.Add(new DataColumn("totalCount", typeof(Int32)));
            var row = totalDataTable.NewRow();
            row["totalCount"] = backData.Count;
            totalDataTable.Rows.Add(row);

            reportLotterys = reportLotterys.OrderBy(o => o.identityid).ToList();
            reportLotterys = reportLotterys.Skip((afterParameters.PageNum - 1) * afterParameters.PageSize).Take(afterParameters.PageSize).ToList();

            DataSet ds = new DataSet();
            ds.Tables.Add(totalDataTable);
            ds.Tables.Add(reportLotterys.ToDataTable<ReportPlatform>());

            return ds;
        }

        /// <summary>
        /// 平台綜合報表
        /// </summary>
        /// <param name="afterParameters"></param>
        /// <returns></returns>
        public DataSet GetPlatformReport(AfterParameters afterParameters)
        {
            var response = GetReportAsync(afterParameters, "GetPlatformIntegratedReport");
            var dataStr = response.Result.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<List<PlatformReport>>(dataStr);
            //轉換前端Model
            List<ReportPlatform> reportLotterys = ConvertReport.ConvertReportPlatformModel(backData);

            DataTable totalDataTable = new DataTable();
            totalDataTable.Columns.Add(new DataColumn("totalCount", typeof(Int32)));
            var row = totalDataTable.NewRow();
            row["totalCount"] = backData.Count;
            totalDataTable.Rows.Add(row);

            reportLotterys = reportLotterys.OrderBy(o => o.identityid).ToList();
            reportLotterys = reportLotterys.Skip((afterParameters.PageNum - 1) * afterParameters.PageSize).Take(afterParameters.PageSize).ToList();

            DataSet ds = new DataSet();
            ds.Tables.Add(totalDataTable);
            ds.Tables.Add(reportLotterys.ToDataTable<ReportPlatform>());

            return ds;
        }

        /// <summary>
        /// 等級報表
        /// </summary>
        /// <param name="afterParameters"></param>
        /// <returns></returns>
        public string GetUserProfit(AfterParameters afterParameters)
        {
            var response = GetReportAsync(afterParameters, "GetUserProfit");
            var dataStr = response.Result.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<string>(dataStr);
          
            return backData;
        }

        /// <summary>
        ///  直播後台-主播報表
        /// </summary>
        /// <param name="afterParameters"></param>
        /// <returns></returns>
        public return_anchor_tip_record GetAnchorsReport(LiveParameters liveParameters)
        {
            var response = GetLiveReport(liveParameters, "GetAnchorsReport");
            var dataStr = response.Result.Content.ReadAsStringAsync().Result;
            var backData = JsonConvert.DeserializeObject<AnchorListReport>(dataStr);
            //轉換前端Model
            return_anchor_tip_record reportLotterys = ConvertReport.ConvertReportAnchorsModel(backData);

            return reportLotterys;
        }

        /// <summary>
        /// 訪問WebAPI
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> GetLiveReport(LiveParameters postData, string action)
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
                    // ReportUri = "http://localhost:19487/api/ReportAfter/";
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

        /// <summary>
        /// 訪問WebAPI
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> GetReportAsync(AfterParameters postData, string action)
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
                    // ReportUri = "http://localhost:19487/api/ReportAfter/";
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
