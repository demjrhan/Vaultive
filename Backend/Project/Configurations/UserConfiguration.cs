using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models;

namespace Project.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(u => u.WatchHistories)
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserId);

            builder.HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId);

            builder.HasMany(u => u.Confirmations)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);

            builder.Property(u => u.Firstname).HasMaxLength(50);
            builder.Property(u => u.Lastname).HasMaxLength(50);
            builder.Property(u => u.Nickname).HasMaxLength(50).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(50).IsRequired();
            builder.Property(u => u.Country).HasMaxLength(50).IsRequired();

            builder.Property(u => u.Status)
                .HasConversion<string>();

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Nickname).IsUnique();
        }
    }
}