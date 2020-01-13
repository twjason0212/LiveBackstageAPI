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
    public class ManagerListController : ApiController
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
                    //驗證權限
                    var operationManager = db.dt_Manager.Find(managerId);

                    var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();

                    if (operationManagerRole.Manager == false)
                    {
                        result.Code = ResultHelper.NotAuthorized;
                        result.StrCode = ResultHelper.NotAuthorizedMsg;
                        return result;
                    }
                    //
                    var query = from m in db.dt_Manager
                                join r in db.dt_ManagerRole
                                on m.admin_role equals r.Id
                                where m.admin_role != 2
                                orderby m.id ascending
                                select new
                                {
                                    m.id,
                                    m.user_name,
                                    m.real_name,
                                    m.add_time,
                                    m.Status,
                                    r.RoleName
                                };

                    var managerList = query.ToList().Select(o => new ManagerList()
                    {
                        ID = o.id,
                        UserName = o.user_name,
                        RealName = o.real_name,
                        AdminRole = o.RoleName,
                        AddTime = o.add_time.ToString("yyyy-MM-dd HH:mm:ss"),
                        Status = o.Status
                    }).ToList();

                    result.BackData = managerList;

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("ManagerList", "ManagerList", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
