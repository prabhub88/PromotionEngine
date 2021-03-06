using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PromotionEngine.Models;
using PromotionEngine;
using PromotionEngine.Interface;

namespace PromotionEngineTest
{
    [TestClass]
    public class EngineTest
    {
        private List<Cart> products = new List<Cart>
        {
            new Cart{ sku=new SKU{ Id='A',Price= 50M }, Quanity=5 },
            new Cart{ sku=new SKU{ Id='B',Price= 30M }, Quanity=1 },
            new Cart{ sku=new SKU{ Id='C',Price= 20M }, Quanity=8 }
        };

        private List<Promotion> promotins = new List<Promotion> {
                 new Promotion{ SKUs= new List<PromotinSkus>{ new PromotinSkus { Id = 'A', Count = 3 } },  DiscountPrice=130M },
        };

        [TestMethod]
        public void Return_zero_Product_Is_Null()
        {
            Engine engine = new Engine(null, promotins);
            Assert.AreEqual(0, engine.CalculateCartTotalWithBestPromotion());
        }

        [TestMethod]
        public void Return_ActualTotal_Promotions_Is_Null()
        {
            products = new List<Cart>
        {
            new Cart{ sku=new SKU{ Id='A',Price= 50M }, Quanity=1 },
            new Cart{ sku=new SKU{ Id='B',Price= 30M }, Quanity=1 },
            new Cart{ sku=new SKU{ Id='C',Price= 20M }, Quanity=1 }
        };

            IEngine engine = new Engine(products, null);
            Assert.AreEqual(100, engine.CalculateCartTotalWithBestPromotion());
        }

        [TestMethod]
        public void Verify_A_SKU_Promotion_total()
        {

            products = new List<Cart>
        {
            new Cart{ sku=new SKU{ Id='A',Price= 50M }, Quanity=5 },
            new Cart{ sku=new SKU{ Id='B',Price= 30M }, Quanity=1 },
            new Cart{ sku=new SKU{ Id='C',Price= 20M }, Quanity=8 }
        };

            promotins = new List<Promotion> {
                 new Promotion{ SKUs= new List<PromotinSkus>{ new PromotinSkus { Id = 'A', Count = 3 } },  DiscountPrice=130M },
        };

            IEngine engine = new Engine(products, promotins);
            Assert.AreEqual(420, engine.CalculateCartTotalWithBestPromotion());
        }

        [TestMethod]
        public void Verify_B_SKU_Promotion_total()
        {

            products = new List<Cart>
        {
            new Cart{ sku=new SKU{ Id='A',Price= 50M }, Quanity=5 },
            new Cart{ sku=new SKU{ Id='B',Price= 30M }, Quanity=7 },
            new Cart{ sku=new SKU{ Id='C',Price= 20M }, Quanity=8 }
        };


            promotins = new List<Promotion> {
                new Promotion{ SKUs= new List<PromotinSkus>{ new PromotinSkus { Id = 'B', Count = 2 } },  DiscountPrice=45M },
            };

            IEngine engine = new Engine(products, promotins);
            Assert.AreEqual(575, engine.CalculateCartTotalWithBestPromotion());
        }

        [TestMethod]
        public void Verify_TwoCombo_CD_SKU_Promotion_total()
        {

            products = new List<Cart>
        {
            new Cart{ sku=new SKU{ Id='A',Price= 50M }, Quanity=5 },
            new Cart{ sku=new SKU{ Id='B',Price= 30M }, Quanity=7 },
            new Cart{ sku=new SKU{ Id='C',Price= 20M }, Quanity=4 },
            new Cart{ sku=new SKU{ Id='D',Price= 15M }, Quanity=3 }
        };

            promotins = new List<Promotion> {
                new Promotion{ SKUs= new List<PromotinSkus>{ new PromotinSkus { Id = 'C', Count = 2 }

                ,new PromotinSkus{  Count=1, Id='D'} },  DiscountPrice=30M }
            };

            IEngine engine = new Engine(products, promotins);
            Assert.AreEqual(535, engine.CalculateCartTotalWithBestPromotion());
        }

