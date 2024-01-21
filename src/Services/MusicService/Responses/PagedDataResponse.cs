using System.Diagnostics.CodeAnalysis;

using Musdis.MusicService.Pagination;

namespace Musdis.MusicService.Responses;

/// <summary>
/// Represents response with pagination info.
/// </summary>
/// <typeparam name="TData">Type of data.</typeparam>
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
    public required IEnumerable<TData> Data { get; init; }
    public required PaginationInfo PaginationInfo { get; init; }
}


