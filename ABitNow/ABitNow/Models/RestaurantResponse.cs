using System;
using System.Collections.Generic;
using System.Text;

namespace ABitNow.Models
{
    public class RestaurantResponse
    {
        public int restaurantID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string mainPhone { get; set; }
        public string mainFax { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string stateProvince { get; set; }
        public string postalCode { get; set; }
        public string image { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public double distance { get; set; }
        public double rating { get; set; }
    }
}
