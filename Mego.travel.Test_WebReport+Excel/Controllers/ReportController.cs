using ClosedXML.Excel;
using Mego.travel.Test_WebReport_Excel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mego.travel.Test_WebReport_Excel.Controllers
{
    /// <summary>
    /// Контроллер отвечает за работу с отчетами
    /// </summary>
    public class ReportController : Controller
    {
        private OrderContext _orderContext;

        public ReportController(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            // TODO: refactor
            bool isDateIndicated = false;
            if (startDate != null || endDate != null)
            {
                isDateIndicated = true;
            }

            using (var workbook = new XLWorkbook())
            {
                var workshhet = workbook.Worksheets.Add($"Отчет");
                var currentRow = 1;

                #region Header
                SetHeaders(ref workshhet, currentRow);
                #endregion

                #region Body
                var orders = _orderContext.Orders;

                if (isDateIndicated)
                {                   
                    SetBetweenOrders(orders, ref workshhet, ref currentRow, startDate, endDate);                    
                }
                if(!isDateIndicated)
                {
                    SetAllOrders(orders, ref workshhet, ref currentRow);
                }
                if (endDate == null & startDate != null)// если не указали endDate
                {
                    SetLeftOrders(orders, ref workshhet, ref currentRow, startDate);
                }
                if (startDate == null & endDate != null)// если не указали startDate
                {
                    SetRightOrders(orders, ref workshhet, ref currentRow, endDate);
                }
                #endregion

                using (var stream = new MemoryStream())
                {
                    string format = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    string bookName = $"Отчет с {startDate} по {endDate}.xlsx";

                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        format,
                        bookName
                        );
                }
            }            
        }
        private void SetHeaders(ref IXLWorksheet workshhet, int currentRow)
        {
            workshhet.Cell(currentRow, column: 1).Value = "Дата";
            workshhet.Cell(currentRow, column: 2).Value = "Количество заказов с суммой от 0 до 1000";
            workshhet.Cell(currentRow, column: 3).Value = "Количество заказов с суммой от 1001 до 5000";
            workshhet.Cell(currentRow, column: 4).Value = "Количество заказов с суммой от 5001";
        }
        private void SetAllOrders(DbSet<Order> orders, ref IXLWorksheet workshhet, ref int currentRow)
        {
            foreach (var order in orders)
            {
                currentRow++;
                if (order.Price > 0 & order.Price <= 1000)
                {
                    workshhet.Cell(currentRow, column: 2).Value = 1;
                }
                if (order.Price >= 1001 & order.Price <= 5000)
                {
                    workshhet.Cell(currentRow, column: 3).Value = 1;
                }
                if (order.Price >= 5001)
                {
                    workshhet.Cell(currentRow, column: 4).Value = 1;
                }

                workshhet.Cell(currentRow, column: 1).Value = order.Date;
            }
        }
        private void SetLeftOrders(DbSet<Order> orders, ref IXLWorksheet workshhet, ref int currentRow, DateTime? startDate)
        {
            var leftOrders = orders.Where(o => o.Date >= startDate);
            foreach (var order in leftOrders)
            {
                currentRow++;
                if (order.Price > 0 & order.Price <= 1000)
                {
                    workshhet.Cell(currentRow, column: 2).Value = 1;
                }
                if (order.Price >= 1001 & order.Price <= 5000)
                {
                    workshhet.Cell(currentRow, column: 3).Value = 1;
                }
                if (order.Price >= 5001)
                {
                    workshhet.Cell(currentRow, column: 4).Value = 1;
                }

                workshhet.Cell(currentRow, column: 1).Value = order.Date;
            }
        }
        private void SetRightOrders(DbSet<Order> orders, ref IXLWorksheet workshhet, ref int currentRow, DateTime? endDate)
        {
            var rightOrders = orders.Where(o => o.Date <= endDate);
            foreach (var order in rightOrders)
            {
                currentRow++;
                if (order.Price > 0 & order.Price <= 1000)
                {
                    workshhet.Cell(currentRow, column: 2).Value = 1;
                }
                if (order.Price >= 1001 & order.Price <= 5000)
                {
                    workshhet.Cell(currentRow, column: 3).Value = 1;
                }
                if (order.Price >= 5001)
                {
                    workshhet.Cell(currentRow, column: 4).Value = 1;
                }

                workshhet.Cell(currentRow, column: 1).Value = order.Date;
            }
        }
        private void SetBetweenOrders(DbSet<Order> orders, ref IXLWorksheet workshhet, ref int currentRow, DateTime? startDate, DateTime? endDate)
        {
            var betweenOrders = orders.Where(o => o.Date >= startDate & o.Date <= endDate);
            foreach (var order in betweenOrders)
            {
                currentRow++;
                if (order.Price > 0 & order.Price <= 1000)
                {
                    workshhet.Cell(currentRow, column: 2).Value = 1;
                }
                if (order.Price >= 1001 & order.Price <= 5000)
                {
                    workshhet.Cell(currentRow, column: 3).Value = 1;
                }
                if (order.Price >= 5001)
                {
                    workshhet.Cell(currentRow, column: 4).Value = 1;
                }

                workshhet.Cell(currentRow, column: 1).Value = order.Date;
            }
        }
    }
}
