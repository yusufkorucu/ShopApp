using Microsoft.EntityFrameworkCore;
using ShopApp.Entities;
using ShoppApp.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ShoppApp.DataAccess.Concrete.EfCore
{
    public class EfCoreCategoryDal : EfCoreGenericRepository<Category, ShopContext>,
        ICategoryDal
    {
        public void DeleteFromCategory(int categoryId, int productId)
        {
            using (var context=new ShopContext())
            {
                var cmd = @"delete from ProductCategory where ProductId=@p0 And CategoryId=@p1";
                context.Database.ExecuteSqlRaw(cmd, productId, categoryId);

            }
        }

        public Category GetByIdWithProducts(int id)
        {
            using (var contex = new ShopContext())
            {
                return contex.Categories.Where(x => x.Id == id)
                    .Include(x => x.ProductCategories)
                    .ThenInclude(x => x.Product)
                    .FirstOrDefault();

            }
        }
    }
}
