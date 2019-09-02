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
    public class LogoutController : ApiController
    {
        [HttpPost]
        public ResultInfoT<object> Post()
        {
            var result = new ResultInfoT<object>() { IsLogin = ResultHelper.NotLogin };
            var request = HttpContext.Current.Request;
            var session = HttpContext.Current.Session;

            try
            {
                // 缺少Log紀錄

                //int managerId = (int)(session["ManagerId"] ?? -1);

                //if (managerId > 0)
                //{
                //    session.Clear();
                //    session.Abandon();

                //    result.Code = ResultHelper.Success;
                //    result.StrCode = "退出成功！";

                //    return result;
                //}

                session.Clear();
                session.Abandon();

                result.Code = ResultHelper.Success;
                result.StrCode = "退出成功！";
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Logout", "Logout", ex.Message.ToString());
                result.Code = ResultHelper.ExecutingError;
                result.StrCode = ResultHelper.ExecutingErrorMsg;
                return result;
            }
        }
    }
}
