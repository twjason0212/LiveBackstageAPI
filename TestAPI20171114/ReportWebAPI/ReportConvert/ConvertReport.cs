using ReportWebAPI.ReportViewModel;
using ReportWebAPI.ReportChangeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportWebAPI.ReportCommon;

namespace ReportWebAPI.ReportConvert
{
    public static class ConvertReport
    {
        //等級報表轉換
        public static List<ReportGroup> ConvertReportGroupModel(List<GradeReport> models) 
        {
            List<ReportGroup> reportGroups = new List<ReportGroup>();
            foreach(var model in models)
            {
                ReportGroup reportGroup = new ReportGroup()
                {
                    group_name = model.GradeName,
                    BetNum = model.GradeBetNum,
                    PrepaidNum = model.GradeDepositNum,
                    PrepaidMoney = model.GradeDepositMoney,
                    OutMoney = model.GradeWithdrawMoney,
                    BetMoney = model.GradeBetMoney,
                    WinMoney = model.GradeWinMoney,
                    BetOrderNum = model.GradeBetCount,
                    ActivityDiscountAccountMoney = model.GradeActivityMoney,
                    Out_RebateAccount = model.GradeRebateMoney
                };
                reportGroups.Add(reportGroup);
            }

            return reportGroups;
        }

        //終端報表轉換
        public static List<ReportSource> ConvertReportSourceModel(List<SourceReport> models)
        {
            List<ReportSource> reportSources = new List<ReportSource>();
            foreach (var model in models)
            {
                ReportSource reportSource = new ReportSource()
                {
                    group_name = model.SourceName,
                    BetNum = model.SourceBetNum,
                    PrepaidNum = model.SourceDepositNum,
                    PrepaidMoney = model.SourceDepositMoney,
                    OutMoney = model.SourceWithdrawMoney,
                    BetMoney = model.SourceBetMoney,
                    WinMoney = model.SourceWinMoney,
                    BetOrderNum = model.SourceBetCount,
                    ActivityDiscountAccountMoney = model.SourceActivityMoney,
                    Out_RebateAccount = model.SourceRebateMoney,
                    SourceAdminRejectMoney = model.SourceAdminRejectMoney,
                    RegNum = model.SourceRegisterNum,
                    NewPrepaidNum = model.SourceNewPayNum
                };
                reportSources.Add(reportSource);
            }

            return reportSources;
        }

        //彩種報表轉換
        public static List<ReportLottery> ConvertReportLotteryModel(List<LotteryReport> models)
        {
            List<ReportLottery> reportLotterys = new List<ReportLottery>();
            foreach (var model in models)
            {
                ReportLottery reportLottery = new ReportLottery()
                {
                    BetMoney = model.LotteryBetMoney,
                    BetNum = model.LotteryBetNum,
                    CancelMoney = model.LotteryCancelMoney,
                    LotteryCode = model.LotteryCode,
                    LotteryName = model.LotteryName,
                    RebateMoney = model.LotteryRebateMoney,
                    WinMoney = model.LotteryWinMoney
                };
                reportLotterys.Add(reportLottery);
            }

            return reportLotterys;
        }

        //首充報表轉換
        public static List<ReportNewPays> ConvertReportNewPayModel(List<NewPaysReport> models)
        {
            List<ReportNewPays> reportGroups = new List<ReportNewPays>();
            foreach (var model in models)
            {
                ReportNewPays reportGroup = new ReportNewPays()
                {
                    UserName = model.UserName,
                    Level = model.UserGrade,
                    UpAgent = model.UserAgentName,
                    Balance = model.UserCapitalMoney,
                    RechargeMongy = model.UserDepositMoney,
                    RechargeClient = model.UserDepositSource,
                    RechargeWay = model.UserDepositType,
                    Time = model.UserDepositTime,
                    SignupTime = model.UserRegisterTime
                };
                reportGroups.Add(reportGroup);
            }

            return reportGroups;
        }
        
        //會員報表轉換
        public static List<ReportMember> ConvertReportMemberModel(List<MembersReport> models)
        {
            List<ReportMember> reportMembers = new List<ReportMember>();
            foreach (var model in models)
            {
                ReportMember reportMember = new ReportMember()
                {
                    UserName = model.UserName,
                    UserId = model.UserId.ToString(),
                    AgentName = model.UserAgentName,
                    Balance = model.UserCapital,
                    BetMoney = model.UserBetMoney,
                    DiscountMoney = model.UserActivityMoney,
                    GroupName = model.UserGrade,
                    InMoney = model.UserDepositMoney,
                    OutMoney = model.UserWithdrawMoney,
                    RebateMoney = model.UserRebateMoney,
                    RewardMoney = model.UserRewardMoney,
                    WinMoney = model.UserWinMoney,
                    ProfitLoss = model.UserProfitMoney,
                    ProfitRate = model.UserProfitRate
                };
                reportMembers.Add(reportMember);
            }

            return reportMembers;
        }

