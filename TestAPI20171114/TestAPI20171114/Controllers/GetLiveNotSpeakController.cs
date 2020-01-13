using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestAPI20171114.Models;
using TestAPI20171114.Common;
using System.Web;
using ServiceStack.Templates;

namespace TestAPI20171114.Controllers
{
    public class GetLiveNotSpeakController : ApiController
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
                    var query = from o in db.dt_UserBarrageNoSpeak
                                select o;

                    string username = request.Form["UserName"] ?? "";
                    string identityid = request.Form["IdentityId"] ?? "";
                    string duration = request.Form["Duration"] ?? "";
                    
                    if (!string.IsNullOrEmpty(username))
                    {
                        query = query.Where(o => o.UserName == username);
                    }
                    if (!string.IsNullOrEmpty(identityid))
                    {
                        query = query.Where(o => o.identityid == identityid);
                    }
                    if (!string.IsNullOrEmpty(duration))
                    {
                        query = query.Where(o => o.Duration.ToString() == duration);
                    }

                    var list = query.OrderByDescending(o => o.AddTime).ToList();
                    list = list.Where(o => o.AddTime.Value.AddDays(Convert.ToDouble(o.Duration)) > DateTime.Now || o.Duration == 0).ToList();

                    result.BackData = list.Select(o => new
                    {
                        ID = o.id,
                        UserName = o.UserName,
                        IdentityId = o.identityid,
                        Duration = o.Duration,
                        State = o.Type,
                        AddTime = (Convert.ToDateTime(o.AddTime)).ToString("yyyy-MM-dd HH:mm:ss"),
                        Staff = o.OperateUser,
                        Remark = o.Remark

                    }).ToList();

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetLiveNotSpeak", "GetLiveNotSpeak", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
