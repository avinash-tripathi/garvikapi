using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model.Food.Restaurant
{
    public class RestaurantMenu
    {
        public string restaurantcode { get; set; }
        public string menucode { get; set; }
        public string menuname { get; set; }
        public string menudescription { get; set; }
        public float price { get; set; }
        public string menuurl { get; set; }
        public string createdby { get; set; }
    }
}
