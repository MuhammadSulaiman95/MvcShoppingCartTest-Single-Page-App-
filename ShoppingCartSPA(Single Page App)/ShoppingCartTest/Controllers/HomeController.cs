using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ShoppingCartTest.Controllers
{
    public class HomeController : ApiController
    {

        public class Item
        {
            public int Count { get; set; }
            public Product Product { get; set; }
        }

        public class Cart
        {
            public List<Item> Items { get; set; }
        }

        public class ShoppingCartApiController : ApiController
        {
           

            [Route("api/CountItems")]
            [HttpGet]
            public int CountItems()
            {
                Cart sc = HttpContext.Current.Session["cart"] as Cart;
                if (sc != null)
                {
                    return sc.Items.Count;
                }
                else
                {
                    return 0;
                }
            }

            [Route("api/GetProducts")]
            [HttpGet]
            public List<Product> GetProducts()
            {
                var db = new ShoppingCartTestEntities();
                db.Configuration.ProxyCreationEnabled = false;

                return db.Products.ToList();
            }


            [Route("api/GetCart")]
            [HttpGet]
            public Cart GetCart()
            {
                Console.WriteLine("abc");
                return HttpContext.Current.Session["cart"] as Cart;
            }

            [Route("api/AddToCart/{Id}")]
            [HttpGet]
            public int AddToCart(int id)
            {

                Cart cart = null;

                if (HttpContext.Current.Session["cart"] == null)
                    cart = new Cart();
                else
                    cart = HttpContext.Current.Session["cart"] as Cart;

                if (cart.Items == null)
                    cart.Items = new List<Item>();

                var db = new ShoppingCartTestEntities();
                db.Configuration.ProxyCreationEnabled = false;

                var pro = db.Products.Find(id);

                Item item = new Item();
                item.Count = 1;
                item.Product = pro;

                var product = cart.Items.Find(i => i.Product.Id == id);
                if (product != null)
                    product.Count++;
                else
                    cart.Items.Add(item);

                HttpContext.Current.Session["cart"] = cart;

                return CountItems();
            }

        }

    }
}
