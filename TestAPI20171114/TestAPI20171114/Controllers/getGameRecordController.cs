using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestAPI20171114.Models;
using TestAPI20171114.Common;
using System.Web;

namespace TestAPI20171114.Controllers
{
    public class GetGameRecordController : ApiController
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
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.NotLoginMsg;
                    result.IsLogin = ResultHelper.NotLogin;
                    return result;
                }

                var now = DateTime.Now;
                var gameId = request.Form["GameID"] ?? "";
                var issue = request.Form["Issue"] ?? "";

                DateTime startTime = DateTime.TryParse(request.Form["StartTime"] ?? "", out startTime)
                    ? startTime
                    : new DateTime(now.Year, now.Month, now.Day);
                DateTime endTime = DateTime.TryParse(request.Form["EndTime"] ?? "", out endTime)
                    ? endTime.AddDays(1)
                    : new DateTime(now.Year, now.Month, now.Day).AddDays(1);

                int pageSize = int.TryParse(request.Form["PageSize"] ?? "", out pageSize) ? pageSize : 20;
                int pageIndex = int.TryParse(request.Form["PageIndex"] ?? "", out pageIndex) ? pageIndex : 0;

                if (pageSize <= 0 && pageIndex < 0 && endTime < startTime)
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                using (var db = new livecloudEntities())
                {
                    var openRecordQuery = from r in db.dt_liveOpenRecord
                                          where r.add_time >= startTime && r.add_time < endTime
                                          select r;

                    if (!string.IsNullOrEmpty(gameId))
                        openRecordQuery = openRecordQuery.Where(r => r.liveId == gameId);

                    if (!string.IsNullOrEmpty(issue))
                        openRecordQuery = openRecordQuery.Where(r => r.issueNo == issue);

                    var totalCount = openRecordQuery.Count();

                    //if (totalCount > 0)
                    //{
                    var anchorList = db.dt_dealer.Select(a => new
                    {
                        Id = a.id,
                        Name = a.dealerName
                    }).ToList();

                    var openRecordList = openRecordQuery
                        .OrderByDescending(r => r.add_time)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize)
                        .ToList();

                    var resultList = openRecordList.Select(r => new
                    {
                        GameID = r.code,
                        Issue = r.issueNo,
                        StartTime = r.startTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        EndTime = r.endTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        Name = r.code + "-" + anchorList.Where(a => a.Id == r.dealerId).FirstOrDefault().Name,
                        Results = r.openNum,
                        Time = r.add_time.ToString("yyyy-MM-dd HH:mm:ss")
                    }).ToList();

                    result.BackData = resultList;
                    // }

                    result.DataCount = totalCount;
                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetGameRecord", "GetGameRecord", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }

        }
    }
}
