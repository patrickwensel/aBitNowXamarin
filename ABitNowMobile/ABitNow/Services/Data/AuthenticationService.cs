using ABitNow.Constants;
using ABitNow.Contracts.Repository;
using ABitNow.Contracts.Services.Data;
using ABitNow.Contracts.Services.General;
using ABitNow.Models;
using ABitNow.Services.Data.Base;
using Akavache;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ABitNow.Services.Data
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly IGenericRepository _genericRepository;
        private readonly ISettingsService _settingsService;
        public AuthenticationService(IGenericRepository genericRepository, ISettingsService settingsService, IBlobCache cache = null) : base(cache)
        {
            _settingsService = settingsService;
            _genericRepository = genericRepository;
        }

        public async Task<LoginResponse> Authenticate(LoginRequest loginRequest)
        {
            UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.AuthenticateEndpoint
            };

            return await _genericRepository.PostAsync<LoginRequest, LoginResponse>(builder.ToString(), loginRequest);
        }

        public async Task<LoginResponse> ExternalLoginConfirmation(ExternalRegistrationConfirmation externalRegistrationConfirmation)
        {
            UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.ExternalLoginConfirmation
            };

            return await _genericRepository.PostAsync<ExternalRegistrationConfirmation, LoginResponse>(builder.ToString(), externalRegistrationConfirmation);
        }

        public async Task<IList<ExternalLoginProvider>> GetAllExternalLoginProviders()
        {
            UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.ExternalLoginProviderEndpoint,
                Query = ApiConstants.ExternalLoginProviderQuery,
            };
            return await _genericRepository.GetAsync<IList<ExternalLoginProvider>>(builder.ToString());
        }

        public async Task<User> GetCurrentUser()
        {
            User user = await GetFromCache<User>(CacheNameConstants.CurrentUser);

            if (user != null)//loaded from cache
            {
                return user;
            }
            else
            {
                UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
                {
                    Path = ApiConstants.CurrentUserEndpoint
                };

                user = await _genericRepository.GetAsync<User>(builder.ToString(), _settingsService.AccessTokenSetting);
                await Cache.InsertObject(CacheNameConstants.CurrentUser, user, DateTimeOffset.Now.AddMinutes(30));
                return user;
            }
        }

        public bool IsUserAuthenticated()
        {
            return !string.IsNullOrEmpty(_settingsService.AccessTokenSetting);
        }

    }
}
