using DAL.Entity;

namespace BLL.DTO
{
    public class CreateVacancyDto
    {
        public string Title { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public string Location { get; set; } = string.Empty;
        public EmploymentType EmploymentType { get; set; }
        public WorkFormat WorkFormat { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string Description { get; set; } = string.Empty;
        public VacancyStatus Status { get; set; } = VacancyStatus.Draft;
        public int UserId { get; set; }
    }
}