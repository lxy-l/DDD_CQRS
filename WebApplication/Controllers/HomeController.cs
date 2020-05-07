using Application.Interfaces;
using Application.ViewModel;
using Domain.Commands;
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

        public IStudentAppService _studentAppService { get; }

        public HomeController(ILogger<HomeController> logger,IStudentAppService studentAppService)
        {
            Logger = logger;
            _studentAppService = studentAppService;
        }

        public IActionResult Index()
        {
            return View(_studentAppService.GetAll());
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Student/Create
        // 方法
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentViewModel studentViewModel)
        {
            try
            {
                //_cache.Remove("ErrorData");
                //ViewBag.ErrorData = null;
                // 视图模型验证
                if (!ModelState.IsValid)
                    return View(studentViewModel);

                #region 删除命令验证
                ////添加命令验证
                //RegisterStudentCommand registerStudentCommand = new RegisterStudentCommand(studentViewModel.Name, studentViewModel.Email, studentViewModel.BirthDate, studentViewModel.Phone);

                ////如果命令无效，证明有错误
                //if (!registerStudentCommand.IsValid())
                //{
                //    List<string> errorInfo = new List<string>();
                //    //获取到错误，请思考这个Result从哪里来的 
                //    foreach (var error in registerStudentCommand.ValidationResult.Errors)
                //    {
                //        errorInfo.Add(error.ErrorMessage);
                //    }
                //    //对错误进行记录，还需要抛给前台
                //    ViewBag.ErrorData = errorInfo;
                //    return View(studentViewModel);
                //} 
                #endregion

                // 执行添加方法
                _studentAppService.Register(studentViewModel);

                //var errorData = _cache.Get("ErrorData");
                //if (errorData == null)

                // 是否存在消息通知
                //if (!_notifications.HasNotifications())
                //    ViewBag.Sucesso = "Student Registered!";

                return View(studentViewModel);
            }
            catch (Exception e)
            {
                return View(e.Message);
            }
        }


        private async Task<string> GetWeChatTokenAsync(string AppID, string AppSecret)
        {
            using HttpClient httpClient = new HttpClient();
            string url = $@"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={AppID}&secret={AppSecret}";
            var res = await httpClient.GetAsync(url);
            if (res.StatusCode != HttpStatusCode.OK)
                return null;
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(await res.Content.ReadAsStringAsync());
                return jo["access_token"].ToString();
            }
            catch (Exception)
            {
                return null;
            }  
        }

        [HttpPost]
        public async Task<string> UploadWeChatFileAsync()
        {
            var file = Request.Form.Files[0];
            string token = await GetWeChatTokenAsync("wxa6cde3e5acabcdab","a8d6d19659a27cebe395af215fc4e108"), type = "video";
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
            string token = await GetWeChatTokenAsync("wxa6cde3e5acabcdab", "a8d6d19659a27cebe395af215fc4e108");
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
