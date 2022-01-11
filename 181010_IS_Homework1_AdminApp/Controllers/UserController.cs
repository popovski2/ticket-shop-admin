using _181010_IS_Homework1_AdminApp.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace _181010_IS_Homework1_AdminApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ImportUsers(IFormFile file)
        {
            //make a copy 
            //currently on my local machine
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using(FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            //read data from the copied file
            List<User> users = getAllUsersFromFile(file.FileName);

            HttpClient client = new HttpClient();

            string URL = "https://localhost:44351/API/Admin/ImportAllUsers";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var data = response.Content.ReadAsAsync<bool>().Result;

            return RedirectToAction("Index", "Order");
        }

        private List<User> getAllUsersFromFile(string fileName)
        {
            List<User> users = new List<User>();

            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using(var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using(var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        users.Add(new Models.User
                        {
                            Email = reader.GetValue(0).ToString(),
                            Name = reader.GetValue(1).ToString(),
                            Surname = reader.GetValue(2).ToString(),
                            Address = reader.GetValue(3).ToString(),
                            Password = reader.GetValue(4).ToString(),
                            ConfirmPassword = reader.GetValue(5).ToString()
                        });;
                    }
                }
            }

            return users;
        }
    }
}
