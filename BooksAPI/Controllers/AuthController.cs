using BooksAPI.DTOs.UserDtos;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        var result = await _userService.RegisterUserAsync(dto);
        if (result.IsSuccesed)
        {
            return Ok("User Created!");
        }

        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginUserDto dto)
    {
        var result = await _userService.LoginUserAsync(dto);

        if (result.IsSuccesed)
        {
            return Ok(result.Token);
        }

        return BadRequest(result.ErrorMessage);
    }

    [HttpPut("[action]")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto) 
    {
        var result = await _userService.ChangePasswordAsync(dto);
        if (result.IsSuccesed)
        {
            return Ok("Password Changed!!!");
        }

        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("[action]")]
    [Authorize]
    public async Task<IActionResult> DeleteAccount(string email)
    {
        var result = await _userService.DeleteAccountAsync(email);
        if (result.IsSuccesed)
        {
            return Ok("Account Deleted!");
        }

        return BadRequest(result.errorMessage);
    }
}
