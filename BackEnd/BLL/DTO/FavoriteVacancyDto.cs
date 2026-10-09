namespace BLL.DTO
{
    public class FavoriteVacancyDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VacancyId { get; set; }
        public VacancyDto? Vacancy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
