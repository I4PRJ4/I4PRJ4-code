using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_StudieTips.Models.ViewModels
{
    public class DegreeCourse
    {
        public Course Courses { get; set; }

        public List<Degree> Degrees { get; set; }
    }
}
