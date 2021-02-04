using Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Helpers;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class ConnectViewModel : BaseViewModel, IDraggable
    {
        [Reactive] public Point StartPoint { get; set; }
        [Reactive] public Point EndPoint { get; set; }
        [Reactive] public RightConnectorViewModel FromConnector { get; set; }
        [Reactive] public LeftConnectorViewModel ToConnector { get; set; }

        public ConnectViewModel(RightConnectorViewModel connectorFrom, Point point = default)
        {
            FromConnector = connectorFrom;
            StartPoint = point;
            EndPoint = point;
        }

        public void EndInit(LeftConnectorViewModel ConnectorTo)
        {
            ConnectorTo.WhenAnyValue(ct => ct.Position).BindTo(this, vm => vm.EndPoint);
        }
        
        public void DragOver(Point currentPosition)
        {
            this.EndPoint = currentPosition;
        }

        public void Drop(bool success)
        {
          
        }
    }
}
