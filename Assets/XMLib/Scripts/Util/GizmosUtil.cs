/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/10/2018 3:04:27 PM
 */

using UnityEngine;

namespace XMLib
{
    /// <summary>
    /// Gizmos 工具
    /// </summary>
    public static class GizmosUtil
    {
        /// <summary>
        /// 绘制扇形
        /// </summary>
        /// <param name="position">   世界坐标 </param>
        /// <param name="radius">     半径 </param>
        /// <param name="color">      颜色 </param>
        /// <param name="smoothness"> 平滑 </param>
        /// <param name="angle">      角度 </param>
        /// <param name="rotate">     旋转 </param>
        public static void DrawSector2D(Vector3 position, float radius, Color color, float smoothness = 30, float angle = 0, float rotate = 0)
        {
            #region 校验数据

            if (smoothness < 3)
            {
                smoothness = 3;
            }

            if (radius < 0)
            {
                radius = 0;
            }

            if (angle > 360 || angle < -360)
            {
                angle %= 360;
            }

            if (rotate > 360 || rotate < -360)
            {
                rotate %= 360;
            }

            #endregion 校验数据

            //=========================================================

            Vector3 beginPoint = Vector3.zero;
            Vector3 endPoint = Vector3.zero;

            float delta = 2 * Mathf.PI / smoothness;

            float rotDeg = rotate * Mathf.Deg2Rad;//旋转的弧度
            float deg = angle * Mathf.Deg2Rad;//扇形的弧度

            float startDeg = -deg / 2 + rotDeg;//开始绘制的弧度
            float endDeg = deg / 2 + rotDeg;//结束绘制的弧度

            //=========================================================

            //设置颜色
            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            //
            beginPoint = position;
            for (float i = startDeg; i < endDeg; i += delta)
            {
                endPoint.x = radius * Mathf.Cos(i) + position.x;
                endPoint.y = radius * Mathf.Sin(i) + position.y;

                Gizmos.DrawLine(beginPoint, endPoint);

                beginPoint = endPoint;
            }

            //绘制最后一根线
            endPoint.x = radius * Mathf.Cos(endDeg) + position.x;
            endPoint.y = radius * Mathf.Sin(endDeg) + position.y;
            Gizmos.DrawLine(beginPoint, endPoint);
            Gizmos.DrawLine(endPoint, position);
            //

            //还原颜色
            Gizmos.color = oldColor;
        }

        /// <summary>
        /// 画线
        /// </summary>
        /// <param name="start"> 起点 </param>
        /// <param name="end">   终点 </param>
        /// <param name="color"> 颜色 </param>
        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            //设置颜色
            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            Gizmos.DrawLine(start, end);

            //还原颜色
            Gizmos.color = oldColor;
        }

        /// <summary>
        /// 画线
        /// </summary>
        /// <param name="start"> 起点 </param>
        /// <param name="end">   终点 </param>
        /// <param name="color"> 颜色 </param>
        public static void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            //设置颜色
            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            Gizmos.DrawLine(start, end);

