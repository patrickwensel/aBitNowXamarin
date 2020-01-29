using ABitNow.Constants;
using ABitNow.Contracts.Repository;
using ABitNow.Contracts.Services.Data;
using ABitNow.Contracts.Services.General;
using ABitNow.Models;
using ABitNow.Services.Data.Base;
using Akavache;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ABitNow.Services.Data
{
    public class RestaurantService : BaseService, IRestaurantService
    {
        private readonly IGenericRepository _genericRepository;
        private readonly ISettingsService _settingsService;
        public RestaurantService(IGenericRepository genericRepository, ISettingsService settingsService, IBlobCache cache = null) : base(cache)
        {
            _settingsService = settingsService;
            _genericRepository = genericRepository;
        }

        public async Task<IList<RestaurantResponse>> GetAllFavoriteRestaurants(RestaurantRequest restaurantRequest)
        {
            UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.FavoriteRestaurantsEndpoint
            };

            return await _genericRepository.PostAsync<RestaurantRequest, IList<RestaurantResponse>>(builder.ToString(), restaurantRequest);
        }

        public async Task<IList<RestaurantResponse>> GetAllHighRatingRestaurants(RestaurantRequest restaurantRequest)
        {
            UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.HighRatingRestaurantsEndpoint
            };

            return await _genericRepository.PostAsync<RestaurantRequest, IList<RestaurantResponse>>(builder.ToString(), restaurantRequest);
        }

    }
}
