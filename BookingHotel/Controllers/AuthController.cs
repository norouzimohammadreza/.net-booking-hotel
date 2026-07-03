using BookingHotel.Contracts;
using BookingHotel.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Controllers;

[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class AuthController(IUsersService usersService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<GetUserDto>> Register(CreateUserDto createUserDto)
    {
       var result = await usersService.Register(createUserDto);
       if (!result.IsSuccess)
       {
           return MapErrorsToResponse(result.Errors);
       }
       return ToActionResult(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginUserDto loginUserDto)
    {
        var result = await usersService.Login(loginUserDto);
        if (!result.IsSuccess)
        {
           return MapErrorsToResponse(result.Errors);
        }
        return ToActionResult(result);
    }
}