using Application.Common.ViewModels;
using Application.Mocks.Queries.GetMocks;
using Shouldly;
using TestsProject.Common;
using Xunit;

namespace TestsProject.Mocks.Queries;

[Collection("QueryCollection")]
public class GetMocksHandlerTests : TestCommandBase
{
    [Fact]
    public async Task GetMocksQueryHandler_Success()
    {
        // Arrange
        var handler = new GetMocksHandler(Context, Mapper);
        // Act
        var result = await handler.Handle(new GetMocks(), CancellationToken.None);
        // Assert
        result.ShouldBeOfType<List<MockVm>>();
        result.Count.ShouldBe(2);
        //Assert.Equal(2, result.Count);
    }
}