﻿using Spectre.Console;

namespace CodingTracker;

public enum RecordsMenuOptions
{
    [Title("Quit")]
    Quit,
    [Title("Show all records")]
    All,
    [Title("Show records per day")]
    Day,
    [Title("Show records per week")]
    Week,
    [Title("Show records per year")]
    Year,
}
public class ShowRecordsMenu : IMenu<RecordsMenuOptions>
{
    public IPrompt<RecordsMenuOptions> GetMenu()
    {
        return new SelectionPrompt<RecordsMenuOptions>()
            .Title("Choose an option: ")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
            .AddChoices(Enum.GetValues<RecordsMenuOptions>()).UseConverter<RecordsMenuOptions>(EnumHelper.GetTitle);
    }
}
