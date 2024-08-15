using CodingTrackerLibrary.Controllers;
using CodingTrackerLibrary;

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
                OutputRenderer.ShowCodingSessions(connectionString);
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
            Console.WriteLine("\nInserting...");
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
            Console.WriteLine("\nUpdating...");
            OutputRenderer.ShowCodingSessions(connectionString);
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
            Console.WriteLine("\nDeleting...");
            OutputRenderer.ShowCodingSessions(connectionString);
            int userId = DisplayMenu.ShowMenu<int>(new IdMenu());
            CodingSessionController.DeleteCodingSession(new CodingSession { Id = userId }, connectionString);
            Console.WriteLine("\nRecord deleted successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
