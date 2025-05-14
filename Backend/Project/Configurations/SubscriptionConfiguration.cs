using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models;

namespace Project.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasMany(s => s.Confirmations)
                .WithOne(sc => sc.Subscription)
                .HasForeignKey(sc => sc.SubscriptionId);

            builder.HasOne(s => s.StreamingService)
                .WithMany(ss => ss.Subscriptions)
                .HasForeignKey(s => s.StreamingServiceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); 


            builder.Property(s => s.DefaultPrice).IsRequired();

        }
    }
}
