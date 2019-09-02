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
    public class DelWordsManageController : ApiController
    {
        Regex regex = new Regex("^([0-9]{1,})$|^(([0-9]{1,}\\,){1,}([0-9]{1,}))$");

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

                string ids = request.Form["ID"] ?? "";

                if (string.IsNullOrEmpty(ids) || !regex.IsMatch(ids))
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

                //    if (operationManagerRole.WordsManage == false)
                //    {
                //        result.Code = ResultHelper.NotAuthorized;
                //        result.StrCode = ResultHelper.NotAuthorizedMsg;
                //        return result;
                //    }
                //}


                using (var db = new livecloudEntities())
                {
                    var idList = ids.Split(',').Select(i => int.Parse(i)).ToList();

                    var query = from w in db.dt_SensitiveWords
                                where idList.Contains(w.id)
                                select w;

                    var removeList = db.dt_SensitiveWords.RemoveRange(query);

                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = managerId,
                        ManagerName = db.dt_Manager.Find(managerId).user_name,
                        ActionType = "delWordsManage",
                        AddTime = now,
                        Remarks = "删除禁用敏感字词ID:" + ids,
                        IP = NetworkTool.GetClientIP(HttpContext.Current)
                    };

                    db.dt_ManageLog.Add(manageLog);

                    db.SaveChanges();
                    UpdateMsg.PostUpdate("dt_SensitiveWords");
                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("DelWordsManage", "DelWordsManage", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }

        }
    }
}
