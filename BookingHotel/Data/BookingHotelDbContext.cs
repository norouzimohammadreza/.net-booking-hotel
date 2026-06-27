using System;
using System.Collections.Generic;
using BookingHotel.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingHotel.Data;

public partial class BookingHotelDbContext : DbContext
{
    public BookingHotelDbContext()
    {
    }

    public BookingHotelDbContext(DbContextOptions<BookingHotelDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=BookingHotelDb;Username=postgres;Password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasIndex(e => e.CountryId, "IX_Hotels_CountryId");

            entity.HasOne(d => d.Country).WithMany(p => p.Hotels).HasForeignKey(d => d.CountryId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
