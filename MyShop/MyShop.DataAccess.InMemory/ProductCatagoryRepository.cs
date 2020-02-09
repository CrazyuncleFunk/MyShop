using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCatagoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCatagory> productCatagories = new List<ProductCatagory>();
        public ProductCatagoryRepository()
        {
            productCatagories = cache["productCatagories"] as List<ProductCatagory>;
            if (productCatagories == null)
            {
                productCatagories = new List<ProductCatagory>();
            }

        }
        public void Commit()
        {
            cache["productCatagories"] = productCatagories;
        }
        public void Insert(ProductCatagory p)
        {
            productCatagories.Add(p);
        }
        public void Update(ProductCatagory productCatagory)
        {
            ProductCatagory catagoryToUpdate = productCatagories.Find(x => x.Id == productCatagory.Id);
            if (catagoryToUpdate != null)
            {
                catagoryToUpdate = productCatagory;
            }
            else
            {
                throw new Exception("Catagory not found!");
            }
        }
        public ProductCatagory Find(string Id)
        {
            ProductCatagory catagoryToFind = productCatagories.Find(x => x.Id == Id);
            if (catagoryToFind != null)
            {
                return catagoryToFind;
            }
            else
            {
                throw new Exception("Catagory not found!");
            }

        }
        public IQueryable<ProductCatagory> Collection()
        {
            return productCatagories.AsQueryable();
        }
        public void Delete(string Id)
        {
            ProductCatagory catagoryToDelete = productCatagories.Find(x => x.Id == Id);
            if (catagoryToDelete != null)
            {
                productCatagories.Remove(catagoryToDelete);
            }
            else
            {
                throw new Exception("Catagory not found!");
            }

        }
    }
}

