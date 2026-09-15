using System;
using System.Collections.Generic;

namespace BLL.DTO
{
    public class UserDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<VacancyDto> Vacancies { get; set; } = new List<VacancyDto>();
    }
}
