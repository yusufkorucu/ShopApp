using Microsoft.EntityFrameworkCore;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppApp.DataAccess.Concrete.EfCore
{
    public class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();

            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if (context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(Categories);

                }
                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategory);
                }
                context.SaveChanges();
            }
        }
        private static Category[] Categories =
        {
            new Category(){Name="Telefon"},
            new Category(){Name="Bilgisayar"},
            new Category(){Name="Elektronik"}
        };
        private static Product[] Products =
       {
            new Product(){Name="Samsung S4",Price=1500,ImageUrl="1.jpg",Description="<p>Samsung markasının s sınıfı telefonları</p>"},
            new Product(){Name="Samsung S5",Price=2500,ImageUrl="2.jpg",Description="<p>Samsung markasının s sınıfı telefonları</p>"},
            new Product(){Name="Samsung S6",Price=3500,ImageUrl="3.jpg",Description="<p>Samsung markasının s sınıfı telefonları</p>"},
            new Product(){Name="Samsung S7",Price=4500,ImageUrl="4.jpg",Description="<p>Samsung markasının s sınıfı telefonları</p>"},
            new Product(){Name="Samsung S8",Price=5500,ImageUrl="5.jpg",Description="<p>Samsung markasının s sınıfı telefonları</p>"},
            new Product(){Name="Samsung S9",Price=6500,ImageUrl="6.jpg",Description="<p>Samsung markasının s sınıfı telefonları</p>"},
            new Product(){Name="Samsung S10",Price=7500,ImageUrl="7.jpg",Description="<p>Samsung markasının s sınıfı telefonları</p>"},
        };
        private static ProductCategory[] ProductCategory =
        {
            new ProductCategory(){Product=Products[0],Category=Categories[0]},
            new ProductCategory(){Product=Products[0],Category=Categories[2]},
            new ProductCategory(){Product=Products[1],Category=Categories[0]},
            new ProductCategory(){Product=Products[2],Category=Categories[1]},
            new ProductCategory(){Product=Products[3],Category=Categories[2]},
            new ProductCategory(){Product=Products[3],Category=Categories[0]},
            new ProductCategory(){Product=Products[4],Category=Categories[1]}
        };
    }

}

