﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NotatkiWEB.Models
{
    public class Note
    {
        [Key]
        public int IDNote { get; set; }

        [Required(ErrorMessage = "Musisz wprowadzić tytuł notatki")]
        [DataType(DataType.Text)]
        [Display(Name = "Tytuł notatki")]
        public string Title { get; set; }
        [Required(ErrorMessage ="Wprowadź treść notatki")]
        [DataType(DataType.Text)]
        [Display(Name = "Treść notatki")]
        public string NoteContent { get; set; }

        [Display(Name = "Lokalizacja pliku")]
        public string NoteFileURL { get; set; }
        [Display(Name = "Opis pliku")]
        public string NoteFileCaption { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Wybierz przedmiot:")]
        public int IDSubject { get; set; }
        [ForeignKey("IDSubject")]
        [Display(Name = "Przedmiot")]
        public virtual SubjectList SubjectList { get; set; }
    }
}
