using System.Linq;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Models;
using System.Collections.Concurrent;
using TestAPI20171114.Common;

namespace TestAPI20171114.Controllers
{
    public class apiController : ApiController
    {
        private static readonly ConcurrentDictionary<string, bool> dicAllowIP;

        static apiController()
        {
            dicAllowIP = new ConcurrentDictionary<string, bool>();
            using (livecloudEntities db = new Models.livecloudEntities())
            {
                var allowIpList = db.dt_AllowAccessIPList.ToList();
                foreach (var allowIp in allowIpList)
                {
                    if (!dicAllowIP.ContainsKey(allowIp.Ip))
                        dicAllowIP.TryAdd(allowIp.Ip, true);
                }
            }
        }

        public static bool AddIpList(string ip)
        {
            if (!dicAllowIP.ContainsKey(ip))
            {
                dicAllowIP.TryAdd(ip, true);
                return true;
            }

            return false;

        }

        public static bool RemoveIpList(string ip)
        {
            bool removeSignal = false;

            if(dicAllowIP.ContainsKey(ip))
            {
                dicAllowIP.TryRemove(ip, out removeSignal);
                return removeSignal;
            }

            return removeSignal;
        }

        public static bool EditIpList(string oldIp,string newIp)
        {

            if(dicAllowIP.ContainsKey(oldIp))
            {
                RemoveIpList(oldIp);
                AddIpList(newIp);
                return true;
            }

            return false;
            
        }

