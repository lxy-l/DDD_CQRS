using Application.Interfaces;
using Application.Services;
using Christ3D.Infra.Data.Repository;
using Domain.Core.Bus;
using Domain.Interfaces;
using Infrastructure.Bus;
using Infrastructure.Context;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication.Extension
{
    public class InjectorSetup
    {
        public static void RegisterServices(IServiceCollection services)
        {

            // 注入 Application 应用层
            services.AddScoped<IStudentAppService, StudentAppService>();
            //注入我们的中介总线接口
            services.AddScoped<IMediatorHandler, InMemoryBus>();
            //注入工作单元
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // 注入 Infra - Data 基础设施数据层
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<StudyContext>();//上下文

        }
    }
}
