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

    public async Task<Result<GetBookingDto>> UpdateBooking(int hotelId, int bookingId, UpdateBookingDto updateBookingDto)
    {
        var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (userId == null || string.IsNullOrWhiteSpace(userId))
        {
            return Result<GetBookingDto>.Failure(new Error("Validation","User is required."));
        }

        var nights = updateBookingDto.CheckOut.DayNumber - updateBookingDto.CheckIn.DayNumber;

        if (nights <= 0)
        {
            return Result<GetBookingDto>.Failure(new Error("Validation","Checkout must be after CheckIn."));
        }
        
        if (updateBookingDto.Guests <= 0)
        {
            return Result<GetBookingDto>.Failure(new Error("Validation","Guests must be at least 1."));
        }
        
        var overlaps = await context.Bookings.AnyAsync(b =>
            b.HotelId == hotelId
            && b.Status != BookingStatus.Cancelled
            && b.CheckIn < updateBookingDto.CheckOut
            && updateBookingDto.CheckIn < b.CheckOut
            &&  b.UserId == userId
        );
        if (overlaps)
        {
            return Result<GetBookingDto>.Failure(new Error("Conflict","There is conflict."));
        }

        var booking = await context.Bookings
            .Include(booking => booking.Hotel!)
            .FirstOrDefaultAsync(b =>
                b.Id == bookingId
                && b.HotelId == hotelId
                && b.UserId == userId);

        if (booking == null)
        {
            return Result<GetBookingDto>.Failure(new Error("NotFound","Booking not found"));
        }

        if (booking.Status == BookingStatus.Cancelled)
        {
            return Result<GetBookingDto>.Failure(new Error("Conflict","Cancelled booking"));
        }

        var perNight = booking.Hotel!.PerNightRating;
        booking.CheckIn = updateBookingDto.CheckIn;
        booking.CheckOut = updateBookingDto.CheckOut;
        booking.Guests = updateBookingDto.Guests;
        booking.TotalPrice = perNight * (updateBookingDto.CheckOut.DayNumber - updateBookingDto.CheckIn.DayNumber);
        booking.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        var updated = new GetBookingDto(
            booking.Id,
            booking.HotelId,
            booking.Hotel!.Name,
            booking.CheckIn,
            booking.CheckOut,
            booking.Guests,
            booking.TotalPrice,
            booking.Status.ToString(),
            booking.CreatedAt,
            booking.UpdatedAt
        );
        return Result<GetBookingDto>.Success(updated);

    }

    public async Task<Result> CancelBooking(int hotelId, int bookingId)
    {
        var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (userId == null || string.IsNullOrWhiteSpace(userId))
        {
            return Result.Failure(new Error("Validation","User is required."));
        }
        
        var booking = await context.Bookings
            .Include(booking => booking.Hotel!)
            .FirstOrDefaultAsync(b =>
                b.Id == bookingId
                && b.HotelId == hotelId
                && b.UserId == userId);

        if (booking == null)
        {
            return Result.Failure(new Error("NotFound","Booking not found"));
        }

        if (booking.Status == BookingStatus.Cancelled)
        {
            return Result.Failure(new Error("Conflict","Cancelled booking"));
        }
        
        booking.Status = BookingStatus.Cancelled;
        booking.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return Result.Success(); 
    }
}