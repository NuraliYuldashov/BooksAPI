using System.ComponentModel.DataAnnotations;

namespace BooksAPI.DTOs.UserDtos;

public class RegisterUserDto : LoginUserDto
{
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
}
