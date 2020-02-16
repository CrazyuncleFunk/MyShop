using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService : IBasketService
    {
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext)
        {
            this.basketContext = BasketContext;
            this.productContext = ProductContext;
        }
        private Basket GetBasket(HttpContextBase httpContext,bool CreateIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);
            Basket basket = new Basket();

            if(cookie != null)
            {
                string basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = basketContext.Find(basketId);
                }
                else
                {
                    if (CreateIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (CreateIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }
            return basket;
        }
        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            basketContext.Insert(basket);
            basketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);
            return basket;
        }
        public void AddToBasket(HttpContextBase httpContext,string productId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem basketItem = basket.BasketItems.FirstOrDefault(x => x.ProductId == productId);
            if(basketItem == null)
            {
                basketItem = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1

                };
                basket.BasketItems.Add(basketItem);
            }
            else
            {
                basketItem.Quantity++;
            }
            basketContext.Commit();
        }
        public void RemoveFromBasket(HttpContextBase httpContext,string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem basketItem = basket.BasketItems.FirstOrDefault(x => x.Id == itemId);
            if(basketItem != null)
            {
                basket.BasketItems.Remove(basketItem);
                basketContext.Commit();
            }
        }
        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            if(basket != null)
            {
                var result = 
                    (from b in basket.BasketItems 
                     join p in productContext.Collection() on b.ProductId equals p.Id
                     select new BasketItemViewModel() { 
                        Id = b.Id,
                        Quantity = b.Quantity,
                        ProductName = p.Name,
                        Price = p.Price,
                        Image = p.Image
                     }).ToList();
                return result;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }
        }
        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);
            if(basket != null)
            {
                int? basketcount = (from item in basket.BasketItems
                                    select item.Quantity).Sum();
                decimal? basketTotal = (from item in basket.BasketItems
                                        join p in productContext.Collection() on item.ProductId equals p.Id
                                        select item.Quantity * p.Price).Sum();
                model.BasketCount = basketcount ?? 0;
                model.BasketTotal = basketTotal ?? decimal.Zero;
                return model;
            }
            else
            {
                return model;
            }
        }
    }
}
