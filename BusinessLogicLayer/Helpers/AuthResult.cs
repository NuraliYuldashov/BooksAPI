using Microsoft.AspNetCore.Identity;

namespace BusinessLogicLayer.Helpers;

public record AuthResult (bool isSuccess, 
                          List<string> errorMessage)
{
    public bool IsSuccesed = isSuccess;
    public List<string> ErrorMessage = errorMessage;

    public static implicit operator AuthResult(IdentityResult result)
        => new(isSuccess: result.Succeeded, 
           errorMessage: result.Errors
                               .Select(error => error.Description)
                               .ToList());
}
