using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestAPI20171114.Models;
using TestAPI20171114.Common;
using System.Web;
using TestAPI20171114.Models.Request;

namespace TestAPI20171114.Controllers
{
    public class ManageLogController : ApiController
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
                
                var userName = request.Form["UserName"] ?? "";
                var actionType = request.Form["Type"] ?? "";
                int pageSize = int.TryParse(request.Form["PageSize"] ?? "", out pageSize) ? pageSize : 10;
                int pageIndex = int.TryParse(request.Form["PageIndex"] ?? "", out pageIndex) ? pageIndex : 0;
                ////驗證權限
                //using (var db = new livecloudEntities())
                //{
                //    var operationManager = db.dt_Manager.Find(managerId);

                //    var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();

                //    if (operationManagerRole.ManageLog == false)
                //    {
                //        result.Code = ResultHelper.NotAuthorized;
                //        result.StrCode = ResultHelper.NotAuthorizedMsg;
                //        return result;
                //    }
                //}

                if (pageSize <= 0 || pageIndex < 0)
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }


                using (var db = new livecloudEntities())
                {
                    var query = from o in db.dt_ManageLog
                                select o;

                    if (!string.IsNullOrEmpty(userName))
                        query = query.Where(o => o.ManagerName == userName);

                    if (!string.IsNullOrEmpty(actionType))
                        query = query.Where(o => o.ActionType == actionType);

                    var totalCount = query.Count();

                    var list = query
                        .OrderByDescending(o => o.Id)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize)
                        .ToList()
                        .Select(o => new ManageLog()
                        {
                            IP = o.IP,
                            ID = o.Id,
                            Mark = o.Remarks,
                            Time = o.AddTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            Type = o.ActionType,
                            UserName = o.ManagerName
                        })
                        .ToList();

                    result.DataCount = totalCount;
                    result.BackData = list;

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("ManageLog", "ManageLog", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
