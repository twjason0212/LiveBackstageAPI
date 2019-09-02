using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Models;
using TestAPI20171114.Models.Request;
using TestAPI20171114.Common;

namespace TestAPI20171114.Controllers
{
    public class DelSystemBarrageController : ApiController
    {
        [HttpPost]
        public ResultInfoT<object> Post()
        {
            var result = new ResultInfoT<object>() { IsLogin = ResultHelper.IsLogin };
            var request = HttpContext.Current.Request;
            var session = HttpContext.Current.Session;
            var now = DateTime.Now;

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

                int id = int.TryParse(request.Form["ID"] ?? "", out id) ? id : -1;

                if (id <= 0)
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }
                ////驗證權限
                //using (var db = new livecloudEntities())
                //{
                //    var operationManager = db.dt_Manager.Find(managerId);

                //    var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();

                //    if (operationManagerRole.SystemBarrage == false)
                //    {
                //        result.Code = ResultHelper.NotAuthorized;
                //        result.StrCode = ResultHelper.NotAuthorizedMsg;
                //        return result;
                //    }
                //}

                using (var db = new livecloudEntities())
                {

                    var sysBarrage = db.dt_SystemBarrage.Find(id);

                    if (sysBarrage == null)
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = "ID:" + id + "的系统弹幕不存在！";
                        return result;
                    }

                    db.dt_SystemBarrage.Remove(sysBarrage);

                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = managerId,
                        ManagerName = db.dt_Manager.Find(managerId).user_name,
                        ActionType = "delSystemBarrage",
                        AddTime = now,
                        Remarks = "删除系统弹幕ID：" + id,
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
                Log.Error("DelSentenceManage", "DelSentenceManage", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
