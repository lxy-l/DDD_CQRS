using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewComponents
{
    public class AlertsViewComponent : ViewComponent
    {

        protected IMemoryCache _cache;
        public AlertsViewComponent(IMemoryCache cache)
        {
            _cache = cache;
        }


        /// <summary>
        /// Alerts 视图组件
        /// 可以异步，也可以同步，注意方法名称，同步的时候是Invoke
        /// 我写异步是为了为以后做准备
        /// </summary>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // 获取到缓存中的错误信息
            var errorData = _cache.Get("ErrorData");
            var notificacoes = await Task.Run(() => (List<string>)errorData);
            // 遍历添加到ViewData.ModelState 中
            notificacoes?.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c));
            return View();
        }
    }
}
