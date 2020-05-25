using System;
using System.Collections.Generic;
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
    }
}
