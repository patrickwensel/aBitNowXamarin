using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ABitNowMobileApp.Interfaces;
using ABitNowMobileApp.Messages;
using ABitNowMobileApp.UiItemModels;
using Xamarin.Forms;

namespace ABitNowMobileApp.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IMessagingService _messagingService;
        private const string FilterButtonText = "􀌉 Filters";

        private string _search;
        private bool _hasAnySuggestions;
        private IList<FilterTag> _filterButtons;
        private IList<Suggestion> _suggestions;
        private IList<SearchedRestaurant> _searchedRestaurants;
        private IList<SearchedRestaurant> _filteredRestaurants;

        public string Search
        {
            get { return _search; }
            set { SetProperty(ref _search, value); }
        }

        public bool HasAnySuggestions
        {
            get { return _hasAnySuggestions; }
            set { SetProperty(ref _hasAnySuggestions, value); }
        }

        public IList<FilterTag> FilterButtons
        {
            get { return _filterButtons; }
            set { SetProperty(ref _filterButtons, value); }
        }

        public IList<Suggestion> Suggestions
        {
            get { return _suggestions; }
            set { SetProperty(ref _suggestions, value); }
        }

        public IList<SearchedRestaurant> SearchedRestaurants
        {
            get { return _searchedRestaurants; }
            set { SetProperty(ref _searchedRestaurants, value); }
        }

        public IList<SearchedRestaurant> FilteredRestaurants
        {
            get { return _filteredRestaurants; }
            set { SetProperty(ref _filteredRestaurants, value); }
        }

        public ICommand BackCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SearchTextChangedCommand { get; set; }
        public ICommand FilterButtonTapCommand { get; set; }
        public ICommand SuggestionTapCommand { get; set; }
        public ICommand SuggestionRemoveCommand { get; set; }

        public SearchViewModel(INavigationService navigationService, IMessagingService messagingService)
        {
            _navigationService = navigationService;
            _messagingService = messagingService;
            BackCommand = new Command(NavigateOnBackPage);
            FilterButtonTapCommand = new Command<FilterTag>(OnFilterButtonSelected);
            SearchCommand = new Command(SearchRestaurantAsync);
            SearchTextChangedCommand = new Command(OnSearchTextChangerd);
            SuggestionTapCommand = new Command<Suggestion>(OnSuggestionTapped);
            SuggestionRemoveCommand = new Command<Suggestion>(OnSuggestionRemoved);

            FilterButtons = new List<FilterTag>()
            {
                new FilterTag() { Tag = "Eat In", IsJit = true, TapCommand = FilterButtonTapCommand },
                new FilterTag() { Tag = "Eat In", TapCommand = FilterButtonTapCommand },
                new FilterTag() { Tag = "Delivery", TapCommand = FilterButtonTapCommand },
                new FilterTag() { Tag = FilterButtonText, TapCommand = FilterButtonTapCommand }
            };
        }

        private async void NavigateOnBackPage()
        {
            await _navigationService.GoBackAsync();
        }

        private void OnFilterButtonSelected(FilterTag filterTag)
        {
            filterTag.IsSelected = !filterTag.IsSelected;
            bool isJitEnabled = FilterButtons.Any(x => x.IsJit && x.IsSelected);
            var selectedFilter = FilterButtons.Where(x => x.IsSelected).Select(x => x.Tag).ToList();
            System.Diagnostics.Debug.WriteLine($"Filtet Button Tapped : {filterTag.IsSelected}");
            if (filterTag.Tag.Equals(FilterButtonText))
            {
                // Navigate on Filter screen.
                _navigationService.NavigateToAsync(Enums.PageKey.Filter);
            }
            else
            {
                FiltereList(selectedFilter, isJitEnabled);
            }
        }

        private void FiltereList(IList<string> selectedTags, bool isJitEnabled)
        {
            if (SearchedRestaurants == null)
                return;

            if (selectedTags != null && selectedTags.Count > 0)
            {
                var filteredList = SearchedRestaurants.Where(x => x.Tags.Any(y => selectedTags.Any(x1 => x1.Equals(y)))).ToList();
                FilteredRestaurants = isJitEnabled ? filteredList.Where(x => x.IsJit).ToList() : filteredList;
            }
            else
                FilteredRestaurants = SearchedRestaurants;
        }

        private void OnSearchTextChangerd()
        {
            if (string.IsNullOrEmpty(Search))
            {
                if (Suggestions != null && Suggestions.Count > 0)
                    Suggestions.Clear();

                _messagingService.Send<HideUnhideViewAnimationMessage>(new HideUnhideViewAnimationMessage(true));
                return;
            }

            if (Search.Length < 3)
            {
                if (Suggestions != null && Suggestions.Count > 0)
                    Suggestions.Clear();

                HasAnySuggestions = false;
                return;
            }

            // Add API Stuff here.
            Suggestions = new ObservableCollection<Suggestion>()
            {
                new Suggestion() { Content = "French Cuisine Restaurants", TapCommand = SuggestionTapCommand, RemoveCommand = SuggestionRemoveCommand }
            };

            HasAnySuggestions = Suggestions.Count > 0;
        }

        private void OnSuggestionTapped(Suggestion suggestion)
        {
            Search = suggestion.Content;

            if (Suggestions != null && Suggestions.Count > 0)
                Suggestions.Clear();

            HasAnySuggestions = false;
            SearchRestaurantAsync();
        }

        private void OnSuggestionRemoved(Suggestion suggestion)
        {
            Suggestions.Remove(suggestion);
            HasAnySuggestions = Suggestions.Count > 0;
        }

        private async void SearchRestaurantAsync()
        {
            _messagingService.Send<HideUnhideViewAnimationMessage>(new HideUnhideViewAnimationMessage(true));
            // Add API stuff here.
            await Task.Delay(5000);
            SearchedRestaurants = new ObservableCollection<SearchedRestaurant>()
            {
                new SearchedRestaurant(){ Image = "Restaurant1", Name = "The French Bros", IsJit = true, Rate = 4, AwayInMins = 5, DistanceInMiles = 1, Tags = new List<string>() { "Eat In", "Delivery" } },
                new SearchedRestaurant(){ Image = "Food1", Name = "French Hot Dogs", IsJit = true, Rate = 4.5, AwayInMins = 14, DistanceInMiles = 3, Tags = new List<string>() { "Eat In", "Inside" } },
                new SearchedRestaurant(){ Image = "Restaurant2", Name = "Super French burgers", IsJit = true, Rate = 4.6, AwayInMins = 19, DistanceInMiles = 5, Tags = new List<string>() { "Eat In", "Outside" } },
                new SearchedRestaurant(){ Image = "Food2", Name = "Super French burgers", IsJit = false, Rate = 4.2, AwayInMins = 19, DistanceInMiles = 4, Tags = new List<string>() { "Inside", "Delivery" } },
                new SearchedRestaurant(){ Image = "Restaurant3", Name = "Le Petit Sommelier", IsJit = false, Rate = 4, AwayInMins = 5, DistanceInMiles = 1, Tags = new List<string>() { "Outside", "Delivery" } },
                new SearchedRestaurant(){ Image = "Food3", Name = "Chez Casimir", IsJit = true, Rate = 4, AwayInMins = 14, DistanceInMiles = 1, Tags = new List<string>() { "Eat In", "Inside" } },
                new SearchedRestaurant(){ Image = "Restaurant2", Name = "Relais d’Entrecôte", IsJit = false, Rate = 4, AwayInMins = 19, DistanceInMiles = 3, Tags = new List<string>() { "Eat In", "Outside" } },
                new SearchedRestaurant(){ Image = "Food1", Name = "The French Bros", IsJit = false, Rate = 4, AwayInMins = 21, DistanceInMiles = 7, Tags = new List<string>() { "Eat In", "Delivery", "Inside", "Outside" } },
            };

            FilteredRestaurants = SearchedRestaurants;
            _messagingService.Send<HideUnhideViewAnimationMessage>(new HideUnhideViewAnimationMessage(false));
        }
    }
}
