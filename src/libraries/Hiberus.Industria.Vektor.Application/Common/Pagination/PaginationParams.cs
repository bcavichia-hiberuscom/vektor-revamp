namespace Hiberus.Industria.Vektor.Application.Common.Pagination;

/// <summary>
/// Pagination parameters for paginated queries.
/// Default page size: 20 items, maximum: 100 items.
/// </summary>
public record PaginationParams
{
    private const int DefaultPageSize = 20;
    private const int MaxPageSize = 100;
    private int _pageNumber = 1;
    private int _pageSize = DefaultPageSize;

    /// <summary>
    /// Gets or sets the page number (1-based index). Must be greater than 0.
    /// </summary>
    public int PageNumber
    {
        get => _pageNumber;
        init => _pageNumber = value > 0 ? value : 1;
    }

    /// <summary>
    /// Gets or sets the page size. Clamped between 1 and MaxPageSize (100).
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        init =>
            _pageSize = value <= 0 ? DefaultPageSize : (value > MaxPageSize ? MaxPageSize : value);
    }

    /// <summary>
    /// Initializes a new instance of the PaginationParams record.
    /// </summary>
    public PaginationParams() { }

    /// <summary>
    /// Initializes a new instance with specified page number and size.
    /// </summary>
    public PaginationParams(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    /// <summary>
    /// Calculates the number of records to skip for the current page.
    /// </summary>
    public int GetSkip() => (PageNumber - 1) * PageSize;
}
