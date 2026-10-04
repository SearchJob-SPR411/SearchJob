using DAL.Entity;

namespace BLL.DTO
{
    public class VacancyDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public EmploymentType EmploymentType { get; set; }
        public WorkFormat WorkFormat { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string Description { get; set; } = string.Empty;
        public VacancyStatus Status { get; set; }
        public DateTime PostedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int UserId { get; set; }
    }
}