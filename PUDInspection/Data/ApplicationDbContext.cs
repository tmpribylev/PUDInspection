using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PUDInspection.Models;
using PUDInspection.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PUDInspection.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Inspection> Inspections { get; set; }
        public DbSet<Validation> Validations { get; set; }
        public DbSet<CheckResult> CheckResults { get; set; }
        public DbSet<CheckResultEvaluation> CheckResultEvaluations { get; set; }
        public DbSet<CheckVsCriteria> CheckVsCriterias { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Criteria> Criterias { get; set; }
        public DbSet<CriteriaEmailText> CriteriaEmailTexts { get; set; }
        public DbSet<EmailText> EmailTexts { get; set; }
        public DbSet<InspectionSpace> InspectionSpaces { get; set; }
        public DbSet<PUD> PUDs { get; set; }
        public DbSet<PUDAllocation> PUDAllocations { get; set; }
        public DbSet<PUDChange> PUDChanges { get; set; }
        public DbSet<ReportPattern> ReportPatterns { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<EduProgram> EduPrograms { get; set; }
        public DbSet<Department> Departments { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
    }
}
