using Avalonia.Input;
using Avalonia.Interactivity;
using System.Threading.Tasks;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public interface IDraggeble
    {
        void OnDragEnter(DragEventArgs e);
        void OnDragLeave(RoutedEventArgs e);
        void OnDrop(DragEventArgs e);
        void OnDragOver(DragEventArgs e);
    }

    //public static class IDraggeblaExtensions
    //{
    //    public static Task<DragDropEffects> DoDragDrop<TControl>(this TControl draggebla, PointerEventArgs triggerEvent, IDataObject data, DragDropEffects allowedEffects)
    //        where TControl : class, IDraggeble, IInteractive
    //    {
    //        DragDrop.DragEnterEvent.AddClassHandler<TControl>((x, e) => x.OnDragEnter(e));
    //        DragDrop.DragLeaveEvent.AddClassHandler<TControl>((x, e) => x.OnDragLeave(e));
    //        DragDrop.DragOverEvent.AddClassHandler<TControl>((x, e) => x.OnDragOver(e));
    //        DragDrop.DropEvent.AddClassHandler<TControl>((x, e) => x.OnDrop(e));

    //        return DragDrop.DoDragDrop(triggerEvent, data, allowedEffects);
    //    }
    //}
}
