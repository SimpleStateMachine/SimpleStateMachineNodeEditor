using Avalonia;
using Avalonia.Input;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public interface IDraggable
    {
        void DragOver(object handler, DragEventArgs e);
        void Drop(object handler, DragEventArgs e);
    }
}