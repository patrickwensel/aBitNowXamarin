using System;
using System.Collections.Generic;
using System.Timers;
using ABitNowMobileApp.Constants;
using ABitNowMobileApp.Interfaces;
using ABitNowMobileApp.Messages;
using ABitNowMobileApp.ViewModels;
using Xamarin.Forms;

namespace ABitNowMobileApp.Views
{
    public partial class Search : ContentPage
    {
        private readonly SearchViewModel _searchViewModel;
        private readonly IMessagingService _messagingService;
        private readonly Timer _rotationTaskTimer;

        public Search(SearchViewModel searchViewModel, IMessagingService messagingService, string suggestion = null)
        {
            InitializeComponent();
            _rotationTaskTimer = new Timer(AppThemeConstants.AnimationSpeed + 100);
            _rotationTaskTimer.Elapsed += RotationTaskTimer_Elapsed;
            _searchViewModel = searchViewModel;
            _messagingService = messagingService;
            BindingContext = _searchViewModel;

            if (!string.IsNullOrEmpty(suggestion))
                TxtSearch.Text = suggestion;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            TxtSearch.Focus();
            _messagingService.Subscribe<HideUnhideViewAnimationMessage>(this, OnHideUnhideViewAnimationMessage);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _messagingService.Unsubscribe<HideUnhideViewAnimationMessage>(this);
        }

        private void BackTapped(object sender, EventArgs e)
        {

        }

        private void BtnCloseClicked(object sender, EventArgs e)
        {
            TxtSearch.Text = string.Empty;
            TxtSearch.Unfocus();
        }

        private void OnHideUnhideViewAnimationMessage(object sender, HideUnhideViewAnimationMessage message)
        {
            if (message.IsHiding)
            {
                if (!string.IsNullOrEmpty(TxtSearch.Text))
                {
                    LblLoader.IsVisible = true;
                    LblLoader.FadeTo(1, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                    LblLoader.TranslateTo(0, Height / 3.5, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                    _rotationTaskTimer.Start();
                }

                LvSearchedRestaurants.TranslateTo(0, Height, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                LvSearchedRestaurants.FadeTo(0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            }
            else
            {
                _rotationTaskTimer.Stop();
                LblLoader.Animate("HideLoader", (x) =>
                {
                    LblLoader.TranslationY = (Height / 3.5) * (1 - x);
                }, length: AppThemeConstants.AnimationSpeed, easing: Easing.SinInOut, finished: (x, y) =>
                {
                    LblLoader.IsVisible = false;
                });
                LblLoader.FadeTo(0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                LvSearchedRestaurants.TranslateTo(0, 0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                LvSearchedRestaurants.FadeTo(1, AppThemeConstants.AnimationSpeed, Easing.SinInOut);

            }
        }

        private void RotationTaskTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            LblLoader.Animate("RotateLoader", (x) =>
            {
                LblLoader.Rotation = 360 * x;
            }, length: AppThemeConstants.AnimationSpeed, easing: Easing.SinInOut);
        }
    }
}
