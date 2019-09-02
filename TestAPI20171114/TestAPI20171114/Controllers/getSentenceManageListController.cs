using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Models;
using TestAPI20171114.Common;
using TestAPI20171114.Models.Request;

namespace TestAPI20171114.Controllers
{
    public class GetSentenceManageListController : ApiController
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
                byte state = byte.TryParse(request.Form["State"] ?? "", out state) ? state : (byte)255; //类型, 黑名单｜白名单  ，为空时全部
                int pageSize = int.TryParse(request.Form["PageSize"] ?? "", out pageSize) ? pageSize : 20;
                int pageIndex = int.TryParse(request.Form["PageIndex"] ?? "", out pageIndex) ? pageIndex : 0;
                var content = request.Form["Content"] ?? "";

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
                    var query = from s in db.dt_SensitiveSentences
                                select s;

                    if (state < 255)
                        query = query.Where(w => w.state == state);

                    if (!string.IsNullOrEmpty(content))
                        query = query.Where(o=>o.content.Contains(content));

                    if (!string.IsNullOrEmpty(startTimeString) && DateTime.TryParse(startTimeString,out startTime))
                        query = query.Where(o => o.addtime >= startTime);

                    if (!string.IsNullOrEmpty(endTimeString) && DateTime.TryParse(endTimeString, out endTime))
                    {
                        endTime = endTime.AddDays(1);
                        query = query.Where(o => o.addtime < endTime);
                    }

                    result.DataCount = query.Count();

                    var sentenceList = query
                                        .OrderByDescending(w => w.id)
                                        .Skip(pageSize * pageIndex)
                                        .Take(pageSize)
                                        .ToList();

                    result.BackData = sentenceList.Select(w => new
                    {
                        ID = w.id,
                        Content = w.content,
                        State = w.state,
                        AddTime = w.addtime.ToString("yyyy-MM-dd HH:mm:ss"),
                        Staff = w.adminname,
                        Remark = w.remark
                    }).ToList();

                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetSentenceManageList", "GetSentenceManageList", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
