using ABitNow.Contracts.Repository;
using ABitNow.Contracts.Services.Data;
using ABitNow.Contracts.Services.General;
using ABitNow.Repository;
using ABitNow.Services.Data;
using ABitNow.Services.General;
using ABitNow.ViewModels;
using Autofac;
using System;

namespace ABitNow.Bootstrap
{
    public class AppContainer
    {
        private static IContainer _container;

        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            //ViewModels
            builder.RegisterType<LoginViewModel>();
            builder.RegisterType<ExternalLoginViewModel>();
            builder.RegisterType<RegistrationViewModel>();
            builder.RegisterType<MainViewModel>();
            builder.RegisterType<ExternalRegistrationViewModel>();

            //services - data
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();

            //services - general
            builder.RegisterType<ConnectionService>().As<IConnectionService>();
            builder.RegisterType<NavigationService>().As<INavigationService>();
            builder.RegisterType<DialogService>().As<IDialogService>();
            builder.RegisterType<SettingsService>().As<ISettingsService>().SingleInstance();

            //General
            builder.RegisterType<GenericRepository>().As<IGenericRepository>();

            _container = builder.Build();
        }

        public static object Resolve(Type typeName)
        {
            return _container.Resolve(typeName);
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
