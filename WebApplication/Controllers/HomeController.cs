using Application.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ILogger<HomeController> Logger { get; }

        public IStudentAppService StudentAppService { get; }

        public HomeController(ILogger<HomeController> logger,IStudentAppService studentAppService)
        {
            Logger = logger;
            StudentAppService = studentAppService;
        }

        public IActionResult Index()
        {
            return View(StudentAppService.GetAll());
        }
        public async Task<string> UploadWeChatFileAsync()
        {
            string token = "", type = "image";
            string url = $"https://api.weixin.qq.com/cgi-bin/material/add_material?access_token={token}&type={type}";
            HttpClient httpClient = new HttpClient();
            var res =await  httpClient.PostAsync(url, new FormUrlEncodedContent(new Dictionary<string, string> {
                {"filename","name"},
                { "filelength",""},
                { "content-type",""}
            }));
            return res.Content.ToString();
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
