using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Common;
using TestAPI20171114.Models;

namespace TestAPI20171114.Controllers
{
    public class DelBlackWordManageController : ApiController
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

                string multiId = request.Form["ID"] ?? "";
                DateTime now = DateTime.Now;

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

                    var BlackWordsList = db.dt_BlackWords.Where(o => idList.Contains(o.id)).ToList();

                    foreach (var item in BlackWordsList)
                    {
                        var manageLog = new dt_ManageLog()
                        {
                            ManagerId = managerId,
                            ManagerName = manager.user_name,
                            ActionType = "delLiveNotSpeak",
                            AddTime = now,
                            Remarks = "删除黑詞ID:" + item.id + "",
                            IP = NetworkTool.GetClientIP(HttpContext.Current)
                        };
                        db.dt_ManageLog.Add(manageLog);
                    }

                    db.dt_BlackWords.RemoveRange(BlackWordsList);
                    db.SaveChanges();
                    UpdateMsg.PostUpdate("dt_BlackWords");

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }
                return result;

            }
            catch
            {

            }
            return result;
        }

    }
}
