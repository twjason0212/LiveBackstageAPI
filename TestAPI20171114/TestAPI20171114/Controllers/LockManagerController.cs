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
    public class LockManagerController : ApiController
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

                int id = int.TryParse(request.Form["ID"] ?? "", out id) ? id : -1;
                byte status = byte.TryParse(request.Form["Status"] ?? "", out status) ? status : (byte)255;
                var now = DateTime.Now;

                if (id <= 0 || status > (byte)1)
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                using (var db = new livecloudEntities())
                {
                    ////驗證權限
                    //var operationManager = db.dt_Manager.Find(managerId);

                    //var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();

                    //if (operationManagerRole.DealerManage == false)
                    //{
                    //    result.Code = ResultHelper.NotAuthorized;
                    //    result.StrCode = ResultHelper.NotAuthorizedMsg;
                    //    return result;
                    //}

                    var manager = db.dt_Manager.Find(id);

                    if (manager == null)
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = "ID:" + id + "的管理员不存在！";
                        return result;
                    }

                    manager.Status = status;

                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = managerId,
                        ManagerName = db.dt_Manager.Find(managerId).user_name,
                        ActionType = "ManageisLock",
                        AddTime = now,
                        Remarks = ((status == (byte)0) ? "停用" : "启用") + "管理员:" + manager.user_name + "(ID:" + manager.id + ")",
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
                Log.Error("LockManager", "LockManager", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
