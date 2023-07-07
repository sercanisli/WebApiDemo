﻿using System;
using System.Collections.Generic;
using System.Linq;
using WebApiDemo.Entities;
using WebApiDemo.Models;

namespace WebApiDemo.DataAccess
{
    public class EfProductDal : EfEntityRepositoryBase<Product, NorthwindContext>, IProductDal
    {
        public List<ProductModel> GetProductsWithDetails()
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var result = from p in context.Products
                    join c in context.Categories on p.CategoryId equals c.CategoryId
                    select new ProductModel
                    {
                        ProductId = p.ProductId, 
                        ProductName = p.ProductName, 
                        CategoryName = c.CategoryName,
                        UnitPrice = p.UnitPrice
                    };
                return result.ToList();
            }
        }
    }
}
