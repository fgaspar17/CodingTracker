using CodingTrackerLibrary;
using CodingTrackerLibrary.Controllers;
using Spectre.Console;
using System.Globalization;
using System.Reflection;

namespace CodingTracker;

public static class TableRenderer
{
    private static readonly Dictionary<Type, PropertyInfo[]> PropertiesCache = new Dictionary<Type, PropertyInfo[]>();
    public static void ShowCodingSessions(string connectionString, RecordsMenuOptions option = RecordsMenuOptions.All, OrderOptions order = OrderOptions.Asc) 
    {
        List<CodingSession> sessions = CodingSessionController.GetCodingSessions(connectionString);

        sessions = FilterSessions(sessions, option);
        sessions = SortSessions(sessions, order);

        ShowTable<CodingSession>(sessions, title: "Coding Sessions");
    }

    private static List<CodingSession> FilterSessions(List<CodingSession> sessions, RecordsMenuOptions option)
    {
        DateTime today = DateTime.Today;
        switch (option)
        {
            case RecordsMenuOptions.Day:
                return sessions.Where(s => s.StartTime.Date == today).ToList();
            case RecordsMenuOptions.Week:
                int currentWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                    today, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                return sessions.Where(s =>
                    CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                        s.StartTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday) == currentWeek
                ).ToList();
            case RecordsMenuOptions.Year:
                return sessions.Where(s => s.StartTime.Year == today.Year).ToList();
            default:
                return sessions;
        }
    }

    private static List<CodingSession> SortSessions(List<CodingSession> sessions, OrderOptions order)
    {
        return order == OrderOptions.Asc ?
            sessions.OrderBy(s => s.StartTime).ToList() :
            sessions.OrderByDescending(s => s.StartTime).ToList();
    }

    /// <summary>
    /// Displays a list of objects in a table format.
    /// </summary>
    /// <param name="values">List of objects to display.</param>
    /// <typeparam name="T">Type of object, must be a class.</typeparam>

    private static void ShowTable<T>(List<T> values, string title) where T : class
    {
        // Create a table
        var table = new Table();

        // Set the table border and style options
        table.Border(TableBorder.Rounded);
        table.BorderColor(Color.LightCoral);
        table.Title($"[bold yellow]{title}[/]");
        table.Caption("Generated on " + DateTime.Now.ToString("g"));

        PropertyInfo[] properties = GetCachedProperties<T>();

        // Add columns
        foreach (PropertyInfo property in properties)
        {
            //table.AddColumn(new TableColumn(property.Name).Centered());
            table.AddColumn(new TableColumn(new Markup("[bold yellow]" + property.Name + "[/]")).Centered().PadRight(2));
        }
        
        // Add rows
        foreach(T value in values)
        {
            List<string> row = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                row.Add(property.GetValue(value)?.ToString() ?? "N/A");
            }
            table.AddRow(row.ToArray());
        }

        AnsiConsole.Write(table);
    }

    // Cache to avoid performance issues due to reflection
    private static PropertyInfo[] GetCachedProperties<T>() where T : class
    {
        Type type = typeof(T);

        if(!PropertiesCache.TryGetValue(type, out PropertyInfo[] properties))
        {
            properties = type.GetProperties();
            PropertiesCache.Add(type, properties);
        }

        return properties;
    }
}
