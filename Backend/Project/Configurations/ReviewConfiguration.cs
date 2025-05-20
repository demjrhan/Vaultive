using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models;

namespace Project.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            
            builder.HasKey(r => r.Id);

            
            builder.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            builder.HasOne(r => r.MediaContent)
                .WithMany(m => m.Reviews)
                .HasForeignKey(r =>  r.MediaId);

            /* Do not let deleting the review if the watch history exists. */
            builder.HasOne(r => r.WatchHistory)
                .WithMany() 
                .HasForeignKey(r => new { r.UserId, r.MediaId })
                .HasPrincipalKey(w => new { w.UserId, w.MediaId })
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Property(r => r.Comment).HasMaxLength(50).IsRequired();
            
            
        }
    }
}
