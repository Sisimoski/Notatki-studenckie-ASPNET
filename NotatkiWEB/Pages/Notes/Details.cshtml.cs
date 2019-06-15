using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NotatkiWEB.Data;
using NotatkiWEB.Models;

namespace NotatkiWEB.Pages.Notes
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly NotatkiWEB.Data.ApplicationDbContext _context;

        public DetailsModel(NotatkiWEB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Note Note { get; set; }

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
            return Page();
        }
    }
}
