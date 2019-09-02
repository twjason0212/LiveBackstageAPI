using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWebAPI.ReportChangeModel
{

    /// <summary>
    /// 彩种盈利统计表
    /// </summary>
    public class ReportLottery
    {
        /// <summary>
        /// 添加时间
        /// </summary>
        public virtual DateTime AddTime { get; set; }
        /// <summary>
        /// 投注人数
        /// </summary>
        public virtual decimal BetNum { get; set; }
        /// <summary>
        /// 投注金额
        /// </summary>
        public virtual decimal BetMoney { get; set; }
        /// <summary>
        /// 撤单金额
        /// </summary>
        public virtual decimal CancelMoney { get; set; }
        /// <summary>
        /// 站长ID
        /// </summary>
        public virtual string identityid { get; set; }
        /// <summary>
        /// 彩种编码
        /// </summary>
        public virtual string LotteryCode { get; set; }
        /// <summary>
        /// 彩种名称
        /// </summary>
        public virtual string LotteryName { get; set; }
        /// <summary>
        /// 盈利总额＝投注-中奖－返点
        /// </summary>
        public virtual decimal ProfitMoney
        {
            get { return (BetMoney - WinMoney - RebateMoney); }
            set { ProfitMoney = (BetMoney - WinMoney - RebateMoney); }
        }
        /// <summary>
        /// 盈率
        /// </summary>
        public virtual decimal ProfitRate
        {
            get { return BetMoney > 0 ? (ProfitMoney / BetMoney) * 100 : 0; }
            set { ProfitRate = BetMoney > 0 ? (ProfitMoney / BetMoney) * 100 : 0; }
        }
        /// <summary>
        /// 返点金额
        /// </summary>
        public virtual decimal RebateMoney { get; set; }
        /// <summary>
        /// 终端名称
        /// </summary>
        public virtual string SourceName { get; set; }
        /// <summary>
        /// 中奖金额
        /// </summary>
        public virtual decimal WinMoney { get; set; }
    }
}
