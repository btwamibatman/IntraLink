using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Example table:
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public bool IsActive { get; set; }
}