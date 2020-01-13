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
    public class GetLiveListController : ApiController
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

                using (var db = new livecloudEntities())
                {
                    var query = from s in db.dt_liveList
                                join t in db.dt_liveTable on s.tableId equals t.id
                                join d in db.dt_dealer on s.dealerId equals d.id
                                join l in db.dt_live on s.liveId equals l.liveId
                                select new
                                {
                                    s.id,
                                    s.code,
                                    t.tableName,
                                    s.dealerId,
                                    l.liveName,
                                    d.dealerName,
                                    s.state,
                                    s.stop_time,
                                    s.add_time
                                };

                    var list = query
                                .OrderByDescending(o => o.id)
                                .ToList();

                    if (list.Count == 0)
                    {
                        var querynull = from s in db.dt_liveList
                                join t in db.dt_liveTable on s.tableId equals t.id
                                join l in db.dt_live on s.liveId equals l.liveId
                                select new
                                {
                                    s.id,
                                    s.code,
                                    t.tableName,
                                    s.dealerId,
                                    l.liveName,
                                    s.state,
                                    s.stop_time,
                                    s.add_time
                                };
                  
                    var list2 = querynull
                                .OrderByDescending(o => o.id)
                                .ToList();
                        result.BackData = list2.Select(s => new
                        {
                            GameID = s.code,
                            GameKind = s.tableName,
                            GameName = s.liveName,
                            //AnchorName = s.dealerName.TrimEnd(),
                            GameState = s.state == 1 ? "运行中" : "停运",
                            RestartTime = (!string.IsNullOrEmpty(s.stop_time))
                                                ? ""
                                                : s.stop_time,
                            AnchorState = s.state == 1
                                                ? (db.AnchorsClockLog
                                                    .Where(c => c.LiveId == s.code && c.AnchorId == s.dealerId && s.state == (byte)1)
                                                    .OrderByDescending(c => c.AddTime).FirstOrDefault() != null)
                                                ?
                                                (int)(DateTime.Now - db.AnchorsClockLog
                                                    .Where(c => c.LiveId == s.code && c.AnchorId == s.dealerId && s.state == (byte)1)
                                                    .OrderByDescending(c => c.AddTime).FirstOrDefault().AddTime).TotalSeconds
                                                    : 0
                                                : 0,
                            Time = s.add_time.ToString("yyyy-MM-dd HH:mm:ss")
                        }).ToList();
                    }
                    else
                    {
                        result.BackData = list.Select(s => new
                        {
                            GameID = s.code,
                            GameKind = s.tableName,
                            GameName = s.liveName,
                            AnchorName = s.dealerName.TrimEnd(),
                            GameState = s.state == 1 ? "运行中" : "停运",
                            RestartTime = (!string.IsNullOrEmpty(s.stop_time))
                                                                        ? ""
                                                                        : s.stop_time,
                            AnchorState = s.state == 1
                                                                        ? (db.AnchorsClockLog
                                                                            .Where(c => c.LiveId == s.code && c.AnchorId == s.dealerId && s.state == (byte)1)
                                                                            .OrderByDescending(c => c.AddTime).FirstOrDefault() != null)
                                                                        ?
                                                                        (int)(DateTime.Now - db.AnchorsClockLog
                                                                            .Where(c => c.LiveId == s.code && c.AnchorId == s.dealerId && s.state == (byte)1)
                                                                            .OrderByDescending(c => c.AddTime).FirstOrDefault().AddTime).TotalSeconds
                                                                            : 0
                                                                        : 0,
                            Time = s.add_time.ToString("yyyy-MM-dd HH:mm:ss")
                        }).ToList();
                    }
                    

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetLiveList", "GetLiveList", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
