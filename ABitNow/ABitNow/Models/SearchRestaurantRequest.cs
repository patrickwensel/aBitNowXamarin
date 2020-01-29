using System;
using System.Collections.Generic;
using System.Text;

namespace ABitNow.Models
{
    public class SearchRestaurantRequest
    {
        public Location location { get; set; }
        public RestaurantOption restaurantOption { get; set; }
        public string searchName { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
}
