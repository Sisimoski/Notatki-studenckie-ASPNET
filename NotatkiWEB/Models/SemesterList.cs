using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotatkiWEB.Models
{
    public class SemesterList
    {
        [Key]
        public int IDSemester { get; set; }
        [Required(ErrorMessage ="Musisz wybrać semestr.")]
        public string SemesterName { get; set; }


    }
}
