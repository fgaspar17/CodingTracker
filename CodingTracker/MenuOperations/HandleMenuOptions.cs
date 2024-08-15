using CodingTrackerLibrary.Controllers;
using CodingTrackerLibrary;
using Spectre.Console;

namespace CodingTracker;

public static class HandleMenuOptions
{
    public static void HandleCrudMenuOption(CrudMenuOptions option, string connectionString, ref bool continueRunning)
    {
        switch (option)
        {
            case CrudMenuOptions.Quit:
                continueRunning = false;
                break;
            case CrudMenuOptions.Create:
                HandleCreate(connectionString);
                break;
            case CrudMenuOptions.Update:
                HandleUpdate(connectionString);
                break;
            case CrudMenuOptions.Delete:
                HandleDelete(connectionString);
                break;
            case CrudMenuOptions.Show:
                HandleShowRecordsMenu(connectionString);
                break;
            case CrudMenuOptions.Stopwatch:
                HandleStopwatch(connectionString);
                break;
            case CrudMenuOptions.Reports:
                // TODO: Implements reports
                break;
            default:
                break;
        }
    }

    private static void HandleCreate(string connectionString)
    {
        try
        {
            Console.Clear();
            var rule = new Rule("[blue]Inserting[/]");
            AnsiConsole.Write(rule);
            DateTime userStartTime = Convert.ToDateTime(DisplayMenu.ShowMenu<string>(new StartTimeMenu()));
            DateTime userEndTime = Convert.ToDateTime(DisplayMenu.ShowMenu<string>(new EndTimeMenu(userStartTime)));
            CodingSessionController.InsertCodingSession(new CodingSession { StartTime = userStartTime, EndTime = userEndTime }, connectionString);
            Console.WriteLine("Record inserted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void HandleUpdate(string connectionString)
    {
        try
        {
            Console.Clear();
            var rule = new Rule("[yellow]Updating[/]");
            AnsiConsole.Write(rule);
            TableRenderer.ShowCodingSessions(connectionString);
            int userId = DisplayMenu.ShowMenu<int>(new IdMenu());
            DateTime userStartTime = Convert.ToDateTime(DisplayMenu.ShowMenu<string>(new StartTimeMenu()));
            DateTime userEndTime = Convert.ToDateTime(DisplayMenu.ShowMenu<string>(new EndTimeMenu(userStartTime)));
            CodingSessionController.UpdateCodingSession(new CodingSession { Id = userId, StartTime = userStartTime, EndTime = userEndTime }, connectionString);
            Console.WriteLine("\nRecord updated successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void HandleDelete(string connectionString)
    {
        try
        {
            Console.Clear();
            var rule = new Rule("[red]Deleting[/]");
            AnsiConsole.Write(rule);
            TableRenderer.ShowCodingSessions(connectionString);
            int userId = DisplayMenu.ShowMenu<int>(new IdMenu());
            CodingSessionController.DeleteCodingSession(new CodingSession { Id = userId }, connectionString);
            Console.WriteLine("\nRecord deleted successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void HandleStopwatch(string connectionString)
    {
        DateTime startTime = DateTime.Now;

        var panel = new Panel(new Markup("Coding Time (Press any key to stop): [bold yellow]0:00:00[/]"))
            .Expand()
            .Border(BoxBorder.Rounded);

        
            var liveDisplay = AnsiConsole.Live(panel);
            liveDisplay.AutoClear = true;

            // Start Live display
            liveDisplay.Start(ctx =>
            {
                while (!Console.KeyAvailable)
                {
                    TimeSpan codingTime = DateTime.Now - startTime;

                    // Update the context with the new panel
                    ctx.UpdateTarget(new Panel(new Markup($"Coding Time (Press any key to stop): [bold yellow]{codingTime:hh':'mm':'ss}[/]"))
                            .Expand()
                            .Border(BoxBorder.Rounded));

                    ctx.Refresh();

                    Thread.Sleep(1000);
                }

                Console.ReadKey(true);
            });
        

        CodingSessionController.InsertCodingSession(new CodingSession { StartTime = startTime, EndTime = DateTime.Now }, connectionString);
        Console.WriteLine("New record added via Stopwatch.");
    }

    private static void HandleShowRecordsMenu(string connectionString)
    {
        ShowRecordsMenu menu = new ShowRecordsMenu();
        RecordsMenuOptions option = DisplayMenu.ShowMenu<RecordsMenuOptions>(menu);

        OrderMenu orderMenu = new OrderMenu();
        OrderOptions optionOrder = DisplayMenu.ShowMenu<OrderOptions>(orderMenu);

        switch (option)
        {
            case RecordsMenuOptions.Quit:
                break;
            case RecordsMenuOptions.All:
                TableRenderer.ShowCodingSessions(connectionString, order: optionOrder);
                break;
            case RecordsMenuOptions.Day:
            case RecordsMenuOptions.Week:
            case RecordsMenuOptions.Year:
                TableRenderer.ShowCodingSessions(connectionString, option, optionOrder);
                break;
            default:
                break;
        }


    }
}
