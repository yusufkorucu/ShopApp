using ShopApp.Entities;
using ShoppApp.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppApp.DataAccess.Concrete.EfCore
{
   public class EfCoreOrderDal:EfCoreGenericRepository<Order,ShopContext>,IOrderDal
    {

    }
}
