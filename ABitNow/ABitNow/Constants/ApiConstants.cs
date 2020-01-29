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


        public const string RegisterUserEndpoint = "/api/user/registerUser";

        // Menu
        public const string MenuByRestaurantEndpoint = "/api/Menu/getmenubyrestaurant";    // pass {id}

        // MenuGroup
        public const string MenuGroupByMenuEndpoint = "/api/MenuGroup/getmenugroupbymenu";    // pass {id}

        // MenuItem
        public const string MenuItemEndpoint = "/api/MenuItem";    // pass {id}
        public const string MenuItemByRestaurantEndpoint = "/api/MenuItem/getmenuitembyrestaurant";    // pass {restaurantId}/{itemName}
        public const string MenuItemByMenuGroupEndpoint = "/api/MenuItem/getmenuitembymenugroup";    // pass {id}

        // MenuItemAddOn
        public const string MenuItemAddOnByMenuItemAndRestaurantEndpoint = "/api/MenuItemAddOn/GetMenuItemAddOnsByMenuItemAndRestaurantID";    // pass {menuItemID}/{restaurantID}

        // MenuItemOption
        public const string MenuItemOptionByMenuItemAndRestaurantEndpoint = "/api/MenuItemOption/GetMenuItemOptionWithGroupByMenuItemAndRestaurant";    // pass {menuItemID}/{restaurantID}

        // Restaurant
        public const string FavoriteRestaurantsEndpoint = "/api/Restaurant/FavoriteRestaurants";
        public const string HighRatingRestaurantsEndpoint = "/api/Restaurant/RecentHighRatingRestaurants";
        public const string SearchRestaurantsEndpoint = "/api/Restaurant/SearchRestaurants";
        public const string GetRestaurant = "/api/Restaurant";  // pass {id}

        // RestaurantOption
        public const string RestaurantOptionsEndpoint = "/api/RestaurantOption/SearchAllRestaurantOptions";
        public const string RestaurantOptionByIdEndpoint = "/api/RestaurantOption/SearchRestaurantOption";   // pass {id}


    }
}
