using System.Windows.Media;

namespace SimpleStateMachineNodeEditor.Helpers.Extensions
{
    public static class MatrixExtension
    {
        public static Matrix ScaleAt(double scaleX, double scaleY, double centerX, double centerY)
        {
            return new Matrix(scaleX, 0, 0, scaleY, centerX - (scaleX * centerX), centerY - (scaleY * centerY));
        }

        public static Matrix ScaleAtPrepend(Matrix matrix, double scaleX, double scaleY, double centerX, double centerY)
        {
            return ScaleAt(scaleX, scaleY, centerX, centerY) * matrix;
        }

        public static Matrix ScaleAtPrepend(Matrix matrix, double scaleX, double scaleY)
        {
            return ScaleAt2(scaleX, scaleY, matrix.OffsetX, matrix.OffsetY) * matrix;
        }
        public static Matrix ScaleAt2(double scaleX, double scaleY, double centerX, double centerY)
        {
            return new Matrix(scaleX, 0, 0, scaleY, 1, 1);
        }
    }
}
