using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookingHotel.Contracts;
using BookingHotel.DTOs.User;
using BookingHotel.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BookingHotel.Services;

public class UsersService(UserManager<IdentityUser> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IUsersService
{
    public async Task<Result<GetUserDto>> Register(CreateUserDto createUserDto)
    {
        var userModel = new IdentityUser
        {
            Email = createUserDto.Email,
            UserName = createUserDto.Email,
        };
        var result = await userManager.CreateAsync(userModel,createUserDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => new Error("BadRequest", error.Description)).ToArray();
            return Result<GetUserDto>.Failure(errors);
        }
        
        await userManager.AddToRoleAsync(userModel,createUserDto.Role);
        var registeredUserDto = new GetUserDto
        {
            Id = userModel.Id,
            Email = userModel.Email,
            Role = createUserDto.Role
        };
        return Result<GetUserDto>.Success(registeredUserDto);
    }

    public async Task<Result<string>> Login(LoginUserDto loginUserDto)
    {
        var user = await userManager.FindByEmailAsync(loginUserDto.Email);

        if (user == null)
        {
            return Result<string>.Failure(new Error("BadRequest","User not found"));
        }

        var valid = await userManager.CheckPasswordAsync(user, loginUserDto.Password);
        if (valid == false)
        {
            return Result<string>.Failure(new Error("BadRequest", "Invalid login"));
        }
        var token = await GenerateToken(user);
        return Result<string>.Success(token);
    }

    public async Task<string> GenerateToken(IdentityUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id), 
            new Claim(JwtRegisteredClaimNames.Email, user.Email!), 
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.PreferredUsername , user.Email!)
        };
        var roles = await userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

        claims = claims.Union(roleClaims).ToList();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["JwtSettings:Issuer"],
            audience: configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["JwtSettings:DurationInMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token); 
    }

    public string GetUserId()
    {
        return httpContextAccessor?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
               ?? httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? string.Empty ;
    }
}