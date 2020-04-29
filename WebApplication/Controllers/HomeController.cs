using Application.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
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

        /// <summary>
        /// 获取微信登录token
        /// </summary>
        public static async Task<string> GetWeChatTokenAsync()
        {
            string AppID = "wxa6cde3e5acabcdab", AppSecret = "a8d6d19659a27cebe395af215fc4e108";
            HttpClient httpClient = new HttpClient();
            string url = $@"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={AppID}&secret={AppSecret}";
            var res = await httpClient.GetAsync(url);
            if (res.StatusCode == HttpStatusCode.OK)
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(res.Content.ReadAsStringAsync().Result);
                return jo["access_token"].ToString();
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<string> UploadWeChatFileAsync()
        {
            var file = Request.Form.Files[0];
            string token = await GetWeChatTokenAsync(), type = "image";
            string url = $"https://api.weixin.qq.com/cgi-bin/media/upload?access_token={token}&type={type}";
            using HttpClient httpClient = new HttpClient();
            using Stream fileStream = file.OpenReadStream();
            byte[] fileByte = new byte[fileStream.Length];
            fileStream.Read(fileByte, 0, fileByte.Length);
            string boundary = Guid.NewGuid().ToString();
            using var content = new MultipartFormDataContent(boundary);
            content.Headers.Remove("Content-Type");
            content.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);
            using var contentByte = new ByteArrayContent(fileByte);
            content.Add(contentByte);
            contentByte.Headers.Remove("Content-Disposition");
            contentByte.Headers.TryAddWithoutValidation("Content-Disposition", $"form-data; name=\"media\";filename=\"{file.FileName}\"");
            contentByte.Headers.Remove("Content-Type");
            contentByte.Headers.TryAddWithoutValidation("Content-Type", file.ContentType);
            var res =await  httpClient.PostAsync(url, content);
            if (res.StatusCode==HttpStatusCode.OK)
                return await res.Content.ReadAsStringAsync();
            return "NULL";
        }

        [HttpPost]
        public async Task<string> DeleteWeChatFileAsync(string id)
        {
            //uT8yh - v7fhkQYPHuKX_ODaUgU3lwEfz9dLXS8Gux - sU
            string token = await GetWeChatTokenAsync();
            string url = $"https://api.weixin.qq.com/cgi-bin/material/del_material?access_token={token}&media_id={id}";
            using HttpClient httpClient = new HttpClient();
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>("media_id",id));
            var res= await httpClient.PostAsync(url, new FormUrlEncodedContent(new Dictionary<string, string> { {"media_id", id } }));
            if (res.StatusCode == HttpStatusCode.OK)
                return await res.Content.ReadAsStringAsync();
            return "NULL";
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
