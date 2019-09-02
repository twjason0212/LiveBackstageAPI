using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestAPI20171114.Models
{
    public class RedisConfig
    {
        public static RedisClientConfig redisClientConfig
        {
            get
            {
                RedisClientConfig _redisClientConfig = new RedisClientConfig();
                _redisClientConfig.ServerIP = "40.83.121.38";
                _redisClientConfig.Password = "";
                _redisClientConfig.ServerPort = 6379;
                return _redisClientConfig;
            }
        }
        /// <summary>
        /// Redis消息发送服务
        /// </summary>
        public static PublishSubscribeService _rublishSubscribeService;
        public static PublishSubscribeService publishSubscribeService
        {
            get
            {
                if (_rublishSubscribeService == null)
                {
                    _rublishSubscribeService = new PublishSubscribeService();
                }
                _rublishSubscribeService.ServerIP = "40.83.121.38";
                _rublishSubscribeService.Password = "";
                _rublishSubscribeService.ServerPort = 6379;
                return _rublishSubscribeService;
            }
        }

    }
}