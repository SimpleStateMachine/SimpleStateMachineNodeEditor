using Avalonia;
using Avalonia.Input;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class NodesCanvas : BaseView<NodesCanvasViewModel>
    {
        public NodesCanvas()
        {
            InitializeComponent();
        } 
        public void StartDrag(PointerEventArgs e, Point startPosition)
        {
            DataObject data = new DataObject();
            // data.SetDraggable();
            DragDrop.DoDragDrop(e, data, DragDropEffects.Move);
            
            // ViewModel.DraggableConnector = new DraggableConnectorViewModel(node, startPosition);
            // pointer.Capture(this.DraggableConnector);
        }
    }
}
