using Microsoft.ApplicationInsights.DataContracts;
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
    public class ManagerEditController : ApiController
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

                int id = int.TryParse(request.Form["ID"] ?? "-1", out id) ? id : -1;
                var userName = request.Form["UserName"] ?? "";
                var password = request.Form["Password"] ?? "";
                var realName = request.Form["RealName"] ?? "";
                var adminRole = request.Form["AdminRole"] ?? "";
                var action = request.Form["Type"] ?? "";
                var now = DateTime.Now;

                ////驗證權限
                //using (var db = new livecloudEntities())
                //{
                //    var operationManager = db.dt_Manager.Find(managerId);

                //    var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();

                //    if (operationManagerRole.Manager == false)
                //    {
                //        result.Code = ResultHelper.NotAuthorized;
                //        result.StrCode = ResultHelper.NotAuthorizedMsg;
                //        return result;
                //    }
                //}

                switch (action.ToLower())
                {
                    case "add":
                        {
                            if (string.IsNullOrEmpty(userName) ||
                                string.IsNullOrEmpty(password) ||
                                string.IsNullOrEmpty(adminRole))
                            {
                                result.Code = ResultHelper.ParamFail;
                                result.StrCode = ResultHelper.ParamFailMsg;
                                return result;
                            }
                            break;
                        }

                    case "edit":
                        {
                            if (id <= 0)
                            {
                                result.Code = ResultHelper.ParamFail;
                                result.StrCode = ResultHelper.ParamFailMsg;
                                return result;
                            }
                            //if (!string.IsNullOrEmpty(userName))
                            //{
                            //    result.Code = ResultHelper.ParamFail;
                            //    result.StrCode = "不允许修改管理员帐户名称！";
                            //    return result;
                            //}
                            break;
                        }
                    default:
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = ResultHelper.ParamFailMsg;
                        return result;
                }
                
                using (var db = new livecloudEntities())
                {
                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = managerId,
                        ManagerName = db.dt_Manager.Find(managerId).user_name,
                        ActionType = "ManagerEdit",
                        AddTime = now,
                        IP = NetworkTool.GetClientIP(HttpContext.Current)
                    };

                    dt_ManagerRole role = null;

                    if (!string.IsNullOrEmpty(adminRole))
                    {
                        int roleId = int.TryParse(adminRole, out roleId) ? roleId : -1;

                        role = (roleId > 0)
                            ? db.dt_ManagerRole.Find(roleId)
                            : db.dt_ManagerRole.Where(r => r.RoleName == adminRole).FirstOrDefault();

                        if (role == null)
                        {
                            result.Code = ResultHelper.ParamFail;
                            result.StrCode = "找不到管理员角色ID:" + roleId + "的数据！";
                            return result;
                        }
                    }

                    switch (action.ToLower())
                    {
                        case "add":
                            {
                                var dupNameManager = db.dt_Manager.Where(a => a.user_name == userName).FirstOrDefault();

                                if (dupNameManager != null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = "已存在相同名称的管理员！";
                                    return result;
                                }

                                var manager = new dt_Manager()
                                {
                                    user_name = userName,
                                    real_name = realName,
                                    password = password,
                                    admin_role = role.Id,
                                    add_time = now,
                                    Status = 1
                                };

                                db.dt_Manager.Add(manager);

                                manageLog.Remarks = "新增管理员帐户:" + userName;

                                break;
                            }

                        case "edit":
                            {
                                var manager = db.dt_Manager.Find(id);

                                if (manager == null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = "ID:" + id + "的管理员不存在！";
                                    return result;
                                }

                                if (!string.IsNullOrEmpty(realName))
                                    manager.real_name = realName;
                                if (!string.IsNullOrEmpty(password))
                                    manager.password = password;
                                if (role != null)
                                    manager.admin_role = role.Id;

                                manageLog.Remarks = "修改管理员信息:" + manager.user_name + "(ID:" + manager.id + ")";

                                break;
                            }
                    }

                    db.dt_ManageLog.Add(manageLog);

                    db.SaveChanges();
                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("ManagerEdit", "ManagerEdit", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
