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

namespace NotatkiWEB.Pages.SubjectsList
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class IndexModel : PageModel
    {
        private readonly NotatkiWEB.Data.ApplicationDbContext _context;

        public IndexModel(NotatkiWEB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<SubjectList> SubjectList { get;set; }
        [TempData]
        public string Message { get; set; }
        public string SubjectSort { get; set; }
        public string SemesterSort { get; set; }

        public async Task OnGetAsync(string sortOrder)
        {
            SemesterSort = String.IsNullOrEmpty(sortOrder) ? "semester_desc" : "";
            SubjectSort = sortOrder == "subject" ? "subject_desc" : "subject";

            IQueryable<SubjectList> subjectIQ = from s in _context.SubjectList
                                            select s;

            switch (sortOrder)
            {
                case "subject":
                    subjectIQ = subjectIQ.OrderBy(s => s.SubjectName);
                    break;
                case "subject_desc":
                    subjectIQ = subjectIQ.OrderByDescending(s => s.SubjectName);
                    break;
                case "semester_desc":
                    subjectIQ = subjectIQ.OrderByDescending(s => s.SemesterList.SemesterName);
                    break;
                default:
                    subjectIQ = subjectIQ.OrderBy(s => s.SemesterList.SemesterName);
                    break;
            }

            SubjectList = await _context.SubjectList
                .Include(s => s.SemesterList).ToListAsync();
        }
    }
}
