using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductCatagoryManagerController : Controller
    {
        
        ProductCatagoryRepository context;

        public ProductCatagoryManagerController()
        {
            context = new ProductCatagoryRepository();
        }
        public ActionResult Index()
        {
            List<ProductCatagory> productCatagories = context.Collection().ToList();
            return View(productCatagories);
        }
        public ActionResult Create()
        {
            ProductCatagory productCatagory = new ProductCatagory();
            return View(productCatagory);
        }
        [HttpPost]
        public ActionResult Create(ProductCatagory productCatagory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCatagory);
            }
            else
            {
                context.Insert(productCatagory);
                context.Commit();
                return RedirectToAction("Index");
            }

        }
        public ActionResult Edit(string Id)
        {
            ProductCatagory productCatagory = context.Find(Id);
            if (productCatagory == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCatagory);
            }
        }
        [HttpPost]
        public ActionResult Edit(ProductCatagory productCatagory, string Id)
        {
            ProductCatagory productToEdit = context.Find(Id);
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productCatagory);
                }
                productToEdit.Catagory = productCatagory.Catagory;
               
                context.Commit();
                return RedirectToAction("Index");
            }
        }
        public ActionResult Delete(string Id)
        {
            ProductCatagory productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            ProductCatagory productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}