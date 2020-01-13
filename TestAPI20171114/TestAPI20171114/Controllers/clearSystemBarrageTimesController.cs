using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Common;
using TestAPI20171114.Models;

namespace TestAPI20171114.Controllers
{
    public class ClearSystemBarrageTimesController : ApiController
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

                var now = DateTime.Now;

                using (var db = new livecloudEntities())
                {
                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = managerId,
                        ManagerName = db.dt_Manager.Find(managerId).user_name,
                        ActionType = "clearSystemBarrageTimes",
                        AddTime = now,
                        IP = NetworkTool.GetClientIP(HttpContext.Current),
                        Remarks = "系统弹幕使用次数清零"
                    };

                    db.dt_ManageLog.Add(manageLog);

                    foreach (var data in db.dt_SystemBarrageTimes)
                    {
                        data.times = 0;
                        data.updatetime = now;
                    }

                    db.SaveChanges();
                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("ClearSystemBarrageTimes", "ClearSystemBarrageTimes", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }


    }
}