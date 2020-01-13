using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestAPI20171114.Models;
using TestAPI20171114.Common;
using System.Web;
using System.Data.Entity;

namespace TestAPI20171114.Controllers
{
    public class SetLiveNotSpeakController : ApiController
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

                // 前端送入
                string userName = request.Form["UserName"] ?? "";
                string identityid = request.Form["IdentityId"] ?? "";
                int duration = int.TryParse(request.Form["Duration"] ?? "0", out duration) ? duration : 0;
                string remark = request.Form["Remark"] ?? "";
                int type = int.TryParse(request.Form["State"] ?? "0", out type) ? type : 0;


                // 自定義參數
                string userNickName = "";

                using (var db = new livecloudEntities())
                {
                    var query = from p in db.dt_UserBarrageNoSpeak
                                where p.UserName == userName && p.Type == type
                                select p;

                    if (query.ToList().Count > 0 && CheckNoSpeak(query))
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = "此用户已存在";
                        return result;
                    }

                    dt_UserBarrageNoSpeak UserBarrageNoSpeak = new dt_UserBarrageNoSpeak()
                    {
                        identityid = identityid,
                        UserName = userName,
                        UserNickName = string.IsNullOrEmpty(userNickName) ? "" : userNickName,
                        Remark = remark,
                        Type = type,
                        Duration = duration,
                        AddTime = DateTime.Now,
                        OperateUser = db.dt_Manager.Find(managerId).user_name
                    };

                    db.dt_UserBarrageNoSpeak.Add(UserBarrageNoSpeak);
                    db.SaveChanges();
                    UpdateMsg.PostUpdate("dt_UserBarrageNoSpeak");
                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("SetAnchor", "SetAnchor", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }

        private bool CheckNoSpeak(System.Linq.IQueryable<dt_UserBarrageNoSpeak> query)
        {
            bool isForevery = query.Where(o => o.Duration == 0).ToList().Count > 0 ? true : false;

            //新增時間大於現在時間檢調封印時間
            var result = (
                from s in query
                where s.AddTime > DbFunctions.AddDays(DateTime.Now, -s.Duration)
                select s
                );
            bool isInTime = result.ToList().Count > 0 ? true : false;

            return (isForevery == true || isInTime == true) ? true : false;
        }
    }
}
