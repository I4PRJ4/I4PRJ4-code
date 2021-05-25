using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _user;

        public TipsController(DatabaseContext context, UserManager<IdentityUser> user)
        {
            _context = context;
            _user = user;
        }

        // GET: Tip
        public async Task<IActionResult> Index(int? id, string sortOrder)
        {
            //Default page
            if (id == null)
            {
                return RedirectToAction("Index", "Home"); // bliver sendt tilbage til forsiden
            }

            

            var context = _context.Tips
                .Include(t => t.Course)
                .Where(t => t.CourseId == id & t.IsVerified == false);

            ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_desc" : "date_asc";

            switch (sortOrder)
            {
                case "date_asc":
                    context = context.OrderBy(t => t.Date);
                    break;
                case "date_desc":
                    context = context.OrderByDescending(t => t.Date);
                    break;
                default:
                    context = context.OrderByDescending(t => t.Date);
                    break;
            }

            var list = await context.ToListAsync();

            try
            {
                ViewBag.CourseName = list[0].Course.CourseName;
                ViewBag.CourseId = list[0].CourseId;
            }
            catch (ArgumentOutOfRangeException)
            {

                try
                {
                    var course = await _context.Courses.Where(c => c.CourseId == id).FirstAsync();
                    ViewBag.CourseName = $"{course.CourseName} har ingen tips. Tryk 'Tilføj nyt tip' for at tilføje";
                    ViewBag.CourseId = id;
                }
                catch (InvalidOperationException)
                {
                    return NotFound();
                }
                
            }

            return View(list);
        }

        public async Task<IActionResult> SearchTip([Bind("SearchTerm")]SearchDto search)
        {
            //Default page
            if (search.SearchTerm == null)
            {
                return RedirectToAction("Index", "Home"); // bliver sendt tilbage til forsiden
            }

            var context = await _context.Tips
                .Include(t => t.Course)
                .Where(t => (t.Headline.Contains(search.SearchTerm) || t.Text.Contains(search.SearchTerm) || t.Course.CourseName.Contains(search.SearchTerm)) & t.IsVerified == false).ToListAsync();


            try
            {
                ViewBag.CourseName = context[0].Course.CourseName;
                ViewBag.CourseId = context[0].CourseId;
            }
            catch (ArgumentOutOfRangeException)
            {

                try
                {
                    ViewBag.CourseName = $"Vi kunne ikke finde nogle tips som indeholdte søgsordene.";
                    ViewBag.CourseId = 0;
                }
                catch (InvalidOperationException)
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
                .FirstOrDefaultAsync(m => m.TipId == id);
            if (tip == null)
            {
                return NotFound();
            }

            return View(tip);
        }

        // GET: Tip/Create

        public IActionResult Create(int? value)
        {
            ViewBag.CourseId = value;
            return View();
        }

        // POST: Tip/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("TipId,Username,CourseId,Date,Headline,Text")] Tip tip)
        {
            tip.Username = _user.GetUserName(User);
            tip.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(tip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = tip.CourseId });
            }

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

            if (tip.Username != _user.GetUserName(User))
            {
                return RedirectToAction(nameof(Index));
            }

            return View(tip);
        }

        // POST: Tip/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipId,Username,CourseId,Date,Headline,Text")] Tip tip)
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
                .FirstOrDefaultAsync(m => m.TipId == id);


            if (tip.Username != _user.GetUserName(User))
            {
                return RedirectToAction(nameof(Index));
            }

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
