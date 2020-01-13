using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestAPI20171114.Models;

namespace TestAPI20171114.Controllers
{
    public class GetBlackWordManageController : ApiController
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

                string action = request.Form["Action"] ?? "";
                string content = request.Form["Content"] ?? "";
                int pageIndex = int.TryParse(request.Form["PageIndex"] ?? "", out pageIndex) ? pageIndex : 0;
                int pageSize = int.TryParse(request.Form["PageSize"] ?? "", out pageSize) ? pageSize : 20;
                DateTime now = DateTime.Now;

                if (pageIndex < 0 || pageSize <= 0)
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                using (var db = new livecloudEntities())
                {
                    var query = from b in db.dt_BlackWords
                                select b;
                   
                    if (!string.IsNullOrEmpty(content))
                        query = query.Where(b => b.content.Contains(content));

                    int totalCount = query.Count();

                    var list = query
                                .OrderByDescending(b => b.addtime)
                                .Skip(pageIndex * pageSize)
                                .Take(pageSize)
                                .ToList();

                    var managerList = db.dt_Manager.ToList();

                    result.DataCount = totalCount;

                    // 要確認TrimEnd等等的動作必要性
                    result.BackData = list.Select(b => new
                    {
                        ID = b.id,
                        Content = b.content.ToString(),
                        AddTime = b.addtime.ToString("yyyy-MM-dd HH:mm:ss"),
                        Staff = b.adminname.ToString(),
                        Remark = b.remark.ToString()
                    }).ToList();

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }

                return result;

            }
            catch
            {
                return result;
            }
        }

    }
}
