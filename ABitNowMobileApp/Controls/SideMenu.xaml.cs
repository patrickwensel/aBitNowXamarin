using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using ABitNowMobileApp.Constants;
using ABitNowMobileApp.UiItemModels;
using Xamarin.Forms;

namespace ABitNowMobileApp.Controls
{
    public partial class SideMenu : ContentView
    {
        public static readonly BindableProperty UserInfoProperty = BindableProperty.Create(nameof(UserInfo), typeof(UserInfo), typeof(SideMenu), null, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((SideMenu)bindable).SetUserInfo();
        });

        public UserInfo UserInfo
        {
            get { return (UserInfo)GetValue(UserInfoProperty); }
            set { SetValue(UserInfoProperty, value); }
        }

        public static readonly BindableProperty IsUserLoggedInProperty = BindableProperty.Create(nameof(IsUserLoggedIn), typeof(bool), typeof(SideMenu), false, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((SideMenu)bindable).SetIsLoggedIn();
        });

        public bool IsUserLoggedIn
        {
            get { return (bool)GetValue(IsUserLoggedInProperty); }
            set { SetValue(IsUserLoggedInProperty, value); }
        }

        public static readonly BindableProperty MenuOptionsProperty = BindableProperty.Create(nameof(MenuOptions), typeof(IList<MenuOption>), typeof(SideMenu), null, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((SideMenu)bindable).SetMenuOptions();
        });

        public IList<MenuOption> MenuOptions
        {
            get { return (IList<MenuOption>)GetValue(MenuOptionsProperty); }
            set { SetValue(MenuOptionsProperty, value); }
        }

        public static readonly BindableProperty SelectedMenuItemProperty = BindableProperty.Create(nameof(SelectedMenuItem), typeof(MenuOption), typeof(SideMenu), null, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((SideMenu)bindable).SetSelectedMenuOption();
        });

        public MenuOption SelectedMenuItem
        {
            get { return (MenuOption)GetValue(SelectedMenuItemProperty); }
            set { SetValue(SelectedMenuItemProperty, value); }
        }

        public static readonly BindableProperty LogoutCommandProperty = BindableProperty.Create(nameof(LogoutCommand), typeof(ICommand), typeof(SideMenu), null, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((SideMenu)bindable).SetLogoutCommand();
        });

        public ICommand LogoutCommand
        {
            get { return (ICommand)GetValue(LogoutCommandProperty); }
            set { SetValue(LogoutCommandProperty, value); }
        }

        public static readonly BindableProperty MenuSelectedCommandProperty = BindableProperty.Create(nameof(MenuSelectedCommand), typeof(ICommand), typeof(SideMenu), null, BindingMode.TwoWay);

        public ICommand MenuSelectedCommand
        {
            get { return (ICommand)GetValue(MenuSelectedCommandProperty); }
            set { SetValue(MenuSelectedCommandProperty, value); }
        }

        public static readonly BindableProperty LoginCommandProperty = BindableProperty.Create(nameof(LoginCommand), typeof(ICommand), typeof(SideMenu), null, BindingMode.TwoWay);

        public ICommand LoginCommand
        {
            get { return (ICommand)GetValue(LoginCommandProperty); }
            set { SetValue(LoginCommandProperty, value); }
        }

        public event EventHandler OnMenuShow;

        public event EventHandler OnMenuHide;

        public SideMenu()
        {
            InitializeComponent();
            CloseSideMenu();

            BottomStack.Padding = new Thickness(29, 0, 29, App.SafeAreaBottomMargin);
        }

        public async void ShowSideMenu()
        {
            OnMenuShow?.Invoke(this, new EventArgs());
            await Task.WhenAll(new List<Task>()
            {
                this.TranslateTo(0, 0, AppThemeConstants.AnimationSpeed, Easing.SinInOut),
                this.FadeTo(1, AppThemeConstants.AnimationSpeed, Easing.SinInOut),
                PVLeftSide.FadeTo(0.95, AppThemeConstants.AnimationSpeed, Easing.SinInOut),
            });
        }

        public async void CloseSideMenu()
        {
            OnMenuHide?.Invoke(this, new EventArgs());
            await Task.WhenAll(new List<Task>()
            {
                this.TranslateTo(App.ScreenSize.Width, 0, AppThemeConstants.AnimationSpeed, Easing.SinInOut),
                PVLeftSide.FadeTo(0, AppThemeConstants.AnimationSpeed, Easing.SinInOut),
                this.FadeTo(0, AppThemeConstants.AnimationSpeed, Easing.SinInOut),
            });
        }

        private void OnLoginSectionTapped(object sender, EventArgs e)
        {
            CloseSideMenu();
            LoginCommand?.Execute(null);
        }

        private void CloseSideMenuTapped(object sender, EventArgs e)
        {
            CloseSideMenu();
        }

        private void OnMenuOptionSelected(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            SelectedMenuItem = e.ItemData as MenuOption;
            MenuSelectedCommand?.Execute(e.ItemData);
        }

        private void SetUserInfo()
        {
            CIUserIcon.Source = !string.IsNullOrEmpty(UserInfo.ProfileImage) ? UserInfo.ProfileImage : "IcoUser";
            LblName.Text = UserInfo.Name;
            LblEmail.Text = UserInfo.Email;
        }

        private void SetMenuOptions()
        {
            LstMenu.ItemsSource = MenuOptions;
        }

        private void SetSelectedMenuOption()
        {
            LstMenu.SelectedItem = SelectedMenuItem;
        }

        private void SetLogoutCommand()
        {
            BtnLogout.Command = this.LogoutCommand;
        }

        private void SetIsLoggedIn()
        {
            UserInfoGrid.IsVisible = this.IsUserLoggedIn;
            BottomStack.IsVisible = this.IsUserLoggedIn;
            BVLogin.IsVisible = !this.IsUserLoggedIn;
            FrLogin.IsVisible = !this.IsUserLoggedIn;
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (e.TotalX > 0)
                CloseSideMenu();
        }
    }
}
