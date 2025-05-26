using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Models.Enumerations;

namespace Project.Context;

public class MasterContext : DbContext
{
 
    public MasterContext(DbContextOptions<MasterContext> options) : base(options) { }
    
    public DbSet<AudioOption> AudioOptions { get; set; }
    public DbSet<Documentary> Documentaries { get; set; }
    public DbSet<MediaContent> MediaContents { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<ShortFilm> ShortFilms { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<StreamingService> StreamingServices { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<SubscriptionConfirmation> SubscriptionConfirmations { get; set; }
    public DbSet<SubtitleOption> SubtitleOptions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<WatchHistory> WatchHistories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MasterContext).Assembly);
    }
}