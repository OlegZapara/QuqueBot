using Microsoft.EntityFrameworkCore;
using QuqueBot.Models;

namespace QuqueBot.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TelegramUser>()
            .HasMany(e => e.Queues)
            .WithMany(e => e.Users)
            .UsingEntity<QueueTelegramUser>();
    }

    public DbSet<Queue> Queues { get; set; }

    public DbSet<QueueTelegramUser> QueueTelegramUser { get; set; }
    
    public DbSet<TelegramUser> Users { get; set; }
    
    public DbSet<UserRating> Ratings { get; set; }
    
    public DbSet<Timetable> Timetables { get; set; }
    
    public DbSet<TimetableDay> TimetableDays { get; set; }
    
    public DbSet<TimetableEntry> TimetableEntries { get; set; }
}