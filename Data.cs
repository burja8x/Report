using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Report
{
    public static class Data
    {
        public static string sqlConnStr { get; set; }
        public static List<ReportRow> GetReportTable()
        {
            List<ReportRow> reportTable = new List<ReportRow>();

            SqlConnection conn = new SqlConnection(sqlConnStr);
            string sql = "SELECT * from Report";
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    reportTable.Add(new ReportRow((IDataRecord)reader));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
            return reportTable;
        }

        public static bool InsertReport(string content, bool is_test, string mobi, string mail, string info)
        {
            SqlConnection conn = new SqlConnection(sqlConnStr);

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"INSERT INTO Report (content, is_test, mobi, email, info) VALUES(@content, @is, @mobi, @mail, @info);", conn);
                cmd.Parameters.AddWithValue("@content", content);
                cmd.Parameters.AddWithValue("@is", is_test ? 1: 0);
                cmd.Parameters.AddWithValue("@mobi", mobi);
                cmd.Parameters.AddWithValue("@mail", mail.ToLower());
                cmd.Parameters.AddWithValue("@info", info);
                int numAffectedRows = cmd.ExecuteNonQuery();
                if (numAffectedRows > 0)
                {
                    conn.Close();
                    return true;
                }
                else
                {
                    Console.WriteLine($"ERROR InsertReport {cmd.CommandText}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                conn.Close();
            }

            return false;
        }

        public static bool UpdateReportInfo(int id, string info)
        {
            SqlConnection conn = new SqlConnection(sqlConnStr);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"UPDATE Report SET info = '{info}' WHERE id = {id};", conn);
                int numAffectedRows = cmd.ExecuteNonQuery();
                if (numAffectedRows > 0)
                {
                    conn.Close();
                    return true;
                }
                else
                {
                    Console.WriteLine($"UpdateReport -> numAffectedRows = {numAffectedRows}");
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR UpdateReportInfo:");
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
            return false;
        }
        public static DateTime? GetSysDateTime()
        {
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmd = new SqlCommand("SELECT SYSDATETIME();", conn);
            conn.Open();
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string datetime = reader[0].ToString();
                    //Console.WriteLine(datetime);
                    var n = DateTime.Parse(datetime);
                    conn.Close();

                    return n;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }
    }
}
