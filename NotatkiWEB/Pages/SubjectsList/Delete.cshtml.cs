using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NotatkiWEB.Data;
using NotatkiWEB.Models;

namespace NotatkiWEB.Pages.SubjectsList
{
    public class DeleteModel : PageModel
    {
        private readonly NotatkiWEB.Data.ApplicationDbContext _context;

        public DeleteModel(NotatkiWEB.Data.ApplicationDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SubjectList = await _context.SubjectList.FindAsync(id);

            if (SubjectList != null)
            {
                _context.SubjectList.Remove(SubjectList);
                await _context.SaveChangesAsync();
            }

            Message = "został usunięty z listy.";
            return RedirectToPage("./Index");
        }
    }
}
