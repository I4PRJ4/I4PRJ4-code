using System.Collections.Generic;

namespace Projekt_StudieTips.Models
{
    public class Degree
    {
        public int DegreeId { get; set; }
        public string DegreeName { get; set; }

        public List<Course> Courses { get; set; }
    }
}
