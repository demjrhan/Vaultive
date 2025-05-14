using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Modules;

namespace Project.Configurations
{
    public class AudioLanguageConfiguration : IEntityTypeConfiguration<AudioLanguage>
    {
        public void Configure(EntityTypeBuilder<AudioLanguage> builder)
        {
            builder.HasKey(al => al.Id);
            builder.Property(al => al.Language).HasMaxLength(50).IsRequired();
            
            builder.HasOne(a => a.AudioOption)
                .WithMany(ao => ao.AudioLanguages)
                .HasForeignKey(al => al.AudioOptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
