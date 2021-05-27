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

        private readonly ICourseRepository _courseRepository;
        private readonly IDegreeRepository _degreeRepository;

        public CoursesController(ICourseRepository courseRepository, IDegreeRepository degreeRepository)
        {

            _courseRepository = courseRepository;
            _degreeRepository = degreeRepository;
        }

        // GET: Courses
        public IActionResult Index(int? DegreeId)
        {
            var courses = _courseRepository.FindCourses(DegreeId);

            ViewBag.DegreeId = DegreeId;

            return View(courses);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Redirect(int? CourseId, string? submit)
        {
            ViewBag.CourseId = CourseId;

            if (submit == "Gå til")
            {
                return RedirectToAction("Index", "Tips", new { id = CourseId });
            }

            else if (submit == "Rediger")
            {
                return RedirectToAction("Edit", "Courses", new { id = CourseId });
            }

            else if (submit == "Slet")
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
            DegreeCourse ViewModeDegreeCourse = new();

            ViewBag.DegreeId = DegreeId;

            ViewModeDegreeCourse.Degrees = _degreeRepository.GetDegrees();

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
                await _courseRepository.AddCourse(course);


                return RedirectToAction("Index", "Courses", new { course.DegreeId });
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

            DegreeCourse ViewModelDegreeCourse = new();

            ViewModelDegreeCourse.Degrees = _degreeRepository.GetDegrees();
            ViewModelDegreeCourse.Courses = _courseRepository.GetCourse(id);

            ViewBag.Course = _courseRepository.GetCourse(id);

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

            Course oldCourse = _courseRepository.GetCourse(id);

            await _courseRepository.RemoveCourse(oldCourse);

            if (ModelState.IsValid)
            {
                try
                {
                    await _courseRepository.UpdateCourse(course);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_courseRepository.CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Courses", new { course.DegreeId });
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

            Course course = _courseRepository.GetCourse(id);

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
            Course course = _courseRepository.GetCourse(id);
            await _courseRepository.RemoveCourse(course);
            return RedirectToAction("Index", "Courses", new { course.DegreeId });
        }

    }
}