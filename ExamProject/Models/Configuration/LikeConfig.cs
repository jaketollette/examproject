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
    public class LikeConfig : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasOne<User>().WithMany().HasForeignKey(l => l.UserId);
            builder.HasOne<Idea>().WithMany(i => i.Likes).HasForeignKey(l => l.IdeaId);
        }
    }
}