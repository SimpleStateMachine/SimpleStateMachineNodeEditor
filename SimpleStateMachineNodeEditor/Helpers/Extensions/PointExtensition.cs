using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace SimpleStateMachineNodeEditor.Helpers.Extensions
{
    public static class PointExtensition
    {
        public static MyPoint ToMyPoint(this Point point)
        {
            return MyPoint.CreateFromPoint(point);
        }

        public static bool IsClear(this Point point)
        {
             return (point.X == 0) && (point.Y == 0);
        }
        public static Point Clear(this Point point)
        {
            return new Point(0,0);
        }
        public static Point Mirror(this Point point, bool onX = true, bool onY = true)
        {
            return new Point(onX ? -point.X : point.X, onY ? -point.Y : point.Y);
        }

        public static Point Copy(this Point point)
        {
            return new Point(point.X, point.Y);
        }


        public static Point Addition(this Point point1, Point point2)
        {
            return new Point(point1.X + point2.X, point1.Y + point2.Y);
        }
        public static Point Addition(this Point point1, int number)
        {
            return new Point(point1.X + number, point1.Y + number);
        }
        public static Point Addition(this Point point1, int x, int y)
        {
            return new Point(point1.X + x, point1.Y + y);
        }
        public static Point Addition(this Point point1, double number)
        {
            return new Point(point1.X + number, point1.Y + number);
        }
        public static Point Addition(this Point point1, double x, double y)
        {
            return new Point(point1.X + x, point1.Y + y);
        }



        public static Point Subtraction(this Point point1, Point point2)
        {
            return new Point(point1.X - point2.X, point1.Y - point2.Y);
        }

        public static Point Subtraction(this Point point1, int number)
        {
            return new Point(point1.X - number, point1.Y - number);
        }
        public static Point Subtraction(this Point point1, int x, int y)
        {
            return new Point(point1.X - x, point1.Y - y);
        }

        public static Point Subtraction(this Point point1, double number)
        {
            return new Point(point1.X - number, point1.Y - number);
        }
        public static Point Subtraction(this Point point1, double x, double y)
        {
            return new Point(point1.X - x, point1.Y - y);
        }


        public static Point Division(this Point point1, Point point2)
        {
            return new Point(point1.X / point1.X, point1.Y / point1.Y);
        }
        public static Point Division(this Point point1, int number)
        {
            return new Point(point1.X / number, point1.Y / number);
        }
        public static Point Division(this Point point1, int x, int y)
        {
            return new Point(point1.X / x, point1.Y / y);
        }
        public static Point Division(this Point point1, double number)
        {
            return new Point(point1.X / number, point1.Y / number);
        }
        public static Point Division(this Point point1, double x, double y)
        {
            return new Point(point1.X / x, point1.Y / y);
        }


        public static Point Multiply(this Point point1, Point point2)
        {
            return new Point(point1.X * point2.X, point1.Y * point2.Y);
        }
        public static Point Multiply(this Point point1, int number)
        {
            return new Point(point1.X * number, point1.Y * number);
        }
        public static Point Multiply(this Point point1, int x, int y)
        {
            return new Point(point1.X * x, point1.Y * y);
        }
        public static Point Multiply(this Point point1, double number)
        {
            return new Point(point1.X * number, point1.Y * number);
        }
        public static Point Multiply(this Point point1, double x, double y)
        {
            return new Point(point1.X * x, point1.Y * y);
        }
    }
}
