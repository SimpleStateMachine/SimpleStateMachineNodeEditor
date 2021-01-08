using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class RightConnector
    {
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {     
                base.SetupEvents();
                this.EllipseConnector.Events().PointerPressed.Subscribe(e => OnEllipsePointerPressed(e)).DisposeWith(disposable);
                this.TextBoxConnector.AddHandler(TextBox.PointerPressedEvent, OnTextBoxPointerPressed, RoutingStrategies.Bubble,true);
            });
           
        }
        
        public void OnTextBoxPointerPressed(object sender, PointerPressedEventArgs e)
        {
            //for select connector on texbox click
            e.Handled = false;
            e.Source = this;
        }


        private void OnEllipsePointerPressed(PointerPressedEventArgs e)
        {
            //var t = EllipseConnector.FindAncestorOfType<RightConnector>(true);
            //if (this.IsVisible)
            //{
            //    var positionConnectPoint = this.EllipseConnector.TranslatePoint(new Point(EllipseConnector.Width / 2, EllipseConnector.Height / 2), this);

            //    positionConnectPoint = this.TranslatePoint(positionConnectPoint.Value, NodesCanvas.Current);
            //}
            this.ViewModel.AddConnectCommand.ExecuteWithSubscribe();
            e.Handled = true;
            //this.EllipseConnector.Events().PointerMoved.Subscribe(e => OnEllipsePointerReleased(e));
            //this.EllipseConnector.Poin += OnPointerMove;
            DataObject data = new DataObject();
            data.Set("Connect", this.ViewModel.Connect);
            DragDrop.DoDragDrop(e, data, DragDropEffects.Move);
        }
        
        private void OnEllipsePointerReleased(PointerEventArgs e)
        {
            //Trace.WriteLine(e.GetPosition(this).ToString());
            //Console.WriteLine("232");
        }
    }
}
