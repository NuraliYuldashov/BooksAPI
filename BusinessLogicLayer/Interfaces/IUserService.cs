using BooksAPI.DTOs.UserDtos;
using BusinessLogicLayer.Helpers;

namespace BusinessLogicLayer.Interfaces;

public interface IUserService
{
    Task<AuthResult> RegisterUserAsync(RegisterUserDto dto);
    Task<LoginResult> LoginUserAsync(LoginUserDto dto);

    Task<AuthResult> ChangePasswordAsync(ChangePasswordDto dto);
    Task<AuthResult> DeleteAccountAsync(string email);
}
