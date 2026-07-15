using System.Security.Claims;
using BookingHotel.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BookingHotel.AuthorizationFilters;

public class HotelOrSystemAdminFilter(BookingHotelDbContext dbContext):IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpUser = context.HttpContext.User;
        if (httpUser?.Identity?.IsAuthenticated == false)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        if (httpUser!.IsInRole("Admin"))
        {
            return;
        }

        var userId = httpUser.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? httpUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            context.Result = new ForbidResult();
            return;
        }
        
        context.RouteData.Values.TryGetValue("hotelId", out var hotelIdObj);
        int.TryParse(hotelIdObj?.ToString(),out int hotelId );
        if (hotelId == 0)
        {
            context.Result = new ForbidResult();
            return;
        }
        
        var isHotelAdmin = await dbContext.HotelAdmins.AnyAsync(q=> q.HotelId == hotelId && q.UserId == userId);
        if (!isHotelAdmin)
        {
            context.Result = new ForbidResult();
            return;
        }
        
    }
}