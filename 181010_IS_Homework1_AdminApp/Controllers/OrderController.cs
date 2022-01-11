using _181010_IS_Homework1_AdminApp.Models;
using ClosedXML.Excel;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace _181010_IS_Homework1_AdminApp.Controllers
{
    public class OrderController : Controller
    {
        public OrderController()
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        public IActionResult IndexAsync()
        {
            HttpClient client = new HttpClient();

            string URL = "https://localhost:44351/API/Admin/GetAllActiveOrders";

            HttpResponseMessage response = client.GetAsync(URL).Result;

            //dodadeno petar
            if (response.IsSuccessStatusCode == false)
                throw new ArgumentException("Could not retrieve tasks.");

            //var data = await response.Content.ReadAsAsync<List<Order>>();
            var stream = response.Content.ReadAsStreamAsync().Result; 
            StreamReader reader = new StreamReader(stream); 
            string text = reader.ReadToEnd();
            string content = text;
            var des = JsonConvert.DeserializeObject<List<Order>>(content);

            return View(des);
        }

        public IActionResult Details(Guid orderId)
        {
            HttpClient client = new HttpClient();

            string URL = "https://localhost:44351/API/Admin/GetDetailsForOrder";

            var model = new
            {
                Id = orderId
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var data = response.Content.ReadAsAsync<Order>().Result;

            return View(data);
        }

        [HttpGet]
        public FileContentResult ExportAllOrders()
        {
          // string fileName = "Orders1.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using(var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");

                worksheet.Cell(1, 1).Value = "Order ID";
                worksheet.Cell(1, 2).Value = "Customer Email";
                worksheet.Cell(1, 3).Value = "Customer Name";
                worksheet.Cell(1, 4).Value = "Customer Surname";

                HttpClient client = new HttpClient();

                string URI = "https://localhost:44351/API/Admin/GetAllActiveOrders";

                HttpResponseMessage responseMessage = client.GetAsync(URI).Result;

                var result = responseMessage.Content.ReadAsAsync<List<Order>>().Result;

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i-1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.OrderedBy.Email.ToString();
                    worksheet.Cell(i + 1, 3).Value = item.OrderedBy.Name.ToString();
                    worksheet.Cell(i + 1, 4).Value = item.OrderedBy.Surname.ToString();

                    for (int t = 0; t < item.Tickets.Count(); t++)
                    {
                        worksheet.Cell(1, t + 5).Value = "Ticket - " + (t + 1);
                        worksheet.Cell(i + 1, t + 5).Value = item.Tickets.ElementAt(t).Ticket.Title;
                    }
                }

                using(var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, "Orders.xlsx"); // defined at the beginning of the function
                }

            }

        }

        public IActionResult CreateInvoice(Guid orderId)
        {
            //communicating with the main app
            HttpClient client = new HttpClient();

            string URL = "https://localhost:44351/API/Admin/GetDetailsForOrder";

            var model = new
            {
                Id = orderId
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            

            //getting the app's response 
            var data = response.Content.ReadAsAsync<Order>().Result;

            //getting and loading the path of the created invoice word form
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "invoice.docx");
            var document = DocumentModel.Load(templatePath);

            //imputing the values for the order from the app's models
            document.Content.Replace("{{OrderNumber}}", data.Id.ToString());
            document.Content.Replace("{{UserId}}", data.OrderedBy.Name.ToString() + " " + data.OrderedBy.Surname.ToString());

            StringBuilder sb = new StringBuilder();
            var totalPrice = 0.0;

            foreach (var item in data.Tickets)
            {
                sb.AppendLine(item.Ticket.Title + "\nQuantity: " + item.Quantity + "\nPrice: " + item.Ticket.Price + "$\n\n");
                totalPrice += item.Quantity * item.Ticket.Price;
            }

            document.Content.Replace("{{TicketList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + "$");

            //saving the created document as a pdf
            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());

            //downloading the pdf
            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }

    }
}
