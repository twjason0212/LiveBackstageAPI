using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Redis;
using ServiceStack;

namespace TestAPI20171114.Models
{
    public class RedisClientConfig
    {
        private string _ServerIP = "";

        private int _ServerPort = 6379;

        private string _Password = "";

        private string ServiceStackLicenseKey = "4228-e1JlZjo0MjI4LE5hbWU6ZGFmYSxUeXBlOkJ1c2luZXNzLEhhc2g6YkJKUXFlL1BJdmt5dUU4dTJXbVhmc3ZpTXQwU1I1V2U0bnBmSHVHVjN5UXBpMFZoSkJyQUg3OU5HTG5rU2t0NGJHUFd6RVhHaFI4QVpZMG9GVm9lV29ia1ZIRkdncGs4WlJhekZyajU0K0wrditUSnRFQkZHQ0JBWWJzMGFBSlViWkZnOXJjMmZGLzE4eUxRcWN1eFZRZ2Y4QnBVUXNEVjA5NC9RYVJOT0hZPSxFeHBpcnk6MjAxNy0wOS0zMH0 =";

        public string ServerIP
        {
            get
            {
                return _ServerIP;
            }
            set
            {
                this._ServerIP = value;
            }
        }

        public int  ServerPort
        {
            get
            {
                return _ServerPort;
            }
            set
            {
                this._ServerPort = value;
            }
        }

        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                this._Password = value;
            }
        }

        public RedisClientConfig()
        {
            Licensing.RegisterLicense(ServiceStackLicenseKey);
        }

        public RedisClient GetRedisClient()
        {
            return new RedisClient(this.ServerIP, this.ServerPort, this.Password, 0L);
        }

        public RedisClient GetRedis()
        {
            return new RedisClient(this.ServerIP, this.ServerPort, this.Password, 0L);
        }

        public RedisClient GetRedisClient(string strServerIP, int iServerPort)
        {
            return new RedisClient(strServerIP, iServerPort);
        }

        public IRedisClientsManager GetRedisClientManager()
        {
            return new PooledRedisClientManager(new string[]
            {
                this.ServerIP + ":" + this.ServerPort
            });
        }
    }
}