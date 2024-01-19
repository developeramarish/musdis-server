
using Microsoft.AspNetCore.Http.HttpResults;

using Musdis.Results;

namespace Musdis.IdentityService.Extensions;

public static class ErrorExtensions
{
    public static Results ToProblemResult(this Error error)
    {
        return;
    }
}

