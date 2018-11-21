using ExamProject.Models;
using ExamProject.Models.Entities;
using ExamProject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExamProject.Controllers
{
    public class IdeaController : Controller
    {
        private readonly ApplicationDbContext context;

        public IdeaController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public IActionResult Create(IdeaViewModel model)
        {
            if (model == null)
            {
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var idea = new Idea
            {
                Content = model.Content,
                UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
            };

            context.Add(idea);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Like(int id)
        {
            var userId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            using (context)
            {
                var idea = context.Ideas
                     .Include(i => i.Likes)
                    .FirstOrDefault(i => i.Id == id);

                var currentLikeCount = idea.Likes.Count();

                if (idea.Likes.Any(l => l.UserId == userId))
                {
                    return Ok();
                }

                if (idea == null)
                {
                    return BadRequest("Idea not found");
                }

                var like = new Like
                {
                    IdeaId = id,
                    UserId = userId
                };

                context.Add(like);
                context.SaveChanges();
                currentLikeCount++;

                return Ok(currentLikeCount);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            using (context)
            {
                var idea = context.Ideas.FirstOrDefault(i => i.Id == id && i.UserId == userId);

                if (idea != null)
                {
                    // delete idea
                    context.Remove(idea);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}