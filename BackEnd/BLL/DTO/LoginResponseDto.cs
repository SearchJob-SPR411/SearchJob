namespace BLL.DTO
{
    public class LoginResponseDto
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? Token { get; set; }

        public UserDto? User { get; set; }
    }
}
