﻿using Domain.Core.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    /// <summary>
    /// 值对象 Goods
    /// 属性都是值对象
    /// </summary>
    public class Goods : ValueObject<Goods>
    {
        public string Name;
        //...
    }
}
