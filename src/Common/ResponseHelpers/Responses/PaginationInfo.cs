using System.Diagnostics.CodeAnalysis;

namespace Musdis.ResponseHelpers.Responses;

/// <summary>
///     Contains pagination details.
/// </summary>
public class PaginationInfo
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PaginationInfo"/> class.
    /// </summary>
    public PaginationInfo() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PaginationInfo"/> class.
    /// </summary>
    /// 
    /// <param name="currentPage">
    ///     The current page number.
    /// </param>
    /// <param name="pageSize">
    ///     The number of items per page.
    /// </param>
    /// <param name="totalCount">
    ///     The total number of items in the entire dataset.
    /// </param>
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
    public int TotalPages => PageSize <= 0
        ? 0
        : (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    ///     Is there a previous page of data items.
    /// </summary>
    public bool HasPrevious => CurrentPage > 1;

    /// <summary>
    ///     Is there a next page of data items.
    /// </summary>
    public bool HasNext => CurrentPage + 1 < TotalPages;
}