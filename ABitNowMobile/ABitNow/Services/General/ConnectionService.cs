using ABitNow.Contracts.Services.General;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace ABitNow.Services.General
{
    public class ConnectionService : IConnectionService
    {
        private readonly IConnectivity _connectivity;
        private readonly IDialogService _dialogService;
        public ConnectionService(IDialogService dialogService)
        {
            _dialogService = dialogService;
            _connectivity = CrossConnectivity.Current;
            _connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.IsConnected)
            {
                _dialogService.ShowToast("Internet Connection Available.");
            }
            else
            {
                _dialogService.ShowToast("No Internet Connection.");
            }

            ConnectivityChanged?.Invoke(this, new ConnectivityChangedEventArgs() { IsConnected = e.IsConnected });
        }

        public bool IsConnected => _connectivity.IsConnected;

        public event ConnectivityChangedEventHandler ConnectivityChanged;
    }
}
