using System;
using System.ComponentModel.DataAnnotations;

namespace Projekt_StudieTips.Models
{
    public class Tip
    {
        public int TipId { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Headline { get; set; }

        [Required]
        public string Text { get; set; }

    }
}