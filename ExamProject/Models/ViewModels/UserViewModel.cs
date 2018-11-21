using ExamProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProject.Models.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }
        public int IdeaCount { get; set; }
        public int LikeCount { get; set; }
    }
}