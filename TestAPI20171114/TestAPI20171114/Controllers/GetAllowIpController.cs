using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Common;
using TestAPI20171114.Models;

namespace TestAPI20171114.Controllers
{
    public class GetAllowIpController : ApiController
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

                //驗證權限
                using (var db = new livecloudEntities())
                {
                    var operationManager = db.dt_Manager.Find(managerId);

                    var operationManagerRole = Cache.Role.Where(o => o.Id == operationManager.admin_role).FirstOrDefault();

                    if (operationManagerRole.AllowIp == false)
                    {
                        result.Code = ResultHelper.NotAuthorized;
                        result.StrCode = ResultHelper.NotAuthorizedMsg;
                        return result;
                    }
                }

                int id = int.TryParse(request.Form["Id"] ?? "", out id) ? id : -1;
                var allowIp = request.Form["Iplimit"] ?? "";
                var remark = request.Form["Remark"] ?? "";
                int pageIndex = int.TryParse(request.Form["PageIndex"] ?? "", out pageIndex) ? pageIndex : 0;
                int pageSize = int.TryParse(request.Form["PageSize"] ?? "", out pageSize) ? pageSize : 10;

                if (pageIndex < 0 || pageSize <= 0)
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                // 可以考慮做快取
                using (var db = new livecloudEntities())
                {
                    var query = from a in db.dt_AllowAccessIPList
                                select a;

                    if (id > 0)
                        query = query.Where(o => o.Id == id);

                    if (!string.IsNullOrEmpty(allowIp))
                        query = query.Where(o => o.Ip == allowIp.Trim());

                    int totalCount = query.Count();

                    var list = query
                                .OrderBy(a => a.Id)
                                .Skip(pageIndex * pageSize)
                                .Take(pageSize)
                                .ToList();

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                    result.DataCount = totalCount;
                    // 要確認TrimEnd等等的動作必要性
                    result.BackData = list.Select(a => new
                    {
                        Id = a.Id,
                        Iplimit = a.Ip.ToString(),
                        Remark = a.Remark?.TrimEnd(),
                        AddTime = a.AddTime?.ToString("yyyy-MM-dd HH:mm:ss")
                    }).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetAllowIpList", "GetAllowIpList", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
