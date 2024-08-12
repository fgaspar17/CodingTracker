using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var configuration = builder.Build();

// Access settings
string connectionString = configuration["AppSettings:ConnectionString"];

Console.WriteLine($"Setting1: {connectionString}");