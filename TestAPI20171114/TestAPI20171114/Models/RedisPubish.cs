using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestAPI20171114.Common;

namespace TestAPI20171114.Models
{
    public static class RedisPubish
    {
        public static string channelName = "bao2ininder";

        public static bool PublishRedis(string msg)
        {
            PublishSubscribeService publishSubscribeService = RedisConfig.publishSubscribeService;
            bool signal = false;

            try
            {
                publishSubscribeService.redisClient.PublishMessage(channelName, msg);
                signal = true;
            }
            catch (Exception ex)
            {
                Log.Error("redis", "pub", ex.Message);
            }
            return signal;

        }

    }
}