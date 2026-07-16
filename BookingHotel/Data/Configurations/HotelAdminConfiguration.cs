using BookingHotel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingHotel.Data.Configurations;

public class HotelAdminConfiguration: IEntityTypeConfiguration<HotelAdmin>
{
    public void Configure(EntityTypeBuilder<HotelAdmin> builder)
    {
        builder.Property(x => x.UserId)
            .IsRequired();
        
        builder.Property(x => x.Hotel)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Hotel)
            .WithMany(h => h.Admins)
            .HasForeignKey(x => x.HotelId);

        builder.HasIndex(ha => ha.UserId);
        builder.HasIndex(ha => ha.HotelId);
    }
}