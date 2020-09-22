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
        public static NodesCanvas Current { get; private set; }
        public NodesCanvas()
        {
            Current = this;
            AddHandler(DragDrop.DragOverEvent, OnConnectDrop);
        }
        public void OnConnectDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine(e.GetPosition(this).ToString());
        }
    }
}
