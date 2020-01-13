using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Models;
using TestAPI20171114.Common;
using System.Data.Entity.Infrastructure;

namespace TestAPI20171114.Controllers
{
    public class SetSystemBarrageController : ApiController
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
                var content = request.Form["Content"] ?? "";
                var remark = request.Form["Remark"] ?? "";
                int id = int.TryParse(request.Form["ID"] ?? "", out id) ? id : -1;
                var now = DateTime.Now;

                switch (action.ToLower())
                {
                    case "add":
                        {
                            if (string.IsNullOrEmpty(content))
                            {
                                result.Code = ResultHelper.ParamFail;
                                result.StrCode = ResultHelper.ParamFailMsg;
                                return result;
                            }
                            break;
                        }

                    case "edit":
                        {
                            if (id <= 0 ||
                               string.IsNullOrEmpty(content))
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
                    ////驗證權限(不確定是否為相應的欄位)
                    //var operationManager = db.dt_Manager.Find(managerId);

                    //var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();

                    //if (operationManagerRole.SystemBarrage == false)
                    //{
                    //    result.Code = ResultHelper.NotAuthorized;
                    //    result.StrCode = ResultHelper.NotAuthorizedMsg;
                    //    return result;
                    //}

                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = managerId,
                        ManagerName = db.dt_Manager.Find(managerId).user_name,
                        ActionType = "setSystemBarrage",
                        AddTime = now,
                        IP = NetworkTool.GetClientIP(HttpContext.Current)
                    };

                    switch (action.ToLower())
                    {
                        case "add":
                            {
                                var dupContent = db.dt_SystemBarrage.Where(s => s.content == content).FirstOrDefault();

                                if (dupContent != null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = "已存在相同内容的系统弹幕！";
                                    return result;
                                }

                                var totalCount = db.dt_SystemBarrage.Count();

                                if (totalCount >= Conf.MaxSystemBarrageCount)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = "系统弹幕已达" + Conf.MaxSystemBarrageCount.ToString() + "条，无法继续添加！";
                                    return result;
                                }

                                var sysBarrage = new dt_SystemBarrage()
                                {
                                    content = content,
                                    add_time = now,
                                    update_time = now,
                                    state = (byte)1,
                                    times = 0,
                                    operUser = db.dt_Manager.Find(managerId).user_name,
                                    Remark = remark
                                };

                                db.dt_SystemBarrage.Add(sysBarrage);

                                manageLog.Remarks = "添加系统弹幕:" + content;

                                break;
                            }

                        case "edit":
                            {
                                var sysBarrage = db.dt_SystemBarrage.Find(id);

                                if (sysBarrage == null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = ResultHelper.ParamFailMsg + " ID:" + id + "的系统弹幕不存在";
                                    return result;
                                }

                                sysBarrage.content = content;
                                sysBarrage.update_time = now;
                                sysBarrage.operUser = db.dt_Manager.Find(managerId).user_name;
                                if (!string.IsNullOrEmpty(remark))
                                    sysBarrage.Remark = remark;

                                manageLog.Remarks = "修改系统弹幕:" + content + "(ID:" + sysBarrage.id + ")";

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
            catch (DbUpdateException ex)
            {
                Log.Info("SetSystemBarrage", "SetSystemBarrage", ex.InnerException.Message.ToString());
                result.Code = ResultHelper.ParamFail;
                result.StrCode = "已有重复的内容，请刷新页面";
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("SetSystemBarrage", "SetSystemBarrage", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }


    }
}
