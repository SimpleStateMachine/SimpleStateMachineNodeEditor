using Avalonia.Input;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public static class DragEventArgsExtenstions
    {
        public static IDraggable GetDraggable(this DragEventArgs dragEventArgs)
        {
            return (dragEventArgs.Data as DataObject)?.GetDraggable();
        }
        
    }
}