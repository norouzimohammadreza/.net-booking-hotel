using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingHotel.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "012480c1-2e64-43a4-a30c-f931d1c0920a",
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "012480c1-2e64-43a4-a30c-f931d1c0920a"
            },
            new IdentityRole
            {
                Id = "e3d430c7-6604-44a5-94b1-07bcf5d0fbc8",
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = "e3d430c7-6604-44a5-94b1-07bcf5d0fbc8"
            },
            new IdentityRole
            {
            Id = "012480c1-6604-44a5-94b1-f931d1c0920a",
            Name = "Hotel Admin",
            NormalizedName = "HOTEL ADMIN",
            ConcurrencyStamp = "012480c1-6604-44a5-94b1-f931d1c0920a"
               }
        );
    }
}