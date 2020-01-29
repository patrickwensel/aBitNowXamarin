using System;
using System.Windows.Input;
using ABitNowMobileApp.Constants;
using ABitNowMobileApp.Data.Models;
using ABitNowMobileApp.Enums;
using Xamarin.Forms;

namespace ABitNowMobileApp.UiItemModels
{
    public class SocialButton
    {
        public ExternalLoginProvider ExternalLoginProvider { get; set; }

        public ICommand TapCommand { get; set; }

        public SocialAppIntegrationType SocialAppIntegrationType => GetSocialAppType();

        public string SocialAppIcon => GetSocialAppIcon();

        public Color ButtonColor => GetSocialAppButtonColor();

        public SocialButton(ExternalLoginProvider externalLoginProvider, ICommand tapCommand)
        {
            ExternalLoginProvider = externalLoginProvider;
            TapCommand = tapCommand;
        }

        private SocialAppIntegrationType GetSocialAppType()
        {
            if (ExternalLoginProvider.Name.ToLower().Equals("facebook"))
                return SocialAppIntegrationType.Facebook;
            else if (ExternalLoginProvider.Name.ToLower().Equals("google"))
                return SocialAppIntegrationType.Google;
            else if (ExternalLoginProvider.Name.ToLower().Equals("twitter"))
                return SocialAppIntegrationType.Twitter;
            else if (ExternalLoginProvider.Name.ToLower().Equals("microsoft"))
                return SocialAppIntegrationType.Microsoft;

            return SocialAppIntegrationType.Facebook;
        }

        private string GetSocialAppIcon()
        {
            switch(SocialAppIntegrationType)
            {
                case SocialAppIntegrationType.Facebook:
                    return "IcoFacebook";
                case SocialAppIntegrationType.Google:
                    return "IcoGoogle";
                case SocialAppIntegrationType.Twitter:
                    return "IcoTwitter";
                case SocialAppIntegrationType.Microsoft:
                    return "IcoMicrosoft";
                default:
                    return "IcoFacebook";
            }
        }

        private Color GetSocialAppButtonColor()
        {
            switch (SocialAppIntegrationType)
            {
                case SocialAppIntegrationType.Facebook:
                    return AppThemeConstants.GradientPurpleColor;
                case SocialAppIntegrationType.Google:
                    return AppThemeConstants.RedColor;
                case SocialAppIntegrationType.Twitter:
                    return AppThemeConstants.LightBlueColor;
                case SocialAppIntegrationType.Microsoft:
                    return AppThemeConstants.CoffeeColor;
                default:
                    return AppThemeConstants.GradientPurpleColor;
            }
        }
    }
}
