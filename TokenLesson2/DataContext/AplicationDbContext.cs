using Microsoft.EntityFrameworkCore;
using TokenLesson2.Models.User;

namespace TokenLesson2.DataContext;

public class AplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AplicationDbContext(DbContextOptions<AplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();
    }
}