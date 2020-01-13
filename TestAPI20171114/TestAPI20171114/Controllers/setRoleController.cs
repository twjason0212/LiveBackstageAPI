using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using TestAPI20171114.Models;
using TestAPI20171114.Common;
using TestAPI20171114.Models.Request;

namespace TestAPI20171114.Controllers
{
    public class SetRoleController : ApiController
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

                int id = int.TryParse(request.Form["ID"] ?? "", out id) ? id : -1;
                var action = request.Form["Type"] ?? "";
                var name = request.Form["Name"] ?? "";
                var roleListJson = request.Form["RoleList"] ?? "";
                var now = DateTime.Now;

                PermissionsList permissionsList = null;

                try
                {
                    permissionsList = (string.IsNullOrEmpty(roleListJson))
                        ? null
                        : JsonConvert.DeserializeObject<PermissionsList>(roleListJson);
                }
                catch (Exception jsonEx)
                {
                    permissionsList = null;
                }

                switch (action.ToLower())
                {
                    case "add":
                        {
                            if (string.IsNullOrEmpty(name) ||
                                permissionsList == null)
                            {
                                result.Code = ResultHelper.ParamFail;
                                result.StrCode = ResultHelper.ParamFailMsg;
                                return result;
                            }
                            break;
                        }
                    case "edit":
                        {
                            if (id <= 0)
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
                    ////驗證權限(不確定是否為相應的欄位)
                    //var operationManager = db.dt_Manager.Find(managerId);

                    //var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();

                    //if (operationManagerRole.RoleManage == false)
                    //{
                    //    result.Code = ResultHelper.NotAuthorized;
                    //    result.StrCode = ResultHelper.NotAuthorizedMsg;
                    //    return result;
                    //}

                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = managerId,
                        ManagerName = db.dt_Manager.Find(managerId).user_name,
                        ActionType = "ManagerEdit",
                        AddTime = now,
                        IP = NetworkTool.GetClientIP(HttpContext.Current)
                    };

                    if (!string.IsNullOrEmpty(name))
                    {
                        var dupNameRole = action.ToLower()=="add" ? db.dt_ManagerRole.Where(a => a.RoleName == name.Trim()).FirstOrDefault() : db.dt_ManagerRole.Where(a => a.RoleName == name.Trim() & a.Id != id).FirstOrDefault();

                        if (dupNameRole != null)
                        {
                            result.Code = ResultHelper.ParamFail;
                            result.StrCode = "已存在相同名称的角色！";
                            return result;
                        }
                    }

                    switch (action.ToLower())
                    {
                        case "add":
                            {
                                var role = new dt_ManagerRole()
                                {
                                    RoleName = name,
                                    LiveCmsManage = permissionsList.liveCmsManage.ToBoolByOnOffString(),
                                    DealerManage = permissionsList.AnchorManage.ToBoolByOnOffString(),
                                    DealerList = permissionsList.AnchorList.ToBoolByOnOffString(),
                                    DealerPost = permissionsList.AnchorPost.ToBoolByOnOffString(),
                                    DealerTime = permissionsList.AnchorTime.ToBoolByOnOffString(),
                                    LiveManage = permissionsList.liveManage.ToBoolByOnOffString(),
                                    VideoList = permissionsList.videoList.ToBoolByOnOffString(),
                                    BarrageManage = permissionsList.barrageManage.ToBoolByOnOffString(),
                                    SystemBarrage = permissionsList.systemBarrage.ToBoolByOnOffString(),
                                    WordsManage = permissionsList.wordsManage.ToBoolByOnOffString(),
                                    ManualReview = permissionsList.manualReview.ToBoolByOnOffString(),
                                    GiftManage = permissionsList.giftManage.ToBoolByOnOffString(),
                                    GiftList = permissionsList.giftList.ToBoolByOnOffString(),
                                    DealerTable = permissionsList.AnchorTable.ToBoolByOnOffString(),
                                    Manager = permissionsList.Manager.ToBoolByOnOffString(),
                                    ManagerList = permissionsList.managerList.ToBoolByOnOffString(),
                                    RoleManage = permissionsList.roleManage.ToBoolByOnOffString(),
                                    ManageLog = permissionsList.manageLog.ToBoolByOnOffString(),
                                    ShieldedRecord = permissionsList.shieldedRecord.ToBoolByOnOffString(),
                                    LiveNotSpeak = permissionsList.liveNotSpeak.ToBoolByOnOffString(),
                                    BlackWordManage = permissionsList.blackWordManage.ToBoolByOnOffString(),
                                    RealTimeBarrage = permissionsList.realTimeBarrage.ToBoolByOnOffString(),
                                    AllowIp = permissionsList.AllowIp.ToBoolByOnOffString(),
                                    AddTime = now
                                };

                                db.dt_ManagerRole.Add(role);

                                manageLog.Remarks = "添加角色:" + name;

                                break;
                            }
                        case "edit":
                            {
                                var role = db.dt_ManagerRole.Find(id);

                                if (role == null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = "ID:" + id + "的角色不存在！";
                                    return result;
                                }

                                if (!string.IsNullOrEmpty(name))
                                    role.RoleName = name;

                                if (permissionsList != null)
                                {
                                    if (!string.IsNullOrEmpty(permissionsList.liveCmsManage))
                                        role.LiveCmsManage = permissionsList.liveCmsManage.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.AnchorManage))
                                        role.DealerManage = permissionsList.AnchorManage.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.AnchorList))
                                        role.DealerList = permissionsList.AnchorList.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.AnchorPost))
                                        role.DealerPost = permissionsList.AnchorPost.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.AnchorTime))
                                        role.DealerTime = permissionsList.AnchorTime.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.liveManage))
                                        role.LiveManage = permissionsList.liveManage.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.videoList))
                                        role.VideoList = permissionsList.videoList.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.barrageManage))
                                        role.BarrageManage = permissionsList.barrageManage.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.systemBarrage))
                                        role.SystemBarrage = permissionsList.systemBarrage.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.sentenceManage))
                                        role.SentenceManage = permissionsList.sentenceManage.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.wordsManage))
                                        role.WordsManage = permissionsList.wordsManage.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.manualReview))
                                        role.ManualReview = permissionsList.manualReview.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.giftManage))
                                        role.GiftManage = permissionsList.giftManage.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.giftList))
                                        role.GiftList = permissionsList.giftList.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.AnchorTable))
                                        role.DealerTable = permissionsList.AnchorTable.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.Manager))
                                        role.Manager = permissionsList.Manager.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.managerList))
                                        role.ManagerList = permissionsList.managerList.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.roleManage))
                                        role.RoleManage = permissionsList.roleManage.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.manageLog))
                                        role.ManageLog = permissionsList.manageLog.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.shieldedRecord))
                                        role.ShieldedRecord = permissionsList.shieldedRecord.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.liveNotSpeak))
                                        role.LiveNotSpeak = permissionsList.liveNotSpeak.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.blackWordManage))
                                        role.BlackWordManage = permissionsList.blackWordManage.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.realTimeBarrage))
                                        role.RealTimeBarrage = permissionsList.realTimeBarrage.ToBoolByOnOffString();

                                    if (!string.IsNullOrEmpty(permissionsList.AllowIp))
                                        role.AllowIp = permissionsList.AllowIp.ToBoolByOnOffString();
                                }
                                
                                manageLog.Remarks = "修改角色:" + name;

                                break;
                            }
                    }

                    db.dt_ManageLog.Add(manageLog);

                    db.SaveChanges();

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("SetRole", "SetRole", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
