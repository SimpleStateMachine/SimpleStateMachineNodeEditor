using Avalonia;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public interface IDraggable
    {
        void DragOver(Point currentPosition);
        void Drop(bool success);
    }
}