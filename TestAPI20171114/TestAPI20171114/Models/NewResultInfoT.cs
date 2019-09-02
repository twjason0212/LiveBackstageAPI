using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestAPI20171114.Models
{
    public class NewResultInfoT<T>
    {
        /// <summary>
        /// 1正常返回,且已登录
        /// 0正常返回,且未登录
        /// 2用于提示需要验证码了(仅登录使用)
        /// -1传入信息有误
        /// -2后台处理错误
        /// -7系统维护
        /// -8账户冻结
        /// -9返点验证不通过(仅投注使用)
        /// </summary>
        public int Code
        {
            get;
            set;
        }
        public string StrCode
        {
            get;
            set;
        }
        public int DataCount
        {
            get;
            set;
        }
        public bool IsLogin
        {
            get;
            set;
        }
        public anchorReport<T> data
        {
            get;
            set;
        }

    }

    public class anchorReport<T>
    {
        public List<T> result
        {
            get;
            set;
        }
    }
}