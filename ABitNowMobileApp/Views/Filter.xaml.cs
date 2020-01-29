using System;
using System.Collections.Generic;
using ABitNowMobileApp.ViewModels;
using Xamarin.Forms;

namespace ABitNowMobileApp.Views
{
    public partial class Filter : ContentPage
    {
        private readonly FilterViewModel _filterViewModel;

        public Filter(FilterViewModel filterViewModel)
        {
            InitializeComponent();
            _filterViewModel = filterViewModel;
            BindingContext = _filterViewModel;
        }

        private void SfRSDistances_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals(nameof(SfRSDistances.RangeStart)) || e.PropertyName.Equals(nameof(SfRSDistances.RangeEnd)))
            {
                AdjustRangeSliderValueLabels(SfRSDistances.RangeStart, SfRSDistances.RangeEnd);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _filterViewModel.OnViewAppearAsync();
            AdjustRangeSliderValueLabels(SfRSDistances.RangeStart, SfRSDistances.RangeEnd);
        }

        private void AdjustRangeSliderValueLabels(double start, double end)
        {
            double startValuePosition = GetXPostionAsPerValue(start) - (LblRangeStart.Width / 2);
            double endValuePosition = GetXPostionAsPerValue(end) - (LblRangeEnd.Width / 2);
            double totalWidth = SfRSDistances.Width;

            if (startValuePosition < 0)
                startValuePosition = 0;

            if ((startValuePosition + LblRangeStart.Width) > totalWidth)
                startValuePosition = totalWidth - LblRangeStart.Width;

            if (endValuePosition < 0)
                endValuePosition = 0;

            if ((endValuePosition + LblRangeEnd.Width) > totalWidth)
                endValuePosition = totalWidth - LblRangeEnd.Width;

            LblRangeStart.TranslationX = startValuePosition;
            LblRangeEnd.TranslationX = endValuePosition;
        }

        private double GetSingleStepValue()
        {
           return SfRSDistances.Width / SfRSDistances.Maximum;
        }

        private double GetXPostionAsPerValue(double value)
        {
            return value * GetSingleStepValue();
        }
    }
}
