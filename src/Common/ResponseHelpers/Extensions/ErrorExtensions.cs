using Microsoft.AspNetCore.Http;

using Musdis.OperationResults;
using Musdis.ResponseHelpers.Errors;

namespace Musdis.ResponseHelpers.Extensions;

/// <summary>
///     Extension methods for <see cref="Error"/> class.
/// </summary>
public static class ErrorExtensions
{
    /// <summary>
    ///     Converts any type of <see cref="Error"/> into HTTP <see cref="IResult"/>.
    /// </summary>
    /// <remarks>
    ///     Returns <see cref="HttpError.ToProblemHttpResult(string)"/> for errors 
    ///     derived from <see cref="HttpError"/>. 
    ///     Other errors converted into <see cref="InternalServerError"/> and 
    ///     its <see cref="HttpError.ToProblemHttpResult(string)"/> returned.
    /// </remarks>
    /// 
    /// <param name="error">
    ///     An <see cref="Error"/> to convert.
    /// </param>
     /// <param name="instance">
    ///     A URI reference that identifies the specific occurrence of the problem.
    /// </param>
    /// 
    /// <returns>
    ///     <see cref="IResult"/> as problem details.
    /// </returns>
    public static IResult ToHttpResult(this Error error, string instance)
    {
        return error switch
        {
            HttpError httpError => httpError.ToProblemHttpResult(instance),
            NoContentError => Results.NoContent(),
            _ => new InternalServerError(error.Description).ToProblemHttpResult(instance),
        };
    }
}