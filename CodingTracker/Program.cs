using Microsoft.Extensions.Configuration;
using CodingTrackerLibrary;

var builder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var configuration = builder.Build();

// Access settings
string connectionString = configuration["AppSettings:ConnectionString"];

SetupDatabase setupDatabase = new SetupDatabase(connectionString!);
setupDatabase.InitializeDatabase();
setupDatabase.SeedData();