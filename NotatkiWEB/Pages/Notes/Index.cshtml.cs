using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NotatkiWEB.Data;
using NotatkiWEB.Models;
using NotatkiWEB.Models.ViewModel;

namespace NotatkiWEB.Pages.Notes
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly NotatkiWEB.Data.ApplicationDbContext _context;

        public IndexModel(NotatkiWEB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PaginatedList<Note> Note { get;set; }
        [BindProperty]
        public UserViewModel UserVM { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public string TitleSort { get; set; }
        public string SubjectSort { get; set; }
        public string FirstNameSort { get; set; }
        public string LastNameSort { get; set; }
        public string SchoolSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex, string userId = null)
        {
            CurrentSort = sortOrder;
            TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            SubjectSort = sortOrder == "subject" ? "subject_desc" : "subject";
            FirstNameSort = sortOrder == "firstname" ? "firstname_desc" : "firstname";
            LastNameSort = sortOrder == "lastname" ? "lastname_desc" : "lastname";
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

            IQueryable<Note> noteIQ = from n in _context.Note
                                                    select n;
            if (!String.IsNullOrEmpty(searchString))
            {
                noteIQ = noteIQ.Where(n => n.Title.Contains(searchString)
                || n.SubjectList.SubjectName.Contains(searchString)
                || n.ApplicationUser.FirstName.Contains(searchString)
                || n.ApplicationUser.FirstName.Contains(searchString)
                || n.ApplicationUser.School.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title":
                    noteIQ = noteIQ.OrderBy(n => n.Title);
                    break;
                case "title_desc":
                    noteIQ = noteIQ.OrderByDescending(n => n.Title);
                    break;
                case "subject":
                    noteIQ = noteIQ.OrderBy(n => n.SubjectList.SubjectName);
                    break;
                case "subject_desc":
                    noteIQ = noteIQ.OrderByDescending(n => n.SubjectList.SubjectName);
                    break;
                case "firstname":
                    noteIQ = noteIQ.OrderBy(n => n.ApplicationUser.FirstName);
                    break;
                case "firstname_desc":
                    noteIQ = noteIQ.OrderByDescending(n => n.ApplicationUser.FirstName);
                    break;
                case "lastname":
                    noteIQ = noteIQ.OrderBy(n => n.ApplicationUser.LastName);
                    break;
                case "lastname_desc":
                    noteIQ = noteIQ.OrderByDescending(n => n.ApplicationUser.LastName);
                    break;
                case "school":
                    noteIQ = noteIQ.OrderBy(n => n.ApplicationUser.School);
                    break;
                case "school_desc":
                    noteIQ = noteIQ.OrderByDescending(n => n.ApplicationUser.School);
                    break;
                default:
                    noteIQ = noteIQ.OrderBy(n => n.IDNote);
                    break;
            }

            int pageSize = 5;
            Note = await PaginatedList<Note>.CreateAsync(noteIQ
                .Include(n => n.ApplicationUser)
                .Include(n => n.SubjectList)
                .AsNoTracking(), pageIndex ?? 1, pageSize);

            if (userId == null)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                userId = claim.Value;
            }
            //Note = await _context.Note
            //    .Include(n => n.ApplicationUser)
            //    .Include(n => n.SubjectList).ToListAsync();

            UserVM = new UserViewModel()
            {
                UserObj = await _context.ApplicationUser.FirstOrDefaultAsync(u => u.Id == userId)
            };
        }
    }
}
