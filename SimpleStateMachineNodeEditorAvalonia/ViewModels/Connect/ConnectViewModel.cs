using Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class ConnectViewModel : BaseViewModel
    {
        [Reactive] public Point StartPoint { get; set; }
        [Reactive] public Point EndPoint { get; set; } = new Point(500, 500);
        [Reactive] public RightConnectorViewModel FromConnector { get; set; }
        [Reactive] public LeftConnectorViewModel ToConnector { get; set; }

        public ConnectViewModel(RightConnectorViewModel connectorFrom)
        {
            FromConnector = connectorFrom;
        }

        public void EndInit(LeftConnectorViewModel ConnectorTo)
        {
            ConnectorTo.WhenAnyValue(ct => ct.Position).BindTo(this, vm => vm.EndPoint);
        }
    }
}
