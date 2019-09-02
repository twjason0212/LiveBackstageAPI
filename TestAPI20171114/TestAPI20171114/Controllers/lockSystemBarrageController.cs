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
    public class LockSystemBarrageController : ApiController
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

                int id = int.TryParse(request.Form["ID"], out id) ? id : -1;
                var doLock = request.Form["State"] ?? "";

                if (id <= 0 || (doLock.ToLower() != "true" && doLock.ToLower() != "false"))
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                using (var db = new livecloudEntities())
                {
                    //驗證權限
                    //var operationManager = db.dt_Manager.Find(managerId);

                    //var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();

                    //if (operationManagerRole.DealerManage == false)
                    //{
                    //    result.Code = ResultHelper.NotAuthorized;
                    //    result.StrCode = ResultHelper.NotAuthorizedMsg;
                    //    return result;
                    //}

                    var sysBarrage = db.dt_SystemBarrage.Find(id);

                    if (sysBarrage == null)
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = "ID:" + id + "的系统弹幕不存在！";
                        return result;
                    }

                    sysBarrage.state = (doLock.ToLower() == "true")
                        ? (byte)1
                        : (byte)0;

                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = managerId,
                        ManagerName = db.dt_Manager.Find(managerId).user_name,
                        ActionType = "setSystemBarrage",
                        AddTime = DateTime.Now,
                        Remarks = (doLock.ToLower() == "true") ? "启用" : "停用" + "系统弹幕ID:" + id,
                        IP = NetworkTool.GetClientIP(HttpContext.Current)
                    };

                    db.dt_ManageLog.Add(manageLog);

                    db.SaveChanges();

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("LockSystemBarrage", "LockSystemBarrage", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
