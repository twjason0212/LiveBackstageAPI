using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TestAPI20171114.Common;
using TestAPI20171114.Models;

namespace TestAPI20171114
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        System.Timers.Timer liveSwitchTimer = new System.Timers.Timer();
        System.Timers.Timer sendMessageTimer = new System.Timers.Timer();
        System.Timers.Timer barrageStatisticsTimer = new System.Timers.Timer();


        protected void Application_Start()
        {
            InitSystemBarrageStatistics();

            Log.Info("WebConf", "WebConf", "WS:", System.Configuration.ConfigurationManager.AppSettings["WSUrl"]);
            Log.Info("WebConf", "WebConf", "CN:", System.Configuration.ConfigurationManager.ConnectionStrings["livecloudEntities"].ConnectionString);

            //初始化報表uri
            Conf.ReportUri = System.Configuration.ConfigurationManager.AppSettings["ReportUri"] ?? "";


            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            liveSwitchTimer.AutoReset = false;
            liveSwitchTimer.Interval = 1000;
            liveSwitchTimer.Elapsed += new System.Timers.ElapsedEventHandler(aitoLiveSwitch_Elapsed);
            liveSwitchTimer.Start();

            sendMessageTimer.AutoReset = false;
            sendMessageTimer.Interval = 1000;
            sendMessageTimer.Elapsed += new System.Timers.ElapsedEventHandler(sendMessage_Elapsed);
            sendMessageTimer.Start();

            barrageStatisticsTimer.AutoReset = false;
            barrageStatisticsTimer.Interval = 5000;
            barrageStatisticsTimer.Elapsed += new System.Timers.ElapsedEventHandler(barrageStatistics_Elapsed);
            barrageStatisticsTimer.Start();

            TestAPI20171114.Models.Cache.refreshRole();

        }


        private void sendMessage_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                string msg = string.Empty;

                while (Conf.MseeageQueue.TryTake(out msg))
                {
                    UpdateMsg.SendMessage(msg);
                }

            }
            catch (Exception ex)
            {
                Log.Error("global", "sendMessage_Elapsed", ex.ToString());
            }
            finally
            {
                sendMessageTimer.Start();
            }
        }

        private void barrageStatistics_Elapsed(object ender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UpdateSystemBarrageStatistics();
            }
            catch (Exception ex)
            {
                Log.Error("global", "barrageStatistics_Elapsed", ex.ToString());
            }
            finally
            {
                barrageStatisticsTimer.Start();
            }
        }

        private void aitoLiveSwitch_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UpdateLiveStatus();
            }
            catch (Exception ex)
            {
                Log.Error("global", "aitoLiveSwitch_Elapsed", ex.ToString());
            }
            finally
            {
                liveSwitchTimer.Start();
            }
        }

        /// <summary>
        /// 更新直播狀態
        /// </summary>
        private static void UpdateLiveStatus()
        {
            using (var db = new livecloudEntities())
            {
                foreach (var live in db.dt_liveList.Where(s => s.stop_time != "").ToList())
                {
                    var livetime = Convert.ToDateTime(live.stop_time);
                    if (livetime <= DateTime.Now)
                    {
                        live.state = Convert.ToByte(1);
                        live.stop_time = "";
                        live.update_time = DateTime.Now;
                        db.SaveChanges();
                    }
                }
            }
        }

        private static bool SystemBarrageStatisticsInitOK = false;
        private static void InitSystemBarrageStatistics()
        {
            if (!SystemBarrageStatisticsInitOK)
            {
                try
                {
                    DateTime weekDate = DateTime.Now.AddDays(-7);

                    using (var db = new livecloudEntities())
                    {
                        db.Database.ExecuteSqlCommand("update dt_SystemBarrageLog set signal = 2 where add_time < @addTime and signal < 2",
                            new SqlParameter[] { new SqlParameter("@addTime", weekDate) });

                        if (db.ChangeTracker.HasChanges())
                        {
                            db.SaveChanges();
                        }

                        db.Database.ExecuteSqlCommand("update dt_SystemBarrageLog set signal = 1 where add_time >= @addTime",
                            new SqlParameter[] { new SqlParameter("@addTime", weekDate) });

                        if (db.ChangeTracker.HasChanges())
                        {
                            db.SaveChanges();
                        }
                    }

                    using (var db = new livecloudEntities())
                    {
                        var query = from o in db.dt_SystemBarrageLog
                                    where o.add_time > weekDate && o.signal == 1
                                    group o by o.barrageId into g
                                    select new
                                    {
                                        barrageId = g.Key,
                                        count = g.Count()
                                    };

                        var sysbarrageStatisticsList = query.ToList();

                        foreach (var barrageStatis in sysbarrageStatisticsList)
                        {
                            var sysbarrage = db.dt_SystemBarrage.Find(barrageStatis.barrageId);

                            if (sysbarrage != null)
                            {
                                sysbarrage.times = barrageStatis.count;
                            }
                        }

                        if (db.ChangeTracker.HasChanges())
                        {
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("global", "UpdateSystemBarrageStatistics", ex.ToString());
                }
                finally
                {
                    SystemBarrageStatisticsInitOK = true;
                }
            }
        }

        /// <summary>
        /// 更新系統彈幕次數
        /// </summary>
        private static void UpdateSystemBarrageStatistics()
        {
            try
            {
                DateTime weekDate = DateTime.Now.AddDays(-7);

                using (var db = new livecloudEntities())
                {
                    //增加
                    var listSystemBarrageLog = db.dt_SystemBarrageLog.Where(s => s.signal == 0 && s.add_time >= weekDate).ToList();

                    foreach (dt_SystemBarrageLog systemBarrageLog in listSystemBarrageLog)
                    {
                        dt_SystemBarrage editSystemBarrage = db.dt_SystemBarrage.Find(systemBarrageLog.barrageId);

                        if (editSystemBarrage != null)
                        {
                            editSystemBarrage.times++;
                            systemBarrageLog.signal = 1;
                        }
                    }

                    if (db.ChangeTracker.HasChanges())
                    {
                        db.SaveChanges();
                    }

                    //減少
                    var listSystemBarrageLogDiff = db.dt_SystemBarrageLog.Where(s => s.signal == 1 && s.add_time <= weekDate).ToList();

                    foreach (dt_SystemBarrageLog systemBarrageLog in listSystemBarrageLogDiff)
                    {
                        dt_SystemBarrage editSystemBarrage = db.dt_SystemBarrage.Find(systemBarrageLog.barrageId);

                        if (editSystemBarrage != null)
                        {
                            editSystemBarrage.times--;
                            systemBarrageLog.signal = 2;
                        }
                    }

                    if (db.ChangeTracker.HasChanges())
                    {
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("global", "UpdateSystemBarrageStatistics", ex.ToString());
            }
        }

        protected void Application_PostAuthorizeRequest()
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }
    }
}
