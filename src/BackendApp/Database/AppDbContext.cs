using Microsoft.EntityFrameworkCore;

namespace BackendApp.Database;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<CategoryEntity> Categories { get; set; }
}