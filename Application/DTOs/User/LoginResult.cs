namespace Application.DTOs.User
{
    public enum LoginResult
    {
        Success,
        UserNotFound,
        PasswordOrUserNameNotCorrect,
        EmailNotConfirmed
    }
}
