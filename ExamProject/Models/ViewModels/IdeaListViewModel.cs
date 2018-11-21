using ExamProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProject.Models.ViewModels
{
    public class IdeaListViewModel
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public List<Idea> Ideas { get; set; }
        public IdeaViewModel IdeaViewModel { get; set; }
    }
}