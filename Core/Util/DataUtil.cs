/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 2019/7/8 22:13:30
 */

using Google.Protobuf;
using Newtonsoft.Json;
using System;

namespace XMLib
{
    public static class DataUtil
    {
        public static JsonSerializerSettings jsonSetting = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };

        public static string ToJson<T>(T obj)
        {
            string json = JsonConvert.SerializeObject(obj, jsonSetting);
            return json;
        }

        public static T FromJson<T>(string json)
        {
            T obj = JsonConvert.DeserializeObject<T>(json, jsonSetting);
            return obj;
        }

        public static string ToJson(object obj, Type type)
        {
            string json = JsonConvert.SerializeObject(obj, type, jsonSetting);
            return json;
        }

        public static object FromJson(string json, Type type)
        {
            object obj = JsonConvert.DeserializeObject(json, type, jsonSetting);
            return obj;
        }

        public static byte[] Encode<T>(T obj)
        {
            string json = ToJson(obj);
            byte[] buf = EncodeFromStr(json);
            return buf;
        }

        public static T Decode<T>(byte[] buf)
        {
            string json = DecodeToStr(buf);
            T obj = FromJson<T>(json);
            return obj;
        }

        public static T FromProto<T>(byte[] buf) where T : IMessage<T>, new()
        {
            T obj = new T();
            obj.MergeFrom(buf);
            return obj;
        }

        public static byte[] ToProto<T>(T obj) where T : IMessage<T>
        {
            return obj.ToByteArray();
        }

        #region 数据加解密

        public static string DecodeToStr(byte[] buf)
        {
            string json = ZipUtil.Decode(buf);
            return json;
        }

        public static byte[] EncodeFromStr(string str)
        {
            byte[] buf = ZipUtil.Encode(str);
            return buf;
        }

        #endregion 数据加解密
    }
}