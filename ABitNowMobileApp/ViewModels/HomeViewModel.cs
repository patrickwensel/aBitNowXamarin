using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ABitNowMobileApp.Interfaces;
using ABitNowMobileApp.Messages;
using ABitNowMobileApp.UiItemModels;
using Acr.UserDialogs;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ABitNowMobileApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IPersistanceService _persistanceService;
        private readonly IMessagingService _messagingService;

        private string _search;
        private string _searchLocation;
        private string _currentLocation;
        private string _currentLocationWithZipCode;
        private bool _isSearchListVisible;
        private bool _isUserLoggedIn;
        private IList<Restaurant> _nearestFavouriteRestaurants;
        private IList<Restaurant> _nearestHighRatingRestaurants;
        private IList<SearchedRestaurant> _searchedRestaurants;
        private IList<FilterTag> _filterTags;
        private IList<RecentLocation> _recentLocations;
        private IList<string> _recentSearches;
        private IList<Suggestion> _suggestions;
        private IList<LocationSuggestion> _locationSuggestions;
        private UserInfo _loggedInUserInfo;
        private MenuOption _selectedMenuOption;
        private IList<MenuOption> _menuOptions;

        public string Search
        {
            get { return _search; }
            set { SetProperty(ref _search, value); }
        }

        public string SearchLocation
        {
            get { return _searchLocation; }
            set { SetProperty(ref _searchLocation, value); }
        }

        public string CurrentLocation
        {
            get { return _currentLocation; }
            set { SetProperty(ref _currentLocation, value); }
        }

        public string CurrentLocationWithZipCode
        {
            get { return _currentLocationWithZipCode; }
            set { SetProperty(ref _currentLocationWithZipCode, value); }
        }

        public bool IsSearchListVisible
        {
            get { return _isSearchListVisible; }
            set { SetProperty(ref _isSearchListVisible, value); }
        }

        public bool IsUserLoggedIn
        {
            get { return _isUserLoggedIn; }
            set { SetProperty(ref _isUserLoggedIn, value); }
        }

        public UserInfo LoggedInUserInfo
        {
            get { return _loggedInUserInfo; }
            set { SetProperty(ref _loggedInUserInfo, value); }
        }

        public MenuOption SelectedMenuOption
        {
            get { return _selectedMenuOption; }
            set { SetProperty(ref _selectedMenuOption, value); }
        }

        public IList<Restaurant> NearestFavouriteRestaurants
        {
            get { return _nearestFavouriteRestaurants; }
            set { SetProperty(ref _nearestFavouriteRestaurants, value); }
        }

        public IList<Restaurant> NearestHighRatingRestaurants
        {
            get { return _nearestHighRatingRestaurants; }
            set { SetProperty(ref _nearestHighRatingRestaurants, value); }
        }

        public IList<SearchedRestaurant> SearchedRestaurants
        {
            get { return _searchedRestaurants; }
            set { SetProperty(ref _searchedRestaurants, value); }
        }

        public IList<FilterTag> FilterTags
        {
            get { return _filterTags; }
            set { SetProperty(ref _filterTags, value); }
        }

        public IList<RecentLocation> RecentLocations
        {
            get { return _recentLocations; }
            set { SetProperty(ref _recentLocations, value); }
        }

        public IList<string> RecentSearches
        {
            get { return _recentSearches; }
            set { SetProperty(ref _recentSearches, value); }
        }

        public IList<Suggestion> Suggestions
        {
            get { return _suggestions; }
            set { SetProperty(ref _suggestions, value); }
        }

        public IList<LocationSuggestion> LocationSuggestions
        {
            get { return _locationSuggestions; }
            set { SetProperty(ref _locationSuggestions, value); }
        }

        public IList<MenuOption> MenuOptions
        {
            get { return _menuOptions; }
            set { SetProperty(ref _menuOptions, value); }
        }

        public ICommand LogoutCommand { get; set; }

        public ICommand LoginCommand { get; set; }

        public ICommand SelectedMenuOptionCommand { get; set; }

        public ICommand FilterTagSelectedCommand { get; set; }

        public ICommand SuggestionTappedCommand { get; set; }

        public ICommand SuggestionRemoveCommand { get; set; }

        public ICommand SearchRestaurantCommand { get; set; }

        public ICommand SearchLocationCommand { get; set; }

        public ICommand SearchLocationsTextChangedCommand { get; set; }

        public ICommand CloseLocationSuggestionsCommand { get; set; }

        public HomeViewModel(INavigationService navigationService, IPersistanceService persistanceService, IMessagingService messagingService)
        {
            _navigationService = navigationService;
            _persistanceService = persistanceService;
            _messagingService = messagingService;

            LoggedInUserInfo = new UserInfo()
            {
                Name = "Michael Robinson",
                Email = "michi@me.com"
            };

            LogoutCommand = new Command(LogoutAsync);
            LoginCommand = new Command(LoginAsync);
            SelectedMenuOptionCommand = new Command(OnMenuOptionSelected);
            FilterTagSelectedCommand = new Command<FilterTag>(OnFilterTagSelected);
            SuggestionTappedCommand = new Command<Suggestion>(OnSuggestionTapped);
            SuggestionRemoveCommand = new Command<Suggestion>(OnSuggestionRemoved);
            SearchRestaurantCommand = new Command(SearchRestaurants);
            SearchLocationCommand = new Command(SearchLocations);
            SearchLocationsTextChangedCommand = new Command(OnSearchLocationsTextChanged);
            CloseLocationSuggestionsCommand = new Command(CloseLocationSuggestions);
        }

        public async Task OnViewAppearAsync()
        {
            UserDialogs.Instance.ShowLoading();
            IsUserLoggedIn = _persistanceService.IsLoggedIn;
            // Add API call here.

            NearestFavouriteRestaurants = new List<Restaurant>()
            {
                new Restaurant() { Image = "Restaurant1.png", Name = "Porto's Bakery and cafee", Type = "Bakery", Address = "7640 Beach Blvd", Rate = 4.2 },
                new Restaurant() { Image = "Restaurant2.png", Name = "Perch", Type = "French", Address = "448 S Hill ST", Rate = 4.2 },
                new Restaurant() { Image = "Restaurant3.png", Name = "Rosco", Type = "Cafe", Address = "448 S Hill ST", Rate = 4.2 },
            };

            NearestHighRatingRestaurants = new List<Restaurant>()
            {
                new Restaurant() { Image = "Food1.png", Name="Salsa & Beer", Rate = 5.0, Type = "Bakery", Address="11669 Sherman way", FriendlyTime="Less than a minute ago" },
                new Restaurant() { Image = "Food2.png", Name="Salsa & Beer", Rate = 4.2, Type = "Bakery", Address="11669 Sherman way", FriendlyTime="Less than a minute ago" },
                new Restaurant() { Image = "Food3.png", Name="Salsa & Beer", Rate = 3.8, Type = "Bakery", Address="11669 Sherman way", FriendlyTime="Less than a minute ago" },
            };

            FilterTags = new List<FilterTag>()
            {
                new FilterTag(){ Id = "1", Tag = "Eat in", IsJit = true, IsSelected = false, TapCommand = FilterTagSelectedCommand },
                new FilterTag(){ Id = "2", Tag = "Eat in", IsSelected = false, TapCommand = FilterTagSelectedCommand },
                new FilterTag(){ Id = "3", Tag = "Delivery", IsSelected = false, TapCommand = FilterTagSelectedCommand },
                new FilterTag(){ Id = "4", Tag = "Inside", IsSelected = false, TapCommand = FilterTagSelectedCommand },
                new FilterTag(){ Id = "5", Tag = "Outside", IsSelected = false, TapCommand = FilterTagSelectedCommand },
                new FilterTag(){ Id = "6", Tag = "Bar", IsSelected = false, TapCommand = FilterTagSelectedCommand },
                new FilterTag(){ Id = "7", Tag = "Other Filter", IsSelected = false, TapCommand = FilterTagSelectedCommand },
                new FilterTag(){ Id = "8", Tag = "+ More", IsSelected = false, TapCommand = FilterTagSelectedCommand },
                new FilterTag(){ Id = "9", Tag = "Cafe", IsSelected = false, TapCommand = FilterTagSelectedCommand },
                new FilterTag(){ Id = "10", Tag = "Bakeries", IsSelected = false, TapCommand = FilterTagSelectedCommand },
                new FilterTag(){ Id = "11", Tag = "Take out", IsSelected = false, TapCommand = FilterTagSelectedCommand },
                new FilterTag(){ Id = "12", Tag = "Groups", IsSelected = false, TapCommand = FilterTagSelectedCommand },
                new FilterTag(){ Id = "13", Tag = "Pub", IsSelected = false, TapCommand = FilterTagSelectedCommand },
                new FilterTag(){ Id = "14", Tag = "Another filter", IsSelected = false, TapCommand = FilterTagSelectedCommand },
                new FilterTag(){ Id = "15", Tag = "4 Stars", IsSelected = false, TapCommand = FilterTagSelectedCommand },
            };

            RecentSearches = new List<string>()
            {
                "French Cuisine",
                "French Cuisine 1",
                "French Cuisine 2"
            };

            Suggestions = new ObservableCollection<Suggestion>()
            {
               new Suggestion() { Content = "French Cuisine Restaurants", TapCommand = SuggestionTappedCommand, RemoveCommand = SuggestionRemoveCommand },
            };

            RecentLocations = new List<RecentLocation>()
            {
                new RecentLocation() { Location = "Santa Barba, CA", Distance = 95 },
                new RecentLocation() { Location = "Palo Alto, CA", Distance = 357 },
                new RecentLocation() { Location = "San Francisco, CA", Distance = 381 },
            };

            if (IsUserLoggedIn)
            {
                // If user logged in then following menu options will be visible
                MenuOptions = new List<MenuOption>()
                {
                    new MenuOption(){ Id = 1, Name = "Preferences" },
                    new MenuOption(){ Id = 2, Name = "Menu" },
                    new MenuOption(){ Id = 3, Name = "Status" },
                    new MenuOption(){ Id = 4, Name = "Orders" },
                    new MenuOption(){ Id = 5, Name = "Details" },
                    new MenuOption(){ Id = 6, Name = "Table" },
                    new MenuOption(){ Id = 7, Name = "Ratings" },
                    new MenuOption(){ Id = 8, Name = "About" },
                    new MenuOption(){ Id = 9, Name = "Contact" },
                };
            }
            else
            {
                // If user does not logged in then following menu options will be visible
                MenuOptions = new List<MenuOption>()
                {
                    new MenuOption(){ Id = 1, Name = "Menu" },
                    new MenuOption(){ Id = 2, Name = "Ratings" },
                    new MenuOption(){ Id = 3, Name = "About" },
                    new MenuOption(){ Id = 4, Name = "Contact" },
                };
            }

            UserDialogs.Instance.HideLoading();

            Location location = await Xamarin.Essentials.Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best));
            if (location == null)
                return;

            var currentLocation = await Xamarin.Essentials.Geocoding.GetPlacemarksAsync(location);

            var locations = currentLocation.GetEnumerator();
            if (locations != null && locations.MoveNext())
            {
                Placemark placemark = locations.Current;
                CurrentLocation = $"{placemark.Locality}, {placemark.AdminArea}";
                CurrentLocationWithZipCode = $"{placemark.PostalCode} {CurrentLocation}";
                System.Diagnostics.Debug.WriteLine($"Current Location : {CurrentLocation}, {CurrentLocationWithZipCode}");
            }
        }

        public void SearchRestaurantsAsync()
        {
            System.Diagnostics.Debug.WriteLine($"Search button tapped : {Search}");

            // Add API stuff here.
            var suggestions = new ObservableCollection<Suggestion>();
            for (int i = 0; i < Search.Length; i++)
            {
                suggestions.Add(new Suggestion() { Content = $"French Cuisine Restaurants {i + 1}", TapCommand = SuggestionTappedCommand, RemoveCommand = SuggestionRemoveCommand });
            }

            Suggestions = suggestions;
        }

        public void SearchLocationsAsync()
        {
            System.Diagnostics.Debug.WriteLine($"Searching for locations : {SearchLocation}");

            if (string.IsNullOrEmpty(SearchLocation) || SearchLocation.Length < 3)
                return;

            // Add API Stuff here.
            LocationSuggestions = new ObservableCollection<LocationSuggestion>()
            {
                new LocationSuggestion() { LocationName = "San Diego", Address = "California", DriveTime = new System.TimeSpan(1, 42, 0), Distance = 120 },
                new LocationSuggestion() { LocationName = "San Diego International Airport", Address = "3225 N Harbor Dr, San Diego, CA 92101", DriveTime = new System.TimeSpan(1, 42, 0), Distance = 150 },
                new LocationSuggestion() { LocationName = "San Diego Convention Center", Address = "111 W Harbor Dr, San Diego, CA 92101", DriveTime = new System.TimeSpan(2, 5, 0), Distance = 155 }
            };
        }

        private void OnSearchLocationsTextChanged()
        {
            SearchLocationsAsync();
        }

        private void CloseLocationSuggestions()
        {
            if (LocationSuggestions.Count > 0)
                LocationSuggestions.Clear();
        }

        private void SearchLocations()
        {
            System.Diagnostics.Debug.WriteLine("Search Location command....");
        }

        private void SearchRestaurants()
        {
            System.Diagnostics.Debug.WriteLine($"Search command.... {Search}");
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("suggestion", Search);
            _navigationService.NavigateToAsync(Enums.PageKey.Search, parameters);
        }

        private void OnSuggestionTapped(Suggestion suggestion)
        {
            System.Diagnostics.Debug.WriteLine($"Suggestion Tapped : {suggestion}");
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("suggestion", suggestion.Content);
            _navigationService.NavigateToAsync(Enums.PageKey.Search, parameters);
        }

        private void OnSuggestionRemoved(Suggestion suggestion)
        {
            System.Diagnostics.Debug.WriteLine($"Suggestion Removed : {suggestion}");
            if (Suggestions.Contains(suggestion))
                Suggestions.Remove(suggestion);

            _messagingService.Send<SuggestionRemovedMessage>(new SuggestionRemovedMessage(suggestion));
        }

        private async void LogoutAsync()
        {
            System.Diagnostics.Debug.WriteLine("Logout Tapped");
        }

        private async void LoginAsync()
        {
            await _navigationService.NavigateToAsync(Enums.PageKey.Login);
        }

        private void OnMenuOptionSelected()
        {
            if (SelectedMenuOption == null)
                return;

            System.Diagnostics.Debug.WriteLine($"Selected Menu Option : {SelectedMenuOption.Name}");

            //SelectedMenuOption = null;
        }

        private void OnFilterTagSelected(FilterTag filterTag)
        {
            filterTag.IsSelected = !filterTag.IsSelected;
            System.Diagnostics.Debug.WriteLine($"Filtet tag selected : {filterTag.IsSelected}");
        }
    }
}
