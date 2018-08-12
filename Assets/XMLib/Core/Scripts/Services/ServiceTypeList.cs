using System;
using System.Collections.Generic;

namespace XM.Services
{
    /// <summary>
    /// 服务类型列表
    /// </summary>
    public class ServiceTypeList<AE> : List<Type> where AE : IAppEntry<AE>
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
        public void Add<T>() where T : IService<AE>
        {
            Add(typeof(T));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>是否成功</returns>
        public bool Remove<T>() where T : IService<AE>
        {
            return Remove(typeof(T));
        }
    }
}