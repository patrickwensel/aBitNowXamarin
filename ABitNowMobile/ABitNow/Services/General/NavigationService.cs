using ABitNow.Bootstrap;
using ABitNow.Contracts.Services.Data;
using ABitNow.Contracts.Services.General;
using ABitNow.ViewModels;
using ABitNow.ViewModels.Base;
using ABitNow.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace ABitNow.Services.General
{
    public class NavigationService : INavigationService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly Dictionary<Type, Type> _mappings;

        protected Application CurrentApplication => Application.Current;

        public NavigationService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _mappings = new Dictionary<Type, Type>();

            CreatePageViewModelMappings();
        }

        public async Task InitializeAsync()
        {
            if (_authenticationService.IsUserAuthenticated())
            {
                await NavigateToAsync<MainViewModel>();
            }
            else
            {
                await NavigateToAsync<LoginViewModel>();
            }
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return InternalNavigateToAsync(viewModelType, null);
        }

        public async Task ClearBackStack()
        {
            await CurrentApplication.MainPage.Navigation.PopToRootAsync();
        }

        public Task NavigateToAsync(Type viewModelType, object parameter)
        {
            return InternalNavigateToAsync(viewModelType, parameter);
        }

        public async Task NavigateBackAsync()
        {
            //if (CurrentApplication.MainPage is MainView mainPage)
            //{
            //    await mainPage.Detail.Navigation.PopAsync();
            //}
            //else if (CurrentApplication.MainPage != null)
            //{
            //    await CurrentApplication.MainPage.Navigation.PopAsync();
            //}
        }

        public virtual Task RemoveLastFromBackStackAsync()
        {
            //if (CurrentApplication.MainPage is MainView mainPage)
            //{
            //    mainPage.Detail.Navigation.RemovePage(
            //        mainPage.Detail.Navigation.NavigationStack[mainPage.Detail.Navigation.NavigationStack.Count - 2]);
            //}

            return Task.FromResult(true);
        }

        public async Task PopToRootAsync()
        {
            //if (CurrentApplication.MainPage is MainView mainPage)
            //{
            //    await mainPage.Detail.Navigation.PopToRootAsync();
            //}
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreateAndBindPage(viewModelType, parameter);

            if (page is MainView || page is RegistrationView || page is ExternalRegistrationView || page is LoginView)
            {
                CurrentApplication.MainPage = new ABitNowNavigationPage(page);
            }
            else
            {
                var navigationPage = CurrentApplication.MainPage as ABitNowNavigationPage;

                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    CurrentApplication.MainPage = new ABitNowNavigationPage(page);
                }
            }
            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!_mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            return _mappings[viewModelType];
        }

        protected Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            ViewModelBase viewModel = AppContainer.Resolve(viewModelType) as ViewModelBase;
            page.BindingContext = viewModel;

            return page;
        }

        private void CreatePageViewModelMappings()
        {
            _mappings.Add(typeof(LoginViewModel), typeof(LoginView));
            _mappings.Add(typeof(MainViewModel), typeof(MainView));
            _mappings.Add(typeof(ExternalLoginViewModel), typeof(ExternalLoginView));
            _mappings.Add(typeof(ExternalRegistrationViewModel), typeof(ExternalRegistrationView));
            _mappings.Add(typeof(RegistrationViewModel), typeof(RegistrationView));
        }
    }
}
