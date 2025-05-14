using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Context;

public class MasterContext : DbContext
{
 
    public MasterContext(DbContextOptions<MasterContext> options) : base(options) { }
    
    public DbSet<AudioLanguage> AudioLanguages { get; set; }
    public DbSet<AudioOption> AudioOptions { get; set; }
    public DbSet<Documentary> Documentaries { get; set; }
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<MediaContent> MediaContents { get; set; }
    public DbSet<MediaContentStreamingService> MediaContentStreamingServices { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<StreamingService> StreamingServices { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<SubscriptionConfirmation> SubscriptionConfirmations { get; set; }
    public DbSet<SubtitleLanguage> SubtitleLanguages { get; set; }
    public DbSet<SubtitleOption> SubtitleOptions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<WatchHistory> WatchHistories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MasterContext).Assembly);
    }
}