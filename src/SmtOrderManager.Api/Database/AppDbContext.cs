using Microsoft.EntityFrameworkCore;
using SmtOrderManager.Api.Entities;

namespace SmtOrderManager.Api.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Board> Boards => Set<Board>();
    public DbSet<Component> Components => Set<Component>();
}
