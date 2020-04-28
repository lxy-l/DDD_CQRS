using Domain.Core.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    /// <summary>
    /// 聚合根 Order
    /// 实体有标识ID，有生命周期和状态，通过ID进行区分
    /// 聚合根是一个实体，聚合根的标识ID全局唯一，聚合根中的实体ID在聚合根内部唯一就行
    /// 值对象主要就是值，与状态，标识无关，没有生命周期，用来描述实体状态。
    /// </summary>
    /// 属性都是值对象
    public class Order : AggregateRoot
    {
        public string OrderNo;//值对象
        public Address Address;//值对象
        public List<OrderItem> Items;//实体集合
        //...
    }
}
