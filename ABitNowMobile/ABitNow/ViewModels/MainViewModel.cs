using ABitNow.Contracts.Services.Data;
using ABitNow.Contracts.Services.General;
using ABitNow.Exceptions;
using ABitNow.Models;
using ABitNow.Services.Data.Base;
using ABitNow.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ABitNow.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        public MainViewModel(ISettingsService settingsService, IConnectionService connectionService, INavigationService navigationService, IDialogService dialogService, IAuthenticationService authenticationService) : base(connectionService, navigationService, dialogService, settingsService)
        {
            _authenticationService = authenticationService;
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
            catch(Exception ex)
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
    }
}
