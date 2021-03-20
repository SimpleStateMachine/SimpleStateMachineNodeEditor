using System;
using System.Reactive.Disposables;
using Avalonia.Input;
using SimpleStateMachineNodeEditorAvalonia.Helpers;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class LeftConnector
    {
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                this.Events().PointerMoved.Subscribe(OnRightConnectorPointerPressed).DisposeWith(disposable);
                this.EllipseConnector.AddHandler(DragDrop.DropEvent, HelperDragDrop.DropDraggableDelegate);
            });
        }
        private void OnRightConnectorPointerPressed(PointerEventArgs e)
        {
        }
        public void OnConnectDrop(object sender, PointerEventArgs e)
        {

        }
    }
}
