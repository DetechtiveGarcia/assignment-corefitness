using Application.Abstractions.Identity;
using Application.Abstractions.Persistence;
using Application.Dtos.Members.Requests;
using Application.Services.Members;
using Domain.Entities;
using Moq;
using Application.Dtos.Identity.Results;
using Application.Dtos.Members.Results;


namespace Tests.Application.Services.Members;

public class MemberServiceTests
{
    private readonly Mock<IIdentityService> _identityServiceMock = new();
    private readonly Mock<IMemberRepository> _memberRepositoryMock = new();

    private MemberService CreateSut()
        => new(_identityServiceMock.Object, _memberRepositoryMock.Object);

    [Fact]
    public async Task CreateMemberAsync_ShouldReturnError_WhenEmailAlreadyExists()
    {
        // Arrange
        var sut = CreateSut();

        var request = new RegisterMemberRequest(
            "test@example.com",
            "Password123!",
            null,
            null,
            null,
            null
        );

        _identityServiceMock
            .Setup(x => x.FindExistingAsync(request.Email))
            .ReturnsAsync(true);

        // Act
        var result = await sut.CreateMemberAsync(request);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains("An account with the same email already exists.", result.Errors);

        _identityServiceMock.Verify(x => x.FindExistingAsync(request.Email), Times.Once);
        _identityServiceMock.Verify(
            x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
        _memberRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Member>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateMemberAsync_ShouldCreateMember_WhenRequestIsValid()
    {
        // Arrange
        var sut = CreateSut();

        var request = new RegisterMemberRequest(
            "test@example.com",
            "Password123!",
            null,
            null,
            null,
            null
        );

        _identityServiceMock
            .Setup(x => x.FindExistingAsync(request.Email))
            .ReturnsAsync(false);

        _identityServiceMock
            .Setup(x => x.RegisterAsync(request.Email, request.Password, "Member"))
            .ReturnsAsync(new RegisterResult(true, [], "user-123"));

        _memberRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Member>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await sut.CreateMemberAsync(request);

        // Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Id);
        Assert.Equal("user-123", result.UserId);

        _identityServiceMock.Verify(x => x.FindExistingAsync(request.Email), Times.Once);
        _identityServiceMock.Verify(x => x.RegisterAsync(request.Email, request.Password, "Member"), Times.Once);
        _memberRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Member>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateMemberAsync_ShouldReturnError_WhenMemberDoesNotExist()
    {
        // Arrange
        var sut = CreateSut();

        var request = new UpdateMemberRequest(
            "member-123",
            "Miguel",
            "Vera",
            "0701234567",
            null
        );

        _memberRepositoryMock
            .Setup(x => x.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Member, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Member?)null);

        // Act
        var result = await sut.UpdateMemberAsync(request);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains("Id is missing", result.Errors);

        _memberRepositoryMock.Verify(
            x => x.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Member, bool>>>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _memberRepositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<Member>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetMemberDetailAsync_ShouldReturnError_WhenUserIdIsMissing()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = await sut.GetMemberDetailAsync("");

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains("Id is missing", result.Errors);

        _memberRepositoryMock.Verify(
            x => x.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Member, bool>>>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task DeleteMemberAsync_ShouldReturnError_WhenMemberDoesNotExist()
    {
        // Arrange
        var sut = CreateSut();

        var userId = "user-123";

        _memberRepositoryMock
            .Setup(x => x.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Member, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Member?)null);

        // Act
        var result = await sut.DeleteMemberAsync(userId);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains("Member not found", result.Errors);

        _memberRepositoryMock.Verify(
            x => x.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Member, bool>>>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _memberRepositoryMock.Verify(
            x => x.RemoveAsync(It.IsAny<Member>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _identityServiceMock.Verify(
            x => x.DeleteAsync(It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task GetMemberDetailAsync_ShouldReturnMemberDetails_WhenMemberExists()
    {
        // Arrange
        var sut = CreateSut();

        var member = Member.Create("member-123", "user-123", "Miguel", "Vera", "/images/profiles/test.png");

        _memberRepositoryMock
            .Setup(x => x.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Member, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(member);

        _identityServiceMock
            .Setup(x => x.GetEmailAsync("user-123"))
            .ReturnsAsync("miguel@example.com");

        _identityServiceMock
            .Setup(x => x.GetPhoneNumberAsync("user-123"))
            .ReturnsAsync("0701234567");

        // Act
        var result = await sut.GetMemberDetailAsync("user-123");

        // Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.MemberDetails);
        Assert.Equal("Miguel", result.MemberDetails!.FirstName);
        Assert.Equal("Vera", result.MemberDetails.LastName);
        Assert.Equal("miguel@example.com", result.MemberDetails.Email);
        Assert.Equal("0701234567", result.MemberDetails.PhoneNumber);
    }
}