using System.Diagnostics.CodeAnalysis;

namespace Musdis.ResponseHelpers.Responses;

/// <summary>
///     Represents a data response.
/// </summary>
/// 
/// <typeparam name="TData">
///     The type of data to be encapsulated in the response.
/// </typeparam>
public class DataResponse<TData>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DataResponse{TData}"/> class.
    /// </summary>
    public DataResponse() { }

    /// <summary>   
    ///     Initializes a new instance of the <see cref="DataResponse{TData}"/> class.
    /// </summary>
    /// <param name="data">
    ///     The data to be encapsulated in the response.
    /// </param>
    [SetsRequiredMembers]
    public DataResponse(TData data)
    {
        Data = data;
    }
    /// <summary>
    ///     The encapsulated data in the response.
    /// </summary>
    public virtual required TData Data { get; init; }
}