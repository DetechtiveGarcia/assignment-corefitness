using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ClassBookingConfiguration : IEntityTypeConfiguration<ClassBooking>
{
    public void Configure(EntityTypeBuilder<ClassBooking> builder)
    {
        builder.ToTable("ClassBookings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MemberId)
            .IsRequired();

        builder.Property(x => x.FitnessClassId)
            .IsRequired();

        builder.Property(x => x.BookedAt)
            .IsRequired();

        builder.HasIndex(x => new { x.MemberId, x.FitnessClassId })
            .IsUnique();

        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<FitnessClass>()
            .WithMany()
            .HasForeignKey(x => x.FitnessClassId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}