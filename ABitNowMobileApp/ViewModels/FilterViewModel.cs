using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using ABitNowMobileApp.Interfaces;
using ABitNowMobileApp.UiItemModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ABitNowMobileApp.ViewModels
{
    public class FilterViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private const double MinimumDistance = 1;
        private const double MaximumDistance = 350;

        private double _startDistance = MinimumDistance;
        private double _endDistance = MaximumDistance;
        private string _currentLocation;
        private IList<FilterTag> _filterTags;

        public double StartDistance
        {
            get { return _startDistance; }
            set { SetProperty(ref _startDistance, value); }
        }

        public double EndDistance
        {
            get { return _endDistance; }
            set { SetProperty(ref _endDistance, value); }
        }

        public string CurrentLocation
        {
            get { return _currentLocation; }
            set { SetProperty(ref _currentLocation, value); }
        }

        public IList<FilterTag> FilterTags
        {
            get { return _filterTags; }
            set { SetProperty(ref _filterTags, value); }
        }

        public ICommand BackCommand { get; set; }
        public ICommand RemoveAllCommand { get; set; }

        public FilterViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            BackCommand = new Command(NavigateOnBackPageAsync);
            RemoveAllCommand = new Command(RemoveAllFilters);
        }

        public async Task OnViewAppearAsync()
        {
            // Api Stuff goes here.
            FilterTags = new List<FilterTag>()
            {
                new FilterTag(){ Id = "1", Tag = "Eat in", IsJit = true, IsSelected = false, InfoContent = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et" },
                new FilterTag(){ Id = "2", Tag = "Eat in", IsSelected = false,  },
                new FilterTag(){ Id = "3", Tag = "Delivery", IsSelected = false, },
                new FilterTag(){ Id = "4", Tag = "Inside", IsSelected = false, },
                new FilterTag(){ Id = "5", Tag = "Outside", IsSelected = false, },
                new FilterTag(){ Id = "6", Tag = "Bar", IsSelected = false, },
                new FilterTag(){ Id = "7", Tag = "Other Filter", IsSelected = false, },
                new FilterTag(){ Id = "8", Tag = "+ More", IsSelected = false, },
                new FilterTag(){ Id = "9", Tag = "Cafe", IsSelected = false, },
                new FilterTag(){ Id = "10", Tag = "Bakeries", IsSelected = false, },
                new FilterTag(){ Id = "11", Tag = "Take out", IsSelected = false, },
                new FilterTag(){ Id = "12", Tag = "Groups", IsSelected = false, },
                new FilterTag(){ Id = "13", Tag = "Pub", IsSelected = false, },
                new FilterTag(){ Id = "14", Tag = "Another filter", IsSelected = false, },
                new FilterTag(){ Id = "15", Tag = "4 Stars", IsSelected = false, },
            };

            Location location = await Xamarin.Essentials.Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best));
            if (location == null)
                return;

            var currentLocation = await Xamarin.Essentials.Geocoding.GetPlacemarksAsync(location);

            var locations = currentLocation.GetEnumerator();
            if (locations != null && locations.MoveNext())
            {
                Placemark placemark = locations.Current;
                CurrentLocation = $"{placemark.PostalCode} {placemark.Locality}, {placemark.AdminArea}";
                System.Diagnostics.Debug.WriteLine($"Current Location : {CurrentLocation}");
            }
        }

        private async void NavigateOnBackPageAsync()
        {
            await _navigationService.GoBackAsync();
        }

        private void RemoveAllFilters()
        {
            System.Diagnostics.Debug.WriteLine($"Start : {StartDistance}, End: {EndDistance}");
            StartDistance = MinimumDistance;
            EndDistance = MaximumDistance;
        }
    }
}
