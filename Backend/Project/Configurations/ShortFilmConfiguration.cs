using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models.Enumerations;
using Project.Models;

namespace Project.Configurations
{
    public class ShortFilmConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.Property(m => m.Genres)
                .HasConversion(
                    genres => string.Join(",", genres.Select(g => g.ToString())),
                    value => value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(Enum.Parse<Genre>)
                        .ToList()
                );
        }
    }
}
