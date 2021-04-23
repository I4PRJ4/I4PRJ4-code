using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_StudieTips.Models
{
    public class Course
    {

        [Key]

        public int CourseId { get; set; }

        public string CourseName { get; set; }

        public int DegreeId { get; set; }
    }
}
