﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportModel
{
    /// <summary>
    /// 下級團隊報表
    /// </summary>
    public class AgentReport
    {
        /// <summary>
        /// 會員ID
        /// </summary>        
        public string UserId { get; set; }
        /// <summary>
        /// 會員帳號
        /// </summary>        
        public string UserName { get; set; }
        /// <summary>
        /// 是否代理
        /// </summary>        
        public byte IsAgent { get; set; }
        /// <summary>
        /// 團隊內階級
        /// </summary>        
        public int Uclass { get; set; }
        /// <summary>
        /// 下級階級/類型
        /// </summary>
        public string UserType { get; set; }
        /// <summary>
        /// 團隊投注人數
        /// </summary>        
        public int BetNum { get; set; }
        /// <summary>
        /// 團隊投注金額
        /// </summary>        
        public decimal BetMoney { get; set; }
        /// <summary>
        /// 團隊中獎金額
        /// </summary>        
        public decimal BonusMoney { get; set; }
        /// <summary>
        /// 團隊返點金額
        /// </summary>        
        public decimal RebateMoney { get; set; }
        /// <summary>
        /// 團隊活動金額
        /// </summary>        
        public decimal ActivityMoney { get; set; }
        /// <summary>
        /// 團隊盈利
        /// </summary>        
        public decimal ProfitMoney { get; set; }

    }
}
