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
    public class SetWordsManageController : ApiController
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
                int id = int.TryParse(request.Form["ID"], out id) ? id : -1;
                byte state = byte.TryParse(request.Form["State"], out state) ? state : (byte)255;
                var content = request.Form["Content"] ?? "";
                var remark = request.Form["Remark"] ?? "";
                var action = request.Form["Type"] ?? "";

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

                    //if (operationManagerRole.WordsManage == false)
                    //{
                    //    result.Code = ResultHelper.NotAuthorized;
                    //    result.StrCode = ResultHelper.NotAuthorizedMsg;
                    //    return result;
                    //}

                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = managerId,
                        ManagerName = db.dt_Manager.Find(managerId).user_name,
                        ActionType = "setSystemBarrage",
                        AddTime = now,
                        IP = NetworkTool.GetClientIP(HttpContext.Current)
                    };

                    switch (action.ToLower())
                    {
                        case "add":
                            {
                                var dupWords = db.dt_SensitiveWords
                                    .Where(s => s.content == content.Trim() || (s.state == 0 && content.Trim().Contains(s.content)))
                                    .FirstOrDefault();

                                if (dupWords != null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = (dupWords.content == content.Trim())
                                        ? "已有相同内容的" + ((dupWords.state == 0) ? "禁用" : "敏感") + "词语！"
                                        : "已有包含该内容之禁用词语！";
                                    return result;
                                }

                                var words = new dt_SensitiveWords()
                                {
                                    content = content,
                                    remark = remark,
                                    state = state,
                                    addtime = now,
                                    updatetime = now,
                                    adminid = managerId,
                                    adminname = db.dt_Manager.Find(managerId).user_name
                                };

                                db.dt_SensitiveWords.Add(words);

                                manageLog.Remarks = "添加" + (state == 0 ? "禁用" : "敏感" + "词语:" + content);

                                break;
                            }

                        case "edit":
                            {
                                var words = db.dt_SensitiveWords
                                    .Where(s => (s.content == content.Trim() && s.id != id) || (s.state == 0 && content.Trim().Contains(s.content) && s.id != id))
                                    .FirstOrDefault();

                                if (words != null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = (words.content == content.Trim())
                                        ? "已有相同内容的" + ((words.state == 0) ? "禁用" : "敏感") + "词语！"
                                        : "已有包含该内容之禁用词语！";
                                    return result;
                                }

                                words = db.dt_SensitiveWords.Find(id);

                                if (words == null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = "找不到ID:" + id + "的数据！";
                                    return result;
                                }

                                if (!string.IsNullOrEmpty(content))
                                    words.content = content;

                                if (!string.IsNullOrEmpty(remark))
                                    words.remark = remark;

                                if (state < 255)
                                    words.state = state;

                                words.updatetime = now;
                                words.adminid = managerId;
                                words.adminname = db.dt_Manager.Find(managerId).user_name;

                                manageLog.Remarks = "修改" + (state == 0 ? "禁用" : "敏感" + "词语:" + content);

                                break;
                            }
                    }

                    db.dt_ManageLog.Add(manageLog);

                    db.SaveChanges();
                    UpdateMsg.PostUpdate("dt_SensitiveWords");
                }

                result.Code = ResultHelper.Success;
                result.StrCode = ResultHelper.SuccessMsg;

                return result;
            }
            catch (DbUpdateException ex)
            {
                Log.Info("SetWordsManage", "SetWordsManage", ex.InnerException.Message.ToString());
                result.Code = ResultHelper.ParamFail;
                result.StrCode = "已有重复的内容，请刷新页面";
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("SetWordsManage", "SetWordsManage", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
