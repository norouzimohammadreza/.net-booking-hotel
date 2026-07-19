using BookingHotel.Contracts;
using BookingHotel.Data;
using BookingHotel.Entities;
using BookingHotel.Data.Enums;
using BookingHotel.DTOs.Booking;
using BookingHotel.Results;
using Microsoft.EntityFrameworkCore;

namespace BookingHotel.Services;

public class BookingService(BookingHotelDbContext context, IUsersService usersService) : IBookingService
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

    public async Task<Result<IEnumerable<GetBookingDto>>> GetUserBookings(int hotelId)
    {
        var userId = usersService.GetUserId();
        var hotelExists = await context.Bookings.AnyAsync(b => b.HotelId == hotelId);

        if (!hotelExists)
        {
            return Result<IEnumerable<GetBookingDto>>.Failure(new Error("NotFound","Hotel not found"));
        }

        var bookings = await context.Bookings
            .Where(b => b.HotelId == hotelId && b.UserId == userId)
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
        var userId = usersService.GetUserId();

        if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
        {
            return Result<GetBookingDto>.Failure(new Error("Validation","User is required."));
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
        
        var booking = new Booking
        (
            createBookingDto.HotelId,
            userId,
            createBookingDto.CheckIn,
            createBookingDto.CheckOut,
            createBookingDto.Guests,
            hotel.PerNightRating
        );
        
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
        var userId = usersService.GetUserId();

        if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
        {
            return Result<GetBookingDto>.Failure(new Error("Validation","User is required."));
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

        booking.Update(
            updateBookingDto.CheckIn,
            updateBookingDto.CheckOut,
            updateBookingDto.Guests,
            booking.Hotel!.PerNightRating
            );

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
        var userId = usersService.GetUserId();

        if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
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
        
        booking.Cancel();
        await context.SaveChangesAsync();
        return Result.Success(); 
    }

    public async Task<Result> AdminCancelBooking(int hotelId, int bookingId)
    {
        var userId = usersService.GetUserId();

        if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
        {
            return Result.Failure(new Error("Validation","User is required."));
        }
        
        var booking = await context.Bookings
            .Include(booking => booking.Hotel!)
            .FirstOrDefaultAsync(b =>
                b.Id == bookingId
                && b.HotelId == hotelId);

        if (booking == null)
        {
            return Result.Failure(new Error("NotFound","Booking not found"));
        }

        if (booking.Status == BookingStatus.Cancelled)
        {
            return Result.Failure(new Error("Conflict","Cancelled booking"));
        }
        
        booking.Cancel();
        await context.SaveChangesAsync();
        return Result.Success(); 
    }

    public async Task<Result> AdminConfirmBooking(int hotelId, int bookingId)
    {
        var userId = usersService.GetUserId();

        if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
        {
            return Result.Failure(new Error("Validation","User is required."));
        }
        
        var booking = await context.Bookings
            .Include(booking => booking.Hotel!)
            .FirstOrDefaultAsync(b =>
                b.Id == bookingId
                && b.HotelId == hotelId);

        if (booking == null)
        {
            return Result.Failure(new Error("NotFound","Booking not found"));
        }

        if (booking.Status == BookingStatus.Cancelled)
        {
            return Result.Failure(new Error("Conflict","Cancelled booking"));
        }
        
        booking.Confirm();
        await context.SaveChangesAsync();
        return Result.Success(); 
    }
}