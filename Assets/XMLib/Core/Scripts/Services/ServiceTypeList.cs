using System;
using System.Collections.Generic;

namespace XM.Services
{
    /// <summary>
    /// 服务类型列表
    /// </summary>
    public class ServiceTypeList : List<Type>
    {
        public ServiceTypeList() : base()
        {
        }

        public ServiceTypeList(IEnumerable<Type> collection) : base(collection)
        {
        }

        public ServiceTypeList(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        public void Add<T>() where T : IService
        {
            Add(typeof(T));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>是否成功</returns>
        public bool Remove<T>() where T : IService
        {
            return Remove(typeof(T));
        }
    }
}