namespace XM
{
    /// <summary>
    /// 序列化工具
    /// </summary>
    public static class SerializerUtils
    {
        /// <summary>
        /// 序列化方法
        /// </summary>
        /// <typeparam name="T">序列化类型</typeparam>
        /// <param name="obj">类型实例</param>
        /// <returns>字节数组</returns>
        public static byte[] Serialize<T>(T obj) where T : class
        {
            byte[] data = null;

#if XM_USE_PROTOBUF
            using (MemoryStream stream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize<T>(stream, obj);
                data = stream.ToArray();
            }
#else
            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            data = System.Text.Encoding.UTF8.GetBytes(jsonStr);
#endif

            return data;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">序列化类型</typeparam>
        /// <param name="data">字节数组</param>
        /// <returns>类型实例</returns>
        public static T Deserialize<T>(byte[] data) where T : class
        {
            T obj = null;

#if XM_USE_PROTOBUF
            using (MemoryStream stream = new MemoryStream(data))
            {
                obj = ProtoBuf.Serializer.Deserialize<T>(stream);
            }
#else
            string jsonStr = System.Text.Encoding.UTF8.GetString(data);
            obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonStr);
#endif

            return obj;
        }
    }
}