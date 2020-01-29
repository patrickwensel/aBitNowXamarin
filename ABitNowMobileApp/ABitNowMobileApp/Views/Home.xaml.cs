using System;
using System.Threading.Tasks;
using ABitNowMobileApp.Constants;
using ABitNowMobileApp.Interfaces;
using ABitNowMobileApp.Messages;
using ABitNowMobileApp.ViewModels;
using Xamarin.Forms;

namespace ABitNowMobileApp.Views
{
    public partial class Home : ContentPage
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly IMessagingService _messagingService;

        private bool _isNearByExpanded = false;
        private Rectangle _findVipTableInfoGridRectangle;
        private Rectangle _suggestionContainerRectangle;
        private SizeRequest _findVipTableInfoGridSizeRequest;
        private SizeRequest _currentAndRecentLocationStackSizeRequest;
        private double _findVipTableInfoGridScale;
        private double _previousYPosition;
        private double _bottomPosition = 0.79;
        private double _topPosition = 0.84;

        public Home(HomeViewModel homeViewModel, IMessagingService messagingService)
        {
            InitializeComponent();
            _homeViewModel = homeViewModel;
            _messagingService = messagingService;
            BindingContext = _homeViewModel;

            _findVipTableInfoGridScale = ImgFindVipTable.Scale;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _homeViewModel.OnViewAppearAsync();
            _messagingService.Subscribe<SuggestionRemovedMessage>(this, OnSuggestionRemoved);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _messagingService.Unsubscribe<SuggestionRemovedMessage>(this);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            _isNearByExpanded = false;
            FrNearBy.TranslationY = height * _bottomPosition;
            FrNearBy.HeightRequest = height;
            double margin = height - (height * _topPosition);
            BottomContainerGrid.Margin = new Thickness(0, 0, 0, margin);

            _findVipTableInfoGridRectangle = ImgFindVipTable.Bounds;
            _findVipTableInfoGridSizeRequest = ImgFindVipTable.Measure(double.PositiveInfinity, double.PositiveInfinity);
            System.Diagnostics.Debug.WriteLine($"Size requested : {_findVipTableInfoGridRectangle}, {_findVipTableInfoGridSizeRequest}");
        }

