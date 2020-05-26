using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace SimpleStateMachineNodeEditor.Helpers.Extensions
{
    public static class PointExtensition
    {
        //public static MyPoint ToMyPoint(this Point point)
        //{
        //    return MyPoint.CreateFromPoint(point);
        //}
        public static Point CreatePoint(Point point)
        {
            return new Point(point.X, point.Y);
        }
        public static Point CreatePoint(int value)
        {
            return new Point(value, value);
        }
        public static Point CreatePoint(double value)
        {
            return new Point(value, value);
        }
        public static Point CreatePoint(int x, int y)
        {
            return new Point(x, y);
        }
        public static Point CreatePoint(double x, double y)
        {
            return new Point(x, y);
        }
        public static Point CreatePoint(Size size)
        {
            return new Point(size.Width, size.Height);
        }

        public static Point CreateNew(this Point point, Point point2)
        {
            return new Point(point2.X, point2.Y);
        }
        public static Point CreateNew(this Point point, int value)
        {
            return new Point(value, value);
        }
        public static Point CreateNew(this Point point, double value)
        {
            return new Point(value, value);
        }
        public static Point CreateNew(this Point point, int x, int y)
        {
            return new Point(x, y);
        }
        public static Point CreateNew(this Point point, double x, double y)
        {
            return new Point(x, y);
        }
        public static Point CreateNew(this Point point, Size size)
        {
            return new Point(size.Width, size.Height);
        }

        public static bool IsClear(this Point point)
        {
             return (point.X == 0) && (point.Y == 0);
        }

        public static Point Mirror(this Point point, bool onX = true, bool onY = true)
        {
            return new Point(onX ? -point.X : point.X, onY ? -point.Y : point.Y);
        }

        public static Point Copy(this Point point)
        {
            return new Point(point.X, point.Y);
        }

        /// <summary>
        /// Operation +
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Point Addition(this Point point1, Point point2)
        {
            return new Point(point1.X + point2.X, point1.Y + point2.Y);
        }
        /// <summary>
        /// Operation +
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static Point Addition(this Point point1, int number)
        {
            return new Point(point1.X + number, point1.Y + number);
        }
        /// <summary>
        /// Operation +
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point Addition(this Point point1, int x, int y)
        {
            return new Point(point1.X + x, point1.Y + y);
        }
        /// <summary>
        /// Operation +
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static Point Addition(this Point point1, double number)
        {
            return new Point(point1.X + number, point1.Y + number);
        }
        /// <summary>
        /// Operation +
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point Addition(this Point point1, double x, double y)
        {
            return new Point(point1.X + x, point1.Y + y);
        }
        /// <summary>
        /// Operation +
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point Addition(this Point point1, Size size)
        {
            return new Point(point1.X + size.Width, point1.Y + size.Height);
        }

        /// <summary>
        /// Operation -
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Point Subtraction(this Point point1, Point point2)
        {
            return new Point(point1.X - point2.X, point1.Y - point2.Y);
        }
        /// <summary>
        /// Operation -
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static Point Subtraction(this Point point1, int number)
        {
            return new Point(point1.X - number, point1.Y - number);
        }
        public static Point Subtraction(this Point point1, int x, int y)
        {
            return new Point(point1.X - x, point1.Y - y);
        }
        /// <summary>
        /// Operation -
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static Point Subtraction(this Point point1, double number)
        {
            return new Point(point1.X - number, point1.Y - number);
        }
        /// <summary>
        /// Operation -
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point Subtraction(this Point point1, double x, double y)
        {
            return new Point(point1.X - x, point1.Y - y);
        }
        /// <summary>
        /// Operation -
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point Subtraction(this Point point1, Size size)
        {
            return new Point(point1.X - size.Width, point1.Y - size.Height);
        }

        /// <summary>
        /// Operation /
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Point Division(this Point point1, Point point2)
        {
            return new Point(point1.X / point1.X, point1.Y / point1.Y);
        }
        /// <summary>
        /// Operation /
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static Point Division(this Point point1, int number)
        {
            return new Point(point1.X / number, point1.Y / number);
        }
        public static Point Division(this Point point1, int x, int y)
        {
            return new Point(point1.X / x, point1.Y / y);
        }
        /// <summary>
        /// Operation /
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static Point Division(this Point point1, double number)
        {
            return new Point(point1.X / number, point1.Y / number);
        }
        /// <summary>
        /// Operation /
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point Division(this Point point1, double x, double y)
        {
            return new Point(point1.X / x, point1.Y / y);
        }
        /// <summary>
        /// Operation /
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point Division(this Point point1, Size size)
        {
            return new Point(point1.X / size.Width, point1.Y / size.Height);
        }

        /// <summary>
        /// Operation *
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Point Multiply(this Point point1, Point point2)
        {
            return new Point(point1.X * point2.X, point1.Y * point2.Y);
        }
        /// <summary>
        /// Operation *
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static Point Multiply(this Point point1, int number)
        {
            return new Point(point1.X * number, point1.Y * number);
        }
        /// <summary>
        /// Operation *
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point Multiply(this Point point1, int x, int y)
        {
            return new Point(point1.X * x, point1.Y * y);
        }
        /// <summary>
        /// Operation *
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static Point Multiply(this Point point1, double number)
        {
            return new Point(point1.X * number, point1.Y * number);
        }
        /// <summary>
        /// Operation *
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point Multiply(this Point point1, double x, double y)
        {
            return new Point(point1.X * x, point1.Y * y);
        }
        /// <summary>
        /// Operation *
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point Multiply(this Point point1, Size size)
        {
            return new Point(point1.X * size.Width, point1.Y * size.Height);
        }

        public static string PointToString(Point point)
        {
            return string.Format("{0}, {1}", point.X.ToString(System.Globalization.CultureInfo.InvariantCulture), point.Y.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        public static Point StringToPoint(string str)
        {
            string[] parts = str.Split(",");
            return new Point(double.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture), double.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}
