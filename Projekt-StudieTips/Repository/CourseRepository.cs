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
        private DatabaseContext _context;

        public CourseRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void AddCourse(string name, int id)
        {
            if (!_context.Courses.Any(s => s.CourseName == name))
            {
                Course course = new Course()
                    { CourseName = name, DegreeId = id};


                _context.Courses.Add(course);
                _context.SaveChanges();
            }
        }
    }
}