        private async void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length < 3 || string.IsNullOrEmpty(e.NewTextValue))
            {
                if (string.IsNullOrEmpty(e.OldTextValue))
                    return;

                await HideSuggestionsContainerWithAnimation();
                return;
            }

            SuggestionsContainerStack.IsVisible = true;
            _homeViewModel.SearchRestaurantsAsync();

            if (e.NewTextValue.Length == 3 && e.OldTextValue.Length < e.NewTextValue.Length)
            {
                ShowSuggestionsContainerWithAnimation();
            }
            else
            {
                double tempHeight = e.NewTextValue.Length > e.OldTextValue.Length ? GetPossibleMaxHeightForSuggestionContainer() : GetPossibleMinHeightForSuggestionContainer();
                SuggestionsContainerStack.HeightRequest = tempHeight;
            }
        }

        private void ShowSuggestionsContainerWithAnimation()
        {
            if (Device.RuntimePlatform.Equals(Device.Android))
            {
                SuggestionsTitlesStack.IsVisible = true;
            }

            SuggestionsContainerStack.Animate("ShowContainer", x =>
            {
                double tempHeight = GetPossibleMaxHeightForSuggestionContainer();
                SuggestionsContainerStack.HeightRequest = tempHeight * x;
            });
        }

        private async Task HideSuggestionsContainerWithAnimation()
        {
            SuggestionsContainerStack.Animate("HideContainer", x =>
            {
                double tempHeight = GetPossibleMinHeightForSuggestionContainer();
                SuggestionsContainerStack.HeightRequest = tempHeight * (1 - x);
            });

            if (Device.RuntimePlatform.Equals(Device.Android))
            {
                SuggestionsTitlesStack.IsVisible = false;
                await Task.Delay((int)AppThemeConstants.AnimationSpeed);
                SuggestionsContainerStack.IsVisible = false;
            }
        }

        private double GetPossibleMaxHeightForSuggestionContainer()
        {
            var containerSize = SuggestionsContainerStack.Measure(double.PositiveInfinity, double.PositiveInfinity);
            var suggestionsContainer = SuggestionsContainer.Measure(double.PositiveInfinity, double.PositiveInfinity);

            double tempHeight = Math.Max(containerSize.Request.Height, Math.Max(suggestionsContainer.Request.Height + 20, SuggestionsContainerStack.Height));

            System.Diagnostics.Debug.WriteLine($"Container Size : {containerSize}, SuggestionContianer Size : {suggestionsContainer}, Actual = {SuggestionsContainerStack.Height}, Height To be Set : {tempHeight}");

            return tempHeight;
        }

        private double GetPossibleMinHeightForSuggestionContainer()
        {
            var containerSize = SuggestionsContainerStack.Measure(double.PositiveInfinity, double.PositiveInfinity);
            var suggestionsContainer = SuggestionsContainer.Measure(double.PositiveInfinity, double.PositiveInfinity);

            double tempHeight = Math.Min(containerSize.Request.Height, Math.Min(suggestionsContainer.Request.Height + 20, SuggestionsContainerStack.Height));

            System.Diagnostics.Debug.WriteLine($"Container Size : {containerSize}, SuggestionContianer Size : {suggestionsContainer}, Actual = {SuggestionsContainerStack.Height}, Height To be Set : {tempHeight}");

            return tempHeight;
        }

        private async void OnSuggestionRemoved(Object sender, SuggestionRemovedMessage suggestionRemovedMessage)
        {
            if (_homeViewModel.Suggestions.Count > 0)
            {
                double tempHeight = GetPossibleMinHeightForSuggestionContainer();
                SuggestionsContainerStack.HeightRequest = tempHeight;
            }
            else
            {
                await HideSuggestionsContainerWithAnimation();
            }
        }

        private void SearchFocused(object sender, EventArgs e)
        {
            MainGrid.RaiseChild(TopContainerStack);
            MainGrid.RaiseChild(CIUserIco);
            SearchBarGrid.RaiseChild(FrmSearchbar);

            _suggestionContainerRectangle = SuggestionsContainerStack.Bounds.Height > _suggestionContainerRectangle.Height ? SuggestionsContainerStack.Bounds : _suggestionContainerRectangle;

            SuggestionsContainerStack.IsVisible = false;

            ImgFindVipTable.TranslateTo(0, -ImgFindVipTable.Margin.Top, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            SearchBarGrid.TranslateTo(0, -ImgFindVipTable.Margin.Top, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            Rectangle searchBarGridBounds = SearchBarGrid.Bounds;
            searchBarGridBounds.Height = searchBarGridBounds.Height + ImgFindVipTable.Margin.Top;
            SearchBarGrid.Layout(searchBarGridBounds);
            FilterScroll.TranslateTo(0, 0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            FilterScroll.FadeTo(1, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            CIUserIco.FadeTo(0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            LblSearchIcon.IsVisible = false;
        }

        private void SearchNowTapped(object sender, EventArgs e)
        {
            MainGrid.RaiseChild(TopContainerStack);
            MainGrid.RaiseChild(CIUserIco);
        }

        private void BtnCloseClicked(object sender, EventArgs e)
        {
            TxtSearch.Text = string.Empty;
        }

        private void CloseSearchTapped(object sender, EventArgs e)
        {
            LblSearchIcon.IsVisible = true;
            ImgFindVipTable.TranslateTo(0, 0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            SearchBarGrid.TranslateTo(0, 0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            Rectangle searchBarGridBounds = SearchBarGrid.Bounds;
            searchBarGridBounds.Height = searchBarGridBounds.Height - ImgFindVipTable.Margin.Top;
            SearchBarGrid.Layout(searchBarGridBounds);
            FilterScroll.TranslateTo(0, AppThemeConstants.ScreenHeight, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            FilterScroll.FadeTo(0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            CIUserIco.FadeTo(1, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            MainGrid.RaiseChild(FrNearBy);
            MainGrid.RaiseChild(CIUserIco);
            TxtSearch.Text = string.Empty;
            TxtSearch.Unfocus();
        }

        private void BackOverBarTapped(object sender, EventArgs e)
        {
            if (!BVBar.IsVisible)
                return;

            var pageHeight = Height;

            if (_isNearByExpanded)
            {
                ImgFindVipTable.TranslateTo(0, 0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                ImgFindVipTable.ScaleTo(_findVipTableInfoGridScale, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                SearchBarGrid.TranslateTo(0, 0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                ImgSearchIcon.TranslateTo(0, 0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                ImgSearchIcon.FadeTo(0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                FrNearBy.TranslateTo(0, pageHeight * _bottomPosition, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            }
            else
            {
                ImgFindVipTable.TranslateTo(-(ImgFindVipTable.Margin.Left - 10), -(ImgFindVipTable.Margin.Top + 25), AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                ImgFindVipTable.ScaleTo(_findVipTableInfoGridScale * 0.75, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                SearchBarGrid.TranslateTo(0, ImgFindVipTable.Margin.Top + 25, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                ImgSearchIcon.TranslateTo(45, 0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                ImgSearchIcon.FadeTo(1, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                FrNearBy.TranslateTo(0, pageHeight - (pageHeight * _topPosition), AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            }

            _isNearByExpanded = !_isNearByExpanded;
        }

        private void BackOverBarPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (!BVBar.IsVisible)
                return;

            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    _previousYPosition = e.TotalY;
                    FrNearBy.TranslateTo(0, e.TotalY, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                    break;
                case GestureStatus.Completed:
                    var pageHeight = Height;
                    var bottomPosition = pageHeight * _bottomPosition;
                    var topPosition = pageHeight - (pageHeight * _topPosition);

                    if (_previousYPosition < 0)
                    {
                        // Moving to top
                        ImgFindVipTable.TranslateTo(-(ImgFindVipTable.Margin.Left - 10), -(ImgFindVipTable.Margin.Top + 25), AppThemeConstants.AnimationSpeed, Easing.SinIn);
                        ImgFindVipTable.ScaleTo(_findVipTableInfoGridScale * 0.75, AppThemeConstants.AnimationSpeed, Easing.SinIn);
                        SearchBarGrid.TranslateTo(0, ImgFindVipTable.Margin.Top + 25, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                        ImgSearchIcon.TranslateTo(45, 0, AppThemeConstants.AnimationSpeed, Easing.SinIn);
                        ImgSearchIcon.FadeTo(1, AppThemeConstants.AnimationSpeed, Easing.SinIn);
                        FrNearBy.TranslateTo(0, topPosition, AppThemeConstants.AnimationSpeed, Easing.SinIn);
                        _isNearByExpanded = true;
                    }
                    else
                    {
                        // Moving to bottom
                        ImgFindVipTable.TranslateTo(0, 0, AppThemeConstants.AnimationSpeed, Easing.SinOut);
                        ImgFindVipTable.ScaleTo(_findVipTableInfoGridScale, AppThemeConstants.AnimationSpeed, Easing.SinOut);
                        SearchBarGrid.TranslateTo(0, 0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                        ImgSearchIcon.TranslateTo(0, 0, AppThemeConstants.AnimationSpeed, Easing.SinOut);
                        ImgSearchIcon.FadeTo(0, AppThemeConstants.AnimationSpeed, Easing.SinOut);
                        FrNearBy.TranslateTo(0, bottomPosition, AppThemeConstants.AnimationSpeed, Easing.SinOut);
                        _isNearByExpanded = false;
                    }
                    break;
            }
        }

        private void CurrentLocationTapped(object sender, EventArgs e)
        {
            if (!_isNearByExpanded)
                return;

            var pageHeight = Height;

            var isLocationLabelVisible = LblLocation.IsVisible;

            var recentLocationStackSize = RecentLocationStack.Measure(double.PositiveInfinity, double.PositiveInfinity);
            var currentAndRecentLocationSize = CurrentAndRecentLocationStack.Measure(double.PositiveInfinity, double.PositiveInfinity);
            var suggestionHeight = LocationSuggestionStack.Measure(double.PositiveInfinity, double.PositiveInfinity);

            var suggestionMinHeight = Height * 0.28;
            var heighToBeSet = Math.Max(suggestionMinHeight, suggestionHeight.Request.Height);

            LocationSuggestionStack.HeightRequest = heighToBeSet;

            var locationStackSize = LocationStack.Measure(double.PositiveInfinity, double.PositiveInfinity);
            var locationStackHeight = Math.Max(currentAndRecentLocationSize.Request.Height + heighToBeSet + 90, locationStackSize.Request.Height);

            FrNearBy.TranslateTo(0, pageHeight - ((pageHeight * _topPosition) + (isLocationLabelVisible ? 71 : 0)), AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            Thickness bottomGridMargin = BottomContainerGrid.Margin;
            bottomGridMargin.Bottom += isLocationLabelVisible ? -71 : 71;
            BottomContainerGrid.Margin = bottomGridMargin;

            System.Diagnostics.Debug.WriteLine($"Location Stack Height : {locationStackHeight}, {isLocationLabelVisible}");
            if (isLocationLabelVisible)
            {
                LblLocation.TranslateTo(-80, 30, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                LblLocationSymbol.TranslateTo(-80, 30, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                SvBottomContainer.TranslateTo(0, locationStackHeight, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                LblLocation.IsVisible = false;
                LocationStack.Animate("ShowLocationStack", (x) =>
                {
                    LocationStack.HeightRequest = locationStackHeight * x;
                });
                LocationWithZipCodeStack.TranslateTo(0, 0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            }
            else
            {
                LocationWithZipCodeStack.TranslateTo(80, -30, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                SvBottomContainer.TranslateTo(0, 0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                LocationStack.Animate("HideLocationStack", (x) =>
                {
                    LocationStack.HeightRequest = locationStackHeight * (1 - x);
                }, finished: (x, y) =>
                {
                    System.Diagnostics.Debug.WriteLine($"Finished callback : {x}, {y}");
                    LblLocation.IsVisible = true;
                });
                LblLocationSymbol.TranslateTo(0, 0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
                LblLocation.TranslateTo(0, 0, AppThemeConstants.AnimationSpeed, Easing.SinInOut);
            }
        }

        private void TxtSearchLocationFocused(object sender, EventArgs e)
        {
            HideCurrentAndRecentLocationStack();
        }

        private void TxtSearchLocationUnfocused(object sender, EventArgs e)
        {
            ShowCurrentAndRecentLocationStack();
        }

        private void BtnCloseSearchLocationClicked(object sender, EventArgs e)
        {
            TxtSearchLocation.Text = string.Empty;
            ShowCurrentAndRecentLocationStack();
        }

        private void ShowCurrentAndRecentLocationStack()
        {
            CurrentAndRecentLocationStack.IsVisible = true;
            var currentAndRecentLocationSize = CurrentAndRecentLocationStack.Measure(double.PositiveInfinity, double.PositiveInfinity);

            var heightToBeSet = Math.Max(currentAndRecentLocationSize.Request.Height, _currentAndRecentLocationStackSizeRequest.Request.Height);

            CurrentAndRecentLocationStack.Animate("ShowCurrentAndRecentLocationStack", (x) =>
            {
                CurrentAndRecentLocationStack.HeightRequest = heightToBeSet * x;
            }, length: AppThemeConstants.AnimationSpeed, easing: Easing.SinInOut);
        }

        private void HideCurrentAndRecentLocationStack()
        {
            var currentAndRecentLocationSize = CurrentAndRecentLocationStack.Measure(double.PositiveInfinity, double.PositiveInfinity);
            if (currentAndRecentLocationSize.Request.Height > _currentAndRecentLocationStackSizeRequest.Request.Height)
                _currentAndRecentLocationStackSizeRequest = currentAndRecentLocationSize;

            CurrentAndRecentLocationStack.Animate("HideCurrentAndRecentLocationStack", (x) =>
            {
                CurrentAndRecentLocationStack.HeightRequest = currentAndRecentLocationSize.Request.Height * (1 - x);
            }, length: AppThemeConstants.AnimationSpeed, easing: Easing.SinInOut, finished: (x, y) =>
             {
                 CurrentAndRecentLocationStack.IsVisible = false;
             });
        }

        private void ImgUserTapped(object sender, EventArgs e)
        {
            SMSideMenu.ShowSideMenu();
        }

        private void OnSideMenuShow(object sender, EventArgs e)
        {
            MainGrid.RaiseChild(SMSideMenu);
            System.Diagnostics.Debug.WriteLine("Side menu showing...");
        }

        private void OnSideMenuHide(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Side menu hiding...");
        }
    }
}
