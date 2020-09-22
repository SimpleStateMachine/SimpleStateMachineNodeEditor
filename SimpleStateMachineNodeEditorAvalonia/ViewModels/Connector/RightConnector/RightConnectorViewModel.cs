using Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class RightConnectorViewModel : ConnectorViewModel
    {
        [Reactive] public ConnectViewModel Connect { get; set; }

        public RightConnectorViewModel(NodeViewModel node, string name = "", bool isEnable = true):base(node, name, isEnable)
        {
            //node.WhenAnyValue(x => x.Point1).Subscribe(v=>v)
            Position = new Point(100, 100);
        }
    }
}
