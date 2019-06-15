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
using NotatkiWEB.Utility;

namespace NotatkiWEB.Pages.Semesters
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class DetailsModel : PageModel
    {
        private readonly NotatkiWEB.Data.ApplicationDbContext _context;

        public DetailsModel(NotatkiWEB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public SemesterList SemesterList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SemesterList = await _context.SemesterList.FirstOrDefaultAsync(m => m.IDSemester == id);

            if (SemesterList == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
