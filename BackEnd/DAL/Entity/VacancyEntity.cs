using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public enum EmploymentType
    {
        FullTime,
        PartTime,
        Contract,
        Internship,
        Remote
    }

    public class VacancyEntity: IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Location { get; set; } = string.Empty;

        [Required]
        public EmploymentType Type { get; set; } = EmploymentType.FullTime;

        [Required]
        public decimal? SalaryMin { get; set; }

        [Required]
        public decimal? SalaryMax { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime PostedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public UserEntity? User { get; set; }
    }

}
