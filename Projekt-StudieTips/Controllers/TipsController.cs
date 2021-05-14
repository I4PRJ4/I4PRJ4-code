using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt_StudieTips.Data;
using Projekt_StudieTips.Models;

namespace Projekt_StudieTips.Controllers
{
    public class TipsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ApplicationDbContext _appContext;

        public TipsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Tip
        public async Task<IActionResult> Index(int? id)
        {
            //Default page
            if (id == null)
            {
                return RedirectToAction("Index", "Home"); // bliver sendt tilbage til forsiden
            }


            //var tip = await _context.Tips.FindAsync(id);
            //if (tip == null)
            //{
            //    return NotFound();
            //}

            var context = await _context.Tips
                .Include(t => t.Course)
                .Include(t => t.User)
                .Where(t => t.CourseId == id).ToListAsync();

           // var list = await context.ToListAsync();

            try
            {
                ViewBag.CourseName = context[0].Course.CourseName;
                ViewBag.CourseId = context[0].CourseId;
            }
            catch (ArgumentOutOfRangeException e)
            {

                try
                {
                    var course = await _context.Courses.Where(c => c.CourseId == id).FirstAsync();
                    ViewBag.CourseName = $"{course.CourseName} har ingen tips. Tryk Add Tip for at tilføje";
                    ViewBag.CourseId = id;
                }
                catch (InvalidOperationException a)
                {
                    return NotFound();
                }
                
            }

            return View(context);
        }

        // GET: Tip/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tip = await _context.Tips
                .Include(t => t.Course)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TipId == id);
            if (tip == null)
            {
                return NotFound();
            }

            return View(tip);
        }

        // GET: Tip/Create
        [Authorize]
        public IActionResult Create(int? value)
        {
            ViewBag.CourseId = value;
            //ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Tip/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("TipId,UserId,CourseId,Date,Headline,Text")] Tip tip)
        {
            tip.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(tip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = tip.CourseId });
            }

            //ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", tip.CourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", tip.UserId);
            return View(tip);
        }

        // GET: Tip/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tip = await _context.Tips.FindAsync(id);
            if (tip == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", tip.CourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", tip.UserId);
            return View(tip);
        }

        // POST: Tip/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipId,UserId,CourseId,Date,Headline,Text")] Tip tip)
        {
            if (id != tip.TipId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipExists(tip.TipId))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", tip.CourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", tip.UserId);
            return View(tip);
        }

        // GET: Tip/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tip = await _context.Tips
                .Include(t => t.Course)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TipId == id);
            if (tip == null)
            {
                return NotFound();
            }

            return View(tip);
        }

        // POST: Tip/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tip = await _context.Tips.FindAsync(id);
            _context.Tips.Remove(tip);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipExists(int id)
        {
            return _context.Tips.Any(e => e.TipId == id);
        }
    }
}
