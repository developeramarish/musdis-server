using System.Diagnostics.CodeAnalysis;

namespace Musdis.ResponseHelpers.Responses;

/// <summary>
///     Contains pagination details.
/// </summary>
public class PaginationInfo
{
    public PaginationInfo() { }

    [SetsRequiredMembers]
    public PaginationInfo(int currentPage, int pageSize, int totalCount)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    /// <summary>
    ///     The current page of data items.
    /// </summary>
    public required int CurrentPage { get; init; }

    /// <summary>
    ///     The number of data items per page.
    /// </summary>
    public required int PageSize { get; init; }

    /// <summary>
    ///     The total count of data items.
    /// </summary>
    public required int TotalCount { get; init; }

    /// <summary>
    ///     The total count of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    ///     Is there a previous page of data items.
    /// </summary>
    public bool HasPrevious => CurrentPage > 1;

    /// <summary>
    ///     Is there a next page of data items.
    /// </summary>
    public bool HasNext => CurrentPage + 1 < TotalPages;
}