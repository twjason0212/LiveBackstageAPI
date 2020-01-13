using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Models;
using TestAPI20171114.Common;
using System.Text.RegularExpressions;

namespace TestAPI20171114.Controllers
{
    public class DelLiveNotSpeakController : ApiController
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

                var timeNow = DateTime.Now;
                var multiId = request.Form["ID"] ?? "";

                if (string.IsNullOrEmpty(multiId) || !(new Regex(@"^(([\d]{1,}){1}|(([\d]{1,}\,){1,}([\d]{1,}){1}))$").IsMatch(multiId)))
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                var idList = multiId.Split(',').Select(o => Convert.ToInt32(o)).Distinct().OrderBy(o => o).ToList();

                using (var db = new livecloudEntities())
                {
                    var manager = db.dt_Manager.Find(managerId);

                    var UserBarrageNoSpeakList = db.dt_UserBarrageNoSpeak.Where(o => idList.Contains(o.id)).ToList();

                    foreach (var item in UserBarrageNoSpeakList)
                    {
                        var manageLog = new dt_ManageLog()
                        {
                            ManagerId = managerId,
                            ManagerName = manager.user_name,
                            ActionType = "delLiveNotSpeak",
                            AddTime = timeNow,
                            Remarks = "删除用戶禁言: " + item.identityid + "_" + item.UserName + "(ID:" + item.id + ")",
                            IP = NetworkTool.GetClientIP(HttpContext.Current)
                        };
                        db.dt_ManageLog.Add(manageLog);
                    }

                    db.dt_UserBarrageNoSpeak.RemoveRange(UserBarrageNoSpeakList);

                    db.SaveChanges();
                    UpdateMsg.PostUpdate("dt_UserBarrageNoSpeak");

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("DelManager", "DelManager", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
