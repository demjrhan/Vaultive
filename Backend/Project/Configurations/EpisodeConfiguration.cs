using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models;

namespace Project.Configurations
{
    public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Series)
                .WithMany(s => s.Episodes)
                .HasForeignKey(e => e.SeriesTitle)
                .HasPrincipalKey(s => s.Title);

            builder.Property(e => e.Length);
            builder.Property(e => e.Title).HasMaxLength(50).IsRequired();
            builder.Property(e => e.SeasonNumber).IsRequired();

            builder.HasIndex(e => e.Title).IsUnique();
        }
    }
}
