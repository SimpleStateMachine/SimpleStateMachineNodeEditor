using Avalonia.Input;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public static class HelperDragDrop
    {
        public static void DragDraggableOverDelegate(object handler, DragEventArgs e)
        {
            e.GetDraggable()?.DragOver(handler, e);
        }
        
        public static void DropDraggableDelegate(object handler, DragEventArgs e)
        {
            e.GetDraggable()?.DragOver(handler, e);
        }
    }
}