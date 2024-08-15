using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker;

public class UserInterface
{
    public void DisplayMainMenu(string connectionString)
    {
        bool continueRunning = true;
        CrudMenu crudMenu = new CrudMenu();

        while (continueRunning)
        {
            CrudMenuOptions crudMenuOptionPicked = DisplayMenu.ShowMenu<CrudMenuOptions>(crudMenu);
            HandleMenuOptions.HandleCrudMenuOption(crudMenuOptionPicked, connectionString, ref continueRunning);
        }
    }
}
