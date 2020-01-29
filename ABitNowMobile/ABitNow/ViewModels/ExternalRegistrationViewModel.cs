using ABitNow.Contracts.Services.Data;
using ABitNow.Contracts.Services.General;
using ABitNow.Exceptions;
using ABitNow.Models;
using ABitNow.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ABitNow.ViewModels
{
    public class ExternalRegistrationViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        public ExternalRegistrationViewModel(IConnectionService connectionService, INavigationService navigationService, IDialogService dialogService, ISettingsService settingsService, IAuthenticationService authenticationService) : base(connectionService, navigationService, dialogService, settingsService)
        {
            _authenticationService = authenticationService;
        }

        private ExternalRegistrationConfirmation _selectedExternalRegistrationConfirmation;
        public ICommand RegisterCommand => new Command(OnRegister);

        public ExternalRegistrationConfirmation SelectedExternalRegistrationConfirmation
        {
            get => _selectedExternalRegistrationConfirmation;
            set
            {
                _selectedExternalRegistrationConfirmation = value;
                OnPropertyChanged();
            }
        }

        private async void OnRegister()
        {
            try
            {
                IsBusy = true;
                if (_connectionService.IsConnected)
                {
                    if (!string.IsNullOrWhiteSpace(SelectedExternalRegistrationConfirmation.Email))
                    {
                        var externalLoginConfirmationResponse = await _authenticationService.ExternalLoginConfirmation(SelectedExternalRegistrationConfirmation);

                        if (externalLoginConfirmationResponse.Success)
                        {
                            _settingsService.AccessTokenSetting = externalLoginConfirmationResponse.Token;
                            await _navigationService.NavigateToAsync<MainViewModel>();
                        }
                        else
                        {
                            await _dialogService.ShowDialog("Invalid information.", "Error", "OK");
                        }
                    }
                    else
                    {
                        _dialogService.ShowToast("Please enter email.");
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
            IsBusy = true;
            SelectedExternalRegistrationConfirmation = (ExternalRegistrationConfirmation)data;
            Title = "External Registration Confirmation";
            IsBusy = false;
        }
    }
}
