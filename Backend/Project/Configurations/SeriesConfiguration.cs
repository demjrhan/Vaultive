using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models.Enumerations;
using Project.Models;

namespace Project.Configurations
{
    public class SeriesConfiguration : IEntityTypeConfiguration<Series>
    {
        public void Configure(EntityTypeBuilder<Series> builder)
        {
            builder.HasMany(s => s.Episodes)
                .WithOne(e => e.Series)
                .HasForeignKey(e => e.SeriesTitle);

            builder.Property(s => s.Genres)
                .HasConversion(
                    genres => string.Join(",", genres.Select(g => g.ToString())),
                    value => value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(Enum.Parse<Genre>)
                        .ToList()
                );

        }
    }
}
