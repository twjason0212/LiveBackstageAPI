﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Models;
using TestAPI20171114.Common;

namespace TestAPI20171114.Controllers
{
    public class DelAnchorController : ApiController
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

                var now = DateTime.Now;
                var multiId = request.Form["ID"] ?? "";

                if (string.IsNullOrEmpty(multiId) || !(new Regex(@"^(([\d]{1,}){1}|(([\d]{1,}\,){1,}([\d]{1,}){1}))$").IsMatch(multiId)))
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }
                ////驗證權限
                //using (var db = new livecloudEntities())
                //{
                //    var operationManager = db.dt_Manager.Find(managerId);

                //    var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();
                    
                //    if (operationManagerRole.DealerManage == false)
                //    {
                //        result.Code = ResultHelper.NotAuthorized;
                //        result.StrCode = ResultHelper.NotAuthorizedMsg;
                //        return result;
                //    }
                //}

                var idList = multiId.Split(',').Select(o => Convert.ToInt32(o)).Distinct().OrderBy(o => o).ToList();

                using (var db = new livecloudEntities())
                {
                    var delAnchorList = db.dt_dealer.Where(o => idList.Contains(o.id)).ToList();

                    var manager = db.dt_Manager.Find(managerId);

                    foreach (var anchor in delAnchorList)
                    {
                        var manageLog = new dt_ManageLog()
                        {
                            ManagerId = managerId,
                            ManagerName = manager.user_name,
                            ActionType = "delAnchor",
                            AddTime = now,
                            Remarks = "删除主播:" + anchor.dealerName+"(ID:"+anchor.id+")",
                            IP = NetworkTool.GetClientIP(HttpContext.Current)
                        };
                        db.dt_ManageLog.Add(manageLog);
                    }

                    db.dt_dealer.RemoveRange(delAnchorList);

                    db.SaveChanges();
                    
                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("DelAnchor", "DelAnchor", ex.Message.ToString());                
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
