using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models;

namespace Project.Configurations
{
    public class SubscriptionConfirmationConfiguration : IEntityTypeConfiguration<SubscriptionConfirmation>
    {
        public void Configure(EntityTypeBuilder<SubscriptionConfirmation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(sc => sc.Subscription)
                .WithMany(s => s.Confirmations)
                .HasForeignKey(sc => sc.SubscriptionId);
            
            builder.HasOne(sc => sc.User)
                .WithMany(u => u.Confirmations)
                .HasForeignKey(sc => sc.UserId);
            
            builder.Property(sc => sc.StartTime)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(sc => sc.EndTime)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(sc => sc.PaymentMethod).HasMaxLength(25).IsRequired();

        }
    }
}