            //还原颜色
            Gizmos.color = oldColor;
        }

        /// <summary>
        /// 画圆
        /// </summary>
        /// <param name="position">   世界坐标 </param>
        /// <param name="radius">     半径 </param>
        /// <param name="color">      颜色 </param>
        /// <param name="smoothness"> 平滑 </param>
        public static void DrawCircle2D(Vector3 position, float radius, Color color, float smoothness = 30)
        {
            #region 校验数据

            if (smoothness < 3)
            {
                smoothness = 3;
            }

            if (radius < 0)
            {
                radius = 0;
            }

            #endregion 校验数据

            //=========================================================

            Vector3 beginPoint = position;
            Vector3 endPoint = position;

            float deg = 2 * Mathf.PI;
            float delta = deg / smoothness;

            //=========================================================

            //设置颜色
            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            //
            beginPoint.x = radius * Mathf.Cos(0) + position.x;
            beginPoint.y = radius * Mathf.Sin(0) + position.y;
            for (float i = delta; i < deg; i += delta)
            {
                endPoint.x = radius * Mathf.Cos(i) + position.x;
                endPoint.y = radius * Mathf.Sin(i) + position.y;
                endPoint.z = position.z;

                Gizmos.DrawLine(beginPoint, endPoint);

                beginPoint = endPoint;
            }

            //绘制最后一根线
            endPoint.x = radius * Mathf.Cos(deg) + position.x;
            endPoint.y = radius * Mathf.Sin(deg) + position.y;
            Gizmos.DrawLine(beginPoint, endPoint);
            //

            //还原颜色
            Gizmos.color = oldColor;
        }

        /// <summary>
        /// 绘制方块
        /// </summary>
        /// <param name="position"> 世界坐标 </param>
        /// <param name="rotate">   旋转 </param>
        /// <param name="scale">    缩放 </param>
        /// <param name="size">     大小 </param>
        /// <param name="center">   中心点 </param>
        /// <param name="color">    颜色 </param>
        public static void DrawCube2D(Vector3 position, Quaternion rotate, Vector2 scale, Vector2 size, Vector3 center, Color color)
        {
            Vector3 offset = position - center;
            Matrix4x4 matrix = Matrix4x4.TRS(center, rotate, scale);

            Vector3[] pts = new Vector3[4];
            Vector2 halfSize = size / 2;

            pts[0].z = 0;
            pts[0].x = -halfSize.x;
            pts[0].y = halfSize.y;

            pts[1].z = 0;
            pts[1].x = halfSize.x;
            pts[1].y = halfSize.y;

            pts[2].z = 0;
            pts[2].x = halfSize.x;
            pts[2].y = -halfSize.y;

            pts[3].z = 0;
            pts[3].x = -halfSize.x;
            pts[3].y = -halfSize.y;

            pts[0] += offset;
            pts[1] += offset;
            pts[2] += offset;
            pts[3] += offset;

            //旋转缩放
            pts[0] = matrix.MultiplyPoint(pts[0]);
            pts[1] = matrix.MultiplyPoint(pts[1]);
            pts[2] = matrix.MultiplyPoint(pts[2]);
            pts[3] = matrix.MultiplyPoint(pts[3]);

            //设置颜色
            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            Gizmos.DrawLine(pts[0], pts[1]);
            Gizmos.DrawLine(pts[1], pts[2]);
            Gizmos.DrawLine(pts[2], pts[3]);
            Gizmos.DrawLine(pts[3], pts[0]);

            //还原颜色
            Gizmos.color = oldColor;
        }

        /// <summary>
        /// 绘制箭头
        /// </summary>
        /// <param name="start"> 起点 </param>
        /// <param name="end">   终点 </param>
        /// <param name="color"> 颜色 </param>
        public static void DrawArrow(Vector3 start, Vector3 end, Color color)
        {
            //设置颜色
            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            Gizmos.DrawLine(start, end);

            DrawCircle2D(start, 0.03f, color, 6);

            //还原颜色
            Gizmos.color = oldColor;
        }

        /// <summary>
        /// 绘制箭头
        /// </summary>
        /// <param name="start">  起点 </param>
        /// <param name="end">    终点 </param>
        /// <param name="rotate"> 旋转 </param>
        /// <param name="scale">  缩放 </param>
        /// <param name="center"> 中心点 </param>
        /// <param name="color">  颜色 </param>
        public static void DrawArrow(Vector3 start, Vector3 end, Quaternion rotate, Vector2 scale, Vector3 center, Color color)
        {
            Vector3 vtLineCenter = (end + start) / 2;

            Vector3 offset = vtLineCenter - center;

            start = start - vtLineCenter + offset;
            end = end - vtLineCenter + offset;

            Matrix4x4 matrix = Matrix4x4.TRS(center, rotate, scale);

            start = matrix.MultiplyPoint(start);
            end = matrix.MultiplyPoint(end);

            DrawArrow(start, end, color);
        }

        /// <summary>
        /// 绘制箭头
        /// </summary>
        /// <param name="position">  世界坐标 </param>
        /// <param name="rotate">    旋转 </param>
        /// <param name="scale">     缩放 </param>
        /// <param name="size">      大小 </param>
        /// <param name="center">    中心点 </param>
        /// <param name="direction"> 方向 </param>
        /// <param name="color">     颜色 </param>
        public static void DrawCubeArrow2D(Vector3 position, Quaternion rotate, Vector2 scale, Vector2 size, Vector3 center, int direction, Color color)
        {
            //绘制方框
            DrawCube2D(position, rotate, scale, size, center, color);

            //绘制箭头
            Vector3 start = position;
            Vector3 end;

            switch (direction)
            {
                case 1://up
                    start.y -= size.y / 2;
                    end = start;
                    end.y += size.y;
                    break;

                case 2://down
                    start.y += size.y / 2;
                    end = start;
                    end.y -= size.y;
                    break;

                case 3://right
                    start.x -= size.x / 2;
                    end = start;
                    end.x += size.x;
                    break;

                case 4://left
                    start.x += size.x / 2;
                    end = start;
                    end.x -= size.x;
                    break;

                default:
                    return;
            }

            DrawArrow(start, end, rotate, scale, center, color);
        }
    }
}