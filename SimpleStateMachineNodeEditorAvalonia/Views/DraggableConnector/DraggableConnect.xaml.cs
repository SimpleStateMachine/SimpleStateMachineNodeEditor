using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;


namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class DraggableConnect : BaseView<DraggableConnectorViewModel>, IDraggable
    {
        private readonly PathGeometry PathGeometryConnect;
        private readonly PathFigure PathFigureConnect;
        private BezierSegment BezierSegmentConnect;
        private NodesCanvas _canvas;
        public DraggableConnect()
        {
            InitializeComponent();
            PathGeometryConnect = PathConnect.Data as PathGeometry;
            PathFigureConnect = PathGeometryConnect.Figures.First();
            BezierSegmentConnect = PathFigureConnect.Segments.First() as BezierSegment;
        }
        
        public void StartDrag(params (string key, object value)[] parameters)
        {
            dynamic t = new ExpandoObject();
            t.StartPoint = new Point();
            var startPositionPair = parameters.FirstOrDefault(x => x.key == "StartPosition");
            var startPosition = (Point) startPositionPair.value;
            this.IsVisible = true;
            this.ViewModel.StartPoint = startPosition;
            this.ViewModel.EndPoint = startPosition;
        }

        public void DragOver(object handler, DragEventArgs e)
        {
            if (handler is NodesCanvas canvas)
            {
                this.ViewModel.EndPoint = e.GetPosition(canvas) - new Point(2, 2);
            }
            // throw new System.NotImplementedException();
        }

        public void Drop(object handler, DragEventArgs e)
        {
            this.IsVisible = false;
            // throw new System.NotImplementedException();
        }
    }
}
