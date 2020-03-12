using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace TestAPI20171114.Models
{
    public static class UpdateMsg
    {
        public static bool PostUpdate(string msg)
        {
            Conf.MseeageQueue.TryAdd(msg);
            return true;

        }

        public static bool SendMessage(string msg)
        {
            bool flog = false;
            using (var client = new WebClient() { Encoding = Encoding.UTF8 })
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                var data = "msg=" + msg;
                var result = client.UploadString(Conf.SLCMUrl, "POST", data);
                if (result == "true")
                    flog = true;
                else
                    flog = false;
            }
            using (var client = new WebClient() { Encoding = Encoding.UTF8 })
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                var data = "msg=" + msg;
                var result = client.UploadString(Conf.SLCM2Url, "POST", data);
                if (result == "true")
                    flog = true;
                else
                    flog = false;
            }

            return flog;
        }

        public static string SendImage(byte[] imageData)
        {
            var uri = new Uri(Conf.ImageUrl);

            var from = Encoding.UTF8.GetBytes("live");

            var requestContent = new MultipartFormDataContent();
            var imageContent = new ByteArrayContent(imageData);
            var serviceFrom = new ByteArrayContent(from);

            HttpClient Client;
            HttpClientHandler Handler;
            Handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
            };
            Client = new HttpClient(Handler)
            {
                Timeout = new TimeSpan(0, 0, 20)
            };
            Client.DefaultRequestHeaders.Add("x-is-system", "1");
            ServicePoint serverPoint = ServicePointManager.FindServicePoint(uri);

            // 設定 30 秒沒有活動即關閉連線，預設 -1 (永不關閉)
            serverPoint.ConnectionLeaseTimeout = (int)TimeSpan.FromSeconds(30).TotalMilliseconds;


            requestContent.Add(serviceFrom, "serviceFrom");

            requestContent.Add(imageContent, "files","img.png");

            var response = Client.PostAsync(Conf.ImageUrl, requestContent).GetAwaiter().GetResult();

            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        }

        private static HttpRequestMessage CreateRequestMessage(Uri uri, HttpMethod method, ByteArrayContent content)
        {
            var requestMessage = new HttpRequestMessage
            {
                // 使用 HTTP/1.0
                Version = HttpVersion.Version10,
                Method = method,
                RequestUri = uri
            };

            if (content != null)
            {
                requestMessage.Content = content;
            }

            // 完成後關閉連接, 預設為 false (Keep-Alive)
            requestMessage.Headers.ConnectionClose = true;

            var cacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };

            requestMessage.Headers.CacheControl = cacheControl;
            return requestMessage;
        }
    }
}