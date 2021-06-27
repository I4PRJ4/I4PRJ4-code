using Projekt_StudieTips.Data;
using Projekt_StudieTips.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_StudieTips.Repository
{
    public class CourseRepository : ICourseRepository
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

        public async Task AddCourse(Course course)
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
        public bool CourseExists(int id)
        {
            return Context.Courses.Any(e => e.CourseId == id);
        }

    }
}
