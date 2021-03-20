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
                AddHandler(DragDrop.DragOverEvent, HelperDragDrop.DragDraggableOverDelegate);
                AddHandler(DragDrop.DropEvent, HelperDragDrop.DropDraggableDelegate);
            });
        }


    }
}

