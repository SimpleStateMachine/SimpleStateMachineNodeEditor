using System;
using Avalonia.Input;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public static class DataObjectExtensions
    {
        public static void SetDraggable(this DataObject dataObject, IDraggable draggable)
        {
            dataObject.Set(nameof(IDraggable), draggable);
        }
        
        public static IDraggable GetDraggable(this DataObject dataObject)
        {
            return dataObject.Get(nameof(IDraggable)) as IDraggable;
        }
    }
}