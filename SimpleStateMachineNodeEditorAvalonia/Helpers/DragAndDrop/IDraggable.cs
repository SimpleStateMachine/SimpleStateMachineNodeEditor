using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Input;
using SimpleStateMachineNodeEditorAvalonia.Views;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public interface IDraggable
    {
        void StartDrag(params (string key, object value)[] parameters);
        void DragOver(object handler, DragEventArgs e);
        void Drop(object handler, DragEventArgs e);
    }
    
    public static class IDraggableExtensions
    {
        public static Task<DragDropEffects> DoDragDrop(this IDraggable draggable, PointerEventArgs e, params (string key, object value)[] parameters)
        {
            DataObject data = new DataObject();
            data.SetDraggable(draggable);
            draggable.StartDrag(parameters);
            return DragDrop.DoDragDrop(e, data, DragDropEffects.Move);
        }
    }
}