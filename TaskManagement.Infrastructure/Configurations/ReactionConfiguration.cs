using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Domain;

namespace TaskManagement.Infrastructure.Configurations
{
    public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            builder.HasOne(x => x.User)
                .WithMany(u => u.Reactions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Task)
                .WithMany(t => t.Reactions)
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Comment)
                .WithMany(c => c.Reactions)
                .HasForeignKey(x => x.CommentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasCheckConstraint("CK_Reaction_Target",
                "(TaskId IS NOT NULL AND CommentId IS NULL) OR (TaskId IS NULL AND CommentId IS NOT NULL)");
        }
    }
}
