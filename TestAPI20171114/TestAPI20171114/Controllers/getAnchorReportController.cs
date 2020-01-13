using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Models;
using TestAPI20171114.Common;
using TestAPI20171114.LiveAfterWebService;
using ReportWebAPI;
using TestAPI20171114.Models.Request;

namespace TestAPI20171114.Controllers
{
    public class GetAnchorReportController : ApiController
    {
        [HttpPost]
        public ResultInfoT<object> Post()
        {
            var result = new ResultInfoT<object>() { IsLogin = ResultHelper.IsLogin };
            var request = HttpContext.Current.Request;
            var session = HttpContext.Current.Session;

            try
            {
                int managerId = (int)(session["ManagerId"] ?? -1);

                if (managerId < 0)
                {
                    // 缺少Log紀錄
                    result.Code = ResultHelper.NotAuthorized;
                    result.StrCode = ResultHelper.NotLoginMsg;
                    result.IsLogin = ResultHelper.NotLogin;
                    return result;
                }

                int anchorId = int.TryParse(request.Form["AnchorID"] ?? "", out anchorId) ? anchorId : -1;

                int index = request.Form["PageIndex"] != null ? Convert.ToInt32(request.Form["PageIndex"].TrimEnd()) : 0;
                int size = request.Form["PageSize"] != null ? Convert.ToInt32(request.Form["PageSize"].TrimEnd()) : 0;

                if (index < 0 && (size <= 0 || size > 1000))
                {
                    result.IsLogin = ResultHelper.IsLogin;
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;

                    return result;
                }


                using (var db = new livecloudEntities())
                {
                    if (anchorId > 0 && db.dt_dealer.Find(anchorId) == null)
                    {
                        result.IsLogin = ResultHelper.IsLogin;
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = "找不到此主播信息！";

                        return result;
                    }
                    ReportAfterHelper reportAfterHelper = new ReportAfterHelper(Conf.ReportUri, 3);
                    ReportWebAPI.ReportModel.LiveParameters liveParameters = new ReportWebAPI.ReportModel.LiveParameters();
                    if (anchorId > 0)
                        liveParameters.AnchorId = anchorId;

                    //重構開啟這兩行
                    var newReport = TestAPI20171114.Common.LiveReport.GetAnchorReport<AnchorReport>();
                    var newReportResult = newReport.data.result.ConvertAnchor<dt_anchor_tip_record>();


                    //var reportResult = reportAfterHelper.GetAnchorsReport(liveParameters); //webService.getAnchorReport
                    //var reportResult2 = webService.getAnchorReport((anchorId > 0) ? anchorId.ToString() : "", size, index);

                    //if (reportResult.code != 1)
                    //{
                    //    result.Code = ResultHelper.ParamFail;
                    //    result.StrCode = "報表出錯";
                    //    result.IsLogin = ResultHelper.NotLogin;
                    //    return result;
                    //}
                    var anchorList = anchorId > 0 ? newReportResult.Where(O => O.anchor_id == anchorId).ToList() : newReportResult.ToList();

                    var totalCount = anchorList.Count;   //重構開啟 newReportResult.Count;


                    var anchorNameList = (anchorId > 0)
                        ? db.dt_dealer.Where(a => a.id == anchorId).Select(a => new { Id = a.id, Name = a.dealerName }).ToList()
                        : db.dt_dealer.Select(a => new { Id = a.id, Name = a.dealerName }).ToList();

                    var list = anchorList.OrderBy(o => o.anchor_id).Select(s => new
                    //重構開啟   
                    //newReportResult.OrderBy(o => o.anchor_id).Select(s => new
                    {
                        ID = s.anchor_id,
                        AnchorID = s.anchor_id,
                        Name = (anchorNameList.Where(a => a.Id == s.anchor_id).FirstOrDefault() != null)
                          ? anchorNameList.Where(a => a.Id == s.anchor_id).FirstOrDefault().Name
                          : "",
                        GiftBonus = s.tipmoney_today,
                        NowBonus = s.tipmoney_thismonth,
                        PrevBonus = s.tipmoney_lastmonth,
                        AllBonus = s.tipmoney_total
                    }).ToList();

                    result.BackData = list;
                    result.DataCount = totalCount;

                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetAnchorReport", "GetAnchorReport", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
