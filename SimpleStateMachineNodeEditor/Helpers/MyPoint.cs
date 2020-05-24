using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Windows;

namespace SimpleStateMachineNodeEditor.Helpers
{
    public class MyPoint : ReactiveObject
    {
        [Reactive] public Point Value { get; set; }

        public double X
        {
            get { return Value.X; }
        }

        public double Y
        {
            get { return Value.Y; }
        }

        public MyPoint() : this(0, 0)
        {

        }
        public MyPoint(double x = 0, double y = 0)
        {
            Set(x, y);
        }

        public MyPoint(Point point)
        {
            FromPoint(point);
        }
        public MyPoint(MyPoint point)
        {
            this.Set(point);
        }

        public bool IsClear
        {
            get
            {
                return (this.X == 0) && (this.Y == 0);
            }
        }

        public void Clear()
        {
            Value = new Point();
        }

        public void Mirror(bool onX = true, bool onY = true)
        {
            Value = new Point(onX ? -this.X : this.X, onY ? -this.Y : this.Y);
        }

        public MyPoint Add(MyPoint point)
        {
            Value = new Point(point.X, point.Y);
            return this;
        }

        public MyPoint Add(double x = 0, double y = 0)
        {
            Value = new Point(this.X + x, this.Y + y);
            return this;
        }

        public MyPoint Set(double x = 0, double y = 0)
        {
            Value = new Point(x, y);
            return this;
        }

        public MyPoint Set(Point point)
        {
            this.Set(point.X, point.Y);
            return this;
        }

        public MyPoint Set(MyPoint point)
        {
            this.Set(point.X, point.Y);
            return this;
        }

        public Point ToPoint()
        {
            return MyPoint.ToPoint(this);
        }

        public MyPoint FromPoint(Point point)
        {
            Set(point);
            return this;
        }
        
        public MyPoint Copy()
        {
            return new MyPoint(this);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", this.Value.X.ToString(System.Globalization.CultureInfo.InvariantCulture), this.Value.Y.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        #region Static Methods

        public static Point ToPoint(MyPoint point)
        {
            return (point != null) ? new Point(point.X, point.Y) : new Point();
        }

        public static MyPoint CreateFromPoint(Point point)
        {
            return (point != null) ? new MyPoint(point.X, point.Y) : new MyPoint();
        }

        public static MyPoint operator +(MyPoint point1, MyPoint point2)
        {
            return new MyPoint(point1.X + point2.X, point1.Y + point2.Y);
        }

        public static MyPoint operator -(MyPoint point1, MyPoint point2)
        {
            return new MyPoint(point1.X - point2.X, point1.Y - point2.Y);
        }

        public static MyPoint operator +(MyPoint point1, int number)
        {
            return new MyPoint(point1.X + number, point1.Y + number);
        }

        public static MyPoint operator -(MyPoint point1, int number)
        {
            return new MyPoint(point1.X - number, point1.Y - number);
        }

        public static MyPoint operator /(MyPoint point1, int number)
        {
            return new MyPoint(point1.X / number, point1.Y / number);
        }

        public static MyPoint operator *(MyPoint point1, int number)
        {
            return new MyPoint(point1.X * number, point1.Y * number);
        }

        public static MyPoint operator +(MyPoint point1, double number)
        {
            return new MyPoint(point1.X + number, point1.Y + number);
        }

        public static MyPoint operator -(MyPoint point1, double number)
        {
            return new MyPoint(point1.X - number, point1.Y - number);
        }

        public static MyPoint operator /(MyPoint point1, double number)
        {
            return new MyPoint(point1.X / number, point1.Y / number);
        }

        public static MyPoint operator *(MyPoint point1, double number)
        {
            return new MyPoint(point1.X * number, point1.Y * number);
        }

        public static MyPoint Parse(string str)
        {
            string[] parts = str.Split(",");
            return new MyPoint(double.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture), double.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture));
        }

        #endregion Static Methods

    }
}
