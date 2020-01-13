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
    public class SetPasswordController : ApiController
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
                
                var userName = request.Form["UserName"] ?? "";
                var oldPassword = request.Form["oldPassword"] ?? "";
                var newPassword = request.Form["newPassword"] ?? "";
                var action = request.Form["Type"] ?? "";
                var now = DateTime.Now;

                if (string.IsNullOrEmpty(userName) ||
                    string.IsNullOrEmpty(oldPassword) ||
                    string.IsNullOrEmpty(newPassword) ||
                    action.ToLower() != "edit")
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                using (var db = new livecloudEntities())
                {
                    ////驗證權限(不確定是否為相應的欄位)
                    //var operationManager = db.dt_Manager.Find(managerId);

                    //var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();

                    //if (operationManagerRole.Manager == false)
                    //{
                    //    result.Code = ResultHelper.NotAuthorized;
                    //    result.StrCode = ResultHelper.NotAuthorizedMsg;
                    //    return result;
                    //}

                    var manager = db.dt_Manager.Where(m => m.user_name == userName).FirstOrDefault();

                    if (manager == null)
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = "不存在管理员帐户:" + userName + "！";
                        return result;
                    }

                    if (manager.password != oldPassword)
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = "原密码错误！";
                        return result;
                    }

                    manager.password = newPassword;

                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = managerId,
                        ManagerName = db.dt_Manager.Find(managerId).user_name,
                        ActionType = "setPassword",
                        AddTime = now,
                        Remarks = "修改帐户密码:" + manager.user_name,
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
                Log.Error("SetPassword", "SetPassword", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
