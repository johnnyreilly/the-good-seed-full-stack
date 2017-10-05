using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Seed.Data.Infrastructure
{
    //
    // Required for the dotnet ef command line tools
    //
    public class SeedToolsDbContextFactory : IDesignTimeDbContextFactory<SeedToolsDbContext>
    {
        public SeedToolsDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<SeedToolsDbContext>()
                .UseSqlServer(
                    "Data Source=(localdb)\\MSSqlLocalDB;Initial Catalog=SeedTools;AttachDbFileName=C:\\Work\\seed-tools\\seed-tools.mdf;Integrated Security=True;MultipleActiveResultSets=True")
                .Options;

            return new SeedToolsDbContext(options);
        }
    }
}