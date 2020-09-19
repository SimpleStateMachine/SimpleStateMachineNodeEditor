using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;
using System;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class NodesCanvas : BaseView<NodesCanvasViewModel>
    {
        public NodesCanvas()
        {
            AddHandler(DragDrop.DragOverEvent, OnConnectDrop);
        }
        public void OnConnectDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine(e.GetPosition(this).ToString());
        }
    }
}
