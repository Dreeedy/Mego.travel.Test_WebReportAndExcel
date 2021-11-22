using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mego.travel.Test_WebReport_Excel.Models
{
    /// <summary>
    /// Контекcт данных для заказов
    /// </summary>
    public class OrderContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
            // По умолчанию у нас база данных отсутствуют.
            // Поэтому в конструктор MobileContext определен вызов Database.EnsureCreated(),
            // который при отсутствии базы данных автоматически создает ее.
            // Если база данных уже есть, то ничего не происходит.
            Database.EnsureCreated();
        }
    }
}
