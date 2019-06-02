using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NotatkiWEB.Models;

namespace NotatkiWEB.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SubjectList> SubjectList { get; set; }
        public DbSet<SemesterList> SemesterList { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Note> Note { get; set; }
    }
}
