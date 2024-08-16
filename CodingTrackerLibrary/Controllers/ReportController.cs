using Microsoft.Data.Sqlite;
using System.Data;
using Dapper;
using System.Globalization;

namespace CodingTrackerLibrary;

public static class ReportController
{
    public static Report GetReportLastWeek(string connStr)
    {
        Report report = null;
        int weekOfTheYear = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

        try
        {
            using (IDbConnection connection = new SqliteConnection(connStr))
            {
                connection.Open();

                dynamic rawReport = connection.QueryFirstOrDefault<dynamic>(@$"SELECT Week, SUM(Duration) AS TotalHours, AVG(Duration) AS AverageHours 
                                                                    FROM (SELECT 
                                                                            strftime('%Y-%W', StartTime) AS Week, (julianday(EndTime) - julianday(StartTime)) * 24 AS Duration
                                                                            FROM CodingSessions) AS T 
                                                                    GROUP BY Week HAVING Week = '{DateTime.Today.Year}-{weekOfTheYear}'; ");

                if(rawReport != null)
                {
                    report = new Report()
                    {
                        AverageHours = TimeSpan.FromHours((double)rawReport.AverageHours),
                        TotalHours = TimeSpan.FromHours((double)rawReport.TotalHours),
                    };
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error ocurred: {ex.Message}");
        }

        return report;
    }
}
