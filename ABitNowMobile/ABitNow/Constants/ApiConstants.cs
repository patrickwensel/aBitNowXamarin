namespace ABitNow.Constants
{
    public class ApiConstants
    {
        public const string BaseApiUrl = "https://abitnowservices.azure-api.net/";

        public const string SubscriptionKey = "3b4bf07217a34113bc8fa24cf4e984c7";
        
        public const string AuthenticateEndpoint = "/api/user/authenticate";

        public const string ExternalLoginProviderEndpoint = "api/user/ExternalLogins";
        public const string ExternalLoginProviderQuery = "returnUrl=/&generateState=true";

        public const string ExternalLoginConfirmation = "/api/user/ExternalLoginConfirmation";

        public const string CurrentUserEndpoint = "/api/user/Get";

    }
}
