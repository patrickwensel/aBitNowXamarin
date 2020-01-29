using ABitNow.Contracts.Services.Data;
using ABitNow.Contracts.Services.General;
using ABitNow.Exceptions;
using ABitNow.Models;
using ABitNow.Models.UI;
using ABitNow.Services.Data.Base;
using ABitNow.ViewModels.Base;
using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ABitNow.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;

        private string _search;
        private IList<Restaurant> _nearestFavouriteRestaurants;
        private IList<Restaurant> _nearestHighRatingRestaurants;

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                OnPropertyChanged();
            }
        }

        public IList<Restaurant> NearestFavouriteRestaurants
        {
            get => _nearestFavouriteRestaurants;
            set
            {
                _nearestFavouriteRestaurants = value;

                OnPropertyChanged();
            }
        }

        public IList<Restaurant> NearestHighRatingRestaurants
        {
            get => _nearestHighRatingRestaurants;
            set
            {
                _nearestHighRatingRestaurants = value;

                OnPropertyChanged();
            }

        }

        public ICommand SearchCommand { get; set; }

        public MainViewModel(ISettingsService settingsService, IConnectionService connectionService, INavigationService navigationService, IDialogService dialogService, IAuthenticationService authenticationService) : base(connectionService, navigationService, dialogService, settingsService)
        {
            _authenticationService = authenticationService;

            SearchCommand = new Command(SearchRestaurants);
        }

        private User _currentUser;

        public ICommand LogoutCommand => new Command(OnLogout);

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        private async void OnLogout()
        {
            _settingsService.AccessTokenSetting = null;
            ((BaseService)_authenticationService).InvalidateAllCache();
            await _navigationService.NavigateToAsync(typeof(LoginViewModel));
        }

        public override async Task InitializeAsync(object data)
        {
            try
            {
                IsBusy = true;
                Title = "Main";
                this.CurrentUser = await _authenticationService.GetCurrentUser();
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestExceptionEx && ((HttpRequestExceptionEx)ex)?.HttpCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    OnLogout();
                }
                else
                {
                    await _dialogService.ShowError();
                    new ApplicationExceptionEx(ex);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task OnViewAppearAsync()
        {
            IsBusy = true;
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

            IsBusy = false;
        }

        private void SearchRestaurants()
        {
            System.Diagnostics.Debug.WriteLine($"Search button tapped : {Search}");
        }

    }
}
