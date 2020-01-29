using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABitNowMobileApp.Data;
using ABitNowMobileApp.Enums;
using ABitNowMobileApp.Interfaces;
using ABitNowMobileApp.Views;
using Unity.Resolution;
using Xamarin.Forms;

namespace ABitNowMobileApp.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IPersistanceService _persistanceService;
        private readonly Dictionary<Type, PageKey> _pages;

        private MasterDetailPage _masterDetailPage;
        private NavigationPage _navigation;

        public NavigationService(IPersistanceService persistanceService)
        {
            _persistanceService = persistanceService;
            _pages = new Dictionary<Type, PageKey>();
        }

        public PageKey CurrentPageKey => _pages[_navigation.CurrentPage.GetType()];

        public async Task NavigateToFirstScreenAsync()
        {
            if (_persistanceService.IsLoggedIn)
                await NavigateToAsync(PageKey.Home);
            else
                await NavigateToAsync(PageKey.Login);
        }

        public async Task NavigateToAsync(PageKey pageKey, Dictionary<string, object> parameters = null, bool isAnimated = true)
        {
            Page page = GetPage(pageKey, parameters);
            if (pageKey == PageKey.Login || pageKey == PageKey.Signup || pageKey == PageKey.Home)
            {
                _navigation = new NavigationPage(page);
                Application.Current.MainPage = _navigation;
            }
            else
            {
                await _navigation.PushAsync(page, isAnimated);
            }
        }

        public async Task GoBackAsync(int numberOfPagesToSkip = 0)
        {
            IReadOnlyList<Page> navigationStack = _navigation.Navigation.NavigationStack;
            while (numberOfPagesToSkip > 0)
            {
                _navigation.Navigation.RemovePage(navigationStack[navigationStack.Count - 2]);
                numberOfPagesToSkip--;
            }

            await _navigation.PopAsync();
        }

        public async Task GoBackToPageAsync(PageKey pageKey)
        {
            IReadOnlyList<Page> navigationStack = _navigation.Navigation.NavigationStack;

            var numberOfPagesToSkip = 0;

            if (navigationStack.Count > 2)
            {
                var startIndex = navigationStack.Count - 2;

                for (var i = startIndex; i >= 0; i--)
                {
                    var page = navigationStack[i];

                    var pageType = _pages.First(x => x.Value == pageKey).Key;

                    if (page.GetType() == pageType)
                    {
                        numberOfPagesToSkip = startIndex - i;
                        break;
                    }
                }
            }

            await GoBackAsync(numberOfPagesToSkip);
        }

        public async Task PopToRootAsync()
        {
            await _navigation.PopToRootAsync(true);
        }

        public void PresentMenuView()
        {
            if (_masterDetailPage != null)
            {
                _masterDetailPage.IsPresented = !_masterDetailPage.IsPresented;
            }
        }

        private Page GetPage(PageKey pageKey, Dictionary<string, object> parameters = null)
        {
            Page page;
            ResolverOverride[] resolverOverrides = null;
            if (parameters != null && parameters.Count() > 0)
            {
                resolverOverrides = new ResolverOverride[parameters.Count()];
                for (int i = 0; i < parameters.Count(); i++)
                {
                    var dictionaryItem = parameters.ElementAt(i);
                    resolverOverrides[i] = new ParameterOverride(dictionaryItem.Key, dictionaryItem.Value);
                }
            }

            switch (pageKey)
            {
                case PageKey.Home:
                    page = (ContentPage)ContainerManager.Container.Resolve(typeof(Home), typeof(Home).ToString());
                    break;
                case PageKey.Login:
                    page = (ContentPage)ContainerManager.Container.Resolve(typeof(Login), typeof(Login).ToString());
                    break;
                case PageKey.Signup:
                    page = (ContentPage)ContainerManager.Container.Resolve(typeof(Signup), typeof(Signup).ToString());
                    break;
                case PageKey.Search:
                    page = resolverOverrides != null ? (ContentPage)ContainerManager.Container.Resolve(typeof(Search), typeof(Search).ToString(), resolverOverrides) : (ContentPage)ContainerManager.Container.Resolve(typeof(Search), typeof(Search).ToString());
                    break;
                case PageKey.RestaurantDetail:
                    page = (ContentPage)ContainerManager.Container.Resolve(typeof(RestaurantDetail), typeof(RestaurantDetail).ToString());
                    break;
                case PageKey.Filter:
                    page = (ContentPage)ContainerManager.Container.Resolve(typeof(Filter), typeof(Filter).ToString());
                    break;
                default:
                    throw new ArgumentException(
                        $"No such page: {pageKey}. Did you forget to Register it?", nameof(pageKey));
            }

            if (!_pages.ContainsKey(page.GetType()))
                _pages.Add(page.GetType(), pageKey);

            return page;
        }
    }
}
