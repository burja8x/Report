using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Report
{
    public static class SmsAPI
    {
        public static string url { get; set; }
        public static string username { get; set; }
        public static string pass { get; set; }
        public static string from { get; set; }

        public const string cc = "386";
        public static bool SendSMS(string number, string content) {
            string url1 = $"{url}un={username}&ps={pass}&from={from}&to={number}&m={content}&cc={cc}";
            string response = GetRequest(url1);
            Console.WriteLine(response);
            if (response.Contains("ERROR")) {
                return false;
            }
            return true;
        }
        public static string GetRequest(string url)
        {
            try
            {
                WebClient web = new WebClient();

                string result = web.DownloadString(url);
                Console.Write($"Respone sms api:{result}.");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "SMS Api ERROR";
            }
        }
    }
}
