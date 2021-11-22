using Mego.travel.Test_WebReport_Excel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mego.travel.Test_WebReport_Excel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private OrderContext _orderContext;

/*        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        public HomeController(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        [HttpGet]
        public IActionResult Create()// метод открывает нам страницу с формой форму Create
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Order order)// Метод с формы получает новый order
        {
            _orderContext.Orders.Update(order);

            // сохраняем в бд все изменения
            _orderContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            return View(_orderContext.Orders.ToList());// Передача данных на view
        }

        [HttpGet]
        public IActionResult Update(int id)// метод передает данные из Index в Update
        {
            ViewBag.OrderId = id;
            var order = _orderContext.Orders.Find(id);
            ViewBag.OrderPrice = order.Price;
            ViewBag.OrderDate = order.Date;
            return View();
        }
        [HttpPost]
        public IActionResult Update(Order order)// метод принимает обновленные данные от формы и сохраняет изменения
        {
            _orderContext.Orders.Update(order);

            // сохраняем в бд все изменения
            _orderContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            ViewBag.OrderId = id;
            return RedirectToAction("Index");
        }
        /*[HttpPost]
        public string Delete(Order order)
        {
            _orderContext.Orders.Add(order);


            // сохраняем в бд все изменения
            _orderContext.SaveChanges();
            return "200";
        }*/

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
