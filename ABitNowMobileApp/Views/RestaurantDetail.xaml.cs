using System;
using System.Collections.Generic;
using ABitNowMobileApp.ViewModels;
using Xamarin.Forms;

namespace ABitNowMobileApp.Views
{
    public partial class RestaurantDetail : ContentPage
    {
        private readonly RestaurantDetailViewModel _restaurantDetailViewModel;

        public RestaurantDetail(RestaurantDetailViewModel restaurantDetailViewModel)
        {
            InitializeComponent();
            _restaurantDetailViewModel = restaurantDetailViewModel;
        }
    }
}
