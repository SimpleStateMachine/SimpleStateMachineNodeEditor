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
            (e.Data.Get("Connect") as ConnectViewModel).EndPoint = e.GetPosition(this);
            //e.Data["Connect"] as
            //DataObject data = new DataObject();
            //data.Set("Connect", this.ViewModel.Connect);
            //DragDrop.DoDragDrop(e, data, DragDropEffects.Move);
        }
    }
}
