using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Projekt_StudieTips.Data;
using Projekt_StudieTips.Models;

namespace Projekt_StudieTips.Repository
{
    public class CourseRepository
    {

        public DatabaseContext Context { get; set; }

        public CourseRepository(DatabaseContext context)
        {
            Context = context;
        }

        public List<Course> FindCourses(int? id)
        {
            return Context.Courses.Where(i => i.DegreeId == id).ToList();
        }

        public List<Degree> GetDegrees()
        {
            return Context.Degrees.ToList();
        }

        public async Task AddDegree(Course course)
        {
            Context.Add(course);
            await Context.SaveChangesAsync();
        }

        public Course GetCourse(int? id)
        {
            return Context.Courses.Find(id);
        }

        public async Task RemoveCourse(Course course)
        {
            Context.Courses.Remove(course);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateCourse(Course course)
        {
            Context.Update(course);
            await Context.SaveChangesAsync();
        }

    }
}
