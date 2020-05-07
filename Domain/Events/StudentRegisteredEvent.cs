using Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Events
{
    /// <summary>
    /// Student被添加后引发事件
    /// 继承事件基类标识
    /// </summary>
    public class StudentRegisteredEvent : Event
    {
        // 构造函数初始化，整体事件是一个值对象
        public StudentRegisteredEvent(Guid id, string name, string email, DateTime birthDate, string phone)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            Phone = phone;
        }
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string Phone { get; private set; }
    }
}
