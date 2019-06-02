using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotatkiWEB.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Uczelnia")]
        public string School { get; set; }
    }
}
