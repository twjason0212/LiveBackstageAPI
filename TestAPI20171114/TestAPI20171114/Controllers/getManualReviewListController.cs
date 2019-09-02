using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Common;
using TestAPI20171114.Models;

namespace TestAPI20171114.Controllers
{
    public class GetManualReviewListController : ApiController
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
                byte state = byte.TryParse(request.Form["State"], out state) ? state : (byte)255;
                var words = request.Form["Words"] ?? "";
                var name = request.Form["UserName"] ?? "";
                int pageSize = int.TryParse(request.Form["PageSize"] ?? "-1", out pageSize) ? pageSize : -1;
                int pageIndex = int.TryParse(request.Form["PageIndex"] ?? "-1", out pageIndex) ? pageIndex : -1;
                
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

                    var query = from m in db.dt_ManualReview.AsNoTracking()
                                select m;

                    if (!string.IsNullOrEmpty(words))
                        query = query.Where(m => m.SensitiveWords.Contains(words));

                    if (!string.IsNullOrEmpty(name))
                        query = query.Where(m => m.UserName == name || m.UserNickName == name);

                    if (state < 255)
                        query = query.Where(m => m.State == state);

                    if (!string.IsNullOrEmpty(startTimeString) && DateTime.TryParse(startTimeString, out startTime))
                        query = query.Where(o => o.AddTime >= startTime);

                    if (!string.IsNullOrEmpty(endTimeString) && DateTime.TryParse(endTimeString, out endTime))
                    {
                        endTime = endTime.AddDays(1);
                        query = query.Where(o => o.AddTime < endTime);
                    }

                    var totalCount = query.Count();

                    var tempList = query
                                    .OrderByDescending(m => m.Id)
                                    .Skip(pageSize * pageIndex)
                                    .Take(pageSize)
                                    .ToList();

                    result.DataCount = totalCount;

                    result.BackData = tempList.Select(w => new
                    {
                        ID = w.Id,
                        Content = w.Content,
                        Words = w.SensitiveWords,
                        UserName = w.UserName,
                        UserNickName = w.UserNickName ?? "",
                        SendTime = w.SendTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        AddTime = w.AddTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        Staff = (w.ManagerID > 0)
                                    ? (db.dt_Manager.Where(m => m.id == w.ManagerID).FirstOrDefault() != null ? db.dt_Manager.Where(m => m.id == w.ManagerID).First().user_name : "" )
                                    : "",
                        State = w.State,
                        IdentityId = w.IdentityId
                    }).ToList();
                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetManualReviewList", "GetManualReviewList", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;                
            }

        }
    }
}
