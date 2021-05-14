using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt_StudieTips.Data;
using Projekt_StudieTips.Models;
using Projekt_StudieTips.Models.ViewModels;
using Projekt_StudieTips.Repository;

namespace Projekt_StudieTips.Controllers
{
    public class CoursesController : Controller
    {
        
        private readonly CourseRepository _repository;

        public CoursesController(CourseRepository courseRepository)
        {
            
            _repository = courseRepository;
        }

        // GET: Courses
        public async Task<IActionResult> Index(int? DegreeId)
        {
            List<Course> courses = await _repository.Context.Courses.Where(i => i.DegreeId == DegreeId).ToListAsync();

            ViewBag.DegreeId = DegreeId;

            return View(courses);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Redirect(int? CourseId, string? submit)
        {
            ViewBag.CourseId = CourseId;

            if(submit == "Gå til")
            {
                return RedirectToAction("Index", "Tips", new {id = CourseId});
            }

            else if(submit == "Edit")
            {
                return RedirectToAction("Edit", "Courses", new { id = CourseId });
            }

            else if(submit == "Delete")
            {
                return RedirectToAction("Delete", "Courses", new { id = CourseId });
            }
            else
            {
                return NotFound();
            }

        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _repository.Context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public async Task<IActionResult> Create(int? DegreeId)
        {
            DegreeCourse ViewModeDegreeCourse = new DegreeCourse();

            ViewBag.DegreeId = DegreeId;

            ViewModeDegreeCourse.Degrees = await _repository.Context.Degrees.ToListAsync();

            return View(ViewModeDegreeCourse);
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,CourseName,DegreeId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _repository.Context.Add(course);
                await _repository.Context.SaveChangesAsync();

                return RedirectToAction("Index", "Courses", new { DegreeId = course.DegreeId});
            }
            return View();
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DegreeCourse ViewModelDegreeCourse = new DegreeCourse();

            ViewModelDegreeCourse.Degrees = await _repository.Context.Degrees.ToListAsync();
            ViewModelDegreeCourse.Courses = await _repository.Context.Courses.FindAsync(id);

            ViewBag.Course = await _repository.Context.Courses.FindAsync(id);

            if (ViewModelDegreeCourse.Courses == null)
            {
                return NotFound();
            }
            return View(ViewModelDegreeCourse);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName,DegreeId")] Course course)
        {

            var oldCourse = await _repository.Context.Courses.FindAsync(id);
            _repository.Context.Courses.Remove(oldCourse);
            await _repository.Context.SaveChangesAsync();

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Context.Update(course);
                    await _repository.Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Courses", new { DegreeId = course.DegreeId });    
            }
            return View();
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _repository.Context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _repository.Context.Courses.FindAsync(id);
            _repository.Context.Courses.Remove(course);
            await _repository.Context.SaveChangesAsync();
            return RedirectToAction("Index", "Courses", new { DegreeId = course.DegreeId });
        }

        private bool CourseExists(int id)
        {
            return _repository.Context.Courses.Any(e => e.CourseId == id);
        }
    }
}
