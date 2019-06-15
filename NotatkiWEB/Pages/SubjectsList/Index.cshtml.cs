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
using ReflectionIT.Mvc.Paging;

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

        [BindProperty]
        public PaginatedList<SubjectList> SubjectList { get;set; }
        [TempData]
        public string Message { get; set; }
        public string SubjectSort { get; set; }
        public string SemesterSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            SubjectSort = String.IsNullOrEmpty(sortOrder) ? "subject_desc" : "";
            SemesterSort = sortOrder == "semester" ? "semester_desc" : "semester";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            CurrentFilter = searchString;

            IQueryable<SubjectList> subjectIQ = from s in _context.SubjectList
                                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                subjectIQ = subjectIQ.Where(s => s.SubjectName.Contains(searchString));
            }

            switch (sortOrder)
            {
                
                case "subject_desc":
                    subjectIQ = subjectIQ.OrderByDescending(s => s.SubjectName);
                    break;
                case "semester":
                    subjectIQ = subjectIQ.OrderBy(s => s.SemesterList.SemesterName);
                    break;
                case "semester_desc":
                    subjectIQ = subjectIQ.OrderByDescending(s => s.SemesterList.SemesterName);
                    break;
                default:
                    subjectIQ = subjectIQ.OrderBy(s => s.SubjectName);
                    break;
            }
            //SubjectList = await subjectIQ.AsNoTracking().ToListAsync();

            int pageSize = 5;
            SubjectList = await PaginatedList<SubjectList>.CreateAsync(subjectIQ.Include(s => s.SemesterList).AsNoTracking(), pageIndex ?? 1, pageSize);




            //SubjectList = await subjectIQ
                //.Include(s => s.SemesterList).AsNoTracking().ToListAsync();
        }
    }
}
