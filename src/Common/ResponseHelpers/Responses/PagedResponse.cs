using System.Diagnostics.CodeAnalysis;

namespace Musdis.ResponseHelpers.Responses;

/// <summary>
///     Represents a response with pagination info.
/// </summary>
/// 
/// <typeparam name="TData">The type of data.</typeparam>
public class PagedDataResponse<TData>
{
    public PagedDataResponse() { }

    [SetsRequiredMembers]
    public PagedDataResponse(
        IEnumerable<TData> data,
        int currentPage,
        int pageSize,
        int totalCount
    )
    {
        Data = data;
        PaginationInfo = new PaginationInfo
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalCount = totalCount,
        };
    }

    [SetsRequiredMembers]
    public PagedDataResponse(
        IEnumerable<TData> data,
        PaginationInfo paginationInfo
    )
    {
        Data = data;
        PaginationInfo = paginationInfo;
    }

    /// <summary>
    ///     A requested collection of data. 
    /// </summary>
    public required IEnumerable<TData> Data { get; init; }

    /// <summary>
    ///     Additional information about pagination.
    /// </summary>
    public required PaginationInfo PaginationInfo { get; init; }
}