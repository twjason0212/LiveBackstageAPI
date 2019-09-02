using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Redis;

namespace TestAPI20171114.Models
{
    public class PublishSubscribeService
    {
        public delegate Action<string, string> OnSubscriptionMessageEventHandler(string changel, string msg, HttpApplication CurrentHttpApplication);

        public delegate Action<string, string> OnSubscribeEventHandler(string changel, HttpApplication CurrentHttpApplication);

        public delegate Action<string, string> OnUnSubscribeEventHandler(string changel, HttpApplication CurrentHttpApplication);

        public delegate Action<string, string> OnPublicMessageEventHandler(string changel, string msg, HttpApplication CurrentHttpApplication);

        public delegate Action<string, string> OnPublicStartEventHandler();

        public delegate Action<string, string> OnPublicStopEventHandler();

        public delegate Action<string, string> OnPublicUnSubscribeEventHandler(string channel, HttpApplication CurrentHttpApplication);

        public delegate Action<string, string> OnPublicErrorEventHandler(Exception e);

        public delegate Action<string, string> OnPublicFailoverEventHandler(IRedisPubSubServer s);

        private HttpApplication _mainHttpApplication;

        private RedisClient _redisClient;

        private string _serverIP = "40.83.121.38";

        private int _ServerPort = 6379;

        private string _Password = "";

        private string _channelName = "bao2ininder";

        private event PublishSubscribeService.OnSubscriptionMessageEventHandler _onSubscriptionMessage;

        public event PublishSubscribeService.OnSubscriptionMessageEventHandler OnSubscriptionMessage
        {
            add
            {
                _onSubscriptionMessage += value;
            }
            remove
            {
                _onSubscriptionMessage -= value;
            }
        }

        private event PublishSubscribeService.OnSubscribeEventHandler _onSubscribe;

        public event PublishSubscribeService.OnSubscribeEventHandler OnSubscribe
        {
            add
            {
                _onSubscribe += value;
            }
            remove
            {
                _onSubscribe -= value;
            }
        }

        private event PublishSubscribeService.OnUnSubscribeEventHandler _onUnSubscribe;

        public event PublishSubscribeService.OnUnSubscribeEventHandler OnUnSubscribe
        {
            add
            {
                _onUnSubscribe += value;
            }
            remove
            {
                _onUnSubscribe -= value;
            }
        }

        private event PublishSubscribeService.OnPublicMessageEventHandler _onPublicMessage;

        public event PublishSubscribeService.OnPublicMessageEventHandler OnPublicMessage
        {
            add
            {
                _onPublicMessage += value;
            }
            remove
            {
                _onPublicMessage -= value;
            }
        }

        private event PublishSubscribeService.OnPublicStartEventHandler _onPublicStart;

        public event PublishSubscribeService.OnPublicStartEventHandler OnPublicStart
        {
            add
            {
                _onPublicStart += value;
            }
            remove
            {
                _onPublicStart -= value;
            }
        }

        private event PublishSubscribeService.OnPublicStopEventHandler _onPublicStop;

        public event PublishSubscribeService.OnPublicStopEventHandler OnPublicStop
        {
            add
            {
                _onPublicStop += value;
            }
            remove
            {
                _onPublicStop -= value;
            }
        }

        private event PublishSubscribeService.OnPublicUnSubscribeEventHandler _onPublicUnSubscribe;

        public event PublishSubscribeService.OnPublicUnSubscribeEventHandler OnPublicUnSubscribe
        {
            add
            {
                _onPublicUnSubscribe += value;
            }
            remove
            {
                _onPublicUnSubscribe -= value;
            }
        }

        private event PublishSubscribeService.OnPublicErrorEventHandler _onPublicError;

        public event PublishSubscribeService.OnPublicErrorEventHandler OnPublicError
        {
            add
            {
                _onPublicError += value;
            }
            remove
            {
                _onPublicError -= value;
            }
        }

