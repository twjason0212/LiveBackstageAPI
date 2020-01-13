using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestAPI20171114.Models
{
    public class ResultHelper
    {
        /// <summary>
        /// 1正常返回,且已登录
        /// </summary>
        public const int Success = 1;
        public const string SuccessMsg = "成功信息";

        /// <summary>
        ///  -1传入信息有误
        /// </summary>
        public const int ParamFail = -1;
        public const string ParamFailMsg = "传入信息有误！";
        
        /// <summary>
        ///  -2執行中出錯
        /// </summary>
        public const int ExecutingError = -2;
        public const string ExecutingErrorMsg = "执行中出错！";

        /// <summary>
        ///  -6Ip限制
        /// </summary>
        public const int NotAllowIp = -6;
        public const string NotAllowIpMsg = "您的IP未绑定无法登录";

        /// <summary>
        ///  0正常返回,且未登录
        /// </summary>
        public const bool NotLogin = false;
        public const string NotLoginMsg = "用户未登录！";

        /// <summary>
        ///  1正常返回,且登录
        /// </summary>
        public const bool IsLogin = true;
        public const string IsLoginMsg = "用户已登录！";

        /// <summary>
        ///  0没有此操作权限
        /// </summary>
        public const int NotAuthorized = 0;
        public const string NotAuthorizedMsg = "没有此操作权限！";
    }
}