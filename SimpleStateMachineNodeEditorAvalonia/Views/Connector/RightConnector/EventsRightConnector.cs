using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Views.NodeElements;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class RightConnector
    {
        protected override void SetupEvents()
        {
            base.SetupEvents();
            this.WhenViewModelAnyValue(disposable =>
            {
                this.EllipseConnector.Events().PointerPressed.Subscribe(e => ConnectDrag(e)).DisposeWith(disposable);
            });
        }

        private void ConnectDrag(PointerPressedEventArgs e)
        {
            this.ViewModel.AddConnectCommand.ExecuteWithSubscribe();
            DataObject data = new DataObject();
            data.Set("Connect", this.ViewModel.Connect);
            DragDrop.DoDragDrop(e, data, DragDropEffects.Move);
        }
    }
}
