using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Models;
using TestAPI20171114.Common;

namespace TestAPI20171114.Controllers
{
    public class GetAnchorController : ApiController
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

                    if (operationManagerRole.DealerList == false)
                    {
                        result.Code = ResultHelper.NotAuthorized;
                        result.StrCode = ResultHelper.NotAuthorizedMsg;
                        return result;
                    }
                }

                int anchorId = int.TryParse(request.Form["AnchorID"] ?? "", out anchorId) ? anchorId : -1;
                var anchorName = request.Form["Name"] ?? "";
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
                    var query = from a in db.dt_dealer
                                select a;

                    if (anchorId > 0)
                        query = query.Where(o => o.id == anchorId);

                    if (!string.IsNullOrEmpty(anchorName))
                        query = query.Where(o => o.dealerName == anchorName.Trim());

                    int totalCount = query.Count();

                    var list = query
                                .OrderBy(a => a.id)
                                .Skip(pageIndex * pageSize)
                                .Take(pageSize)
                                .ToList();

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                    result.DataCount = totalCount;
                    // 要確認TrimEnd等等的動作必要性
                    result.BackData = list.Select(a => new
                    {
                        ID = a.id,
                        AnchorID = a.id.ToString(),
                        Name = a.dealerName.TrimEnd(),
                        Sex = a.sex,
                        Age = a.age,
                        City = a.area.TrimEnd(),
                        Height = a.height,
                        BWH = a.bwh.TrimEnd(),
                        Weight = a.weight,
                        Image = a.img.TrimEnd(),
                        Photo = a.img2.TrimEnd(),
                        AddTime = a.add_time.ToString("yyyy-MM-dd HH:mm:ss")
                    }).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetAnchor", "GetAnchor", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
