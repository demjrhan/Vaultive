using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models;

namespace Project.Configurations
{
    public class MediaContentStreamingServiceConfiguration : IEntityTypeConfiguration<MediaContentStreamingService>
    {
        public void Configure(EntityTypeBuilder<MediaContentStreamingService> builder)
        {
            builder.HasKey(mc => new { mc.MediaTitle, mc.StreamingServiceId });

            builder.HasOne(mc => mc.MediaContent)
                .WithMany(m => m.MediaContentStreamingServices)
                .HasForeignKey(mc => mc.MediaTitle);

            builder.HasOne(mc => mc.StreamingService)
                .WithMany(s => s.MediaContentStreamingServices)
                .HasForeignKey(mc => mc.StreamingServiceId);

        }
    }
}