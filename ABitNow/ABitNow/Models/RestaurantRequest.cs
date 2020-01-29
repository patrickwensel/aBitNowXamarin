using System;
using System.Collections.Generic;
using System.Text;

namespace ABitNow.Models
{
    public class RestaurantRequest
    {
        public Location location { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
}
