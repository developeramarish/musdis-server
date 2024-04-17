using Musdis.IdentityService.Dtos;
using Musdis.IdentityService.Requests;
using Musdis.OperationResults;

namespace Musdis.IdentityService.Services.Jwt;

/// <summary> 
///     The service for generating JWTs based on user data.
/// </summary>
public interface IJwtGenerator
{
    /// <summary>
    ///     Generates a JWT based on the provided user data.
    /// </summary>
    /// 
    /// <param name="request">
    ///     The request used to create the JWT.
    /// </param>
    /// 
    /// <returns>
    ///     The result of the operation. The result contains the generated JWT.
    /// </returns>
    Result<string> GenerateToken(GenerateJwtRequest request);
}
