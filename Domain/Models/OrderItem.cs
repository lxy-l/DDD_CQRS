using Domain.Core.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    /// <summary>
    /// 实体 OrderItem
    /// 属性都是值对象
    /// </summary>
    public class OrderItem : Entity
    {
        /// <summary>
        /// 详情名
        /// </summary>
        public string Name { get; private set; }

        public virtual Order Order { get; set; }
    }
}
