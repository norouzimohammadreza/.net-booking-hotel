using System;
using System.Collections.Generic;

namespace BookingHotel.Models;

public partial class Hotel
{
    public int Id { get; set; }
 
    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public double Rating { get; set; }

    public int CountryId { get; set; }

    public virtual Country Country { get; set; } = null!;
}
