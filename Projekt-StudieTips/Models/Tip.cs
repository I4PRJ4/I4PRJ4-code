using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_StudieTips.Models
{
    public class Tip
    {
        public int TipId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Headline { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public bool IsVerified { get; set; } = false;

    }
}