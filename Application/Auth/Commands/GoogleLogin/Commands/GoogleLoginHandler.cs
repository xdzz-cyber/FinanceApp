using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Commands.GoogleLogin.Commands;

public class GoogleLoginHandler : IRequestHandler<GoogleLogin, SignInResult>
{
    private readonly SignInManager<Domain.ApplicationUser> _signInManager;

    public GoogleLoginHandler(SignInManager<Domain.ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }
    
    public async Task<SignInResult> Handle(GoogleLogin request, CancellationToken cancellationToken)
    {
        ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
        
        if (info == null)
        {
            throw new ArgumentNullException(nameof(info), "External login info is null");
        }

        return await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
    }
}