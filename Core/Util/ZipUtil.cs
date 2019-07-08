/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 2019/7/8 22:13:30
 */

using System;
using System.IO;
using System.Text;

namespace XMLib
{
    /// <summary>
    /// 压缩工具
    /// </summary>
    public static class ZipUtil
    {
        public static byte[] Encode(string str)
        {
            byte[] buf = Encoding.UTF8.GetBytes(str);

            byte[] outBuf = null;
            using (MemoryStream outStream = new MemoryStream())
            {
                using (MemoryStream inStream = new MemoryStream(buf))
                {
                    SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();

                    //写入长度
                    outStream.Write(BitConverter.GetBytes(inStream.Length), 0, 8);

                    //写入属性
                    coder.WriteCoderProperties(outStream);

                    //压缩
                    coder.Code(inStream, outStream, inStream.Length, -1, null);

                    outStream.Flush();

                    outBuf = outStream.ToArray();
                }
            }

            return outBuf;
        }

        public static string Decode(byte[] buf)
        {
            string outStr = null;
            using (MemoryStream outStream = new MemoryStream())
            {
                using (MemoryStream inStream = new MemoryStream(buf))
                {
                    SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();

                    //读取长度
                    byte[] fileLengthBytes = new byte[8];
                    inStream.Read(fileLengthBytes, 0, 8);
                    long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

                    //读取属性
                    byte[] properties = new byte[5];
                    inStream.Read(properties, 0, 5);

                    //设置属性
                    coder.SetDecoderProperties(properties);

                    //解压
                    coder.Code(inStream, outStream, inStream.Length, fileLength, null);

                    outStream.Flush();

                    byte[] outBuf = outStream.ToArray();
                    outStr = Encoding.UTF8.GetString(outBuf);
                }
            }

            return outStr;
        }
    }
}