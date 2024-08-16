using Microsoft.Extensions.Configuration;
using CodingTrackerLibrary;
using CodingTracker;

SetupDatabase setupDatabase = new SetupDatabase(Settings.GetConnectionString());
setupDatabase.InitializeDatabase();
//setupDatabase.SeedData();

Application app = new Application();
app.Run(Settings.GetConnectionString());