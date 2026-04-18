using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FitnessClassConfiguration : IEntityTypeConfiguration<FitnessClass>
{
    public void Configure(EntityTypeBuilder<FitnessClass> builder)
    {
        builder.ToTable("FitnessClasses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.InstructorName)
            .HasMaxLength(200);

        builder.Property(x => x.Category)
            .HasMaxLength(100);

        builder.Property(x => x.StartTime)
            .IsRequired();

        builder.Property(x => x.EndTime)
            .IsRequired();
    }
}