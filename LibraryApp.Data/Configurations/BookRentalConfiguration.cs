using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.Data.Configurations;

internal class BookRentalConfiguration : IEntityTypeConfiguration<BookRental>
{
    public void Configure(EntityTypeBuilder<BookRental> builder)
    {
        builder
            .HasKey(br => new { br.UserId, br.BookId });

        builder
            .HasOne(br => br.User)
            .WithMany(u => u.Books)
            .HasForeignKey(br => br.UserId);

        builder
            .HasOne(br => br.Book)
            .WithMany(b => b.RentedUsers)
            .HasForeignKey(br => br.BookId);

        builder
            .Property(br => br.RentalDate)
            .IsRequired();
    }
}