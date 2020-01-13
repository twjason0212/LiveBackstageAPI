using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestAPI20171114.Models;
using TestAPI20171114.Common;
using System.Web;
using System.Text;
using Newtonsoft.Json;

namespace TestAPI20171114.Controllers
{
    public class RevokeLiveBroadCastController : ApiController
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

                int id = (int.TryParse(request.Form["BroadCastID"] ?? "", out id)) ? id : -1;
                var now = DateTime.Now;

                if (id <= 0)
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                using (var db = new livecloudEntities())
                {
                    //驗證權限
                    var operationManager = db.dt_Manager.Find(managerId);

                    var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();

                    if (operationManagerRole.BarrageManage == false)
                    {
                        result.Code = ResultHelper.NotAuthorized;
                        result.StrCode = ResultHelper.NotAuthorizedMsg;
                        return result;
                    }

                    var broadcast = db.dt_AdminBroadcastLog.Find(id);

                    if (broadcast == null)
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = ResultHelper.ParamFailMsg + " ID:" + id + "的广播不存在";
                        return result;
                    }

                    broadcast.Status = 0;
                    broadcast.EndTime = now;

                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = managerId,
                        ManagerName = db.dt_Manager.Find(managerId).user_name,
                        ActionType = "RevokeLiveBroadCast",
                        AddTime = now,
                        Remarks = "撤消广播ID:" + id,
                        IP = NetworkTool.GetClientIP(HttpContext.Current)
                    };

                    db.dt_ManageLog.Add(manageLog);

                    db.SaveChanges();

                    using (var client = new WebClient() { Encoding = Encoding.UTF8 })
                    {
                        var broadcastModel = new
                        {
                            Target = "",
                            GameID = broadcast.LiveId.ToString("0000"),
                            Data = new
                            {
                                Type = "BroadCast",
                                ID = broadcast.Id,
                                Content = broadcast.BroadcastText,
                                StartTime = broadcast.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                EndTime = broadcast.EndTime.ToString("yyyy-MM-dd HH:mm:ss")
                            }
                        };

                        client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        var data = "content=" + JsonConvert.SerializeObject(broadcastModel);
                        
                        var response = client.UploadString(Conf.WSUrl, "POST", data);
                        var broadcastResult = JsonConvert.DeserializeObject<Result>(response.ToString());

                        if (broadcastResult.code == 1)
                        {
                            result.Code = ResultHelper.Success;
                            result.StrCode = "發送成功";
                        }
                        else
                        {
                            result.Code = -1;
                            result.StrCode = "發送廣播時出錯";
                        }
                    }

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }

                return result;

            }
            catch (Exception ex)
            {
                Log.Error("RevokeLiveBroadCast", "RevokeLiveBroadCast", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