        [HttpPost]
        public ResultInfoT<object> Post()
        {
            var result = new ResultInfoT<object>() { IsLogin = ResultHelper.NotLogin };
            var request = HttpContext.Current.Request;
            var session = HttpContext.Current.Session;

            var action = request.Form["Action"] ?? "";

            var IP = NetworkTool.GetClientIP(HttpContext.Current);

            bool allowIP = false;



            if (dicAllowIP.TryGetValue(IP, out allowIP) && dicAllowIP[IP] == true)
            {
                switch (action)
                {
                    #region Authentication

                    case "Login":
                        LoginController loginController = new LoginController();
                        result = loginController.Post();
                        break;

                    case "LogOut":
                        LogoutController logOutController = new LogoutController();
                        result = logOutController.Post();
                        break;

                    #endregion

                    #region Anchor

                    case "getAnchor":
                        GetAnchorController getAnchorController = new GetAnchorController();
                        result = getAnchorController.Post();
                        break;

                    case "setAnchor":
                        SetAnchorController setAnchorController = new SetAnchorController();
                        result = setAnchorController.Post();
                        break;

                    case "delAnchor":
                        DelAnchorController delAnchorController = new DelAnchorController();
                        result = delAnchorController.Post();
                        break;

                    case "getAnchorID":
                        GetAnchorIDController getAnchorIDController = new GetAnchorIDController();
                        result = getAnchorIDController.Post();
                        break;

                    #endregion

                    #region Manager

                    case "ManageList":
                        ManagerListController managerListController = new ManagerListController();
                        result = managerListController.Post();
                        break;

                    case "GetManager":
                        GetManagerController getManagerController = new GetManagerController();
                        result = getManagerController.Post();
                        break;

                    case "ManagerEdit":
                        ManagerEditController managerEditController = new ManagerEditController();
                        result = managerEditController.Post();
                        break;

                    case "ManageisLock":
                        LockManagerController lockManagerController = new LockManagerController();
                        result = lockManagerController.Post();
                        break;

                    case "delManager":
                        DelManagerController delManagerController = new DelManagerController();
                        result = delManagerController.Post();
                        break;

                    case "setPassword":
                        SetPasswordController setPasswordController = new SetPasswordController();
                        result = setPasswordController.Post();
                        break;

                    #endregion

                    #region Role

                    case "RoleManage":
                        GetRoleListController getRoleListController = new GetRoleListController();
                        result = getRoleListController.Post();
                        break;

                    case "GetRoleInfo":
                        GetRoleInfoController getRoleInfoController = new GetRoleInfoController();
                        result = getRoleInfoController.Post();
                        break;

                    case "setRole":
                        SetRoleController setRoleController = new SetRoleController();
                        result = setRoleController.Post();
                        break;

                    case "delRole":
                        DelRoleController delRoleController = new DelRoleController();
                        result = delRoleController.Post();
                        break;

                    #endregion

                    #region Live

                    case "getLiveList":
                        GetLiveListController getLiveListController = new GetLiveListController();
                        result = getLiveListController.Post();
                        break;

                    case "setLiveSwitch":
                        SetLiveSwitchController setLiveSwitchController = new SetLiveSwitchController();
                        result = setLiveSwitchController.Post();
                        break;

                    #endregion

                    #region SystemBarrage

                    case "getSystemBarrageList":
                        GetSystemBarrageListController getSystemBarrageListController = new GetSystemBarrageListController();
                        result = getSystemBarrageListController.Post();
                        break;

                    case "setSystemBarrage":
                        SetSystemBarrageController setSystemBarrageController = new SetSystemBarrageController();
                        result = setSystemBarrageController.Post();
                        break;

                    case "lockSystemBarrage":
                        LockSystemBarrageController lockSystemBarrageController = new LockSystemBarrageController();
                        result = lockSystemBarrageController.Post();
                        break;

                    case "clearSystemBarrageTimes":
                        ClearSystemBarrageTimesController clearSystemBarrageTimesController = new ClearSystemBarrageTimesController();
                        result = clearSystemBarrageTimesController.Post();
                        break;

                    case "delSystemBarrage":
                        DelSystemBarrageController delSystemBarrageController = new DelSystemBarrageController();
                        result = delSystemBarrageController.Post();
                        break;

                    #endregion

                    #region SentenceManage

                    case "getSentenceManageList":
                        GetSentenceManageListController getSentenceManageListController = new GetSentenceManageListController();
                        result = getSentenceManageListController.Post();
                        break;

                    case "setSentenceManage":
                        SetSentenceManageController setSentenceManageController = new SetSentenceManageController();
                        result = setSentenceManageController.Post();
                        break;

                    case "delSentenceManage":
                        DelSentenceManageController delSentenceManageController = new DelSentenceManageController();
                        result = delSentenceManageController.Post();
                        break;

                    #endregion

                    #region SentenceWords

                    case "getWordsManageList":
                        GetWordsManageListController getWordsManageListController = new GetWordsManageListController();
                        result = getWordsManageListController.Post();
                        break;

                    case "setWordsManage":
                        SetWordsManageController setWordsManageController = new SetWordsManageController();
                        result = setWordsManageController.Post();
                        break;

                    case "delWordsManage":
                        DelWordsManageController delWordsManageController = new DelWordsManageController();
                        result = delWordsManageController.Post();
                        break;

                    #endregion

                    #region ManualReview

                    case "getManualReviewList":
                        GetManualReviewListController getManualReviewListController = new GetManualReviewListController();
                        result = getManualReviewListController.Post();
                        break;

                    case "setManualReview":
                        SetManualReviewController setManualReviewController = new SetManualReviewController();
                        result = setManualReviewController.Post();
                        break;

                    #endregion

                    #region LiveNotSpeak 用戶禁言相關

                    case "getLiveNotSpeak":
                        GetLiveNotSpeakController getLiveNotSpeakController = new GetLiveNotSpeakController();
                        result = getLiveNotSpeakController.Post();
                        break;

                    case "setLiveNotSpeak":
                        SetLiveNotSpeakController setLiveNotSpeakController = new SetLiveNotSpeakController();
                        result = setLiveNotSpeakController.Post();
                        break;

                    case "delLiveNotSpeak":
                        DelLiveNotSpeakController delLiveNotSpeakController = new DelLiveNotSpeakController();
                        result = delLiveNotSpeakController.Post();
                        break;

                    #endregion LiveNotSpeak 用戶禁言相關

                    #region BroadCast 管理員公告

                    case "getBlackWordManage":
                        GetBlackWordManageController getBlackWordManageController = new GetBlackWordManageController();
                        result = getBlackWordManageController.Post();
                        break;

                    case "setBlackWordManage":
                        SetBlackWordManageController setBlackWordManage = new SetBlackWordManageController();
                        result = setBlackWordManage.Post();
                        break;

                    case "delBlackWordManage":
                        DelBlackWordManageController delBlackWordManage = new DelBlackWordManageController();
                        result = delBlackWordManage.Post();
                        break;

                    #endregion

                    #region BroadCast 管理員公告

                    case "getLiveBroadCastList":
                        GetLiveBroadCastListController getLiveBroadCastListController = new GetLiveBroadCastListController();
                        result = getLiveBroadCastListController.Post();
                        break;

                    case "setLiveBroadCast":
                        setLiveBroadCastController setLiveBroadCastController = new setLiveBroadCastController();
                        result = setLiveBroadCastController.Post();
                        break;

                    case "RevokeLiveBroadCast":
                        RevokeLiveBroadCastController revokeLiveBroadCastController = new RevokeLiveBroadCastController();
                        result = revokeLiveBroadCastController.Post();
                        break;

                    #endregion

                    #region 報表類

                    case "getAnchorTimeList":
                        GetAnchorTimeListController getAnchorTimeListController = new GetAnchorTimeListController();
                        result = getAnchorTimeListController.Post();
                        break;

                    case "getAnchorReport":
                        GetAnchorReportController getAnchorReportController = new GetAnchorReportController();
                        result = getAnchorReportController.Post();
                        break;

                    #endregion

                    #region 紀錄

                    case "ManageLog":
                        ManageLogController manageLogController = new ManageLogController();
                        result = manageLogController.Post();
                        break;

                    case "getGameRecord":
                        GetGameRecordController getGameRecordController = new GetGameRecordController();
                        result = getGameRecordController.Post();
                        break;

                    case "getShieldedRecord":
                        GetShieldedRecordController getShieldedRecordController = new GetShieldedRecordController();
                        result = getShieldedRecordController.Post();
                        break;

                    #endregion


                    #region IP限制
                    case "getAllowIp":
                        GetAllowIpController getAllowIpController = new GetAllowIpController();
                        result = getAllowIpController.Post();
                        break;

                    case "setAllowIp":
                        SetAllowIpController setAllowIpController = new SetAllowIpController();
                        result = setAllowIpController.Post();
                        break;

                    case "delAllowIp":
                        DelAllowIpController delAllowIpController = new DelAllowIpController();
                        result = delAllowIpController.Post();
                        break;
                    #endregion

                    default:
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = "无此接口！";
                        break;
                }
            }
            else
            {
                result.Code = ResultHelper.NotAllowIp;
                result.StrCode = ResultHelper.NotAllowIpMsg;
                Log.Error("AllowIP", "AllowIP", IP);
            }


            return result;
        }
    }
}
