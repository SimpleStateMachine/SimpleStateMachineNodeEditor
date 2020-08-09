using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
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

            PathConnect = this.FindControlWithExeption<Path>("PathConnect");
            PathGeometryConnect = this.FindWithExeption<PathGeometry>("PathGeometryConnect");
            PathFigureConnect = this.FindWithExeption<PathFigure>("PathFigureConnect");
            BezierSegmentConnect = this.FindWithExeption<BezierSegment>("BezierSegmentConnect");
        }
    }
}
