using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpeakingInBits.Data;
using SpeakingInBits.Models;

namespace SpeakingInBits.Controllers
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-3.1
    /// </summary>
    [Authorize(Roles = IdentityHelper.Instructor)]
    public class VideoLessonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VideoLessonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VideoLessons
        // [AllowAnonymous] // Makes it so anyone can go to the video page
        // [Authorize(Roles = IdentityHelper.InstructorOrStudent)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.VideoLessons.ToListAsync());
        }

        // GET: VideoLessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoLesson = await _context.VideoLessons
                .FirstOrDefaultAsync(m => m.LessonId == id);
            if (videoLesson == null)
            {
                return NotFound();
            }

            return View(videoLesson);
        }

        // GET: VideoLessons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VideoLessons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmbedCode,LessonId,Title,Description")] VideoLesson videoLesson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(videoLesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(videoLesson);
        }

        // GET: VideoLessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoLesson = await _context.VideoLessons.FindAsync(id);
            if (videoLesson == null)
            {
                return NotFound();
            }
            return View(videoLesson);
        }

        // POST: VideoLessons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmbedCode,LessonId,Title,Description")] VideoLesson videoLesson)
        {
            if (id != videoLesson.LessonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoLesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoLessonExists(videoLesson.LessonId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(videoLesson);
        }

        // GET: VideoLessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videoLesson = await _context.VideoLessons
                .FirstOrDefaultAsync(m => m.LessonId == id);
            if (videoLesson == null)
            {
                return NotFound();
            }

            return View(videoLesson);
        }

        // POST: VideoLessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var videoLesson = await _context.VideoLessons.FindAsync(id);
            _context.VideoLessons.Remove(videoLesson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoLessonExists(int id)
        {
            return _context.VideoLessons.Any(e => e.LessonId == id);
        }
    }
}
