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

namespace NotatkiWEB.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public PaginatedList<ApplicationUser> ApplicationUserList { get; set; }
        [TempData]
        public string Message { get; set; }
        public string FirstNameSort { get; set; }
        public string LastNameSort { get; set; }
        public string EmailSort { get; set; }
        public string SchoolSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task<IActionResult> OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            FirstNameSort = String.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "";
            LastNameSort = sortOrder == "lastname" ? "lastname_desc" : "lastname";
            EmailSort = sortOrder == "email" ? "email_desc" : "email";
            SchoolSort = sortOrder == "school" ? "school_desc" : "school";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            CurrentFilter = searchString;

            IQueryable<ApplicationUser> studentIQ = from s in _db.ApplicationUser
                                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                studentIQ = studentIQ.Where(s => s.FirstName.Contains(searchString) 
                || s.LastName.Contains(searchString)
                || s.Email.Contains(searchString)
                || s.School.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "firstname":
                    studentIQ = studentIQ.OrderBy(s => s.FirstName);
                    break;
                case "firstname_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.FirstName);
                    break;
                case "lastname":
                    studentIQ = studentIQ.OrderBy(s => s.LastName);
                    break;
                case "lastname_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.LastName);
                    break;
                case "email":
                    studentIQ = studentIQ.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.Email);
                    break;
                case "school":
                    studentIQ = studentIQ.OrderBy(s => s.School);
                    break;
                case "school_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.School);
                    break;
                default:
                    studentIQ = studentIQ.OrderBy(s => s.Id);
                    break;
            }
            //SubjectList = await subjectIQ.AsNoTracking().ToListAsync();

            int pageSize = 5;
            ApplicationUserList = await PaginatedList<ApplicationUser>.CreateAsync(studentIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            return Page();
        }
    }
}