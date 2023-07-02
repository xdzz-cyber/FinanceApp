using Application.Common.Dtos;
using MediatR;

namespace Application.ApplicationUser.Queries.GetUser;

public class GetUser : IRequest<ApplicationUserDto>
{
    public string Email { get; set; } = null!;
}