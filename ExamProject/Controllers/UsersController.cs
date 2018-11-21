using ExamProject.Models;
using ExamProject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext context;

        public UsersController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Detail(int id)
        {
            using (context)
            {
                var user = context.Users.Include(u => u.Ideas).FirstOrDefault(u => u.Id == id);
                var likes = context.Likes.Where(l => l.UserId == id).ToList();

                var model = new UserViewModel
                {
                    User = user,
                    LikeCount = likes.Count(),
                    IdeaCount = user.Ideas.Count()
                };

                return View(model);
            }
        }
    }
}