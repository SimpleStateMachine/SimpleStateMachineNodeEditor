using ReactiveUI.Fody.Helpers;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class NodesCanvasViewModel : BaseViewModel
    {
        [Reactive] public NodesNodesCanvasViewModel Nodes { get; set; }
        [Reactive] public ConnectsNodesCanvasViewModel Connects { get; set; } = new();
        [Reactive] public SelectorViewModel Selector { get; set; } = new();
        [Reactive] public CutterViewModel Cutter { get; set; } = new();
        [Reactive] public DraggableConnectorViewModel DraggableConnector { get; set; }

        public NodesCanvasViewModel()
        {
            Nodes = new NodesNodesCanvasViewModel(this);
        }
    }
}
