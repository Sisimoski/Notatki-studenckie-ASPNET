using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotatkiWEB.Data;
using NotatkiWEB.Models;

namespace NotatkiWEB.Pages.Notes
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly NotatkiWEB.Data.ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        public EditModel(NotatkiWEB.Data.ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Note Note { get; set; }
        [BindProperty]
        [Display(Name = "Plik:")]
        public IFormFile NoteFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Note = await _context.Note
                .Include(n => n.ApplicationUser)
                .Include(n => n.SubjectList).FirstOrDefaultAsync(m => m.IDNote == id);

            if (Note == null)
            {
                return NotFound();
            }
           ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
           ViewData["IDSubject"] = new SelectList(_context.SubjectList, "IDSubject", "SubjectName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (this.NoteFile != null)
            {
                var fileName = GetUniqueName(this.NoteFile.FileName);
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, fileName);
                this.NoteFile.CopyTo(new FileStream(filePath, FileMode.Create));
                Note.NoteFileURL = uploads;
                Note.NoteFileCaption = fileName; // Set the file name
            }

            _context.Attach(Note).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(Note.IDNote))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private string GetUniqueName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                + "_" + Guid.NewGuid().ToString().Substring(0, 4)
                + Path.GetExtension(fileName);
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.IDNote == id);
        }
    }
}
