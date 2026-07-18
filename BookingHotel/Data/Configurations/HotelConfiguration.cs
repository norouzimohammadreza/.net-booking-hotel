using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingHotel.Entities;

namespace BookingHotel.Data.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {   
        builder.Property(h => h.Name)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(h => h.Address)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(h => h.Rating)
            .HasDefaultValue(0);
        
        builder.Property(h => h.PerNightRating)
            .HasDefaultValue(0);

        builder.Property(h => h.PerNightRating)
            .HasDefaultValue(0);

        builder.HasOne(h => h.Country)
            .WithMany(c => c.Hotels)
            .HasForeignKey(h => h.CountryId);

        builder.HasIndex(h => h.CountryId);
    }
}