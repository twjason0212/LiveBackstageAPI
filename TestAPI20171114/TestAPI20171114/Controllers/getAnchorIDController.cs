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
    public class GetAnchorIDController : ApiController
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

                int id = int.TryParse(request.Form["AnchorID"] ?? "", out id) ? id : -1;

                if (id < 0)
                {
                    result.Code = ResultHelper.ParamFail;
                    result.StrCode = ResultHelper.ParamFailMsg;
                    return result;
                }

                using (var db = new livecloudEntities())
                {
                    var anchor = db.dt_dealer.Find(id);

                    if (anchor == null)
                    {
                        result.Code = ResultHelper.ParamFail;
                        result.StrCode = ResultHelper.ParamFailMsg + " ID:" + id + "的主播不存在";
                        return result;
                    }

                    var tempList = new List<dt_dealer>();
                    tempList.Add(anchor);

                    result.BackData = tempList.Select(s => new
                    {
                        AnchorID = s.id,
                        Name = s.dealerName.TrimEnd(),
                        Sex = s.sex,
                        Age = s.age,
                        City = s.area.TrimEnd(),
                        Height = s.height,
                        BWH = s.bwh.TrimEnd(),
                        Weight = s.weight,
                        Image = s.img.TrimEnd(),
                        Photo = s.img2.TrimEnd(),
                        AddTime = s.add_time.ToString("yyyy-MM-dd HH:mm:ss")
                    }).ToList();

                    result.Code = ResultHelper.Success;
                    result.StrCode = ResultHelper.SuccessMsg;
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("GetAnchorID", "GetAnchorID", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
