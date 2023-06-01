using Application.Auth.Commands.Login.Commands;
using Application.Auth.Commands.Registration.Commands;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using Mock = Moq.Mock;

namespace TestsProject.Auth.Commands;

[Collection("CommandCollection")]
public class RegisterHandlerTests
{
    [Fact]
    public async Task RegistrationHandler_Should_Return_Success_When_Valid_Request()
    {
        // Arrange
        var registrationRequest = new Registration { UserName = "testuser", Email = "test@example.com", Password = "password" };

        // Mock the behavior of UserManager
        var userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        // Mock the behavior of IMediator
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<Login>(), default))
            .ReturnsAsync(true);

        // Create the RegistrationHandler instance
        var registrationHandler = new RegistrationHandler(userManagerMock.Object, mediatorMock.Object);

        // Act
        var result = await registrationHandler.Handle(registrationRequest, CancellationToken.None);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task RegistrationHandler_Should_Return_Error_When_Invalid_Request()
    {
        // Arrange
        var registrationRequest = new Registration { UserName = "testuser", Email = "test@example.com", Password = "password" };

        // Mock the behavior of UserManager
        var userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

        // Mock the behavior of IMediator
        var mediatorMock = new Mock<IMediator>();

        // Create the RegistrationHandler instance
        var registrationHandler = new RegistrationHandler(userManagerMock.Object, mediatorMock.Object);

        // Act
        var result = await registrationHandler.Handle(registrationRequest, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}