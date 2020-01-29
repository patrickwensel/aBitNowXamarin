using ABitNow.Constants;
using ABitNow.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ABitNow.Models.UI
{
    public class SocialButton
    {
        public ExternalLoginProvider ExternalLoginProvider { get; set; }

        public ICommand ExternalLoginProviderTappedCommand { get; set; }

        public SocialAppIntegrationType SocialAppIntegrationType => GetSocialAppType();

        public string SocialAppIcon => GetSocialAppIcon();

        public Color ButtonColor => GetSocialAppButtonColor();

        public SocialButton(ExternalLoginProvider externalLoginProvider, ICommand externalLoginProviderTappedCommand)
        {
            ExternalLoginProvider = externalLoginProvider;
            ExternalLoginProviderTappedCommand = externalLoginProviderTappedCommand;
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
            switch (SocialAppIntegrationType)
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
