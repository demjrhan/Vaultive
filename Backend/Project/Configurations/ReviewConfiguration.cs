using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Modules;

namespace Project.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            builder.HasOne(r => r.MediaContent)
                .WithMany(m => m.Reviews)
                .HasForeignKey(r =>  r.MediaTitle );

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Comment).HasMaxLength(50).IsRequired();
            builder.Property(r => r.Rating).IsRequired();
        }
    }
}
