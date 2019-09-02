using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Models;
using TestAPI20171114.Common;
using System.Data.Entity.Infrastructure;

namespace TestAPI20171114.Controllers
{
    public class SetSentenceManageController : ApiController
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

                int id = int.TryParse(request.Form["ID"], out id) ? id : -1;
                byte state = byte.TryParse(request.Form["State"], out state) ? state : (byte)255; //0黑名单|1白名单
                var content = request.Form["Content"] ?? "";
                var remark = request.Form["Remark"] ?? "";
                var action = request.Form["Type"] ?? "";
                var now = DateTime.Now;

                switch (action.ToLower())
                {
                    case "add":
                        {
                            if (string.IsNullOrEmpty(content) || state == (byte)255)
                            {
                                result.Code = ResultHelper.ParamFail;
                                result.StrCode = ResultHelper.ParamFailMsg;
                                return result;
                            }
                            break;
                        }

                    case "edit":
                        {
                            if (id <= 0 ||
                               string.IsNullOrEmpty(content))
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

                    //if (operationManagerRole.SentenceManage == false)
                    //{
                    //    result.Code = ResultHelper.NotAuthorized;
                    //    result.StrCode = ResultHelper.NotAuthorizedMsg;
                    //    return result;
                    //}

                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = managerId,
                        ManagerName = db.dt_Manager.Find(managerId).user_name,
                        ActionType = "setSentenceManage",
                        AddTime = now,
                        IP = NetworkTool.GetClientIP(HttpContext.Current)
                    };

                    switch (action.ToLower())
                    {
                        case "add":
                            {
                                var sentence = db.dt_SensitiveSentences
                                    .Where(s => s.content == content.Trim())
                                    .FirstOrDefault();

                                if (sentence != null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = "已有相同内容之" + (sentence.state == 0 ? "黑" : "白") + "名单！";
                                    return result;
                                }

                                var blockWords = db.dt_SensitiveWords
                                    .Where(s => s.state == 0 && content.Trim().Contains(s.content))
                                    .FirstOrDefault();

                                if (blockWords != null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = "已有包含该内容之禁用词语！";
                                    return result;
                                }

                                sentence = new dt_SensitiveSentences()
                                {
                                    content = content.Trim(),
                                    remark = remark,
                                    state = state,
                                    addtime = now,
                                    updatetime = now,
                                    adminid = managerId,
                                    adminname = db.dt_Manager.Find(managerId).user_name
                                };

                                manageLog.Remarks = "添加" + ((state == 1) ? "白" : "黑") + "名单:" + content;

                                db.dt_SensitiveSentences.Add(sentence);

                                break;
                            }

                        case "edit":
                            {
                                var sentence = db.dt_SensitiveSentences
                                    .Where(s => s.content == content.Trim() && s.id != id)
                                    .FirstOrDefault();

                                if (sentence != null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = "已有相同内容之" + (sentence.state == 0 ? "黑" : "白") + "名单！";
                                    return result;
                                }

                                var blockWords = db.dt_SensitiveWords
                                    .Where(s => s.state == 0 && content.Trim().Contains(s.content))
                                    .FirstOrDefault();

                                if (blockWords != null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = "已有包含该内容之禁用词语！";
                                    return result;
                                }

                                sentence = db.dt_SensitiveSentences.Find(id);

                                if (sentence == null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = "找不到ID:" + id + "的数据！";
                                    return result;
                                }


                                if (!string.IsNullOrEmpty(content.Trim()))
                                    sentence.content = content.Trim();

                                if (!string.IsNullOrEmpty(remark.Trim()))
                                    sentence.remark = remark.Trim();

                                if (state < 255)
                                    sentence.state = state;

                                sentence.updatetime = now;
                                sentence.adminid = managerId;
                                sentence.adminname = db.dt_Manager.Find(managerId).user_name;

                                manageLog.Remarks = "修改黑白名单ID:" + sentence.id + ", 內容:" + content;

                                break;
                            }
                    }

                    db.dt_ManageLog.Add(manageLog);

                    db.SaveChanges();
                    UpdateMsg.PostUpdate("dt_SensitiveSentences");
                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (DbUpdateException ex)
            {
                Log.Info("SetSentenceManage", "SetSentenceManage", ex.InnerException.Message.ToString());
                result.Code = ResultHelper.ParamFail;
                result.StrCode = "已有重复的内容，请刷新页面";
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("SetSentenceManage", "SetSentenceManage", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
