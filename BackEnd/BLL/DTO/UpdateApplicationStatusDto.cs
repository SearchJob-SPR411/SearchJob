using System.ComponentModel.DataAnnotations;
using DAL.Enums;

namespace BLL.DTO
{
    public class UpdateApplicationStatusDto
    {
        [Required]
        public ApplicationStatus Status { get; set; }
    }
}
