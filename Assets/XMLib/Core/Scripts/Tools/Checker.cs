namespace XM
{
    /// <summary>
    /// 检查者
    /// </summary>
    public static class Checker
    {
        /// <summary>
        /// 不为空
        /// </summary>
        /// <param name="obj">检查对象</param>
        /// <param name="format">格式化文本</param>
        /// <param name="args">参数</param>
        public static void NotNull(object obj, string format, params object[] args)
        {
            if (null != obj)
            {
                return;
            }

            throw new StringException(format, args);
        }

        /// <summary>
        /// 为空
        /// </summary>
        /// <param name="obj">检查对象</param>
        /// <param name="format">格式化文本</param>
        /// <param name="args">参数</param>
        public static void IsNull(object obj, string format, params object[] args)
        {
            if (null == obj)
            {
                return;
            }

            throw new StringException(format, args);
        }

        /// <summary>
        /// 为真
        /// </summary>
        /// <param name="isOk">是否</param>
        /// <param name="format">格式化文本</param>
        /// <param name="args">参数</param>
        public static void IsTrue(bool isOk, string format, params object[] args)
        {
            if (isOk)
            {
                return;
            }

            throw new StringException(format, args);
        }

        /// <summary>
        /// 为假
        /// </summary>
        /// <param name="isOk">是否</param>
        /// <param name="format">格式化文本</param>
        /// <param name="args">参数</param>
        public static void IsFalse(bool isOk, string format, params object[] args)
        {
            if (!isOk)
            {
                return;
            }

            throw new StringException(format, args);
        }
    }
}