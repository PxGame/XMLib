/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 1/11/2019 3:09:13 PM
 */

using System.Collections.Generic;

namespace XMLib
{
    /// <summary>
    /// List 扩展
    /// </summary>
    public static class ListExtend
    {
        /// <summary>
        /// 重置大小并填充实例或移除实例
        /// </summary>
        /// <param name="count">大小</param>
        public static void FixedCountWithInstance<T>(this List<T> list, int count) where T : new()
        {
            int offset = list.Count - count;
            if (0 == offset)
            {
                return;
            }

            if (offset > 0)
            {//移除多余对象
                list.RemoveRange(0, offset);
            }
            else
            {//添加新对象
                while (offset < 0)
                {
                    list.Add(new T());
                    offset++;
                }
            }

            list.TrimExcess();
        }
    }
}