        //會員報表轉換
        public static List<ReportHistory> ConvertReportHistoryMemberModel(List<UsersHistoryReport> models)
        {
            List<ReportHistory> reportMembers = new List<ReportHistory>();
            foreach (var model in models)
            {
                ReportHistory reportMember = new ReportHistory()
                {
                   User_name = model.UserName,
                   groups = model.GroupId.ToString(),
                   ruknum=model.InCount,
                   chuknum = model.OutCount,
                   totalruk = model.InMoney,
                   totalchuk = model.OutMoney,
                   totalplay = model.BetMoney,
                   totalwin = model.WinMoney,
                   totalfd = model.RebateMoney,
                   totalyh = model.ActivityMoney,
                   TotalReward = model.RewardMoney,
                   Deficit = model.ProfitMoney,
                   zctime = model.RegisterTime
                   

                };
                reportMembers.Add(reportMember);
            }

            return reportMembers;
        }


        //代理報表轉換
        public static List<ReportAgent> ConvertReportAgentModel(List<AgentsReport> models)
        {
            List<ReportAgent> reportAgents = new List<ReportAgent>();
            foreach (var model in models)
            {
                ReportAgent reportAgent = new ReportAgent()
                {
                    UserName = model.UserName,
                    UserId = model.UserId.ToString(),
                    NewPrepaidNum = model.NewPayNum,
                    RebateAccount = model.AgentRebate,
                    RegNum = model.RegisterNum,
                    TotalBettingAccount = model.BetMoney,
                    TotalBettingNum = model.BetNum,
                    TotalDiscountAccount = model.ActivityMoney,
                    TotalInMoney = model.RechargeMoney,
                    TotalOutMoney = model.WithdrawMoney,
                    TotalRebateAccount = model.RebateMoney,
                    TotalWinningAccount = model.WinMoney,
                    ProfitLoss = model.ProfitMoney
                };
                reportAgents.Add(reportAgent);
            }

            return reportAgents;
        }

        //綜合報表轉換
        public static List<ReportIntegrated> ConvertReportIntegratedModel(List<IntegratedReport> models)
        {
            List<ReportIntegrated> reportIntegrateds = new List<ReportIntegrated>();
            foreach (var model in models)
            {
                ReportIntegrated reportIntegrated = new ReportIntegrated()
                {
                    InMoneyNum = Mapping.GetMoneyAndNumber(model.InCount, model.InNumber),
                    InMoneyTime = Mapping.FormatSecond(model.InOperateTime),//轉換時分秒
                    In_AlipayPrepaid = Mapping.GetMoneyAndNumber(model.AlipayMoney, model.AlipayNumber),
                    In_ArtificialDeposit = Mapping.GetMoneyAndNumber(model.ManualInMoney, model.ManualInNumber),
                    In_BankPrepaid = Mapping.GetMoneyAndNumber(model.BankpayMoney, model.BankpayNumber),
                    In_FastPrepaid = Mapping.GetMoneyAndNumber(model.FastpayMoney, model.FastpayNumber),
                    In_FourthPrepaid = Mapping.GetMoneyAndNumber(model.FourthpayMoney, model.FourthpayNumber),
                    In_QQPrepaid = Mapping.GetMoneyAndNumber(model.QQpayMoney, model.QQpayNumber),
                    In_UnionpayPrepaid = Mapping.GetMoneyAndNumber(model.UnionpayMoney, model.UnionpayNumber),
                    In_WeChatPrepaid = Mapping.GetMoneyAndNumber(model.WeChatpayMoney, model.WechatpayNumber),
                    LastMonthGrossRate = model.LastMonthGrossRate,
                    LastMonthOut_BettingAccount = Mapping.GetMoneyAndNumber(model.LastMonthBetMoney, 0),
                    LastMonthOut_WinningAccount = model.LastMonthWinMoney.ToString(),
                    LastMonthProfitAndLoss = (model.LastMonthBetMoney - model.LastMonthWinMoney).ToString(),
                    LastMonthProfitLoss = model.LastMonthProfit.ToString("0.00"),
                    LastMonthProfitRate = model.LastMonthProfitRate,
                    MonthProfitLoss = model.ThisMonthProfit.ToString("0.0"),
                    MonthGrossRate = model.ThisMonthGrossRate,
                    MonthOut_BettingAccount = Mapping.GetMoneyAndNumber(model.ThisMonthBetMoney, 0),
                    MonthOut_WinningAccount = model.ThisMonthWinMoney.ToString("0.0"),
                    MonthProfitAndLoss = (model.ThisMonthBetMoney - model.ThisMonthWinMoney).ToString("0.0"),
                    MonthProfitRate = model.ThisMonthProfitRate.ToString(),
                    NewMemberNum = model.RegisterNumber.ToString(),
                    NewPrepaidNum = model.NewPayNumber.ToString(),
                    NowProfit = model.Profit.ToString(),
                    NowProfitRate = model.ProfitRate.ToString(),
                    OnLinessNum = model.OnlineNumber.ToString(),
                    OutMoneyTime = Mapping.FormatSecond(model.OutOperateTime),
                    Out_Account = Mapping.GetMoneyAndNumber(model.OutMoney, model.OutNumber),
                    Out_Account_Num = Mapping.GetMoneyAndNumber(model.OutCount, model.OutNumber),
                    Out_ActivityDiscountAccount = Mapping.GetMoneyAndNumber(model.SystemActivityMoney, model.SystemActivityNumber),
                    Out_AdministrationAccount = Mapping.GetMoneyAndNumber(model.AdminMoney, model.AdminNumber),
                    Out_BettingAccount = Mapping.GetMoneyAndNumber(model.BetMoney, model.BetNumber),
                    Out_BettingAccount_Num = Mapping.GetMoneyAndNumber(model.BetCount, model.BetNumber),
                    Out_CancelAccount = Mapping.GetMoneyAndNumber(model.CancelMoney, model.CancelNumber),
                    Out_MistakeAccount = Mapping.GetMoneyAndNumber(model.MistakeMoney, model.MistakeNumber),
                    Out_OtherDiscountAccount = Mapping.GetMoneyAndNumber(model.OtherActivityMoney, model.OtherActivityNumber),
                    Out_RebateAccount = Mapping.GetMoneyAndNumber(model.RebateMoney, model.RebateNumber),
                    Out_RefuseAccount = Mapping.GetMoneyAndNumber(model.RejectMoney, model.RejectNumber),
                    Out_WinningAccount = Mapping.GetMoneyAndNumber(model.WinMoney, model.WinNumber),
                    RewardCount = Mapping.GetMoneyAndNumber(model.RewardCount, model.RewardNumber),
                    RewardMoney = Mapping.GetMoneyAndNumber(model.RewardMoney, model.RewardNumber),
                    TotalArtificialOut = Mapping.GetMoneyAndNumber(model.ManualOutMoney, model.ManualOutNumber),
                    TotalDiscount = Mapping.GetMoneyAndNumber(model.ActivityMoney, model.ActivityNumber),
                    TotalInMoney = Mapping.GetMoneyAndNumber(model.InMoney, model.InNumber),
                    TotlaBalance = model.UsersCapital.ToString()
                };
                reportIntegrateds.Add(reportIntegrated);
            }

            return reportIntegrateds;
        }

