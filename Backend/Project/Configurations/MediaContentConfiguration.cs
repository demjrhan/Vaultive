using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Modules;

namespace Project.Configurations
{
    public class MediaContentConfiguration : IEntityTypeConfiguration<MediaContent>
    {
        public void Configure(EntityTypeBuilder<MediaContent> builder)
        {

            builder.HasKey(m => m.Title);
            builder.HasMany(m => m.Reviews)
                .WithOne(r => r.MediaContent)
                .HasForeignKey(r => r.MediaTitle );

            builder.HasMany(m => m.WatchHistories)
                .WithOne(w => w.MediaContent)
                .HasForeignKey(w =>  w.MediaTitle );

            builder.HasOne(m => m.SubtitleOption)
                .WithOne(s => s.MediaContent)
                .HasForeignKey<SubtitleOption>(s => s.MediaTitle);
            
            builder.HasOne(m => m.AudioOption)
                .WithOne(a => a.MediaContent)
                .HasForeignKey<AudioOption>(a => a.MediaTitle);

            builder.HasMany(m => m.MediaContentStreamingServices)
                .WithOne(mcss => mcss.MediaContent)
                .HasForeignKey(mcss => mcss.MediaTitle);

            builder.Property(m => m.Description).HasMaxLength(50).IsRequired();
            builder.Property(m => m.ReleaseDate).IsRequired();
            builder.Property(m => m.OriginalLanguage).HasMaxLength(50).IsRequired();
            builder.Property(m => m.Country).HasMaxLength(50).IsRequired();
            builder.Property(m => m.Duration).IsRequired();
            
            builder.HasIndex(m => m.OriginalLanguage);
            builder.HasIndex(m => m.ReleaseDate);

        }
    }
}