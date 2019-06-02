using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NotatkiWEB.Data;
using NotatkiWEB.Models;
using NotatkiWEB.Utility;

namespace NotatkiWEB.Pages.SubjectsList
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class CreateModel : PageModel
    {
        private readonly NotatkiWEB.Data.ApplicationDbContext _context;

        public CreateModel(NotatkiWEB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["IDSemester"] = new SelectList(_context.SemesterList, "IDSemester", "SemesterName");
            return Page();
        }

        [BindProperty]
        public SubjectList SubjectList { get; set; }
        [TempData]
        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.SubjectList.Add(SubjectList);
            await _context.SaveChangesAsync();

            Message = "został dodany do listy.";
            return RedirectToPage("./Index");
        }
    }
}