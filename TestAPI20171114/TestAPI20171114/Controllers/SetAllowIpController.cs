using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Common;
using TestAPI20171114.Models;

namespace TestAPI20171114.Controllers
{
    public class SetAllowIpController : ApiController
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

                var action = request.Form["Type"] ?? "";

                int id = int.TryParse(request.Form["Id"] ?? "-1", out id) ? id : -1;
                var ip = request.Form["Iplimit"] ?? "";
                var remark = request.Form["Remark"] ?? "";
                var now = DateTime.Now;

                ////驗證權限
                using (var db = new livecloudEntities())
                {
                    var operationManager = db.dt_Manager.Find(managerId);

                    var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();

                    if (operationManagerRole.AllowIp == false)
                    {
                        result.Code = ResultHelper.NotAuthorized;
                        result.StrCode = ResultHelper.NotAuthorizedMsg;
                        return result;
                    }
                }

                switch (action.ToLower())
                {
                    case "add":
                        {
                            if (string.IsNullOrEmpty(ip))
                            {
                                result.Code = ResultHelper.ParamFail;
                                result.StrCode = ResultHelper.ParamFailMsg;
                                return result;
                            }
                            break;
                        }

                    case "edit":
                        {
                            if (id < 0 || string.IsNullOrEmpty(ip))
                            {
                                result.Code = ResultHelper.ParamFail;
                                result.StrCode = ResultHelper.ParamFailMsg;
                                return result;
                            }
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
                        ActionType = "setAllowIp",
                        AddTime = now,
                        IP = NetworkTool.GetClientIP(HttpContext.Current)
                    };

                    var dupIp = action.ToLower() == "add" ? db.dt_AllowAccessIPList.Where(a => a.Ip == ip.Trim()).FirstOrDefault() : db.dt_AllowAccessIPList.Where(a => a.Ip == ip.Trim() & a.Id != id).FirstOrDefault();

                    if (dupIp != null)
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = "已存在相同名称的IP！";
                        return result;
                    }

                    bool ipDicSingal = false;

                    switch (action.ToLower())
                    {
                        case "add":
                            {
                                var allowIp = new dt_AllowAccessIPList()
                                {
                                    Ip = ip,
                                    Remark = remark,
                                    Enable = true,
                                    AddTime = now
                                };

                                db.dt_AllowAccessIPList.Add(allowIp);

                                manageLog.Remarks = "添加允許IP信息:" + ip;

                                ipDicSingal = apiController.AddIpList(ip);

                                break;
                            }

                        case "edit":
                            {
                                var allowIp = db.dt_AllowAccessIPList.Find(id);

                                if (allowIp == null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = ResultHelper.ParamFailMsg + " ID:" + id + "的IP不存在";
                                    return result;
                                }

                                ipDicSingal = apiController.EditIpList(allowIp.Ip,ip);

                                if (!string.IsNullOrEmpty(ip)) allowIp.Ip = ip;
                                if (!string.IsNullOrEmpty(remark)) allowIp.Remark = remark;

                                allowIp.AddTime = now;

                                manageLog.Remarks = "修改IP信息:" + allowIp.Ip + "(ID:" + allowIp.Id + ")";
                                
                                break;
                            }
                    }

                    db.dt_ManageLog.Add(manageLog);

                    if (ipDicSingal)
                        db.SaveChanges();
                    else
                    {
                        result.Code = ResultHelper.ExecutingError;
                        result.StrCode = ResultHelper.ExecutingErrorMsg;
                        return result;
                    }
                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("SetAllowIp", "SetAllowIp", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
