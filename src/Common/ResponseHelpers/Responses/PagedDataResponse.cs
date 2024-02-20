using System.Diagnostics.CodeAnalysis;

namespace Musdis.ResponseHelpers.Responses;

/// <summary>
///     Represents a response with pagination info.
/// </summary>
/// 
/// <typeparam name="TData">The type of data.</typeparam>
public class PagedDataResponse<TData> : DataResponse<IEnumerable<TData>> 
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PagedDataResponse{TData}"/> class.
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
    ) : base(data)
    {
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
    ) : base(data)
    {
        PaginationInfo = paginationInfo;
    }

    /// <summary>
    ///     Additional information about pagination.
    /// </summary>
    public required PaginationInfo PaginationInfo { get; init; }
}