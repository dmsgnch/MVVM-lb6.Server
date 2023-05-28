using Microsoft.EntityFrameworkCore;
using MVVM_lb6.Domain.Models;

namespace Server;

public class ApplicationDbContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<BookTimeRange> BookTimeRanges { get; set; }
    public DbSet<SettledTimeRange> SettledTimeRanges { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
       // Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BookTimeRange>()
            .HasOne(btr => btr.BookedRoom)
            .WithMany()
            .HasForeignKey(btr => btr.RoomId);
    }
}