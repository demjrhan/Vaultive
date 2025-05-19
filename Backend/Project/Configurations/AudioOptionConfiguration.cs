using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Models;

namespace Project.Configurations
{
    public class AudioOptionConfiguration : IEntityTypeConfiguration<AudioOption>
    {
        public void Configure(EntityTypeBuilder<AudioOption> builder)
        {
            builder.HasKey(ao => ao.MediaTitle); 

            builder.HasOne(ao => ao.MediaContent)
                .WithOne(m => m.AudioOption)
                .HasForeignKey<AudioOption>(ao => ao.MediaTitle)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            

        }
    }
}
