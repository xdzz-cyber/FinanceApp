using System.Security.Claims;
using Application.Auth.Commands.Login.Commands;
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
public class LoginHandlerTests
{
    [Fact]
    public async Task LoginHandler_Should_Return_Success_When_Valid_Credentials()
    {
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
        var handler = new LoginHandler(userManager.Object, signInManager.Object);

        var user = new ApplicationUser()
        {
            UserName = "testuser",
            Email = "testuser@example.com",
            PasswordHash = "password"
        };
        userManager.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        signInManager.Setup(s => s.PasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(SignInResult.Success));

        // Act
        var login = new Login
        {
            Email = "testuser@example.com",
            Password = "password"
        };
        var result = await handler.Handle(login, CancellationToken.None);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task LoginHandler_Should_Return_Error_When_Invalid_Credentials()
    {
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
        var handler = new LoginHandler(userManager.Object, signInManager.Object);

        var user = new ApplicationUser()
        {
            UserName = "testuser",
            Email = "testuser@example.com",
            PasswordHash = "password"
        };
        userManager.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        signInManager.Setup(s => s.PasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .Returns(Task.FromResult(SignInResult.Failed));

        // Act
        var login = new Login
        {
            Email = "testuser@example.com",
            Password = "password"
        };
        var result = await handler.Handle(login, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}