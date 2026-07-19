namespace BookingHotel.DTOs.Pagination;

public class PaginationQuery
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}