using DAL.Enums;

namespace BLL.DTO
{
    public class JobApplicationDto
    {
        public int Id { get; set; }
        public int VacancyId { get; set; }
        public string VacancyTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string ApplicantName { get; set; } = string.Empty;
        public string ApplicantEmail { get; set; } = string.Empty;
        public string? ApplicantPhone { get; set; }
        public int? ResumeId { get; set; }
        public string? ResumeTitle { get; set; }
        public string? CoverLetter { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime AppliedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
