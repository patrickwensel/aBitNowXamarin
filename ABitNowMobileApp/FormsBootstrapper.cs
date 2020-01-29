using System;
using ABitNowMobileApp.Interfaces;
using ABitNowMobileApp.Services;
using ABitNowMobileApp.ViewModels;
using ABitNowMobileApp.Views;
using Unity;
using Unity.Lifetime;

namespace ABitNowMobileApp
{
    public class FormsBootstrapper
    {
        public static void Initialize(IUnityContainer container)
        {
            // Interface and Class Registration goes here.
            RegisterTypes(container);
            RegisterViewModels(container);
            RegisterViews(container);
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IPersistanceService, PersistanceService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMessagingService, MessagingService>(new HierarchicalLifetimeManager());
            container.RegisterType<INavigationService, NavigationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISecureStorage, SecureStorageService>(new HierarchicalLifetimeManager());
        }

        private static void RegisterViewModels(IUnityContainer container)
        {
            container.RegisterType<HomeViewModel>(new HierarchicalLifetimeManager());
            container.RegisterType<LoginViewModel>(new TransientLifetimeManager());
            container.RegisterType<SignupViewModel>(new TransientLifetimeManager());
            container.RegisterType<SearchViewModel>(new TransientLifetimeManager());
            container.RegisterType<RestaurantDetailViewModel>(new TransientLifetimeManager());
        }

        private static void RegisterViews(IUnityContainer container)
        {
            container.RegisterType<Home>(new TransientLifetimeManager());
            container.RegisterType<Login>(new TransientLifetimeManager());
            container.RegisterType<Signup>(new TransientLifetimeManager());
            container.RegisterType<Search>(new TransientLifetimeManager());
            container.RegisterType<RestaurantDetail>(new TransientLifetimeManager());
        }
    }
}
