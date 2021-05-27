using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_StudieTips.Models
{
    public class Tip
    {
        public int TipId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(100)]
        public string Headline { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; }

        [Required]
        public bool IsVerified { get; set; } = false;

    }
}