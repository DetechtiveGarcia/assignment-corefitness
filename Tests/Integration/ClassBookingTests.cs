using Application.Services.ClassBookings;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;
using Tests.Fixtures;
using Xunit;

namespace Tests.Integration;

public class ClassBookingTests
{
    [Fact]
    public async Task BookAsync_Should_Create_Booking()
    {
        var context = TestDbContextFactory.Create();

        var member = Member.Create("user-1");
        var fitnessClass = FitnessClass.Create(
            "Test Class", null, null, null,
            DateTime.UtcNow.AddHours(1),
            DateTime.UtcNow.AddHours(2)
        );

        context.Members.Add(member);
        context.FitnessClasses.Add(fitnessClass);
        await context.SaveChangesAsync();

        var repo = new ClassBookingRepository(context);
        var memberRepo = new MemberRepository(context);
        var classRepo = new FitnessClassRepository(context);

        var service = new ClassBookingService(repo, memberRepo, classRepo);

        var result = await service.BookAsync("user-1", fitnessClass.Id);

        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task BookAsync_Should_Fail_If_Already_Booked()
    {
        var context = TestDbContextFactory.Create();

        var member = Member.Create("user-1");
        var fitnessClass = FitnessClass.Create(
            "Test Class", null, null, null,
            DateTime.UtcNow.AddHours(1),
            DateTime.UtcNow.AddHours(2)
        );

        context.Members.Add(member);
        context.FitnessClasses.Add(fitnessClass);
        await context.SaveChangesAsync();

        var repo = new ClassBookingRepository(context);
        var memberRepo = new MemberRepository(context);
        var classRepo = new FitnessClassRepository(context);

        var service = new ClassBookingService(repo, memberRepo, classRepo);

        await service.BookAsync("user-1", fitnessClass.Id);
        var result = await service.BookAsync("user-1", fitnessClass.Id);

        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task CancelAsync_Should_Remove_Booking()
    {
        var context = TestDbContextFactory.Create();

        var member = Member.Create("user-1");
        var fitnessClass = FitnessClass.Create(
            "Test Class", null, null, null,
            DateTime.UtcNow.AddHours(1),
            DateTime.UtcNow.AddHours(2)
        );

        context.Members.Add(member);
        context.FitnessClasses.Add(fitnessClass);
        await context.SaveChangesAsync();

        var repo = new ClassBookingRepository(context);
        var memberRepo = new MemberRepository(context);
        var classRepo = new FitnessClassRepository(context);

        var service = new ClassBookingService(repo, memberRepo, classRepo);

        await service.BookAsync("user-1", fitnessClass.Id);

        var result = await service.CancelAsync("user-1", fitnessClass.Id);

        Assert.True(result.Succeeded);
    }
}