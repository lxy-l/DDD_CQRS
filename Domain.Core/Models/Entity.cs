using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Models
{
    /// <summary>
    /// 领域实体基类
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// 实体唯一标识（也可使用Int）
        /// </summary>
        public Guid Id { get; protected set; }
    }
}
