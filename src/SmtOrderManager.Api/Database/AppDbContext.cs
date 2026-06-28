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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Order <-> Board (Many-to-Many)
        modelBuilder.Entity<OrderBoard>()
            .HasKey(ob => new { ob.OrderId, ob.BoardId });

        modelBuilder.Entity<OrderBoard>()
            .HasOne(ob => ob.Order)
            .WithMany(o => o.OrderBoards)
            .HasForeignKey(ob => ob.OrderId);

        modelBuilder.Entity<OrderBoard>()
            .HasOne(ob => ob.Board)
            .WithMany(b => b.OrderBoards)
            .HasForeignKey(ob => ob.BoardId);

        // Board <-> Component (Many-to-Many)
        modelBuilder.Entity<BoardComponent>()
            .HasKey(bc => new { bc.BoardId, bc.ComponentId });

        modelBuilder.Entity<BoardComponent>()
            .HasOne(bc => bc.Board)
            .WithMany(b => b.BoardComponents)
            .HasForeignKey(bc => bc.BoardId);

        modelBuilder.Entity<BoardComponent>()
            .HasOne(bc => bc.Component)
            .WithMany(c => c.BoardComponents)
            .HasForeignKey(bc => bc.ComponentId);
    }
}
