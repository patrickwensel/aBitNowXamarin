using ABitNow.Contracts.Services.Data;
using ABitNow.Contracts.Services.General;
using ABitNow.Exceptions;
using ABitNow.Models;
using ABitNow.Models.UI;
using ABitNow.Utility;
using ABitNow.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private RegisterRequest _registerRequest;

        private IList<SocialButton> _externalLoginProviders;

        private bool _hasExternalLogin;

        public ICommand BackToLoginCommand => new Command(OnBackToLogin);
        public ICommand RegisterCommand => new Command(OnRegister);

        public ICommand RegisterLaterCommand => new Command(OnRegisterLaterAsync);

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

        public RegisterRequest RegisterRequest
        {
            get => _registerRequest;
            set
            {
                _registerRequest = value;
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

        private async void OnRegisterLaterAsync()
        {
            await _navigationService.NavigateToAsync<MainViewModel>();
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
            try
            {
                IsBusy = true;
                if (_connectionService.IsConnected)
                {
                    if (!string.IsNullOrWhiteSpace(RegisterRequest.FirstName) && !string.IsNullOrWhiteSpace(RegisterRequest.LastName) && !string.IsNullOrWhiteSpace(RegisterRequest.Mobile) && !string.IsNullOrWhiteSpace(RegisterRequest.Email) && !string.IsNullOrWhiteSpace(RegisterRequest.Password))
                    {
                        if (Validation.IsValidMobile(RegisterRequest.Mobile))
                        {
                            if (Validation.IsValidEmailAddress(RegisterRequest.Email))
                            {
                                if (RegisterRequest.Password == RegisterRequest.ConfirmPassword)
                                {
                                    var registerResponse = await _authenticationService.RegisterUser(RegisterRequest);

                                    if (registerResponse.Success)
                                    {
                                        _settingsService.AccessTokenSetting = registerResponse.Token;
                                        await _navigationService.NavigateToAsync<MainViewModel>();
                                    }
                                    else
                                    {
                                        await _dialogService.ShowDialog(registerResponse.Message, "Error", "OK");
                                    }
                                }
                                else
                                {
                                    await _dialogService.ShowDialog("Password and confirm password do not match.", "Error", "OK");
                                }
                            }
                            else
                            {
                                await _dialogService.ShowDialog("Please enter valid email address.", "Error", "OK");
                            }
                        }
                        else
                        {
                            await _dialogService.ShowDialog("Please enter valid phone number.", "Error", "OK");
                        }
                    }
                    else
                    {
                        await _dialogService.ShowDialog("Invalid register data.", "Error", "OK");
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

        public override async Task InitializeAsync(object data)
        {
            try
            {
                IsBusy = true;
                Title = "Registration";
                if (_connectionService.IsConnected)
                {
                    RegisterRequest registerRequest = new RegisterRequest();
                    this.RegisterRequest = registerRequest;
                    IList<ExternalLoginProvider> externalLoginProviders = (await _authenticationService.GetAllExternalLoginProviders());
                    this.ExternalLoginProviders = externalLoginProviders.Select(x => new SocialButton(x, ExternalLoginProviderTappedCommand)).ToList();
                    this.HasExternalLogin = this.ExternalLoginProviders != null && this.ExternalLoginProviders.Count > 0;
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
    }
}
