﻿using Spectre.Console;

namespace CodingTracker;

public enum CrudMenuOptions 
{
    [Title("Quit")]
    Quit,
    [Title("Create a coding session")]
    Create,
    [Title("Update a coding session")]
    Update,
    [Title("Delete a coding session")]
    Delete,
    [Title("Show all coding sessions")]
    Show,
    [Title("Reports")]
    Reports, 
}

public class CrudMenu : IMenu<CrudMenuOptions>
{
    public IPrompt<CrudMenuOptions> GetMenu()
    {
        return new SelectionPrompt<CrudMenuOptions>()
            .Title("Choose an option: ")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
            .AddChoices(Enum.GetValues<CrudMenuOptions>()).UseConverter<CrudMenuOptions>(EnumHelper.GetTitle);
    }
}