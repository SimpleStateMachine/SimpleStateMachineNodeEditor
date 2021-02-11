using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connect
    {
        public PathGeometry PathGeometryConnect;
        public PathFigure PathFigureConnect;
        public BezierSegment BezierSegmentConnect;

        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            PathGeometryConnect = PathConnect.Data as PathGeometry;
            PathFigureConnect = PathGeometryConnect.Figures.First();
            BezierSegmentConnect = PathFigureConnect.Segments.First() as BezierSegment;
        }
    }
}
