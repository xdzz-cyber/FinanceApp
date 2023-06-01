using Application.Auth.Commands.Logout.Commands;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Mock = Moq.Mock;

namespace TestsProject.Auth.Commands;

[Collection("CommandCollection")]
public class LogoutHandlerTests
{
    [Fact]
    public async Task LogoutHandler_Should_Logout_User()
    {
        // Arrange
        var logoutRequest = new Logout();

        // Mock the behavior of SignInManager
        // Arrange
        var userManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
        var loggerMock = new Mock<ILogger<SignInManager<ApplicationUser>>>();
        var optionsMock = new Mock<IOptions<IdentityOptions>>();
        var authenticationSchemeProviderMock = new Mock<IAuthenticationSchemeProvider>();

        var signInManager = new Mock<SignInManager<ApplicationUser>>(
            userManager.Object,
            contextAccessorMock.Object,
            claimsFactoryMock.Object,
            optionsMock.Object,
            loggerMock.Object,
            authenticationSchemeProviderMock.Object
        );
        signInManager.Setup(s => s.SignOutAsync())
            .Returns(Task.CompletedTask);

        // Create the LogoutHandler instance
        var logoutHandler = new LogoutHandler(signInManager.Object);

        // Act
        var result = await logoutHandler.Handle(logoutRequest, CancellationToken.None);

        // Assert
        Assert.True(result);
    }
}