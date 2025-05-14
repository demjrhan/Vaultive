using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Modules;

namespace Project.Configurations
{
    public class SubtitleLanguageConfiguration : IEntityTypeConfiguration<SubtitleLanguage>
    {
        public void Configure(EntityTypeBuilder<SubtitleLanguage> builder)
        {
            builder.HasKey(sl => sl.Id);
            builder.Property(sl => sl.Language).HasMaxLength(50).IsRequired();
            builder.HasOne(s => s.SubtitleOption)
                .WithMany(so => so.SubtitleLanguages)
                .HasForeignKey(sl => sl.MediaTitle)
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
