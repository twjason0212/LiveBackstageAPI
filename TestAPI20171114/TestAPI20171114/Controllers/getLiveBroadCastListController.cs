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
    public class GetLiveBroadCastListController : ApiController
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

                //缺少權限檢查

                var liveId = request.Form["GameID"] ?? "";
                int pageIndex = int.TryParse(request.Form["PageIndex"] ?? "", out pageIndex) ? pageIndex : 0;
                int pageSize = int.TryParse(request.Form["PageSize"] ?? "", out pageSize) ? pageSize : 20;
                var now = DateTime.Now;
                
                
                if (pageIndex < 0 || pageSize <= 0)
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                // 可以考慮做快取
                using (var db = new livecloudEntities())
                {
                    var query = from b in db.dt_AdminBroadcastLog
                                select b;

                    int intLiveId = int.TryParse(liveId, out intLiveId) ? intLiveId : -1;

                    if (!string.IsNullOrEmpty(liveId))
                        query = query.Where(b => b.LiveId == intLiveId);

                    int totalCount = query.Count();

                    var list = query
                                .OrderByDescending(b => b.SendTime)
                                .Skip(pageIndex * pageSize)
                                .Take(pageSize)
                                .ToList();

                    var managerList = db.dt_Manager.ToList();

                    result.DataCount = totalCount;

                    // 要確認TrimEnd等等的動作必要性
                    result.BackData = list.Select(b => new
                    {
                        BroadCastID = b.Id,
                        GameID = b.LiveId.ToString("0000"),
                        StartTime = b.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        EndTime = b.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        Content = b.BroadcastText,
                        Staff = managerList.Where(m => m.id == b.ManagerId).FirstOrDefault().user_name,
                        State = (b.Status == 0) ? 0 : 
                                (b.StartTime < now && b.EndTime > now) ? 1 :
                                2
                    }).ToList();

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetLiveBroadCastList", "GetLiveBroadCastList", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
