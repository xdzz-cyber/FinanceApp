using Application.Common.ViewModels;
using Application.Mocks.Queries.GetMock;
using MediatR;

namespace Application.Mocks.Queries.GetMocks;

public class GetMocks : IRequest<List<MockVm>>
{
    // public Guid Id { get; set; }
    // public Guid MockId { get; set; }
}