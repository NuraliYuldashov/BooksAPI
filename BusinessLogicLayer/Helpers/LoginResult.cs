namespace BusinessLogicLayer.Helpers;

public class LoginResult
{
    public bool IsSuccesed { get; set; } = true;
    public List<string> ErrorMessage { get; set; } = new();
    public string Token { get; set; } = string.Empty;
    public LoginResult(bool isSuccessed, 
                       List<string> errorMessages,
                       string token = "")
    {
        IsSuccesed = isSuccessed;
        ErrorMessage = errorMessages;
        Token = token;
    }
}
