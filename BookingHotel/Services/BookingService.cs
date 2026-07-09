using System.IdentityModel.Tokens.Jwt;
using BookingHotel.Contracts;
using BookingHotel.Data;
using BookingHotel.Data.Enums;
using BookingHotel.DTOs.Booking;
using BookingHotel.DTOs.User;
using BookingHotel.Results;
using Microsoft.EntityFrameworkCore;

namespace BookingHotel.Services;

public class BookingService(BookingHotelDbContext context, IHttpContextAccessor httpContextAccessor) : IBookingService
{
    public async Task<Result<IEnumerable<GetBookingDto>>> GetBookingsForHotel(int hotelId)
    {
        var hotelExists = await context.Bookings.AnyAsync(b => b.HotelId == hotelId);

        if (!hotelExists)
        {
            return Result<IEnumerable<GetBookingDto>>.Failure(new Error("NotFound","Hotel not found"));
        }

        var bookings = await context.Bookings
            .Where(b => b.HotelId == hotelId)
            .OrderBy(b => b.CheckIn)
            .Select(b=> new GetBookingDto(
                b.Id,
                b.HotelId,
                b.Hotel!.Name,
                b.CheckIn,
                b.CheckOut,
                b.Guests,
                b.TotalPrice,
                b.Status!.ToString(),
                b.CreatedAt,
                b.UpdatedAt
                ))
            .ToListAsync();
        
         return Result<IEnumerable<GetBookingDto>>.Success(bookings);
    }

    public async Task<Result<GetBookingDto>> CreateBooking(CreateBookingDto createBookingDto)
    {
        var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (userId == null || string.IsNullOrWhiteSpace(userId))
        {
            return Result<GetBookingDto>.Failure(new Error("Validation","User is required."));
        }

        var nights = createBookingDto.CheckOut.DayNumber - createBookingDto.CheckIn.DayNumber;

        if (nights <= 0)
        {
            return Result<GetBookingDto>.Failure(new Error("Validation","Checkout must be after CheckIn."));
        }
        
        if (createBookingDto.Guests <= 0)
        {
            return Result<GetBookingDto>.Failure(new Error("Validation","Guests must be at least 1."));
        }
        
        var hotel = await context.Hotels.Where(h=>h.Id == createBookingDto.HotelId).FirstOrDefaultAsync();

        if (hotel == null)
        {
            return Result<GetBookingDto>.Failure(new Error("NotFound","Hotel not found"));
        }

        var overlaps = await context.Bookings.AnyAsync(b =>
            b.HotelId == hotel.Id
            && b.Status != BookingStatus.Cancelled
            && b.CheckIn < createBookingDto.CheckOut
            && createBookingDto.CheckIn < b.CheckOut
            &&  b.UserId == userId
        );
        if (overlaps)
        {
            return Result<GetBookingDto>.Failure(new Error("Conflict","There is conflict."));
        }

        var totalPrice = hotel.PerNightRating * nights;

        var booking = new Booking
        {
            HotelId = createBookingDto.HotelId,
            UserId = userId,
            CheckIn = createBookingDto.CheckIn,
            CheckOut = createBookingDto.CheckOut,
            Guests = createBookingDto.Guests,
            TotalPrice = totalPrice,
            Status = BookingStatus.Pending
        };
        context.Add(booking);
        await context.SaveChangesAsync();

        var created = new GetBookingDto(
            booking.Id,
            booking.HotelId,
            hotel.Name,
            booking.CheckIn,
            booking.CheckOut,
            booking.Guests,
            booking.TotalPrice,
            booking.Status!.ToString(),
            booking.CreatedAt,
            booking.UpdatedAt
            );
        
        return Result<GetBookingDto>.Success(created);
    }
}