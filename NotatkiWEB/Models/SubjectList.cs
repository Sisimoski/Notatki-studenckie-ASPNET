using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotatkiWEB.Models
{
    public class SubjectList
    {
        [Key]
        public int IDSubject { get; set; }
        [Required(ErrorMessage ="Musisz wprowadzić nazwę przedmiotu")]
        [DataType(DataType.Text)]
        [Display(Name = "Przedmiot")]
        public string SubjectName { get; set; }
        public int IDSemester { get; set; }
        [ForeignKey("IDSemester")]
        public virtual SemesterList SemesterList { get; set; }
    }
}