        private event PublishSubscribeService.OnPublicFailoverEventHandler _onPublicFailover;

        public event PublishSubscribeService.OnPublicFailoverEventHandler OnPublicFailover
        {
            add
            {
                _onPublicFailover += value;
            }
            remove
            {
                _onPublicFailover -= value;
            }
        }

        public HttpApplication MainHttpApplication
        {
            get
            {
                return _mainHttpApplication;
            }
            set
            {
                _mainHttpApplication = value;
            }
        }

        public RedisClient redisClient
        {
            get
            {
                if (_redisClient == null)
                {
                    _redisClient = new RedisClientConfig
                    {
                        ServerIP = ServerIP,
                        Password = Password,
                        ServerPort = ServerPort
                    }.GetRedisClient();
                }
                return _redisClient;
            }
            set
            {
                _redisClient = value;
            }
        }

        public string ServerIP
        {
            get
            {
                return _serverIP;
            }
            set
            {
                _serverIP = value;
            }
        }

        public int ServerPort
        {
            get
            {
                return _ServerPort;
            }
            set
            {
                _ServerPort = value;
            }
        }

        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }

        public string ChannelName
        {
            get
            {
                return _channelName;
            }
            set
            {
                _channelName = value;
            }
        }

        public void Subscription()
        {
            try
            {
                //创建订阅
                ServiceStack.Redis.IRedisSubscription subscription = redisClient.CreateSubscription();
                //接收消息处理Action
                subscription.OnMessage = (changel, msg) =>
                {
                    if (_onSubscriptionMessage != null)
                        _onSubscriptionMessage(changel, msg, _mainHttpApplication);
                };

                //订阅事件处理
                subscription.OnSubscribe = (channel) =>
                {
                    if (_onSubscribe != null)
                        _onSubscribe(channel, _mainHttpApplication);
                };

                //取消订阅事件处理
                subscription.OnUnSubscribe = (channel) =>
                {
                    if (_onUnSubscribe != null)
                        _onUnSubscribe(channel, _mainHttpApplication);
                };

                //订阅频道
                subscription.SubscribeToChannels(_channelName);
            }
            catch (Exception ex)
            {
                //Log.Error("EasyFrameRedis", "Subscription", ex.Message);
            }
        }

        public void Publish()
        {
            try
            {
                ServiceStack.Redis.IRedisClientsManager RedisClientManager = new RedisClientConfig().GetRedisClientManager();
                //发布、订阅服务 IRedisPubSubServer
                ServiceStack.Redis.IRedisPubSubServer pubSubServer = new ServiceStack.Redis.RedisPubSubServer(RedisClientManager, _channelName);
                //接收消息事件
                pubSubServer.OnMessage = (channel, msg) =>
                {
                    if (_onPublicMessage != null)
                        _onPublicMessage(channel, msg, _mainHttpApplication);
                };
                //启动服务事件处理事件
                pubSubServer.OnStart = () =>
                {
                    if (_onPublicStart != null)
                        _onPublicStart();
                };
                //停止服务事件处理事件
                pubSubServer.OnStop = () =>
                {
                    if (_onPublicStop != null)
                        _onPublicStop();
                };
                //取消发布处理事件
                pubSubServer.OnUnSubscribe = (channel) =>
                {
                    if (_onPublicUnSubscribe != null)
                        _onPublicUnSubscribe(channel, _mainHttpApplication);
                };
                //发布报错时处理事件
                pubSubServer.OnError = (e) =>
                {
                    if (_onPublicError != null)
                        _onPublicError(e);
                };
                //发布故障时处理事件
                pubSubServer.OnFailover = (s) =>
                {
                    if (_onPublicFailover != null)
                        _onPublicFailover(s);
                };
                //启动发布服务
                pubSubServer.Start();
            }
            catch (Exception ex)
            {
                //Log.Error("EasyFrameRedis", "Publish", ex.Message);
            }
        }
    }
}