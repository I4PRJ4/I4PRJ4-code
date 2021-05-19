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
using Projekt_StudieTips.Repository;

namespace Projekt_StudieTips.Controllers
{
    public class DegreesController : Controller
    {
       
        private readonly DegreeRepository _repository;

        public DegreesController(DegreeRepository degreeRepository)
        {
            
            _repository = degreeRepository;
        }

        // GET: Degrees
        public IActionResult Index()
        {
            ViewBag.DegreeId = 0;

            return View(_repository.GetDegrees());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Redirect(int? DegreeId, string? submit)
        {
            ViewBag.DegreeId = DegreeId;

            if (submit == "Gå til")
            {
                return RedirectToAction("Index", "Courses", new { DegreeId = DegreeId });
            }

            else if (submit == "Edit")
            {
                return RedirectToAction("Edit", "Degrees", new { id = DegreeId });
            }

            else if (submit == "Delete")
            {
                return RedirectToAction("Delete", "Degrees", new { id = DegreeId });
            }
            else
            {
                return NotFound();
            }

        }

        // GET: Degrees/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var degree = _repository.FindDegree(id);

            if (degree == null)
            {
                return NotFound();
            }

            return View(degree);
        }

        // GET: Degrees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Degrees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("IsAdmin")]
        public async Task<IActionResult> Create([Bind("DegreeId,DegreeName")] Degree degree)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddDegree(degree);

                return RedirectToAction(nameof(Index));
            }
            return View(degree);
        }

        // GET: Degrees/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var degree = _repository.FindDegree(id);
            if (degree == null)
            {
                return NotFound();
            }
            return View(degree);
        }

        // POST: Degrees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DegreeId,DegreeName")] Degree degree)
        {
            if (id != degree.DegreeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                try
                {
                    await _repository.UpdateDegree(degree);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DegreeExists(degree.DegreeId))
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
            return View(degree);
        }

        // GET: Degrees/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var degree = _repository.FindDegree(id);
            if (degree == null)
            {
                return NotFound();
            }

            return View(degree);
        }

        // POST: Degrees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var degree = _repository.FindDegree(id);

            await _repository.RemoveDegree(degree);
         
            return RedirectToAction(nameof(Index));
        }

        private bool DegreeExists(int id)
        {
            return _repository.Context.Degrees.Any(e => e.DegreeId == id);
        }
    }
}
