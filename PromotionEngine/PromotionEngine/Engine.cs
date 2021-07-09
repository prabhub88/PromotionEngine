using System.Collections.Generic;
using PromotionEngine.Models;
using System.Linq;
using System;

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
            var findOfferedTotal = 0m;

            if (_skus == null || _skus?.Count == 0)
                return 0M;

            if (_promotins == null || _promotins?.Count == 0)
                return _skus.Sum(s => s.Quanity * s.sku.Price);

            else
            {
                foreach (var promo in _promotins.Select(s => s.SKUs))
                {
                    var tmp = GetPromotionEligibleSKU(promo);
                    if(tmp.Count > 0)
                        findOfferedTotal = GetTotalOfPromotionSKU(promo, tmp);

                    var nonpromotionsku = GetNonPromotionSKUs(tmp);
                    findOfferedTotal += GetTotalOfNonPromotionSKUs(nonpromotionsku);

                }
            }
            return findOfferedTotal;
        }

        private decimal GetTotalOfPromotionSKU(List<PromotinSkus> promotinSkus, List<Cart> carts)
        {
            decimal totalAmount = 0M;
            var intermediate =

                 promotinSkus.Select(p => new
                 {
                     id = p.Id,

                     promoteSKUQuality = p.Count,

                     actualSKUQuality = carts.FirstOrDefault(s => p.Id == s.sku.Id).Quanity,

                     SkuPrice = carts.FirstOrDefault(s => p.Id == s.sku.Id).sku.Price,

                     quotient = carts.
                 Where(s => p.Id == s.sku.Id).
                 Sum(s => s.Quanity / p.Count),

                     reminder = carts.
                 Where(s => p.Id == s.sku.Id).
                 Sum(s => s.Quanity % p.Count)
                 });

            var MinQuotient = intermediate.Min(m => m.quotient);

            totalAmount += GetTotalAmount(MinQuotient, GetPromotionPrice(promotinSkus.ElementAt(0).Id));

            foreach (var itm in intermediate)
            {
                var reminder = (itm.quotient - MinQuotient) * itm.promoteSKUQuality + itm.reminder;
                totalAmount += GetTotalAmount(reminder, itm.SkuPrice);
            }
            return totalAmount;
        }

        private decimal GetPromotionPrice(char sku)
        {
            var tmp = _promotins.Select(p => new {
                PromotinPrice = p.DiscountPrice,
                SkuList = p.SKUs.Where(s => s.Id == sku)
            });

            return tmp.Where(t => t.SkuList.Count() > 0).Min(d => d.PromotinPrice);
        }
        private decimal GetTotalAmount(int qty, decimal price)
        {
            return qty * price;
        }

        private List<Cart> GetPromotionEligibleSKU(List<PromotinSkus> promotinSku)
        {
            return promotinSku.SelectMany(p => _skus.FindAll(s => p.Id == s.sku.Id && p.Count <= s.Quanity)).ToList();
        }
        private List<Cart> GetNonPromotionSKUs(List<Cart> carts)
        {
            return _skus.Except(carts).ToList();
        }

        private decimal GetTotalOfNonPromotionSKUs(List<Cart> carts)
        {
            return carts.Sum(s => s.sku.Price * s.Quanity);
        }
    }
}
