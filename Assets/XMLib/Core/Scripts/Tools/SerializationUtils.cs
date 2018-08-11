namespace XM.Tools
{
    /// <summary>
    /// 序列化工具
    /// </summary>
    public class SerializationUtils
    {
        /// <summary>
        /// 序列化方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
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
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
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