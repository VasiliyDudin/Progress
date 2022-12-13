using Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity;

public class DatabaseContext:DbContext
{
    public DatabaseContext(DbContextOptions opt)
        : base(opt)
    {
			
    }

    public DbSet<Game> Games { get; set; }
    public DbSet<GameStatistic> GameStatistics { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BattelField> BattelFields { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>();
        modelBuilder.Entity<GameStatistic>();
        modelBuilder.Entity<BattelField>();
        modelBuilder.Entity<User>();
    }
}