using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models;

namespace Project.Configurations
{
    public class AudioOptionConfiguration : IEntityTypeConfiguration<AudioOption>
    {
        public void Configure(EntityTypeBuilder<AudioOption> builder)
        {
            builder.HasKey(ao => ao.MediaId); 

            builder.HasOne(ao => ao.MediaContent)
                .WithOne(m => m.AudioOption)
                .HasForeignKey<AudioOption>(ao => ao.MediaId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(ao => ao.Languages)
                .HasConversion(
                    v => string.Join(",", v ?? new List<string>()),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                );

        }
    }
}
