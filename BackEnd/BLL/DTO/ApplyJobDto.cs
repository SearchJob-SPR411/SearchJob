using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class ApplyJobDto
    {
        [Required]
        public int VacancyId { get; set; }

        public int? ResumeId { get; set; }

        [MaxLength(2000)]
        public string? CoverLetter { get; set; }
    }
}
