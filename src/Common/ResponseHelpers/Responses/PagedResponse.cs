using System.Diagnostics.CodeAnalysis;

namespace Musdis.ResponseHelpers.Responses;

/// <summary>
///     Represents a response with pagination info.
/// </summary>
/// 
/// <typeparam name="TData">The type of data.</typeparam>
public class PagedDataResponse<TData>
{
    /// <summary>
    ///     Creates <see cref="PagedDataResponse{T}"/>.
    /// </summary>
    public PagedDataResponse() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PagedDataResponse{TData}"/> class.
    /// </summary>
    /// 
    /// <param name="data">
    ///     The collection of data items.
    /// </param>
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


    /// <summary>
    ///     Initializes a new instance of the <see cref="PagedDataResponse{TData}"/> class.
    /// </summary>
    /// 
    /// <param name="data">
    ///     The collection of data items.
    /// </param>
    /// <param name="paginationInfo">
    ///     An additional information about pagination.
    /// </param>
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