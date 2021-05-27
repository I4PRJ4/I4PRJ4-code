using Microsoft.AspNetCore.Mvc;
using Projekt_StudieTips.Controllers;
using Projekt_StudieTips.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_StudieTips.Repository
{
    public interface ICourseRepository 
    {

        Course FindCourses(int? id);

        //List<Degree> GetDegrees(int id);

        List<Course> GetCourse(int? id);

        Task AddCourse(Course course);

        Task UpdateCourse(Course course);

        Task RemoveCourse(Course course);

        bool CourseExists(int id);


     
    }
}
