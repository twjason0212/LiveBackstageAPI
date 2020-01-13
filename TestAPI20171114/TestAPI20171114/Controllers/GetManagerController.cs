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
    public class GetManagerController : ApiController
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

                int id = int.TryParse(request.Form["ID"] ?? "", out id) ? id : -1;

                if (id <= 0)
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                using (var db = new livecloudEntities())
                {
                    var manager = db.dt_Manager.Find(id);

                    if (manager == null)
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = ResultHelper.ParamFailMsg + " ID:" + id + "的管理员不存在";
                        return result;
                    }

                    result.BackData = new GetManager()
                    {
                        ID = manager.id,
                        UserName = manager.user_name,
                        RealName = manager.real_name,
                        AdminRoleID = manager.admin_role
                    };
                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetManager", "GetManager", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
