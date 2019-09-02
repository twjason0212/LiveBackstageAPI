using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestAPI20171114.Common;
using TestAPI20171114.Models;
using System.Web;
using System.Text;
using Newtonsoft.Json;

namespace TestAPI20171114.Controllers
{
    public class SetLiveSwitchController : ApiController
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

                var now = DateTime.Now;
                var liveId = request.Form["GameID"] ?? "";
                var action = request.Form["Type"] ?? "";
                var time = request.Form["Time"] ?? "";
                var closeTitle = request.Form["CloseTitle"] ?? "暂停播放";
                var closeContent = request.Form["CloseContent"] ?? "UU直播关闭中...";
                DateTime nextStartTime;

                if (string.IsNullOrEmpty(liveId) || (action.ToLower() != "true" && action.ToLower() != "false") ||
                    (!string.IsNullOrEmpty(time) && !DateTime.TryParse(time, out nextStartTime)))
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                using (var db = new livecloudEntities())
                {
                    ////驗證權限
                    //var operationManager = db.dt_Manager.Find(managerId);

                    //var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();

                    //if (operationManagerRole.LiveManage == false)
                    //{
                    //    result.Code = ResultHelper.NotAuthorized;
                    //    result.StrCode = ResultHelper.NotAuthorizedMsg;
                    //    return result;
                    //}

                    var live = db.dt_liveList.Where(l => l.liveId == liveId).FirstOrDefault();

                    if (live == null)
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = ResultHelper.ParamFailMsg;
                        return result;
                    }

                    //開放後先不更新state等global時間到處理state
                    live.state = (byte)0;
                    live.update_time = now;
                    live.CloseContent = (action.ToLower() == "true") ? "" : closeContent;
                    live.CloseTitle = (action.ToLower() == "true") ? "" : closeTitle;

                    if (!string.IsNullOrEmpty(time))
                    {
                        live.stop_time = time;
                    }
                    else
                    {
                        //開放且不帶時間直接開啟
                        live.state = (action.ToLower() == "true") ? (byte)1 : (byte)0;
                    }
                     

                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = managerId,
                        ManagerName = db.dt_Manager.Find(managerId).user_name,
                        ActionType = "setLiveSwitch",
                        AddTime = now,
                        IP = NetworkTool.GetClientIP(HttpContext.Current)
                    };

                    manageLog.Remarks = "设置直播ID:+" + liveId + "状态为:" + ((action == "true") ? "启用" : "停用");

                    db.dt_ManageLog.Add(manageLog);

                    db.SaveChanges();

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;

                    if (action.ToLower() == "false")
                    {

                        try
                        {
                            using (var client = new WebClient() { Encoding = Encoding.UTF8 })
                            {
                                var broadcastModel = new
                                {
                                    Target = "",
                                    GameID = liveId,
                                    Data = new
                                    {
                                        Type = "LiveType",
                                        CloseTitle = closeTitle ?? "暂停播放",
                                        CloseContent = closeContent ?? "UU直播关闭中..."
                                    }
                                };

                                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                                var data = "content=" + JsonConvert.SerializeObject(broadcastModel);
                                
                                var response = client.UploadString(Conf.WSUrl, "POST", data);
                                var broadcastResult = JsonConvert.DeserializeObject<Result>(response.ToString());

                                if (broadcastResult.code == 1)
                                {
                                    result.Code = ResultHelper.Success;
                                    result.StrCode = ResultHelper.SuccessMsg;
                                }
                                else
                                {
                                    result.Code = -2;
                                    result.StrCode = "数据已储存, 发送广播时出错";
                                }
                            }
                        }
                        catch (Exception te)
                        {
                            result.Code = -2;
                            result.StrCode = "数据已储存, 发送广播时出错";
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("SetLiveSwitch", "SetLiveSwitch", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
