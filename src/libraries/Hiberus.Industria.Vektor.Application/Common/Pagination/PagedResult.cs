namespace Hiberus.Industria.Vektor.Application.Common.Pagination;

/// <summary>
/// Generic paginated result containing items, total count, and pagination metadata.
/// </summary>
/// <typeparam name="T">The type of items in the result set.</typeparam>
public record PagedResult<T>
{
    /// <summary>
    /// Gets the collection of items for the current page.
    /// </summary>
    public IReadOnlyCollection<T> Data { get; init; } = [];

    /// <summary>
    /// Gets the total number of items across all pages (before pagination).
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Gets the current page number (1-based).
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// Gets the page size (number of items per page).
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;

    /// <summary>
    /// Gets a value indicating whether there are more pages available.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Gets a value indicating whether there are previous pages available.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Initializes a new instance of the PagedResult record.
    /// </summary>
    public PagedResult() { }

    /// <summary>
    /// Initializes a new instance with all required pagination data.
    /// </summary>
    /// <param name="data">The collection of items for the current page.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    /// <param name="pageNumber">The current page number (1-based).</param>
    /// <param name="pageSize">The page size.</param>
    public PagedResult(IReadOnlyCollection<T> data, int totalCount, int pageNumber, int pageSize)
    {
        Data = data;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
