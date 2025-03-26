using Duende.IdentityServer.Validation;
using IdentityModel;
using Microservice.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace Microservice.IdentityServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var existingUser = await _userManager.FindByEmailAsync(context.UserName);

            if (existingUser == null)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Email ve ya şifreniz yanlış" });
                context.Result.CustomResponse = errors;
                return;
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, context.Password);

            if (isPasswordValid == false)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Email ve ya şifreniz yanlış" });
                context.Result.CustomResponse = errors;

                return;
            }

            context.Result = new GrantValidationResult(existingUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }
    }
}
