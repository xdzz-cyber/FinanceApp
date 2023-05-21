using Application.Common.ViewModels;
using MediatR;

namespace Application.Mocks.Queries.GetMock;

public class GetMock : IRequest<MockVm>
{
    public Guid Id { get; set; }
}