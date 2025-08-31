using ConfigurationManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<ConfigurationItem> ConfigurationItems { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConfigurationItem>().HasData(
            new ConfigurationItem
            {
                Id = 1,
                Name = "SiteName",
                Type = "string",
                Value = "soty.io",
                IsActive = true,
                ApplicationName = "SERVICE-A"
            },
            new ConfigurationItem
            {
                Id = 2,
                Name = "IsBasketEnabled",
                Type = "bool",
                Value = "true",
                IsActive = true,
                ApplicationName = "SERVICE-B"
            },
            new ConfigurationItem
            {
                Id = 3,
                Name = "MaxItemCount",
                Type = "int",
                Value = "50",
                IsActive = false,
                ApplicationName = "SERVICE-A"
            }
        );
    }


}
