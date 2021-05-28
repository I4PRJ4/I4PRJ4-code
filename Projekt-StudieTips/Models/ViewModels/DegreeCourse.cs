using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_StudieTips.Models.ViewModels
{
    public class DegreeCourse
    {
        public Course Course { get; set; }

        public List<Degree> Degrees { get; set; }
    }
}
