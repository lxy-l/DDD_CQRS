using System;
namespace Domain.Interfaces
{
    /**
     *
     * 维护受业务影响的对象列表，并且协调变化的写入和解决并发问题。
     * 可以用工作单元来实现事务，工作单元就是记录对象数据变化的对象。
     * 只要开始做一些可能对所要记录的对象的数据有影响的操作，就会创建一个工作单元去记录这些变化，
     * 所以，每当创建、修改、或删除一个对象的时候，就会通知工作单元。
     *
     * */
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        //是否提交成功
        bool Commit();
    }
}
