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
        /// <summary>
        /// Роут создает отчет и выдает его в формате xlsx
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
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

                if (isDateIndicated)// если указали обе даты
                {                   
                    SetBetweenOrders(orders, ref workshhet, ref currentRow, startDate, endDate);                    
                }
                if(!isDateIndicated)// ели обе даты не указали
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

        /// <summary>
        /// Метод устаналивает заголовки в exel и стили
        /// </summary>
        /// <param name="workshhet"></param>
        /// <param name="currentRow"></param>
        private void SetHeaders(ref IXLWorksheet workshhet, int currentRow)
        {
            workshhet.Cell(currentRow, column: 1).Value = "Дата";
            workshhet.Cell(currentRow, column: 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            workshhet.Cell(currentRow, column: 2).Value = "Количество заказов с суммой от 0 до 1000";
            workshhet.Cell(currentRow, column: 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            workshhet.Cell(currentRow, column: 3).Value = "Количество заказов с суммой от 1001 до 5000";
            workshhet.Cell(currentRow, column: 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            workshhet.Cell(currentRow, column: 4).Value = "Количество заказов с суммой от 5001";
            workshhet.Cell(currentRow, column: 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            workshhet.Column(1).Width = 20;

            workshhet.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            workshhet.Column(2).Width = 40;

            workshhet.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            workshhet.Column(3).Width = 40;

            workshhet.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            workshhet.Column(4).Width = 40;
        }

        /// <summary>
        /// Метод заполняет exel всеми заказами
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="workshhet"></param>
        /// <param name="currentRow"></param>
        private void SetAllOrders(DbSet<Order> orders, ref IXLWorksheet workshhet, ref int currentRow)
        {
            GroupOrders(orders, ref workshhet, ref currentRow);
        }

        /// <summary>
        /// Метод заполняет exel заказами позже даты
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="workshhet"></param>
        /// <param name="currentRow"></param>
        /// <param name="startDate"></param>
        private void SetLeftOrders(DbSet<Order> orders, ref IXLWorksheet workshhet, ref int currentRow, DateTime? startDate)
        {
            var leftOrders = orders.Where(o => o.Date >= startDate);

            GroupOrders(leftOrders, ref workshhet, ref currentRow);
        }

        /// <summary>
        /// Метод заполняет exel заказами раньше даты
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="workshhet"></param>
        /// <param name="currentRow"></param>
        /// <param name="endDate"></param>
        private void SetRightOrders(DbSet<Order> orders, ref IXLWorksheet workshhet, ref int currentRow, DateTime? endDate)
        {
            var rightOrders = orders.Where(o => o.Date <= endDate);

            GroupOrders(rightOrders, ref workshhet, ref currentRow);
        }        

        /// <summary>
        /// Метод заполняет exel заказами между двух дат
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="workshhet"></param>
        /// <param name="currentRow"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        private void SetBetweenOrders(DbSet<Order> orders, ref IXLWorksheet workshhet, ref int currentRow, DateTime? startDate, DateTime? endDate)
        {
            var betweenOrders = orders.Where(o => o.Date >= startDate & o.Date <= endDate);

            GroupOrders(betweenOrders, ref workshhet, ref currentRow);
        }

        /// <summary>
        /// Метод группирует заказы по дате и цене и заполняет ячейки exel
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="workshhet"></param>
        /// <param name="currentRow"></param>
        private void GroupOrders(IQueryable<Order> orders, ref IXLWorksheet workshhet, ref int currentRow)
        {
            DateTime lastDate = new DateTime();
            foreach (var order in orders)
            {
                if (lastDate != order.Date)
                {
                    currentRow++;

                    var phoneGroups = orders.GroupBy(p => new { p.Date, p.Price })
                            .Where(g => g.Key.Date == order.Date && g.Key.Price > 0 && g.Key.Price <= 1000)
                            .Select(g => new { g.Key.Date, g.Key.Price });

                    if (phoneGroups.Count() > 0)
                    {
                        workshhet.Cell(currentRow, column: 2).Value = phoneGroups.Count();
                    }

                    phoneGroups = orders.GroupBy(p => new { p.Date, p.Price })
                            .Where(g => g.Key.Date == order.Date && g.Key.Price >= 1001 && g.Key.Price <= 5000)
                            .Select(g => new { g.Key.Date, g.Key.Price });

                    if (phoneGroups.Count() > 0)
                    {
                        workshhet.Cell(currentRow, column: 3).Value = phoneGroups.Count();
                    }

                    phoneGroups = orders.GroupBy(p => new { p.Date, p.Price })
                            .Where(g => g.Key.Date == order.Date && g.Key.Price >= 5001)
                            .Select(g => new { g.Key.Date, g.Key.Price });

                    if (phoneGroups.Count() > 0)
                    {
                        workshhet.Cell(currentRow, column: 4).Value = phoneGroups.Count();
                    }

                    lastDate = order.Date;
                    workshhet.Cell(currentRow, column: 1).Value = order.Date;
                }
            }
        }

    }
}
