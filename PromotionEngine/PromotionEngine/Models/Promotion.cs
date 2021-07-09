using System.Collections.Generic;

namespace PromotionEngine.Models
{
    public class Promotion
    {
        public List<PromotinSkus> SKUs { get; set; }

        public decimal DiscountPrice { get; set; }
    }
}
