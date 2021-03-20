using Avalonia.Input;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public static class DragEventArgsExtensions
    {
        public static IDraggable GetDraggable(this DragEventArgs dragEventArgs)
        {
            return (dragEventArgs.Data as DataObject)?.GetDraggable();
        }
        
    }
}