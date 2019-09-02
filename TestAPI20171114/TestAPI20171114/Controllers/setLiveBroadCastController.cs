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
    public class setLiveBroadCastController : ApiController
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
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.NotLoginMsg;
                    result.IsLogin = ResultHelper.NotLogin;
                    return result;
                }

                var action = request.Form["Type"] ?? "";
                int broadcastId = (int.TryParse(request.Form["BroadCastID"] ?? "", out broadcastId)) ? broadcastId : -1;
                int liveId = (int.TryParse(request.Form["GameID"] ?? "", out liveId)) ? liveId : -1;
                var now = DateTime.Now;
                DateTime startTime = DateTime.TryParse(request.Form["StartTime"] ?? "", out startTime) ? startTime : now;
                DateTime endTime = DateTime.TryParse(request.Form["EndTime"] ?? "", out endTime) ? endTime : now;

                var content = request.Form["Content"] ?? "";

                ////驗證權限(不確定是否為相應的欄位)
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

                switch (action.ToLower())
                {
                    case "add":
                        {
                            if (liveId <= 0 || string.IsNullOrEmpty(content) || endTime <= startTime)
                            {
                                result.Code = ResultHelper.ParamFail;
                                result.StrCode = ResultHelper.ParamFailMsg;
                                return result;
                            }
                            break;
                        }

                    case "edit":
                        {
                            if (broadcastId <= 0 || string.IsNullOrEmpty(content) || endTime <= startTime)
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
                        ActionType = "setLiveBroadCast",
                        AddTime = now,
                        IP = NetworkTool.GetClientIP(HttpContext.Current)
                    };

                    dt_AdminBroadcastLog broadcast = null;

                    switch (action.ToLower())
                    {
                        case "add":
                            {
                                broadcast = new dt_AdminBroadcastLog()
                                {
                                    LiveId = liveId,
                                    ManagerId = managerId,
                                    SendTime = now,
                                    StartTime = startTime,
                                    EndTime = endTime,
                                    Status = (byte)1,
                                    BroadcastText = content
                                };

                                db.dt_AdminBroadcastLog.Add(broadcast);

                                manageLog.Remarks = "添加广播至直播:" + liveId + ", 内容:" + content;

                                break;
                            }

                        case "edit":
                            {
                                broadcast = db.dt_AdminBroadcastLog.Find(broadcastId);

                                if (broadcast == null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = ResultHelper.ParamFailMsg + " ID:" + broadcastId + "的广播不存在";
                                    return result;
                                }

                                if (!string.IsNullOrEmpty(content))
                                    broadcast.BroadcastText = content;

                                broadcast.StartTime = startTime;
                                broadcast.EndTime = endTime;

                                manageLog.Remarks = "修改广播ID:" + broadcastId +
                                    ", 新内容:" + content;

                                break;
                            }
                    }

                    db.dt_ManageLog.Add(manageLog);

                    var rowaffected = db.SaveChanges();

                    if (rowaffected > 0)
                    {
                        try
                        {
                            using (var client = new WebClient() { Encoding = Encoding.UTF8 })
                            {
                                var broadcastModel = new
                                {
                                    Target = "",
                                    GameID = liveId.ToString("0000"),
                                    Data = new
                                    {
                                        Type = "BroadCast",
                                        ID = broadcast.Id,
                                        Content = content,
                                        StartTime = startTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                        EndTime = endTime.ToString("yyyy-MM-dd HH:mm:ss")
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
                        }
                        catch (Exception te)
                        {
                            result.Code = -1;
                            result.StrCode = "發送廣播時出錯";
                        }
                    }
                    else
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = "執行時出錯";
                    }

                }

                return result;

            }
            catch (Exception ex)
            {
                Log.Error("setLiveBroadCast", "setLiveBroadCast", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }

    }
}
