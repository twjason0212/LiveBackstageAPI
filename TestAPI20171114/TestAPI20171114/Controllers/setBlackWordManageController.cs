using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Common;
using TestAPI20171114.Models;

namespace TestAPI20171114.Controllers
{
    public class SetBlackWordManageController : ApiController
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

                string action = request.Form["Action"] ?? "";
                int id = int.TryParse(request.Form["ID"] ?? "", out id) ? id : 0;
                string content = request.Form["Content"] ?? "";
                string remark = request.Form["Remark"] ?? "";
                string type = request.Form["Type"] ?? "";
                DateTime now = DateTime.Now;

                switch (type.ToLower())
                {
                    case "add":
                        {
                            if (string.IsNullOrEmpty(content))
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

                using (livecloudEntities db = new livecloudEntities())
                {
                    var manageLog = new dt_ManageLog()
                    {
                        ManagerId = managerId,
                        ManagerName = db.dt_Manager.Find(managerId).user_name,
                        ActionType = "setBlackWordManage",
                        AddTime = now,
                        IP = NetworkTool.GetClientIP(HttpContext.Current)
                    };

                    if (!string.IsNullOrEmpty(content))
                    {
                        var dupcontent = type == "add" ? db.dt_BlackWords.Where(o => o.content == content.Trim()).FirstOrDefault() :
                            db.dt_BlackWords.Where(o => o.content == content.Trim() & o.id != id).FirstOrDefault();

                        if (dupcontent != null)
                        {
                            result.Code = ResultHelper.ParamFail;
                            result.StrCode = "已存在相同名称的黑词！";
                            return result;
                        }
                    }

                    switch (type.ToLower())
                    {
                        case "add":
                            {
                                var black = new dt_BlackWords
                                {
                                    content = content.Trim(),
                                    state = 5,
                                    remark = remark.Trim(),
                                    addtime = now,
                                    updatetime = now,
                                    adminid = manageLog.Id,
                                    adminname = manageLog.ManagerName
                                };

                                db.dt_BlackWords.Add(black);
                                db.SaveChanges();

                                manageLog.Remarks = "添加黑詞名单ID:" + black.id + ", 內容:" + content;

                                break;
                            }
                        case "edit":
                            {
                                var black = db.dt_BlackWords.Where(o => o.content == content.Trim() && o.id != id).FirstOrDefault();

                                if (black != null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = "已有相同内容黑词名单！";
                                    return result;
                                }

                                black = db.dt_BlackWords.Find(id);
                                if (black == null)
                                {
                                    result.Code = ResultHelper.ParamFail;
                                    result.StrCode = "找不到ID:" + id + "的数据！";
                                    return result;
                                }

                                if (!string.IsNullOrEmpty(content))
                                    black.content = content.Trim();

                                if (!string.IsNullOrEmpty(remark))
                                    black.remark = remark.Trim();

                                black.updatetime = now;
                                black.adminid = manageLog.Id;
                                black.adminname = manageLog.ManagerName;

                                manageLog.Remarks = "修改黑詞名单ID:" + black.id + ", 內容:" + content;

                                break;
                            }
                    }

                    db.dt_ManageLog.Add(manageLog);
                    db.SaveChanges();
                    UpdateMsg.PostUpdate("dt_BlackWords");

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
