namespace BookingHotel.Results;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; init; } = [];

    public int Page { get; init; }

    public int PageSize { get; init; }

    public int TotalCount { get; init; }

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}