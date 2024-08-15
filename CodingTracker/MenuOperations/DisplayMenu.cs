using Spectre.Console;

namespace CodingTracker;

public static class DisplayMenu
{
    public static T ShowMenu<T>(IMenu<T> menu)
    {

        while (true)
        {
            Console.WriteLine();
            return AnsiConsole.Prompt<T>(menu.GetMenu());

            //string? input = Console.ReadLine();

            //IValidator<T> validator = ValidatorFactory.GetValidator<T>();
            //(bool inputValid, T userInput) = validator.Validate(input);
            //if (inputValid)
            //{
            //    return userInput;
            //}

            //Console.WriteLine("Invalid input. Please try again.");
        }
    }
}
