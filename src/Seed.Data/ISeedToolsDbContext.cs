using Microsoft.EntityFrameworkCore;
using Seed.Data.Entities;

namespace Seed.Data
{
    public interface ISeedToolsDbContext
    {
        DbSet<TbsAccessToken> TpdAccessTokens { get; }
    }
}