using System.Linq;
using Avalonia.Media;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connect : BaseView<ConnectViewModel>
    {
        public PathGeometry PathGeometryConnect;
        public PathFigure PathFigureConnect;
        public BezierSegment BezierSegmentConnect;
        public Connect()
        {
            InitializeComponent();
            PathGeometryConnect = PathConnect.Data as PathGeometry;
            PathFigureConnect = PathGeometryConnect.Figures.First();
            BezierSegmentConnect = PathFigureConnect.Segments.First() as BezierSegment;
        }
    }
}
