using Application.Mocks.Commands.CreateMock;
using Microsoft.EntityFrameworkCore;
using TestsProject.Common;
using Xunit;

namespace TestsProject.Mocks.Commands;

[Collection("CommandCollection")]
public class CreateMockHandlerTests : TestCommandBase
{
    [Fact]
    public async Task CreateMockCommandHandler_Success()
    {
        // Arrange
        var handler = new CreateMockHandler(Context);
        // Act
        var result = await handler.Handle(new CreateMock(), CancellationToken.None);
        // Assert
        Assert.NotNull(await Context.Mocks.SingleOrDefaultAsync(m => m.Id == result));
    }
}