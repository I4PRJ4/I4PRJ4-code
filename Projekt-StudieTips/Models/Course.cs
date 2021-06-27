using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_StudieTips.Models
{
    public class Course
    {

        public int CourseId { get; set; }

        public string CourseName { get; set; }


        [ForeignKey("Degree")]
        public int DegreeId { get; set; }
        public Degree Degrees { get; set; }

        // One-To-Many relationship with Tip
        public List<Tip> Tips { get; set; }
    }
}
