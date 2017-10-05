using Microsoft.EntityFrameworkCore;
using Seed.Data.Entities;

namespace Seed.Data
{
    public class SeedToolsDbContext : DbContext, ISeedToolsDbContext
    {
        // C'tor
        public SeedToolsDbContext(DbContextOptions<SeedToolsDbContext> options)
            : base(options)
        {
        }

        // DbSets

        public DbSet<TbsAccessToken> TpdAccessTokens { get; set; }
    }
}