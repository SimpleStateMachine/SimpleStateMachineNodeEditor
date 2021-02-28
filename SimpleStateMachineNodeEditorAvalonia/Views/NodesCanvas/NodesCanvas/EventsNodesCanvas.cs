using Avalonia.Input;
using SimpleStateMachineNodeEditorAvalonia.Helpers;


namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class NodesCanvas
    {
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                AddHandler(DragDrop.DragOverEvent, OnNodesCanvasDragOver);
                AddHandler(DragDrop.DropEvent, OnNodesCanvasDrop);
            });
        }
        
        void OnNodesCanvasDragOver(object handler, DragEventArgs e)
        {
            e.GetDraggable().DragOver(e.GetPosition(this));
        }
        
        void OnNodesCanvasDrop(object handler, DragEventArgs e)
        {
            e.GetDraggable().Drop(false);
        }
    }
}

