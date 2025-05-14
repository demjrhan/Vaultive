using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models.Enumerations;
using Project.Models;

namespace Project.Configurations
{
    public class DocumentaryConfiguration : IEntityTypeConfiguration<Documentary>
    {
        public void Configure(EntityTypeBuilder<Documentary> builder)
        {
            builder.Property(m => m.Topics)
                .HasConversion(
                    genres => string.Join(",", genres.Select(g => g.ToString())),
                    value => value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(Enum.Parse<Topic>)
                        .ToList()
                );
            builder.Property(m => m.Length).IsRequired();
        }
    }
}
