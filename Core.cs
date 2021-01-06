using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Report
{
    public class Core
    {
        private Timer _timerSelectDB = new Timer();
        private Random rand = new Random();
        public static string POD_UUID;

        public Core() {
            string? pod_uid = Environment.GetEnvironmentVariable("MY_POD_UID");
            if (pod_uid == null)
            {
                POD_UUID = "DOCKER_DEV_" + rand.Next().ToString();
            }
            else
            {
                POD_UUID = pod_uid;
            }

            _timerSelectDB = new Timer();
            _timerSelectDB.Interval = 5000;
            _timerSelectDB.Elapsed += OnTimedEventSelectDB;
            _timerSelectDB.Start();
        }

        private void OnTimedEventSelectDB(object sender, ElapsedEventArgs e)
        {
            _timerSelectDB.Interval = rand.Next(30000, 40000);

            List<ReportRow> reportRows = Data.GetReportTable();
            DateTime? time = Data.GetSysDateTime();

            if (time != null)
            {
                CheckToSend(reportRows, (DateTime)time);
            }
            else {
                Console.WriteLine("ERROR DateTime from DB == null!");
            }

        }

        private void CheckToSend(List<ReportRow> reportRows, DateTime time) {
            foreach (ReportRow row in reportRows)
            {
                if (row.info.StartsWith("processing") && 
                    row.info.Contains(Core.POD_UUID) && 
                    IsBetween<long>(Math.Abs(row.last_update.Ticks - time.Ticks), 0, 300000000)) { // 30 sec

                    Console.WriteLine($"Sending... ID:{row.id}");
                    // SEND AND UPDATE TABLE.
                    bool response = SmsAPI.SendSMS(row.mobi, row.content);

                    if (response)
                    {
                        Data.UpdateReportInfo(row.id, "OK SMS");
                    }
                    else {
                        Data.UpdateReportInfo(row.id, "ERROR SMS API");
                    }
                }

                if (row.info.Length == 0 || row.info.StartsWith("processing")) {
                    Console.WriteLine($"time {row.last_update.Ticks.ToString()}   {time.Ticks.ToString()}  -{row.last_update.Ticks - time.Ticks}");

                    if (!IsBetween<long>(Math.Abs(row.last_update.Ticks - time.Ticks), 0, 300000000)) {
                        // we will send, but first do reservation.
                        Console.WriteLine($"Row reservation {row.id}.");
                        Data.UpdateReportInfo(row.id, $"processing by: {Core.POD_UUID}");
                        _timerSelectDB.Interval = 5000;
                    }
                }
            }
        }
        public static bool IsBetween<T>(T item, T start, T end)
        {
            return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;
        }

    }
}
