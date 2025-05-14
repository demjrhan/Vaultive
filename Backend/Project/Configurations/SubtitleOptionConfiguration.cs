using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Modules;

namespace Project.Configurations
{
    public class SubtitleOptionConfiguration : IEntityTypeConfiguration<SubtitleOption>
    {
        public void Configure(EntityTypeBuilder<SubtitleOption> builder)
        {
            builder.HasKey(so => so.MediaTitle);

            builder.HasOne(so => so.MediaContent)
                .WithOne(m => m.SubtitleOption)
                .HasForeignKey<SubtitleOption>(so => so.MediaTitle)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
