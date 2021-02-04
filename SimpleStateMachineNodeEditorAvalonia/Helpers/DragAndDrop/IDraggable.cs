using System;
using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public interface IDraggable
    {
        void DragOver(Point currentPosition);
        void Drop(bool success);
    }
}