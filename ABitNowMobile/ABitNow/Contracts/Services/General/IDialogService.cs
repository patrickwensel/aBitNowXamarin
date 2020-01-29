using Acr.UserDialogs;
using System;
using System.Threading.Tasks;

namespace ABitNow.Contracts.Services.General
{
    public interface IDialogService
    {
        Task ShowDialog(string message, string title, string buttonLabel);

        void ShowToast(string message);

        void ShowLoading(string title = null, MaskType? maskType = null);

        void HideLoading();

        void ShowInternetError();

        Task ShowError();

    }
}
