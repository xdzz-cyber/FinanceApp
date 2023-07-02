using Application.Common.Dtos;
using Application.Common.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.ApplicationUser.Queries.GetUser;

public class GetUserHandler : IRequestHandler<GetUser, ApplicationUserDto>
{
    private readonly UserManager<Domain.ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public GetUserHandler(UserManager<Domain.ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    
    public async Task<ApplicationUserDto> Handle(GetUser request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user is null)
        {
            throw new NotFoundException(nameof(Domain.ApplicationUser), request.Email);
        }
        
        return _mapper.Map<ApplicationUserDto>(user);
    }
}