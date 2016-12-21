using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TFSDataService.Tool
{
    public class JsonRequest
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal static string GetRestResponse(string requestUrl)
        {
            string json;
            HttpWebResponse response = TryMethod(requestUrl);

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                json = sr.ReadToEnd();
            }

            return json;
        }

        private static HttpWebResponse TryMethod(string requestUrl)
        {
            WebRequest request = CreateRequest(requestUrl);

            int tries = 0;

            while (tries < 5)
            {
                ++tries;
                try
                {
                    return (HttpWebResponse)request.GetResponse();
                }
                catch (WebException e)
                {
                    logger.Warn("GET Error encountered {0}, let's retry in a few seconds", e.Message);
                    request = null;
                    Thread.Sleep(2000);
                    request = CreateRequest(requestUrl);
                }
            }

            throw new Exception("Too much network error");
        }

        private static WebRequest CreateRequest(string requestUrl)
        {
            var request = WebRequest.Create(requestUrl);
            request.Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("TfsLoginName"), Environment.GetEnvironmentVariable("TfsPassword"));
            return request;
        }

        internal static string PostData(string postURL, string payload)
        {
            WebRequest request = CreateRequest(postURL);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = payload.Length;

            int tries = 0;

            while (tries < 5)
            {
                ++tries;
                try
                {
                    using (Stream webStream = request.GetRequestStream())
                    using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                    {
                        requestWriter.Write(payload);
                    }

                    string json;
                    var response = (HttpWebResponse)request.GetResponse();

                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        json = sr.ReadToEnd();
                    }

                    return json;
                }
                catch (WebException e)
                {
                    logger.Warn("POST Error encountered {0}, let's retry in a few seconds", e.Message);
                    request = null;
                    Thread.Sleep(2000);
                    request = CreateRequest(postURL);

                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.ContentLength = payload.Length;
                }
            }

            throw new Exception("Too much network error");
        }
    }
}
