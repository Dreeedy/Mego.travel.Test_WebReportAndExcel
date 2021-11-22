using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mego.travel.Test_WebReport_Excel.Models
{
    /// <summary>
    /// Модель для заказов
    /// </summary>
    public class Order
    {
        public int Id { get; set; }        
        public int Price { get; set; }
        public DateTime Date { get; set; }
    }
}
