using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Diagnostics;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class RightConnector
    {
        protected override void SetupEvents()
        {
            base.SetupEvents();
            this.WhenViewModelAnyValue(disposable =>
            {
             
                //this.Eve
                //this.EllipseConnector.Events().PointerPressed.Subscribe(e => OnEllipsePointerPressed(e)).DisposeWith(disposable);
               //.DisposeWith(disposable);
                //this.Events().PointerPressed.Subscribe(e=>)
                // this.Events().PointerCaptureLost
            });
        }


        private void OnEllipsePointerPressed(PointerPressedEventArgs e)
        {
            //this.ViewModel.AddConnectCommand.ExecuteWithSubscribe();
            //this.EllipseConnector.Events().PointerMoved.Subscribe(e => OnEllipsePointerReleased(e));
            //this.EllipseConnector.Poin += OnPointerMove;
            //DataObject data = new DataObject();
            //data.Set("Connect", this.ViewModel.Connect);
            //DragDrop.DoDragDrop(e, data, DragDropEffects.Move);
        }

        private void OnEllipsePointerReleased(PointerEventArgs e)
        {
            Trace.WriteLine(e.GetPosition(this).ToString());
            //Console.WriteLine("232");
        }
    }
}
