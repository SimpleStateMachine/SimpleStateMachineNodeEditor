using System.Windows;
using System.Windows.Media;
using System;
using System.Collections.Generic;

namespace SimpleStateMachineNodeEditor.Helpers
{
    public static class MyUtils
    {
        public static TParent FindParent<TParent>(DependencyObject currentObject) where TParent : DependencyObject
        {
            DependencyObject foundObject = currentObject;
            TParent result = default(TParent);
            do
            {
                   foundObject = VisualTreeHelper.GetParent(foundObject);

                if (foundObject == default(DependencyObject))
                    break;

                result = foundObject as TParent;

            } while (result == default(TParent));

            return result;
        }

        public static void FindParents<TParent1, TParent2>(DependencyObject currentObject, out TParent1 parent1, out TParent2 parent2) where TParent1 : DependencyObject where TParent2 : DependencyObject
        {
            DependencyObject foundObject = currentObject;

            parent1 = default(TParent1);
            parent2 = default(TParent2);

            do
            {
                foundObject = VisualTreeHelper.GetParent(foundObject);

                if (foundObject == default(DependencyObject))
                    break;

                if (parent1 == default(TParent1))
                {
                    parent1 = foundObject as TParent1;
                    continue;
                }

                if (parent2 == default(TParent2))
                {
                    parent2 = foundObject as TParent2;
                    continue;
                }

            } while ((parent1 == default(TParent1)) || (parent2 == default(TParent2)));
        }

        public static bool Intersect(MyPoint a1, MyPoint a2, MyPoint b1, MyPoint b2)
        {
            bool par1 = a1.X > b2.X; //второй перед первым
            bool par2 = b1.X > a2.X; //первый перед вторым
            bool par3 = a1.Y > b2.Y; //первый под вторым
            bool par4 = b1.Y > a2.Y; //второй под первым
            //если хоть одно условие выполняется - прямоугольники не пересекаются
            return !(par1 || par2 || par3 || par4);
        }

        public static MyPoint GetStartPointDiagonal(MyPoint a1, MyPoint b1)
        {
            return new MyPoint(Math.Min(a1.X, b1.X), Math.Min(a1.Y, b1.Y));
        }
        public static MyPoint GetEndPointDiagonal(MyPoint a1, MyPoint b1)
        {
            return new MyPoint(Math.Max(a1.X, b1.X), Math.Max(a1.Y, b1.Y));
        }

        #region Check on intersections curve Bezier and line
        // based on https://www.particleincell.com/2013/cubic-line-intersection/
        /*
         You can read more:
         https://github.com/w8r/bezier-intersect/blob/master/dist/bezier-intersect.js
         https://math.stackexchange.com/questions/2347733/intersections-between-a-cubic-b%C3%A9zier-curve-and-a-line
         https://math.stackexchange.com/questions/1337440/cubic-bezier-curve-and-a-straight-line-intersection/
         */


        //Gets coefficients of curve Bezier 
        private static Point[] bezierCoeffs(MyPoint bezierStartPoint, MyPoint bezierPoint1, MyPoint bezierPoint2, MyPoint bezierEndPoint)
        {
            Point[] coeffs = new Point[4];
            double bezierStartPointX_M_3 = bezierStartPoint.X * 3.0;
            double bezierPoint1X_M_3 = bezierPoint1.X * 3.0;
            double bezierPoint2X_M_3 = bezierPoint2.X * 3.0;

            coeffs[0].X = -bezierStartPoint.X + bezierPoint1X_M_3 - bezierPoint2X_M_3 + bezierEndPoint.X;
            coeffs[1].X = bezierStartPointX_M_3 - 6.0 * bezierPoint1.X + bezierPoint2X_M_3;
            coeffs[2].X = -bezierStartPointX_M_3 + bezierPoint1X_M_3;
            coeffs[3].X = bezierStartPoint.X;

            double bezierStartPointY_M_3 = bezierStartPoint.Y * 3.0;
            double bezierPoint1Y_M_3 = bezierPoint1.Y * 3.0;
            double bezierPoint2Y_M_3 = bezierPoint2.Y * 3.0;

            coeffs[0].Y = -bezierStartPoint.Y + bezierPoint1Y_M_3 - bezierPoint2Y_M_3 + bezierEndPoint.Y;
            coeffs[1].Y = bezierStartPointY_M_3 - 6.0 * bezierPoint1.Y + bezierPoint2Y_M_3;
            coeffs[2].Y = -bezierStartPointY_M_3 + bezierPoint1Y_M_3;
            coeffs[3].Y = bezierStartPoint.Y;

            return coeffs;
        }

