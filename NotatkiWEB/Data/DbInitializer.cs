﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NotatkiWEB.Models;
using NotatkiWEB.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotatkiWEB.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch(Exception ex)
            {

            }

            if (_db.Roles.Any(r => r.Name == SD.AdminEndUser)) return;
            _roleManager.CreateAsync(new IdentityRole(SD.AdminEndUser)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.StudentEndUser)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@outlook.com",
                Email = "admin@outlook.com",
                FirstName = "Marcin",
                LastName = "Chmurowski",
                EmailConfirmed = true,
                School = "Politechnika Opolska"
            }, "Admin123*").GetAwaiter().GetResult();

            _userManager.AddToRoleAsync(_db.Users.FirstOrDefaultAsync(u => u.Email == "admin@outlook.com").GetAwaiter().GetResult(), SD.AdminEndUser).GetAwaiter().GetResult();
        }
    }
}
