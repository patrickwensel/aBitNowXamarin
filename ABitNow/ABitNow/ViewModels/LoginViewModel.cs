using ABitNow.Contracts.Services.Data;
using ABitNow.Contracts.Services.General;
using ABitNow.Exceptions;
using ABitNow.Models;
using ABitNow.Models.UI;
using ABitNow.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ABitNow.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginViewModel(IConnectionService connectionService, ISettingsService settingsService, INavigationService navigationService, IAuthenticationService authenticationService, IDialogService dialogService) : base(connectionService, navigationService, dialogService, settingsService)
        {
            _authenticationService = authenticationService;
        }

        private LoginRequest _loginRequest;

        private IList<SocialButton> _externalLoginProviders;

        private bool _hasExternalLogin;

        public ICommand LoginCommand => new Command(OnLogin);

        public ICommand RegisterCommand => new Command(OnRegister);

        public ICommand LoginLaterCommand => new Command(OnLoginLaterAsync);

        public ICommand ExternalLoginProviderTappedCommand => new Command<ExternalLoginProvider>(OnExternalLoginProviderTapped);

        public LoginRequest LoginRequest
        {
            get => _loginRequest;
            set
            {
                _loginRequest = value;
                OnPropertyChanged();
            }
        }

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

        private async void OnLogin()
        {
            try
            {
                IsBusy = true;
                if (_connectionService.IsConnected)
                {
                    if (!string.IsNullOrWhiteSpace(LoginRequest.Username) && !string.IsNullOrWhiteSpace(LoginRequest.Password))
                    {
                        var authenticationResponse = await _authenticationService.Authenticate(LoginRequest);

                        if (authenticationResponse.Success)
                        {
                            _settingsService.AccessTokenSetting = authenticationResponse.Token;
                            await _navigationService.NavigateToAsync<MainViewModel>();
                        }
                        else
                        {
                            await _dialogService.ShowDialog("Invalid username or password.", "Error", "OK");
                        }
                    }
                    else
                    {
                        _dialogService.ShowToast("Please enter username and password.");
                    }
                }
                else
                {
                    _dialogService.ShowInternetError();
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError();
                new ApplicationExceptionEx(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnExternalLoginProviderTapped(ExternalLoginProvider selectedExternalLoginProvider)
        {
            await _navigationService.NavigateToAsync<ExternalLoginViewModel>(selectedExternalLoginProvider);
        }

        private async void OnRegister()
        {
            await _navigationService.NavigateToAsync<RegistrationViewModel>();
        }

        private async void OnLoginLaterAsync()
        {
            await _navigationService.NavigateToAsync<MainViewModel>();
        }

        public override async Task InitializeAsync(object data)
        {
            try
            {
                IsBusy = true;
                Title = "Login";
                LoginRequest loginRequest = new LoginRequest();
                
                loginRequest.Username = "pwensel@hotmail.com";
                loginRequest.Password = "Lena1995";

                this.LoginRequest = loginRequest;
                IList<ExternalLoginProvider> externalLoginProviders = (await _authenticationService.GetAllExternalLoginProviders());

                this.ExternalLoginProviders = externalLoginProviders.Select(x => new SocialButton(x, ExternalLoginProviderTappedCommand)).ToList();

                this.HasExternalLogin = this.ExternalLoginProviders != null && this.ExternalLoginProviders.Count > 0;

            }
            catch (Exception ex)
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
