using ABitNow.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ABitNow.Contracts.Services.Data
{
    public interface IRestaurantService
    {
        Task<IList<RestaurantResponse>> GetAllFavoriteRestaurants(RestaurantRequest restaurantRequest);

        Task<IList<RestaurantResponse>> GetAllHighRatingRestaurants(RestaurantRequest restaurantRequest);
    }
}
