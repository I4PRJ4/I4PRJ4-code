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
        public IActionResult Index(int? DegreeId)
        {
            List<Course> courses = _repository.FindCourses(DegreeId);

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

            else if(submit == "Rediger")
            {
                return RedirectToAction("Edit", "Courses", new { id = CourseId });
            }

            else if(submit == "Slet")
            {
                return RedirectToAction("Delete", "Courses", new { id = CourseId });
            }
            else
            {
                return NotFound();
            }

        }

        // GET: Courses/Create
        [Authorize("IsAdmin")]
        public async Task<IActionResult> Create(int? DegreeId)
        {
            DegreeCourse ViewModeDegreeCourse = new DegreeCourse();

            ViewBag.DegreeId = DegreeId;

            ViewModeDegreeCourse.Degrees = _repository.GetDegrees();

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
                await _repository.AddDegree(course);
              

                return RedirectToAction("Index", "Courses", new { DegreeId = course.DegreeId});
            }
            return View();
        }

        // GET: Courses/Edit/5
        [Authorize("IsAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DegreeCourse ViewModelDegreeCourse = new DegreeCourse();

            ViewModelDegreeCourse.Degrees = _repository.GetDegrees();
            ViewModelDegreeCourse.Courses = _repository.GetCourse(id);

            ViewBag.Course = _repository.GetCourse(id);

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

            var oldCourse = _repository.GetCourse(id);

            await _repository.RemoveCourse(oldCourse);

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.UpdateCourse(course);
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
        [Authorize("IsAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _repository.GetCourse(id);

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
            var course = _repository.GetCourse(id);

            await _repository.RemoveCourse(course);

            return RedirectToAction("Index", "Courses", new { DegreeId = course.DegreeId });
        }

        private bool CourseExists(int id)
        {
            return _repository.Context.Courses.Any(e => e.CourseId == id);
        }
    }
}
