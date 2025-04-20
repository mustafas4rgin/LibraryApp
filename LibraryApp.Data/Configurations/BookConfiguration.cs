using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualBasic;

namespace LibraryApp.Data.Configurations;

internal class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(b => b.ISBN)
            .IsRequired();

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(b => b.Page)
            .IsRequired();

        builder.Property(b => b.Stock)
            .IsRequired();

        builder
            .HasMany(b => b.RentedUsers)
            .WithOne(br => br.Book)
            .HasForeignKey(br => br.BookId);

    }
}