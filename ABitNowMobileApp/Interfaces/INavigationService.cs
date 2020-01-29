using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ABitNowMobileApp.Enums;

namespace ABitNowMobileApp.Interfaces
{
    public interface INavigationService
    {
        PageKey CurrentPageKey { get; }

        Task NavigateToFirstScreenAsync();

        Task NavigateToAsync(PageKey pageKey, Dictionary<string, object> parameters = null, bool isAnimated = false);

        Task PopToRootAsync();

        Task GoBackAsync(int numberOfPagesToSkip = 0);

        Task GoBackToPageAsync(PageKey pageKey);

        void PresentMenuView();
    }
}
