using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Common;
using TestAPI20171114.Models;

namespace TestAPI20171114.Controllers
{
    public class GetShieldedRecordController : ApiController
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

                var now = DateTime.Now;
                int state = int.TryParse(request.Form["State"] ?? "", out state) ? state : -1; //全部""｜黑名单0｜禁用词1 | 人工审核2
                int pageSize = int.TryParse(request.Form["PageSize"] ?? "", out pageSize) ? pageSize : 20;
                int pageIndex = int.TryParse(request.Form["PageIndex"] ?? "", out pageIndex) ? pageIndex : 0;

                var userName = request.Form["UserName"] ?? "";
                var userNickName = request.Form["UserNickName"] ?? "";

                var startTimeString = request.Form["StartTime"] ?? "";
                var endTimeString = request.Form["EndTime"] ?? "";
                DateTime startTime;
                DateTime endTime;

                if (pageSize <= 0 || pageIndex < 0)
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                using (var db = new livecloudEntities())
                {
                    var query = from s in db.ChatMessageBlockLog
                                select s;

                    if (!string.IsNullOrEmpty(userName))
                        query = query.Where(s => s.UserName == userName);

                    if (!string.IsNullOrEmpty(userNickName))
                        query = query.Where(s => s.UserNickName == userNickName);

                    if (state >= 0)
                        query = query.Where(s => s.State == state);
                    else
                    {
                        // 狀態為全部時，去除"人工審核"
                        query = query.Where(s => s.State != 2);
                    }

                    if (!string.IsNullOrEmpty(startTimeString) && DateTime.TryParse(startTimeString, out startTime))
                        query = query.Where(o => o.Time >= startTime);

                    if (!string.IsNullOrEmpty(endTimeString) && DateTime.TryParse(endTimeString, out endTime))
                    {
                        endTime = endTime.AddDays(1);
                        query = query.Where(o => o.Time < endTime);
                    }

                    var totalCount = query.Count();

                    var tempList = query
                                    .OrderByDescending(s => s.Id)
                                    .Skip(pageSize * pageIndex)
                                    .Take(pageSize)
                                    .ToList();

                    result.DataCount = totalCount;
                    result.BackData = tempList.Select(s => new
                    {
                        ID = s.Id,
                        UserName = s.UserName,
                        UserNickName = s.UserNickName ?? "",
                        State = s.State,
                        ShieldedWord = s.BlockWords,
                        Content = s.Content,
                        Time = s.Time.ToString("yyyy-MM-dd HH:mm:ss")
                    }).ToList();
                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetShieldedRecord", "GetShieldedRecord", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}