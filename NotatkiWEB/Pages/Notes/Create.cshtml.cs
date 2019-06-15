using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NotatkiWEB.Data;
using NotatkiWEB.Models;

namespace NotatkiWEB.Pages.Notes
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly NotatkiWEB.Data.ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        public CreateModel(NotatkiWEB.Data.ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet(string userId = null)
        {
            Note = new Note();
            if (userId == null)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                userId = claim.Value;
            }
            Note.UserId = userId;

            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
        ViewData["IDSubject"] = new SelectList(_context.SubjectList, "IDSubject", "SubjectName");
            return Page();
        }

        [BindProperty]
        public Note Note { get; set; }
        [BindProperty]
        [Display(Name = "Plik:")]
        public IFormFile NoteFile { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(this.NoteFile != null)
            {
                var fileName = GetUniqueName(this.NoteFile.FileName);
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, fileName);
                this.NoteFile.CopyTo(new FileStream(filePath, FileMode.Create));
                Note.NoteFileURL = uploads;
                Note.NoteFileCaption = fileName; // Set the file name
            }

            _context.Note.Add(Note);
            await _context.SaveChangesAsync();

            StatusMessage = "Notatka została dodana pomyślnie!";

            return RedirectToPage("./Index");
        }

        private string GetUniqueName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                + "_" + Guid.NewGuid().ToString().Substring(0, 4)
                + Path.GetExtension(fileName);
        }
    }
}