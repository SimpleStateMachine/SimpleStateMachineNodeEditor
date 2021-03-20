using System.Linq;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;


namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class DraggableConnector : BaseView<DraggableConnectorViewModel>, IDraggable
    {
        private readonly PathGeometry PathGeometryConnect;
        private readonly PathFigure PathFigureConnect;
        private BezierSegment BezierSegmentConnect;
        private NodesCanvas _canvas;
        public DraggableConnector()
        {
            InitializeComponent();
            PathGeometryConnect = PathConnect.Data as PathGeometry;
            PathFigureConnect = PathGeometryConnect.Figures.First();
            BezierSegmentConnect = PathFigureConnect.Segments.First() as BezierSegment;
         
        }

        public void DragOver(object handler, DragEventArgs e)
        {
            // throw new System.NotImplementedException();
        }

        public void Drop(object handler, DragEventArgs e)
        {
            // throw new System.NotImplementedException();
        }
    }
}
