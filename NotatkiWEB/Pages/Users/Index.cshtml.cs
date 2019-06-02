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

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            IQueryable<ApplicationUser> studentIQ = from s in _db.ApplicationUser
                                            select s;

            int pageSize = 3;
            ApplicationUserList = await PaginatedList<ApplicationUser>.CreateAsync(studentIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            return Page();
        }
    }
}