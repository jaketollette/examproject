using ExamProject.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProject.Models.Configuration
{
    public class IdeaConfig : IEntityTypeConfiguration<Idea>
    {
        public void Configure(EntityTypeBuilder<Idea> builder)
        {
            builder.Property(i => i.Content).HasColumnType("TEXT");
            builder.HasOne(i => i.User).WithMany(u => u.Ideas).HasForeignKey(i => i.UserId);
            builder.HasMany(i => i.Likes).WithOne().HasForeignKey(l => l.IdeaId);
        }
    }
}