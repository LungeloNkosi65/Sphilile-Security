using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SphileSecurity.Models
{
    public class OrderDetailConFirm
    {
        public Order order { get; set; }
        public OrderAddress address { get; set; }
        public List<OrderItem> items { get; set; }
     
    }
}
