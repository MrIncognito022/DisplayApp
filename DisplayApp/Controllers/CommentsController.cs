using DisplayApp.Data;
using DisplayApp.Models;
using DisplayApp.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DisplayApp.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Comments(string uniqueId)
        {
            if(uniqueId!= null)
            {
            var comments = _context.Comments.Where(x=>x.UniqueId == uniqueId).ToList();
            ViewBag.temp_uniqueId = uniqueId;
            return View(comments);
            }

            return RedirectToAction("comments", "comments");
        }

        [HttpGet]
        public IActionResult Add(string uniqueId)
        {
            ViewBag.UniqueId = uniqueId;
            return View();
        }
        [HttpPost]
        public IActionResult Add(AddCommentsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comments = new Comments()
                {
                    UniqueId = model.UniqueId,
                    Comment = model.Comment,
                    Tags = model.Tags,
                    CreatedAt = DateTime.Now
                };

                _context.Add(comments);
                _context.SaveChanges();
                return RedirectToAction("Comments");
            }
            return View();
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.CommentsId == id);
            if (comment == null)
            {
                return NotFound();
            }

            var model = new EditCommentsViewModel
            {
                CommentsId = comment.CommentsId,
                UniqueId = comment.UniqueId,
                Comment = comment.Comment,
                Tags = comment.Tags
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditCommentsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = _context.Comments.FirstOrDefault(c => c.CommentsId == model.CommentsId);
                if (comment == null)
                {
                    return NotFound();
                }
                comment.Comment = model.Comment;
                comment.Tags = model.Tags;

                _context.Update(comment);
                _context.SaveChanges();

                return RedirectToAction("Comments", new { uniqueId = model.UniqueId });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.CommentsId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.CommentsId == id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return RedirectToAction("Comments", new { uniqueId = comment.UniqueId });
        }




    }
}
