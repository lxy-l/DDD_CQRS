using Domain.Commands;
using Domain.Core.Bus;
using Domain.Core.Commands;
using Domain.Core.Notifications;
using Domain.Events;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.CommandHandlers
{
    /// <summary>
    /// Student命令处理程序
    /// 用来处理该Student下的所有命令
    /// 注意必须要继承接口IRequestHandler<,>，这样才能实现各个命令的Handle方法
    /// </summary>
    public class StudentCommandHandler : CommandHandler,
        IRequestHandler<RegisterStudentCommand, Unit>,
        IRequestHandler<UpdateStudentCommand, Unit>,
        IRequestHandler<RemoveStudentCommand, Unit>
    {
        // 注入仓储接口
        private readonly IStudentRepository _studentRepository;
        // 注入总线
        private readonly IMediatorHandler Bus;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="studentRepository"></param>
        /// <param name="uow"></param>
        /// <param name="bus"></param>
        /// <param name="cache"></param>
        public StudentCommandHandler(IStudentRepository studentRepository,
                                      IUnitOfWork uow,
                                      IMediatorHandler bus,
                                      IMemoryCache cache
                                      ) : base(uow, bus, cache)
        {
            _studentRepository = studentRepository;
            Bus = bus;
        }

        // RegisterStudentCommand命令的处理程序
        // 整个命令处理程序的核心都在这里
        // 不仅包括命令验证的收集，持久化，还有领域事件和通知的添加
        public Task<Unit> Handle(RegisterStudentCommand message, CancellationToken cancellationToken)
        {
            // 命令验证
            if (!message.IsValid())
            {
                // 错误信息收集
                NotifyValidationErrors(message);
                return Task.FromResult(new Unit());
            }

            // 实例化领域模型，这里才真正的用到了领域模型
            // 注意这里是通过构造函数方法实现
            var customer = new Student(Guid.NewGuid(), message.Name, message.Email, message.Phone, message.BirthDate, new Address(message.Province,message.City,message.County,message.Street));

            // 判断邮箱是否存在
            // 这些业务逻辑，当然要在领域层中（领域命令处理程序中）进行处理
            if (_studentRepository.GetByEmail(customer.Email) != null)
            {
                ////这里对错误信息进行发布，目前采用缓存形式
                //List<string> errorInfo = new List<string>() { "该邮箱已经被使用！" };
                //Cache.Set("ErrorData", errorInfo);

                //引发错误事件
                Bus.RaiseEvent(new DomainNotification("", "该邮箱已经被使用！"));
                return Task.FromResult(new Unit());
            }

            // 持久化
            _studentRepository.Add(customer);

            // 统一提交
            if (Commit())
            {
                // 提交成功后，这里需要发布领域事件
                // 比如欢迎用户注册邮件呀，短信呀等
                Bus.RaiseEvent(new StudentRegisteredEvent(customer.Id, customer.Name, customer.Email, customer.BirthDate, customer.Phone));
                // waiting....
            }

            return Task.FromResult(new Unit());

        }

        // 同上，UpdateStudentCommand 的处理方法
        public Task<Unit> Handle(UpdateStudentCommand message, CancellationToken cancellationToken)
        {
            // 省略...
            return Task.FromResult(new Unit());
        }

        // 同上，RemoveStudentCommand 的处理方法
        public Task<Unit> Handle(RemoveStudentCommand message, CancellationToken cancellationToken)
        {
            // 省略...
            return Task.FromResult(new Unit());
        }

        // 手动回收
        public void Dispose()
        {
            _studentRepository.Dispose();
        }
    }
}
