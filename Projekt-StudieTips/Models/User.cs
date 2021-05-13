using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projekt_StudieTips.Models
{
    // MODELLEN SKAL FJERNES SENERE, BRUGES TIL TEST
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } //Unique

        // One-To-Many relationship with Tip
        public List<Tip> Tips { get; set; }
    }
}