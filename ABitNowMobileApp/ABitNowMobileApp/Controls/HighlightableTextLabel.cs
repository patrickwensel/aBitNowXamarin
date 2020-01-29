using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ABitNowMobileApp.Controls
{
    public class HighlightableTextLabel : Label
    {
        public static readonly BindableProperty ContentTextProperty = BindableProperty.Create(nameof(ContentText), typeof(string), typeof(HighlightableTextLabel), null, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
         {
             ((HighlightableTextLabel)bindable).SetText();
         });

        public string ContentText
        {
            get { return (string)GetValue(ContentTextProperty); }
            set { SetValue(ContentTextProperty, value); }
        }

        public static readonly BindableProperty HighlightTextProperty = BindableProperty.Create(nameof(HighlightText), typeof(string), typeof(HighlightableTextLabel), null, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
         {
             ((HighlightableTextLabel)bindable).SetText();
         });

        public string HighlightText
        {
            get { return (string)GetValue(HighlightTextProperty); }
            set { SetValue(HighlightTextProperty, value); }
        }

        public static readonly BindableProperty HighlightTextFontFamilyProperty = BindableProperty.Create(nameof(HighlightTextFontFamily), typeof(string), typeof(HighlightableTextLabel), null, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
         {
             ((HighlightableTextLabel)bindable).SetText();
         });

        public string HighlightTextFontFamily
        {
            get { return (string)GetValue(HighlightTextFontFamilyProperty); }
            set { SetValue(HighlightTextFontFamilyProperty, value); }
        }

        public static readonly BindableProperty HighlightTextColorProperty = BindableProperty.Create(nameof(HighlighTextColor), typeof(Color), typeof(HighlightableTextLabel), Color.Transparent, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
         {
             ((HighlightableTextLabel)bindable).SetText();
         });

        public Color HighlighTextColor
        {
            get { return (Color)GetValue(HighlightTextColorProperty); }
            set { SetValue(HighlightTextColorProperty, value); }
        }

        public static readonly BindableProperty HighlightTextFontSizeProperty = BindableProperty.Create(nameof(HighlightTextFontSize), typeof(double), typeof(HighlightableTextLabel), default(double), BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((HighlightableTextLabel)bindable).SetText();
        });

        public double HighlightTextFontSize
        {
            get { return (double)GetValue(HighlightTextFontSizeProperty); }
            set { SetValue(HighlightTextFontSizeProperty, value); }
        }

        private void SetText()
        {
            if (string.IsNullOrEmpty(ContentText) || string.IsNullOrEmpty(HighlightText))
                return;

            var splittedText = GetSplittedString();

            if (splittedText == null || splittedText.Count == 0)
                return;

            FormattedString formattedString = new FormattedString();
            foreach (string text in splittedText)
            {
                Color textColor = TextColor;
                if (text.Equals(HighlightText, StringComparison.OrdinalIgnoreCase) && HighlighTextColor != Color.Transparent)
                    textColor = HighlighTextColor;

                string fontFamily = FontFamily;
                if (text.Equals(HighlightText, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(HighlightTextFontFamily))
                    fontFamily = HighlightTextFontFamily;

                double fontSize = FontSize;
                if (text.Equals(HighlightText, StringComparison.OrdinalIgnoreCase) && HighlightTextFontSize != default(double))
                    fontSize = HighlightTextFontSize;

                Span span = new Span()
                {
                    Text = text,
                    TextColor = textColor,
                    FontFamily = fontFamily,
                    FontSize = fontSize,
                };

                formattedString.Spans.Add(span);
            }

            if (formattedString.Spans.Count > 0)
                FormattedText = formattedString;
        }

        private List<string> GetSplittedString()
        {
            if(!ContentText.ToLower().Contains(HighlightText.ToLower()))
                return new List<string>() { ContentText };

            int startIndex = ContentText.IndexOf(HighlightText, StringComparison.OrdinalIgnoreCase);
            List<string> splittedTexts = new List<string>();
            string matchedText = ContentText.Substring(startIndex, HighlightText.Length);
            string startText = startIndex > 0 ? ContentText.Substring(0, startIndex) : string.Empty;
            string lastText = ContentText.Substring(startIndex + HighlightText.Length);

            if (!string.IsNullOrEmpty(startText))
                splittedTexts.Add(startText);

            if (!string.IsNullOrEmpty(matchedText))
                splittedTexts.Add(matchedText);

            if (!string.IsNullOrEmpty(lastText))
                splittedTexts.Add(lastText);

            return splittedTexts;
        }
    }
}
