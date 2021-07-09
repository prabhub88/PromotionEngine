using System.Collections.Generic;
using PromotionEngine.Models;
using System.Linq;

namespace PromotionEngine
{
    public class Engine
    {
        List<Cart> _skus;
        List<Promotion> _promotins;
        public Engine(List<Cart> skus, List<Promotion> promotins)
        {
            _skus = skus;
            _promotins = promotins;
        }

        public decimal CalculateTotalOrderValue()
        {
            if (_skus == null || _skus?.Count == 0)
                return 0M;

            if (_promotins == null || _promotins?.Count == 0)
                return _skus.Sum(s => s.Quanity * s.sku.Price);

            else
                return 0M;
        }
    }
}
