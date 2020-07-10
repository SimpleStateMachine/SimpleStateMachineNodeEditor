using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connect
    {
        public Path PathConnect;
        public PathGeometry PathGeometryConnect;
        public PathFigure PathFigureConnect;
        public BezierSegment BezierSegmentConnect;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            PathConnect = this.FindControl<Path>("PathConnect");
            PathGeometryConnect = this.Find<PathGeometry>("PathGeometryConnect");
            PathFigureConnect = this.Find<PathFigure>("PathFigureConnect");
            BezierSegmentConnect = this.Find<BezierSegment>("BezierSegmentConnect");
        }
    }
}
