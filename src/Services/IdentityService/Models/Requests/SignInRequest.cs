namespace IdentityService.Models.Requests;

/// <summary>
/// Represents a request for user sign-in, containing user identification and password.
/// </summary>
/// <param name="UserNameOrEmail"> The user's username or email used for identification during sign-in. </param>
/// <param name="Password"> The user's password for authentication. </param>
public record SignInRequest(
    string UserNameOrEmail,
    string Password
);