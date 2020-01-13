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
    public class GetSystemBarrageListController : ApiController
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

                //缺少權限檢查

                var content = request.Form["Content"] ?? "";
                int pageIndex = int.TryParse(request.Form["PageIndex"] ?? "", out pageIndex) ? pageIndex : 0;
                int pageSize = int.TryParse(request.Form["PageSize"] ?? "", out pageSize) ? pageSize : 50;
                int searchType = int.TryParse(request.Form["Type"] ?? "", out searchType) ? searchType : 1;

                if (pageIndex < 0 || pageSize <= 0 || searchType > 2 || searchType < 0)
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                var now = DateTime.Now;
                var addDays = ((int)now.DayOfWeek == 0) //週日
                                 ? 6
                                 : (int)now.DayOfWeek - 1;
                var startTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(-addDays); //該週周一凌晨12點

                // 可以考慮做快取
                using (var db = new livecloudEntities())
                {
                    var query = from a in db.dt_SystemBarrage
                                join b in db.dt_SystemBarrageTimes.Where(t=>t.updatetime >= startTime) on a.id equals b.systembarrageid into c
                                from d in c.DefaultIfEmpty()
                                select new
                                {
                                    ID = a.id,
                                    Content = a.content,
                                    AddTime = a.add_time,
                                    EditTime = a.update_time,
                                    Staff = a.operUser,
                                    State = a.state,
                                    Times = (int?)d.times,
                                    Remark = a.Remark
                                };

                    if (!string.IsNullOrEmpty(content))
                        query = query.Where(o => o.Content.Contains(content));

                    var totalCount = query.Count();

                    switch (searchType)
                    {
                        case 1:
                            query = query.OrderByDescending(o => o.Times).ThenByDescending(o => o.AddTime);
                            break;
                        case 2:
                            query = query.OrderByDescending(o => o.EditTime);
                            break;
                        default:
                            query = query.OrderBy(o => o.Times).ThenByDescending(o => o.AddTime);
                            break;
                    }

                    var list = query
                                //.OrderByDescending(o => o.AddTime)
                                .Skip(pageSize * pageIndex)
                                .Take(pageSize)
                                .ToList();

                    result.DataCount = totalCount;

                    result.BackData = list.Select(s => new
                    {
                        ID = s.ID,
                        Content = s.Content,
                        AddTime = s.AddTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        EditTime = s.EditTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        Staff = s.Staff,
                        Times = s.Times ?? 0,
                        State = s.State,
                        Remark = s.Remark
                    }).ToList();

                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetSystemBarrageList", "GetSystemBarrageList", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
