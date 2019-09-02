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
    public class GetRoleInfoController : ApiController
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

                int roleId = int.TryParse(request.Form["ID"].ToString(), out roleId) ? roleId : -1;

                // 檢查參數
                if (roleId <= 0)
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                using (var db = new livecloudEntities())
                {
                    var role = db.dt_ManagerRole.Find(roleId);
                   
                    if (role == null)
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = ResultHelper.ParamFailMsg + " ID:" + roleId + "的角色不存在";
                        return result;
                    }

                    var roleData = new
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
                        realTimeBarrage = role.RealTimeBarrage.ToOnOff(),
                        AllowIp = role.AllowIp.ToOnOff()

                    };
                    
                    result.BackData = new
                    {
                        Id = roleId,
                        RoleName = role.RoleName,
                        BodyData = roleData
                    };

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetRoleInfo", "GetRoleInfo", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }

    }
}
