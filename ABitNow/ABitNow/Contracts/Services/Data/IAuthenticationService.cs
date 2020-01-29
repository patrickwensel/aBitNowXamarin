using ABitNow.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABitNow.Contracts.Services.Data
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> Authenticate(LoginRequest loginRequest);

        Task<IList<ExternalLoginProvider>> GetAllExternalLoginProviders();

        Task<LoginResponse> ExternalLoginConfirmation(ExternalRegistrationConfirmation externalRegistrationConfirmation);

        Task<User> GetCurrentUser();

        bool IsUserAuthenticated();

        Task<RegisterResponse> RegisterUser(RegisterRequest registerRequest);
    }
}
