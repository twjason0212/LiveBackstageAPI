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
    public class GetRoleListController : ApiController
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
                    var roleList = db.dt_ManagerRole.ToList();

                    var totalCount = roleList.Count;

                    result.BackData = roleList.OrderBy(r => r.Id).Select(r => new
                    {
                        ID = r.Id,
                        Name = r.RoleName,
                        Time = r.AddTime.ToString("yyyy-MM-dd HH:mm:ss")
                    }).ToList();

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetRoleList", "GetRoleList", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
