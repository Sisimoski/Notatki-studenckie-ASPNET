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
    public class IndexModel : PageModel
    {
        private readonly NotatkiWEB.Data.ApplicationDbContext _context;

        public IndexModel(NotatkiWEB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        //public IList<SemesterList> SemesterList { get;set; }
        public PaginatedList<SemesterList> SemesterList { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public string SemesterSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            SemesterSort = String.IsNullOrEmpty(sortOrder) ? "semester_desc" : "";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            CurrentFilter = searchString;

            IQueryable<SemesterList> semesterIQ = from s in _context.SemesterList
                                      select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                semesterIQ = semesterIQ.Where(s => s.SemesterName.Contains(searchString));
            }

            switch (sortOrder)
            {

                case "semester_desc":
                    semesterIQ = semesterIQ.OrderByDescending(s => s.SemesterName);
                    break;
                default:
                    semesterIQ = semesterIQ.OrderBy(s => s.SemesterName);
                    break;
            }
            //SubjectList = await subjectIQ.AsNoTracking().ToListAsync();

            int pageSize = 5;
            SemesterList = await PaginatedList<SemesterList>.CreateAsync(semesterIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            //SemesterList = await _context.SemesterList.ToListAsync();
        }
    }
}
