using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models;

namespace Project.Configurations
{
    public class MediaContentConfiguration : IEntityTypeConfiguration<MediaContent>
    {
        public void Configure(EntityTypeBuilder<MediaContent> builder)
        {
            builder.HasKey(m => m.Id);
            
            builder.HasMany(m => m.Reviews)
                .WithOne(r => r.MediaContent)
                .HasForeignKey(r => r.MediaId);

            builder.HasMany(m => m.WatchHistories)
                .WithOne(w => w.MediaContent)
                .HasForeignKey(w => w.MediaId);

            builder.HasOne(m => m.SubtitleOption)
                .WithOne(s => s.MediaContent)
                .HasForeignKey<SubtitleOption>(s => s.MediaId);

            builder.HasOne(m => m.AudioOption)
                .WithOne(a => a.MediaContent)
                .HasForeignKey<AudioOption>(a => a.MediaId);

            builder.HasMany(m => m.StreamingServices)
                .WithMany(ss => ss.MediaContents);

            builder.Property(m => m.Description).HasMaxLength(50).IsRequired();
            builder.Property(m => m.ReleaseDate).IsRequired().HasColumnType("datetime");
            builder.Property(m => m.OriginalLanguage).HasMaxLength(50).IsRequired();
            builder.Property(m => m.Country).HasMaxLength(2).IsRequired();
            builder.Property(m => m.Duration).IsRequired();
            builder.Property(m => m.Title).HasMaxLength(50).IsRequired();
            
            builder.Property(m => m.State)
                .HasConversion<string>();
            
            builder.HasIndex(m => m.Title).IsUnique();
        }
    }
}