        //平台站長報表轉換
        public static List<ReportPlatform> ConvertReportPlatformModel(List<PlatformReport> models)
        {
            List<ReportPlatform> reportIntegrateds = new List<ReportPlatform>();
            foreach (var model in models)
            {
                ReportPlatform reportIntegrated = new ReportPlatform()
                {
                    AddDate = model.DateRange,
                    identityid = model.IdentityId,
                    identityName = model.IdentityName,
                    Out_BettingAccount = model.BetMoney,
                    In_WinningAccount = model.WinMoney,
                    In_AccountCount = model.InCount,
                    In_Account = model.InMoney,
                    Out_AccountCount = model.OutCount,
                    Out_Account = model.OutMoney,
                    Out_ActivityDiscountAccount = model.ActivityMoney,
                    Out_BettingAccountCount=model.BetCount,
                    RewardMoney = model.RewardMoney,
                    Out_BettingAccountNum = model.BetNumber,
                    In_AccountNum = model.InNumber,
                    NewMemberNum = model.RegisterNumber,
                    NewPrepaidNum = model.NewPayNumber,
                    RewardNum = model.RewardNumber,
                    ProfitLoss = model.Gross.ToString(),
                    ProfitRate = model.ProfitRate

                };
                reportIntegrateds.Add(reportIntegrated);
            }

            return reportIntegrateds;
        }

        //直播報表轉換
        public static return_anchor_tip_record ConvertReportAnchorsModel(AnchorListReport models)
        {
            return_anchor_tip_record reportAgents = new return_anchor_tip_record();
            reportAgents.anchor_list = new List<dt_anchor_tip_record>();
            foreach (var model in models.AnchorList)
            {
                dt_anchor_tip_record reportAgent = new dt_anchor_tip_record()
                {
                   anchor_id = model.AnchorId,
                   tipmoney_lastmonth = model.TipMoneyLastMonth,
                   tipmoney_thismonth = model.TipMoneyThisMonth,
                   tipmoney_today = model.TipMoneyToday,
                   tipmoney_total = model.TipMoneyTotal
                };
                reportAgents.anchor_list.Add(reportAgent);
            }
            reportAgents.anchor_count = models.AnchorCount;
            return reportAgents;
        }

    }
}
