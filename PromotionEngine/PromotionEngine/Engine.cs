using System.Collections.Generic;
using System.Linq;
using PromotionEngine.Models;
using PromotionEngine.Interface;

namespace PromotionEngine
{
    public class Engine :IEngine
    {
        List<Cart> _skus;
        List<Promotion> _promotins;
        public Engine(List<Cart> skus, List<Promotion> promotins)
        {
            _skus = skus;
            _promotins = promotins;
        }

        public decimal CalculateCartTotalWithMultiplePromotions() {
            var findOfferedTotal = 0m;

            if (_skus == null || _skus?.Count == 0)
                return 0M;

            if (_promotins == null || _promotins?.Count == 0)
                return _skus.Sum(s => s.Quanity * s.sku.Price);

            else
            {
               Dictionary<decimal,List<Cart>> MultiplePromotions = new Dictionary<decimal, List<Cart>>(); 

                foreach (var promo in _promotins.Select(s => s.SKUs))
                {
                    var tmp = GetPromotionEligibleSKU(promo);
                    if (tmp.Count == 0) continue;
                    decimal quotientPromotion = 0M; decimal reminderPromotin = 0M;
                    GetTotalOfPromotionSKU(promo, tmp, out quotientPromotion, out reminderPromotin);
                    MultiplePromotions.Add(quotientPromotion+reminderPromotin,tmp);
                }

                if (MultiplePromotions?.Count > 0)
                {
                    List<Cart> carts = new List<Cart>();
                    foreach (var item in MultiplePromotions.Values.SelectMany(s => s))
                    {
                        carts.Add(item);
                    }
                    var best = MultiplePromotions.Select(s => s.Value);
                    findOfferedTotal = MultiplePromotions.Sum(b => b.Key);

                    var nonpromotionsku = GetNonPromotionSKUs(carts);
                    findOfferedTotal += GetTotalOfNonPromotionSKUs(nonpromotionsku);
                }
                else
                    return _skus.Sum(s => s.Quanity * s.sku.Price);


            }
            return findOfferedTotal;
        }
        public decimal CalculateCartTotalWithBestPromotion()
        {
            var findOfferedTotal = 0m;

            if (_skus == null || _skus?.Count == 0)
                return 0M;

            if (_promotins == null || _promotins?.Count == 0)
                return _skus.Sum(s => s.Quanity * s.sku.Price);

            else
            {
                List<Cart> bestPromotion = new List<Cart>();decimal reminderTotal = 0M;

                foreach (var promo in _promotins.Select(s => s.SKUs))
                {
                    var tmp = GetPromotionEligibleSKU(promo);
                    if (tmp.Count == 0) continue;
                    decimal quotientPromotion = 0M;decimal reminderPromotin = 0M;
                    GetTotalOfPromotionSKU(promo, tmp,out quotientPromotion,out reminderPromotin);
                    if (quotientPromotion > findOfferedTotal)
                    {
                        findOfferedTotal = quotientPromotion;
                        bestPromotion = tmp;
                        reminderTotal = quotientPromotion + reminderPromotin;
                    }

                    findOfferedTotal = 0;

                }

                    if (bestPromotion?.Count > 0)
                    {
                        findOfferedTotal += reminderTotal;
                        var nonpromotionsku = GetNonPromotionSKUs(bestPromotion);
                        findOfferedTotal += GetTotalOfNonPromotionSKUs(nonpromotionsku);
                    }
                    else
                        return _skus.Sum(s => s.Quanity * s.sku.Price);
                

            }
            return findOfferedTotal;
        }

        private void GetTotalOfPromotionSKU(List<PromotinSkus> promotinSkus, List<Cart> carts,out decimal quotientPromotion, out decimal reminderPromotion)
        {
            quotientPromotion = 0M;reminderPromotion = 0M;
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

            quotientPromotion += GetTotalAmount(MinQuotient, GetPromotionPrice(promotinSkus.FirstOrDefault().Id));

            foreach (var itm in intermediate)
            {
                var reminder = (itm.quotient - MinQuotient) * itm.promoteSKUQuality + itm.reminder;
                reminderPromotion += GetTotalAmount(reminder, itm.SkuPrice);
            }
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
