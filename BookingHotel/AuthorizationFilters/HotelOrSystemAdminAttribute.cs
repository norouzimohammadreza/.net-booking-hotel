using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.AuthorizationFilters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method , AllowMultiple = false)]
public class HotelOrSystemAdminAttribute : TypeFilterAttribute
{
    public HotelOrSystemAdminAttribute() : base(typeof(HotelOrSystemAdminFilter))
    {
    }
} 