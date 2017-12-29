using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForEvolve.AspNetCore.Localization.Models
{
    public class ContactViewModel
    {
        [Required]
        [MaxLength(20)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Url]
        [Display(Name = "Web site")]
        public string WebUri { get; set; }

        [Required]
        [MaxLength(50)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Message { get; set; }
    }
}
