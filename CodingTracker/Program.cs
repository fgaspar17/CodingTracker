using Microsoft.Extensions.Configuration;
using CodingTrackerLibrary;
using CodingTracker;

SetupDatabase setupDatabase = new SetupDatabase(Settings.GetConnectionString());
setupDatabase.InitializeDatabase();
//setupDatabase.SeedData();

UserInterface userInterface = new UserInterface();
userInterface.DisplayMainMenu(Settings.GetConnectionString());