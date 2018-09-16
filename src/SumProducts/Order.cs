using System;
using System.ComponentModel.DataAnnotations;

namespace SumProducts {
    class Order {
        [Key]
        public int Id { set; get; }
        public int Total { set; get; }
        public DateTime LastUpdate { set; get; }
        public int OrderStatus { set; get; }
    }
}
