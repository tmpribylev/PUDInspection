using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PUDInspection.Models;
using PUDInspection.ViewModels;

namespace PUDInspection.Data
{
    public class FileReader
    {
        private readonly ApplicationDbContext _context;

        public FileReader()
        {
            this._context = null;
        }
        public FileReader (ApplicationDbContext context)
        {
            this._context = context;
        }

        public List<PUDInfoViewModel> ReadPUD(IFormFile file)
        {
            List<PUDInfoViewModel> puds = new List<PUDInfoViewModel>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    reader.Read();
                    while (reader.Read()) //Each ROW
                    {
                        puds.Add(new PUDInfoViewModel
                        {
                            LinkID = reader.GetValue(0).ToString(),
                            Campus = reader.GetValue(1).ToString(),
                            Faculty = reader.GetValue(2).ToString(),
                            EducationStage = reader.GetValue(3).ToString(),
                            OP = reader.GetValue(4).ToString(),
                            Department = reader.GetValue(5).ToString(),
                            Language = reader.GetValue(7).ToString(),
                            Name = reader.GetValue(6).ToString(),
                            Details = reader.GetValue(8).ToString()
                        });
                    }
                }
            }

            return puds;
        }

        public async Task<bool> UploadPUDToDatabase(IFormFile file)
        {
            List<PUDInfoViewModel> pudModels = this.ReadPUD(file);
            bool success = true;

            var all_puds = await _context.PUDs.Include(i => i.EduProgram).Include(i => i.Department).ToListAsync();
            var all_eduprog = await _context.EduPrograms.Include(i => i.Faculty).ToListAsync();
            var all_depart = await _context.Departments.ToListAsync();
            var all_faculty = await _context.Faculties.Include(i => i.Campus).ToListAsync();
            var all_campus= await _context.Campuses.ToListAsync();

            foreach (PUDInfoViewModel model in pudModels)
            {
                PUD pud = new PUD()
                {
                    LinkId = model.LinkID,
                    EducationStage = model.EducationStage,
                    CourseName = model.Name,
                    Language = model.Language,
                    Details = model.Details
                };

                var campus = all_campus.Find(item => item.Name == model.Campus);
                if (campus == null)
                {
                    campus = new Campus { Name = model.Campus };
                    _context.Add(campus);
                }

                var falulty = all_faculty.Find(item => item.Name == model.Faculty);
                if (falulty == null)
                {
                    falulty = new Faculty { Name = model.Faculty, Campus = campus };
                    _context.Add(falulty);
                }

                var edu_prog = all_eduprog.Find(item => item.Name == model.OP);
                if (edu_prog == null)
                {
                    edu_prog = new EduProgram { Name = model.OP, Faculty = falulty};
                    _context.Add(edu_prog);
                }
                pud.EduProgram = edu_prog;

                var department = all_depart.Find(item => item.Name == model.Department);
                if (department == null)
                {
                    department = new Department { Name = model.Department};
                    _context.Add(department);
                }
                pud.Department = department;

                var db_pud = all_puds.Find(item =>
                {
                    return item.CourseName == pud.CourseName
                           && item.EduProgram.Name == pud.EduProgram.Name
                           && item.EducationStage == pud.EducationStage
                           && item.EduProgram.Faculty.Name == pud.EduProgram.Faculty.Name
                           && item.EduProgram.Faculty.Campus.Name == pud.EduProgram.Faculty.Campus.Name;
                });
                if (db_pud == null)
                {
                    _context.Add(pud);
                }
            }

            await _context.SaveChangesAsync();

            return success;
        }

        public async Task<bool> UploadPUDToDatabase(IFormFile file, Inspection inspection)
        {
            List<PUDInfoViewModel> pudModels = this.ReadPUD(file);
            bool success = true;

            var all_puds = await _context.PUDs.Include(i => i.EduProgram).Include(i => i.Department).Include(i => i.Checks).ToListAsync();
            var all_eduprog = await _context.EduPrograms.Include(i => i.Faculty).ToListAsync();
            var all_depart = await _context.Departments.ToListAsync();
            var all_faculty = await _context.Faculties.Include(i => i.Campus).ToListAsync();
            var all_campus = await _context.Campuses.ToListAsync();

            List<Campus> new_campuses = new List<Campus>();
            List<Faculty> new_faculties = new List<Faculty>();
            List<EduProgram> new_eduPrograms = new List<EduProgram>();
            List<Department> new_departments = new List<Department>();

            foreach (PUDInfoViewModel model in pudModels)
            {
                PUD pud = new PUD()
                {
                    LinkId = model.LinkID,
                    EducationStage = model.EducationStage,
                    CourseName = model.Name,
                    Language = model.Language,
                    Details = model.Details
                };

                var campus = all_campus.Find(item => item.Name == model.Campus);
                var new_campus = new_campuses.Find(item => item.Name == model.Campus);
                if (campus == null && new_campus == null)
                {
                    campus = new Campus { Name = model.Campus };
                    _context.Add(campus);
                    new_campuses.Add(campus);
                }
                else if (campus == null)
                {
                    campus = new_campus;
                }

                var falulty = all_faculty.Find(item => item.Name == model.Faculty);
                var new_faculty = new_faculties.Find(item => item.Name == model.Faculty);
                if (falulty == null && new_faculty == null)
                {
                    falulty = new Faculty { Name = model.Faculty, Campus = campus };
                    _context.Add(falulty);
                    new_faculties.Add(falulty);
                }
                else if (falulty == null)
                {
                    falulty = new_faculty;
                }

                var edu_prog = all_eduprog.Find(item => item.Name == model.OP);
                var new_eduProg = new_eduPrograms.Find(item => item.Name == model.OP);
                if (edu_prog == null && new_eduProg == null)
                {
                    edu_prog = new EduProgram { Name = model.OP, Faculty = falulty };
                    _context.Add(edu_prog);
                    new_eduPrograms.Add(edu_prog);
                }
                else if (edu_prog == null)
                {
                    edu_prog = new_eduProg;
                }
                pud.EduProgram = edu_prog;

                var department = all_depart.Find(item => item.Name == model.Department);
                var new_department = new_departments.Find(item => item.Name == model.Department);
                if (department == null && new_department==null)
                {
                    department = new Department { Name = model.Department };
                    _context.Add(department);
                    new_departments.Add(department);
                }
                else if (department == null)
                {
                    department = new_department;
                }
                pud.Department = department;

                var db_pud = all_puds.Find(item =>
                {
                    return item.CourseName == pud.CourseName
                           && item.EduProgram.Name == pud.EduProgram.Name
                           && item.EducationStage == pud.EducationStage
                           && item.EduProgram.Faculty.Name == pud.EduProgram.Faculty.Name
                           && item.EduProgram.Faculty.Campus.Name == pud.EduProgram.Faculty.Campus.Name;
                });

                if (db_pud == null)
                {
                    pud.Checks = new List<Check>
                    {
                        inspection
                    };

                    _context.Add(pud);
                }
                else
                {
                    if (!db_pud.Checks.Contains(inspection))
                    {
                        db_pud.Checks.Add(inspection);
                    }
                    _context.Update(db_pud);
                }
            }

            await _context.SaveChangesAsync();

            return success;
        }
    }
}
