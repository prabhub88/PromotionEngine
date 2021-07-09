using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PromotionEngine.Models;
using PromotionEngine;

namespace PromotionEngineTest
{
    [TestClass]
    public class UnitTest1
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
            Assert.AreEqual(0, engine.CalculateTotalOrderValue());
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

            Engine engine = new Engine(products, null);
            Assert.AreEqual(100, engine.CalculateTotalOrderValue());
        }
    }
}
