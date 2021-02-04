using System;
using Avalonia.Input;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public static class DataObjectExtensions
    {
        public static void SetDraggable(this DataObject dataObject, IDraggable draggable)
        {
            dataObject.Set(String.Empty, draggable);
        }
        
        public static IDraggable GetDraggable(this DataObject dataObject)
        {
            return dataObject.Get(String.Empty) as IDraggable;
        }
    }
}