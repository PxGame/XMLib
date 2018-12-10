/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/10/2018 3:30:45 PM
 */

using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// Texture工具
    /// </summary>
    public static class TextureUtil
    {
        /// <summary>
        /// 获取图片指定范围内的像素的灰度在指定灰度范围内的比例
        /// </summary>
        /// <param name="texture">   图片 </param>
        /// <param name="rect">      检测区域 </param>
        /// <param name="grayRange"> 灰度范围 </param>
        /// <returns> 比例 </returns>
        public static float GetGrayRatio(Texture2D texture, RectInt rect, Vector2 grayRange)
        {
            float ratio = 0;

            int fullPixelCount = rect.width * rect.height;
            int grayPixelCount = 0;

            int startX = rect.x;
            int startY = rect.y;
            int endX = rect.x + rect.width - 1;
            int endY = rect.y + rect.height - 1;

            if (startX < 0 || startY < 0 ||
                endX >= texture.width || endY >= texture.height)
            {
                throw new System.Exception("Rect范围与Texture2D大小不符。");
            }

            Color color;
            float grap;
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    color = texture.GetPixel(x, y);
                    grap = color.grayscale;
                    if (grap >= grayRange.x && grap <= grayRange.y)
                    {
                        ++grayPixelCount;
                    }
                }
            }

            ratio = grayPixelCount / (float)fullPixelCount;

            return ratio;
        }
    }
}