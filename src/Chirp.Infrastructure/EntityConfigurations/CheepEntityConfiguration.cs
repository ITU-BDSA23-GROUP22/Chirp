// ReferenceLink:
//  https://learn.microsoft.com/en-us/ef/core/modeling/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chirp.Infrastructure.EntityConfigurations
{
    public class CheepEntityConfiguration : IEntityTypeConfiguration<Cheep>
    {
        public void Configure(EntityTypeBuilder<Cheep> builder)
        {
            builder.ToTable("Cheep");
            builder.HasKey(x => x.CheepId);

            builder.Property(x => x.CheepId)
                .IsRequired();

            builder.Property(x => x.AuthorId)
                .IsRequired();

            builder.Property(x => x.Text)
                .HasMaxLength(160)
                .IsRequired();

            builder.Property(x => x.TimeStamp)
                .IsRequired();
        }
    }
}

