using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models;

namespace Project.Configurations
{
    public class SubtitleOptionConfiguration : IEntityTypeConfiguration<SubtitleOption>
    {
        public void Configure(EntityTypeBuilder<SubtitleOption> builder)
        {
            builder.HasKey(so => so.MediaId);

            builder.HasOne(so => so.MediaContent)
                .WithOne(m => m.SubtitleOption)
                .HasForeignKey<SubtitleOption>(so => so.MediaId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(so => so.Languages)
                .HasConversion(
                    v => string.Join(",", v ?? new List<string>()),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                );
        }
    }
}
