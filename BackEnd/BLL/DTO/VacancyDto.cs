using System;
using DAL.Entity;

namespace BLL.DTO
{
    
    public class VacancyDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public EmploymentType Type { get; set; } = EmploymentType.FullTime;

        public decimal? SalaryMin { get; set; }

        public decimal? SalaryMax { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime PostedAt { get; set; }

        public int UserId { get; set; }
    }
}
