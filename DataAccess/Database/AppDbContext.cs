using Domain.Entities.App;
using Domain.Entities.Credentials;
using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Database;

public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
    }

    public DbSet<User> Users { get; set; }
    public DbSet<App> Apps { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<AppStoreConnectCredential> AppStoreConnectCredentials { get; set; }
    public DbSet<AppleSearchAdsCredential> AppleSearchAdsCredentials { get; set; }
}
