using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models;

namespace Project.Configurations
{
    public class WatchHistoryConfiguration : IEntityTypeConfiguration<WatchHistory>
    {
        public void Configure(EntityTypeBuilder<WatchHistory> builder)
        {

            builder.HasAlternateKey(wh => new { wh.UserId, wh.MediaId });
            builder.Property(wh => wh.WatchDate)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(wh => wh.TimeLeftOf).IsRequired();
            
            builder.HasOne(wh => wh.User)
                .WithMany(u => u.WatchHistories)
                .HasForeignKey(wh => wh.UserId);

            builder.HasOne(wh => wh.MediaContent)
                .WithMany(mc => mc.WatchHistories)
                .HasForeignKey(w => w.MediaId );
            

        }
    }
}