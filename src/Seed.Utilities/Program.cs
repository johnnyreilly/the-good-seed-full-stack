using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.CommandLineUtils;
using Seed.Data;

namespace Seed.Utilities
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "Seed Utilities";
            app.HelpOption("-?|-h|--help");

            app.Command("run-migrations", RunMigrations);

            app.Execute(args);
        }

        private static void RunMigrations(CommandLineApplication command)
        {
            command.Description = "Runs any pending Seed Entity Framework migrations";
            command.HelpOption("-?|-h|--help");

            var connectionStringArgument =
                command.Argument("[connection_string]", "The database to run the migrations against");

            command.OnExecute(() =>
            {
                try
                {
                    var connectionString = connectionStringArgument.Value;

                    var options = new DbContextOptionsBuilder<SeedToolsDbContext>()
                        .UseSqlServer(connectionString)
                        .Options;

                    var context = new SeedToolsDbContext(options);
                    context.Database.Migrate();

                    return 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return -1;
                }
            });
        }
    }
}