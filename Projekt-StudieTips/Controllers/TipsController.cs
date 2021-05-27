using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt_StudieTips.Data;
using Projekt_StudieTips.Models;
using Projekt_StudieTips.Repository;
using PagedList;

namespace Projekt_StudieTips.Controllers
{

    public class TipsController : Controller
    {
        private readonly ITipRepository _repository;
        private readonly UserManager<IdentityUser> _user;


        public TipsController(ITipRepository repository, UserManager<IdentityUser> user)
        {
            _repository = repository;
            _user = user;
        }


        // GET: Tip
        public async Task<IActionResult> Index(int? id, string sortOrder, int? page)
        {
            //Default page
            if (id == null)
            {
                return RedirectToAction("Index", "Home"); // bliver sendt tilbage til forsiden
            }

            ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_desc" : "date_asc";
            var context = _repository.GetTips(id, sortOrder).Result;

            try
            {
                ViewBag.CourseName = context[0].Course.CourseName;
                ViewBag.CourseId = context[0].CourseId;
            }
            catch (ArgumentOutOfRangeException)
            {

                try
                {
                    var course = await _repository.GetCourse(id);
                    ViewBag.CourseName = $"{course.CourseName}";
                    ViewBag.NoTip = "har desværre ingen tips..";
                    ViewBag.CourseId = id;
                }
                catch (InvalidOperationException)
                {
                    return NotFound();
                }

            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            return View(context.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult SearchTip(string sortOrder, int? page, [Bind("SearchTerm")] SearchDto search)
        {
            //Default page
            if (search.SearchTerm == null)
            {
                return RedirectToAction("Index", "Home"); // bliver sendt tilbage til forsiden
            }

            ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_desc" : "date_asc";
            var context = _repository.GetTipsWithinSearchTerm(search).Result;

            try
            {
                ViewBag.SearchTerm = search.SearchTerm;
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

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            return View(context.ToPagedList(pageNumber, pageSize));
        }

        // GET: Tip/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tip = _repository.GetTipDetails(id);

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
            if (value == null)
            {
                return NotFound();
            }

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

            tip.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                await _repository.AddTip(tip);
                return RedirectToAction(nameof(Index), new { id = tip.CourseId });
            }

            return View(tip);
        }

        // GET: Tip/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var tip = await _repository.GetTip(id);
            if (tip == null)
            {
                return NotFound();
            }

            var check = User.Claims.FirstOrDefault();

            if (check != null)
            {
                var bruger = User.Claims.FirstOrDefault(c => c.Type == "User" || c.Type == "Admin" || c.Type == "Moderator").Value;

                if (tip.Username != bruger && bruger != "Admin" && bruger != "Moderator")
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(tip);
        }

        // POST: Tip/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
                    await _repository.UpdateTip(tip);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repository.TipExists(tip.TipId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index", "Tips", new { id = tip.CourseId });
            }

            return View(tip);
        }

        // GET: Tip/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var tip = _repository.GetTipDetails(id).Result;


            var check = User.Claims.FirstOrDefault();

            if (check != null)
            {
                var bruger = User.Claims.FirstOrDefault(c => c.Type == "User" || c.Type == "Admin" || c.Type == "Moderator").Value;

                if (tip.Username != bruger && bruger != "Admin" && bruger != "Moderator")
                {
                    return RedirectToAction(nameof(Index));
                }
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
            await _repository.DeleteTip(id);
            return RedirectToAction("Index", "Tips",  id );
        }
    }
}
