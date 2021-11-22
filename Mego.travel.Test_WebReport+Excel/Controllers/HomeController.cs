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
            _orderContext.Orders.Add(order);

            // сохраняем в бд все изменения
            _orderContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            return View(_orderContext.Orders.ToList());// Передача данных на view
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            ViewBag.OrderId = id;
            return View();
        }
        [HttpPost]
        public string Update(Order order)
        {
            _orderContext.Orders.Add(order);


            // сохраняем в бд все изменения
            _orderContext.SaveChanges();
            return "200";
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            ViewBag.OrderId = id;
            return View();
        }
        [HttpPost]
        public string Delete(Order order)
        {
            _orderContext.Orders.Add(order);


            // сохраняем в бд все изменения
            _orderContext.SaveChanges();
            return "200";
        }

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