        [TestMethod]
        public void Verify_ThreeCombo__ACD_SKU_Promo_total()
        {

            products = new List<Cart>
        {
            new Cart{ sku=new SKU{ Id='A',Price= 50M }, Quanity=5 },
            new Cart{ sku=new SKU{ Id='B',Price= 30M }, Quanity=7 },
            new Cart{ sku=new SKU{ Id='C',Price= 20M }, Quanity=4 },
            new Cart{ sku=new SKU{ Id='D',Price= 15M }, Quanity=3 }
        };


            promotins = new List<Promotion> {

                new Promotion{ SKUs= new List<PromotinSkus>{
                     new PromotinSkus { Id = 'A', Count = 3 },
                    new PromotinSkus { Id = 'C', Count = 2 },
                  new PromotinSkus{  Count=1, Id='D'} },  DiscountPrice=30M }
            };

            IEngine engine = new Engine(products, promotins);
            Assert.AreEqual(410, engine.CalculateCartTotalWithBestPromotion());
        }

        [TestMethod]
        public void Verify_A_SKU_Promotion_total_with_nonEligibleCart()
        {

            products = new List<Cart>
        {
            new Cart{ sku=new SKU{ Id='A',Price= 50M }, Quanity=5 },
            new Cart{ sku=new SKU{ Id='B',Price= 30M }, Quanity=2 },
            new Cart{ sku=new SKU{ Id='C',Price= 20M }, Quanity=2 },
            new Cart{ sku=new SKU{ Id='D',Price= 15M }, Quanity=2 }
        };


            promotins = new List<Promotion> {

                new Promotion{ SKUs= new List<PromotinSkus>{
                     new PromotinSkus { Id = 'A', Count = 7 }
            } } };

            IEngine engine = new Engine(products, promotins);
            Assert.AreEqual(380, engine.CalculateCartTotalWithBestPromotion());
        }

        [TestMethod]
        public void Verify_Multiple_SKU_Promotion_total()
        {

            products = new List<Cart>
        {
            new Cart{ sku=new SKU{ Id='A',Price= 50M }, Quanity=5 },
            new Cart{ sku=new SKU{ Id='B',Price= 30M }, Quanity=7 },
            new Cart{ sku=new SKU{ Id='C',Price= 20M }, Quanity=4 },
            new Cart{ sku=new SKU{ Id='D',Price= 15M }, Quanity=3 }
        };


            promotins = new List<Promotion> {

                new Promotion{ SKUs= new List<PromotinSkus>{
                     new PromotinSkus { Id = 'A', Count = 3 }},  DiscountPrice=130M },

                new Promotion{ SKUs= new List<PromotinSkus>{
                     new PromotinSkus { Id = 'B', Count = 2 }},  DiscountPrice=45M }
            };

            IEngine engine = new Engine(products, promotins);
            Assert.AreEqual(520, engine.CalculateCartTotalWithMultiplePromotions());
        }

        [TestMethod]
        public void Verify_BestPromotion_In_Multiple_SKU_Promotion_total()
        {

            products = new List<Cart>
        {
            new Cart{ sku=new SKU{ Id='A',Price= 50M }, Quanity=5 },
            new Cart{ sku=new SKU{ Id='B',Price= 30M }, Quanity=7 },
            new Cart{ sku=new SKU{ Id='C',Price= 20M }, Quanity=4 },
            new Cart{ sku=new SKU{ Id='D',Price= 15M }, Quanity=3 }
        };

            

            promotins = new List<Promotion> {

                new Promotion{ SKUs= new List<PromotinSkus>{
                     new PromotinSkus { Id = 'A', Count = 3 }},  DiscountPrice=130M },

                new Promotion{ SKUs= new List<PromotinSkus>{
                     new PromotinSkus { Id = 'B', Count = 2 }},  DiscountPrice=45M }
            };

            IEngine engine = new Engine(products, promotins);
            Assert.AreEqual(540, engine.CalculateCartTotalWithBestPromotion());
        }
    }

}
