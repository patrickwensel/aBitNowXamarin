using ABitNow.Contracts.Services.Data;
using ABitNow.Contracts.Services.General;
using ABitNow.Exceptions;
using ABitNow.Extensions;
using ABitNow.Models;
using ABitNow.Models.UI;
using ABitNow.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ABitNow.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        public RegistrationViewModel(IConnectionService connectionService, INavigationService navigationService, IDialogService dialogService, ISettingsService settingsService, IAuthenticationService authenticationService) : base(connectionService, navigationService, dialogService, settingsService)
        {
            _authenticationService = authenticationService;
        }

        private IList<SocialButton> _externalLoginProviders;

        private bool _hasExternalLogin;

        public ICommand BackToLoginCommand => new Command(OnBackToLogin);
        public ICommand RegisterCommand => new Command(OnRegister);
        public ICommand ExternalLoginProviderTappedCommand => new Command<ExternalLoginProvider>(OnExternalLoginProviderTapped);

        public IList<SocialButton> ExternalLoginProviders
        {
            get => _externalLoginProviders;
            set
            {
                _externalLoginProviders = value;
                OnPropertyChanged();
            }
        }

        public bool HasExternalLogin
        {
            get => _hasExternalLogin;
            set
            {
                _hasExternalLogin = value;
                OnPropertyChanged();
            }
        }

        private async void OnBackToLogin()
        {
            await _navigationService.NavigateToAsync<LoginViewModel>();
        }

        private async void OnExternalLoginProviderTapped(ExternalLoginProvider selectedExternalLoginProvider)
        {
            await _navigationService.NavigateToAsync<ExternalLoginViewModel>(selectedExternalLoginProvider);
        }

        private async void OnRegister()
        {
            await _dialogService.ShowDialog("Under Development.", "Error", "OK");
        }

        public override async Task InitializeAsync(object data)
        {
            try
            {
                IsBusy = true;
                Title = "Registration";
                IList<ExternalLoginProvider> externalLoginProviders = (await _authenticationService.GetAllExternalLoginProviders());
                this.ExternalLoginProviders = externalLoginProviders.Select(x => new SocialButton(x, ExternalLoginProviderTappedCommand)).ToList();
                this.HasExternalLogin = this.ExternalLoginProviders != null && this.ExternalLoginProviders.Count > 0;
            }
            catch(Exception ex)
            {
                await _dialogService.ShowError();
                new ApplicationExceptionEx(ex);
            }
            finally
            {
                IsBusy = false;
            }
           
        }
    }
}
