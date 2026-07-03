using BookingHotel.DTOs.User;
using BookingHotel.Results;
using Microsoft.AspNetCore.Identity;

namespace BookingHotel.Contracts;

public interface IUsersService
{
     Task<Result<GetUserDto>> Register(CreateUserDto createUserDto);
     Task<Result<string>> Login(LoginUserDto loginUserDto);
     Task<string> GenerateToken(IdentityUser user);
}