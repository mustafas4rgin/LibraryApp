using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.Data.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.PasswordSalt)
            .IsRequired();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(u => u.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(d => d.Role)
            .WithMany()
            .HasForeignKey(d => d.RoleId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(u => u.Books)
            .WithOne(br => br.User)
            .HasForeignKey(br => br.UserId);
    }
}