// ReferenceLink:
//  https://learn.microsoft.com/en-us/ef/core/modeling/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chirp.Infrastructure.EntityConfigurations
{
    public class AuthorAuthorRelationEntityConfiguration : IEntityTypeConfiguration<AuthorAuthorRelation>
    {
        public void Configure(EntityTypeBuilder<AuthorAuthorRelation> builder)
        {
            builder.ToTable("AuthorAuthorRelation");
            builder.HasKey(x => new { x.AuthorId, x.AuthorToFollowId });

            builder.Property(x => x.AuthorId)
                .IsRequired();

            builder.Property(x => x.AuthorToFollowId)
                .IsRequired();

            builder.Property(x => x.TimeStamp)
                .IsRequired();

            builder.HasOne(x => x.Author)
                .WithMany(x => x.Following)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AuthorToFollow);
        }
    }
}