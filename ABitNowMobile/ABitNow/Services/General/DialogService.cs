using ABitNow.Contracts.Services.General;
using Acr.UserDialogs;
using System;
using System.Threading.Tasks;

namespace ABitNow.Services.General
{
    public class DialogService : IDialogService
    {
        public void HideLoading()
        {
            UserDialogs.Instance.HideLoading();
        }

        public Task ShowDialog(string message, string title, string buttonLabel)
        {
            return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
        }

        public async Task ShowError()
        {
            await ShowDialog("OOPS!!! Something went wrong.", "Error", "OK");
        }

        public void ShowInternetError()
        {
            ShowToast("No Internet Connection.");
        }

        public void ShowLoading(string title = null, MaskType? maskType = null)
        {
            UserDialogs.Instance.ShowLoading(title);
        }

        public void ShowToast(string message)
        {
            UserDialogs.Instance.Toast(message);
        }
    }
}
