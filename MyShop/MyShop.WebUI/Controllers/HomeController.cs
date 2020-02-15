using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCatagory> productCatagories;

        public HomeController(IRepository<Product> productContext, IRepository<ProductCatagory> productCatagoryContext)
        {
            context = productContext;
            productCatagories = productCatagoryContext;
        }
        public ActionResult Index( string Catagory=null)
        {
            List<Product> products;
            List<ProductCatagory> catagories = productCatagories.Collection().ToList();
            if(Catagory == null)
            {
                products = context.Collection().ToList();
            }
            else
            {
                products = context.Collection().Where(x => x.Category == Catagory).ToList();
            }
            ProductListViewModel model = new ProductListViewModel();
            model.product = products;
            model.productCatagories = catagories;

            return View(model);
        }
        public ActionResult Details(string Id)
        {
            Product product = context.Find(Id);
            if(product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);

            }
            
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}