using System.Security.Claims;

namespace Musdis.IdentityService.Extensions;

/// <summary>
///     Provides extension methods for working with collections of <see cref="Claim"/>.
/// </summary>
public static class ClaimsExtensions
{
    /// <summary>
    ///     Converts a collection of claims to a sequence of key-value pairs.
    /// </summary>
    /// 
    /// <param name="claims"> 
    /// The collection of claims to convert. 
    /// </param>
    /// 
    /// <returns> 
    ///     An <see cref="IEnumerable{T}"/> of key-value pairs representing the claims. 
    /// </returns>
    public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(
        this IEnumerable<Claim> claims
    )
    {
        return claims.Select(c => new KeyValuePair<string, string>(c.Type, c.Value));
    }
}