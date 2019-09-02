using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Common;
using TestAPI20171114.Models;
using TestAPI20171114.Models.Request;

namespace TestAPI20171114.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public ResultInfoT<object> Post()
        {
            var result = new ResultInfoT<object>() { IsLogin = ResultHelper.NotLogin };
            var request = HttpContext.Current.Request;
            var session = HttpContext.Current.Session;
           
            try
            {
                var pwd = request.Form["Password"] ?? "";
                var userName = request.Form["Username"] ?? "";

                //缺少正則檢查
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(pwd))
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                //缺少已登入檢查
                
                using (livecloudEntities db = new Models.livecloudEntities())
                {
                    var manager = db.dt_Manager
                        .Where(m => m.user_name == userName && m.password == pwd)
                        .FirstOrDefault();

                    //缺少檢查管理員帳號是否已停用
                    if (manager == null) // || manager.Status != 1)
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = "帐户或密码错误";
                        return result;
                    }

                    if (manager.Status == (byte)0)
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = "帐户已被停用";
                        return result;
                    }

                    var role = db.dt_ManagerRole
                        .Where(r => r.Id == manager.admin_role)
                        .FirstOrDefault();

                    if (role == null)
                        throw new Exception("找不到 管理員ID:"+manager.id+" 對應的角色權限ID:"+manager.admin_role+" 的訊息");

                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = manager.id,
                        ManagerName = manager.user_name,
                        ActionType = "Login",
                        AddTime = DateTime.Now,
                        Remarks = "登入成功",
                        IP = NetworkTool.GetClientIP(HttpContext.Current) 
                    };
                    db.dt_ManageLog.Add(manageLog);

                    db.SaveChanges();

                    session["ManagerId"] = manager.id;
                    session["ManagerName"] = manager.user_name;
                    session["RoleId"] = manager.admin_role;

                    result.Code = ResultHelper.Success;
                    result.StrCode = "登录成功";
                    result.IsLogin = ResultHelper.IsLogin;
                    result.BackData = new GetRoleInfoBodyData()
                    {
                        barrageManage = role.BarrageManage.ToOnOff(),
                        AnchorList = role.DealerList.ToOnOff(),
                        AnchorManage = role.DealerManage.ToOnOff(),
                        AnchorPost = role.DealerPost.ToOnOff(),
                        AnchorTable = role.DealerTable.ToOnOff(),
                        AnchorTime = role.DealerTime.ToOnOff(),
                        giftList = role.GiftList.ToOnOff(),
                        giftManage = role.GiftManage.ToOnOff(),
                        livecmsManage = role.LiveCmsManage.ToOnOff(),
                        liveManage = role.LiveManage.ToOnOff(),
                        manageLog = role.ManageLog.ToOnOff(),
                        Manager = role.Manager.ToOnOff(),
                        managerList = role.ManagerList.ToOnOff(),
                        manualReview = role.ManualReview.ToOnOff(),
                        roleManage = role.RoleManage.ToOnOff(),
                        systemBarrage = role.SystemBarrage.ToOnOff(),
                        videoList = role.VideoList.ToOnOff(),
                        wordsManage = role.WordsManage.ToOnOff(),
                        sentenceManage = role.SentenceManage.ToOnOff(),
                        shieldedRecord = role.ShieldedRecord.ToOnOff(),
                        liveNotSpeak = role.LiveNotSpeak.ToOnOff(),
                        blackWordManage = role.BlackWordManage.ToOnOff(),
                        realTimeBarrage = role.RealTimeBarrage.ToOnOff()
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Login", "Login", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                result.IsLogin = ResultHelper.NotLogin;
                return result;
            }
        }
    }
}
