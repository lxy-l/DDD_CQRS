using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core.Models
{
    /// <summary>
    /// 值对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
    }
}
