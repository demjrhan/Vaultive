using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models;

namespace Project.Configurations
{
    public class StreamingServiceConfiguration : IEntityTypeConfiguration<StreamingService>
    {
        public void Configure(EntityTypeBuilder<StreamingService> builder)
        {
            builder.HasKey(ss => ss.Id);

            builder.HasMany(ss => ss.MediaContentStreamingServices)
                .WithOne(mcss => mcss.StreamingService)
                .HasForeignKey(mcss => mcss.StreamingServiceId);

            builder.HasMany(ss => ss.Subscriptions)
                .WithOne(s => s.StreamingService)
                .HasForeignKey(s => s.StreamingServiceId);

            builder.Property(ss => ss.Name).HasMaxLength(50).IsRequired();
            builder.Property(ss => ss.Country).HasMaxLength(50).IsRequired();
            builder.Property(ss => ss.Description).HasMaxLength(100).IsRequired();
        }
    }
}
