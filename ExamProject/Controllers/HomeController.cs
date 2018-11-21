using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamProject.Models;
using Microsoft.AspNetCore.Authorization;
using ExamProject.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExamProject.Controllers
{
    /// <summary>
    /// Default controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            using (context)
            {
                var ideas = context.Ideas
                    .Include(i => i.User)
                    .Include(i => i.Likes)
                    .ToList();

                var name = User.FindFirst("FullName").Value;
                var model = new IdeaListViewModel
                {
                    Name = name,
                    UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    Ideas = ideas,
                    IdeaViewModel = new IdeaViewModel()
                };
                return View(model);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}