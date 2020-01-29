using System;
using ABitNowMobileApp.Interfaces;

namespace ABitNowMobileApp.ViewModels
{
    public class RestaurantDetailViewModel
    {
        private readonly INavigationService _navigationService;

        public RestaurantDetailViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