        //Find cubic roots
        private static double[] cubicRoots(double a, double b, double c, double d)
        {
            double A = b / a;
            double B = c / a;
            double C = d / a;

            double Q, R, D, S, T, Im;

            Q = (3.0 * B - Math.Pow(A, 2.0)) / 9.0;
            R = (9.0 * A * B - 27.0 * C - 2.0 * Math.Pow(A, 3.0)) / 54.0;
            D = Math.Pow(Q, 3.0) + Math.Pow(R, 2.0);

            double[] t = new double[3];

            if (D >= 0)
            {
                #region some optimization

                double sqrtD = Math.Sqrt(D);
                double _AD3 = -A / 3.0;
                double RAsqrtD = R + sqrtD;
                double RSsqrtD = R - sqrtD;
                double D13 = (1.0 / 3.0);

                #endregion some optimization

                S = Math.Sign(RAsqrtD) * Math.Pow(Math.Abs(RAsqrtD), D13);
                T = Math.Sign(RSsqrtD) * Math.Pow(Math.Abs(RSsqrtD), D13);

                //some optimization
                double SST = (S + T);

                t[0] = _AD3 + SST;
                t[1] = _AD3 - SST / 2.0;
                t[2] = t[1];
                Im = Math.Abs(Math.Sqrt(3.0) * (S - T) / 2.0);

                if (Im != 0)
                {
                    t[1] = -1;
                    t[2] = -1;
                }
            }
            else
            {

                #region some optimization
                double th = Math.Acos(R / Math.Sqrt(-Math.Pow(Q, 3.0)));
                double sqrt_QM2 = 2.0 * Math.Sqrt(-Q);
                double AD3 = A / 3.0; 

                double thD3 = (th / 3.0);
                double PIM2D3 = (Math.PI * 2.0) / 3.0;
                #endregion some optimization

                t[0] = sqrt_QM2 * Math.Cos(thD3) - AD3;
                t[1] = sqrt_QM2 * Math.Cos(thD3 + PIM2D3) - AD3;
                t[2] = sqrt_QM2 * Math.Cos(thD3 + 2.0 * PIM2D3) - AD3;
            }

            for (var i = 0; i < 3; i++)
                if (t[i] < 0 || t[i] > 1.0)
                    t[i] = -1;

            Func<double[], double[]> sortSpecial = array =>
            {
                bool flip;
                double temp;
                do
                {
                    flip = false;
                    for (var i = 0; i < array.Length - 1; i++)
                    {
                        if ((array[i + 1] >= 0 && array[i] > array[i + 1]) || (array[i] < 0 && array[i + 1] >= 0))
                        {
                            flip = true;
                            temp = array[i];
                            array[i] = array[i + 1];
                            array[i + 1] = temp;

                        }
                    }
                } while (flip);

                return array;
            };
            return sortSpecial(t);
        }

        //Check on intersections curve Bezier and line
        public static bool ComputeIntersections(MyPoint bezierStartPoint, MyPoint bezierPoint1, MyPoint bezierPoint2, MyPoint bezierEndPoint, MyPoint lineStartPoint, MyPoint lineEndPoint)
        {

            // coefficients of line
            double A = lineEndPoint.Y - lineStartPoint.Y;// A = y2 - y1
            double B = lineStartPoint.X - lineEndPoint.X;// B = x1 - x2
            double C = -lineStartPoint.X * A - lineStartPoint.Y * B;//C=x1*(y1-y2)+y1*(x2-x1) = x1*(-A)+y1*(-B)=-x1*(A)-y1*(B)

            // coefficients of curve Bezier 
            var coeffs = bezierCoeffs(bezierStartPoint, bezierPoint1, bezierPoint2, bezierEndPoint);

            double[] P = new double[4];

            // Transform cubic coefficients to line's coordinate system
            P[0] = A * coeffs[0].X + B * coeffs[0].Y;// t^3
            P[1] = A * coeffs[1].X + B * coeffs[1].Y;// t^2
            P[2] = A * coeffs[2].X + B * coeffs[2].Y;// t
            P[3] = A * coeffs[3].X + B * coeffs[3].Y + C;

            //find roots of cubic
            var r = cubicRoots(P[0], P[1], P[2], P[3]);

            List<MyPoint> X = new List<MyPoint>();
            double t;
            MyPoint p;
            double s;
            double tMt;
            double tMtMt;
            for (int i = 0; i < 3; i++)
            {
                t = r[i];
                #region some optimization

                tMt = t * t; 
                tMtMt = tMt * t; 

                #endregion some optimization
                p = new MyPoint
                (
                   coeffs[0].X * tMtMt + coeffs[1].X * tMt + coeffs[2].X * t + coeffs[3].X,
                   coeffs[0].Y * tMtMt + coeffs[1].Y * tMt + coeffs[2].Y * t + coeffs[3].Y
               );

                if ((lineEndPoint.X - lineStartPoint.X) != 0) // if not vertical line
                    s = (p.X - lineStartPoint.X) / (lineEndPoint.X - lineStartPoint.X);
                else
                    s = (p.Y - lineStartPoint.Y) / (lineEndPoint.Y - lineStartPoint.Y);

                //check point in bounds
                if (t < 0 || t > 1.0 || s < 0 || s > 1.0)
                {
                    continue;
                }

                X.Add(p);
            }
            return X.Count > 0;
        }
        #endregion Check on intersections curve Bezier and line
    }
}
