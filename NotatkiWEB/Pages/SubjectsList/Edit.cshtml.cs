using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotatkiWEB.Data;
using NotatkiWEB.Models;

namespace NotatkiWEB.Pages.SubjectsList
{
    public class EditModel : PageModel
    {
        private readonly NotatkiWEB.Data.ApplicationDbContext _context;

        public EditModel(NotatkiWEB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SubjectList SubjectList { get; set; }
        [TempData]
        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SubjectList = await _context.SubjectList
                .Include(s => s.SemesterList).FirstOrDefaultAsync(m => m.IDSubject == id);

            if (SubjectList == null)
            {
                return NotFound();
            }
           ViewData["IDSemester"] = new SelectList(_context.SemesterList, "IDSemester", "SemesterName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SubjectList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectListExists(SubjectList.IDSubject))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Message = "został zaktualizowany.";
            return RedirectToPage("./Index");
        }

        private bool SubjectListExists(int id)
        {
            return _context.SubjectList.Any(e => e.IDSubject == id);
        }
    }
}
