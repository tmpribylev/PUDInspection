using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUDInspection.Models
{
    public class PUD
    {
        public int Id { get; set; }
        public string LinkId { get; set; }
        public static string LinkPath { get; set; } = "https://lms.hse.ru/index.php?page=discipline_programs&page_point=summary&dp_id=";
        [NotMapped]
        public string Link
        {
            get
            {
                return LinkPath + LinkId;
            }
        }
        public EduProgram EduProgram { get; set; }
        public Department Department { get; set; }
        public string EducationStage { get; set; }
        public string Language { get; set; }
        public string CourseName { get; set; }
        public List<Check> Checks { get; set; }
        public string Details { get; set; }
        public List<InspectionPUDResult> InspectionPUDResults { get; set; }
        public List<ValidationPUDResult> ValidationPUDResults { get; set; }
    }
}
