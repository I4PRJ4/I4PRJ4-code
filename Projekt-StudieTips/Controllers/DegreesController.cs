﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_StudieTips.Models;
using Projekt_StudieTips.Repository;
using System;
using System.Threading.Tasks;

namespace Projekt_StudieTips.Controllers
{
    public class DegreesController : Controller
    {

        private readonly IDegreeRepository _repository;

        public DegreesController(IDegreeRepository degreeRepository)
        {

            _repository = degreeRepository;
        }

        // GET: Degrees
        public async Task<IActionResult> Index()
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

            else if (submit == "Rediger")
            {
                return RedirectToAction("Edit", "Degrees", new { id = DegreeId });
            }

            else if (submit == "Slet")
            {
                return RedirectToAction("Delete", "Degrees", new { id = DegreeId });
            }
            else
            {
                return NotFound();
            }

        }

        // GET: Degrees/Create
        [Authorize("IsAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Degrees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DegreeId,DegreeName")] Degree degree)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddDegree(degree);
                return RedirectToAction(nameof(Index));
            }
            return View(degree);
        }

        // GET: Degrees/Edit/5'
        [Authorize("IsAdmin")]
        public async Task<IActionResult> Edit(int? id)
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
                    if (!_repository.DegreeExists(degree.DegreeId))
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
        [Authorize("IsAdmin")]
        public async Task<IActionResult> Delete(int? id)
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


    }
}
