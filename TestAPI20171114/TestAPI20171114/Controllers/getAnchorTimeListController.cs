using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Models;
using TestAPI20171114.Common;

namespace TestAPI20171114.Controllers
{

    public class GetAnchorTimeListController : ApiController
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
                var name = request.Form["Name"] ?? "";
                int state = int.TryParse(request.Form["State"], out state) ? state : -1; //1在播 | 0下播
                int pageSize = int.TryParse(request.Form["PageSize"], out pageSize) ? pageSize : 20;
                int pageIndex = int.TryParse(request.Form["PageIndex"], out pageIndex) ? pageIndex : 0;

                using (var db = new livecloudEntities())
                {

                    int year = DateTime.Now.Year;
                    int thisMonth = DateTime.Now.Month;
                    int lastMonth = DateTime.Now.AddMonths(-1).Month;

                    var reportThisMonthList = db.AnchorsWorkTotal
                        .Where(s => s.Year == year && s.Month == thisMonth)
                        .Select(s => new
                        {
                            s.AnchorId,
                            s.Year,
                            s.Month,
                            s.WorkSecond
                        }).ToList();

                    var reportLastMonthList = db.AnchorsWorkTotal
                        .Where(s => s.Year == year && s.Month == lastMonth)
                        .Select(s => new
                        {
                            s.AnchorId,
                            s.Year,
                            s.Month,
                            s.WorkSecond
                        }).ToList();

                    var list = reportThisMonthList.Select(s => new
                    {
                        AnchorID = s.AnchorId,
                        Name = db.dt_dealer.Where(d => d.id == s.AnchorId).FirstOrDefault().dealerName,
                        PrevMonth = (reportLastMonthList.Where(o => o.AnchorId == s.AnchorId).FirstOrDefault() != null)
                        ? (reportLastMonthList.Where(o => o.AnchorId == s.AnchorId).FirstOrDefault().WorkSecond / 3600) + "h"
                        : "0h",
                        Month = (s.WorkSecond / 3600) + "h",

                    });

                    if (anchorId > 0)
                        list = list.Where(o => o.AnchorID == anchorId).ToList();

                    if (!string.IsNullOrEmpty(name))
                        list = list.Where(o => o.Name == name).ToList();

                    var totalCount = list.Count();

                    result.DataCount = totalCount;

                    if (totalCount > 0)
                        result.BackData = list.OrderBy(o => o.AnchorID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                    else
                        result.BackData = new List<object>();

                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetAnchorTimeList", "GetAnchorTimeList", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}

