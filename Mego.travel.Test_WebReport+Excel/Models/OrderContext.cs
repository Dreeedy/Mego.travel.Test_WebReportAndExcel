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
            //Database.EnsureCreated(); // Закомментирован тк вызовет конфликт с миграциями
        }

        /// <summary>
        /// Метод вызывается при выполнении миграции и заполняет таблицу начальными данными
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasData(
                new Order[]
                {
                new Order { Id=1, Price=560, Date= new DateTime(2021, 10, 1, 8, 59, 59)},// год, месяц, день, час, минуту и секунду
                new Order { Id=2, Price=550, Date= new DateTime(2021, 10, 5, 9, 59, 59)},
                new Order { Id=3, Price=700, Date= new DateTime(2021, 10, 10, 10, 59, 59)},
                new Order { Id=4, Price=710, Date= new DateTime(2021, 10, 18, 11, 59, 59)},
                new Order { Id=5, Price=330, Date= new DateTime(2021, 11, 25, 18, 59, 59)},

                new Order { Id=6, Price=1001, Date= new DateTime(2021, 11, 1, 8, 59, 59)},
                new Order { Id=7, Price=2002, Date= new DateTime(2021, 11, 2, 9, 59, 59)},
                new Order { Id=8, Price=2033, Date= new DateTime(2021, 11, 3, 10, 59, 59)},
                new Order { Id=9, Price=2005, Date= new DateTime(2021, 11, 4, 11, 59, 59)},
                new Order { Id=10, Price=3003, Date= new DateTime(2021, 11, 5, 12, 59, 59)},
                new Order { Id=11, Price=4004, Date= new DateTime(2021, 11, 5, 13, 59, 59)},
                new Order { Id=12, Price=3200, Date= new DateTime(2021, 11, 5, 14, 59, 59)},
                new Order { Id=13, Price=4560, Date= new DateTime(2021, 11, 6, 15, 59, 59)},
                new Order { Id=14, Price=5000, Date= new DateTime(2021, 11, 7, 16, 59, 59)},
                new Order { Id=15, Price=4999, Date= new DateTime(2021, 11, 8, 16, 59, 59)},
                new Order { Id=16, Price=5500, Date= new DateTime(2021, 11, 8, 16, 59, 59)},
                new Order { Id=17, Price=6000, Date= new DateTime(2021, 11, 8, 16, 59, 59)},
                new Order { Id=18, Price=6500, Date= new DateTime(2021, 11, 8, 16, 59, 59)},

                });
        }
    }
}
