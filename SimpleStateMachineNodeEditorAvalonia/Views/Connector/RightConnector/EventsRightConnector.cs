using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Reactive.Disposables;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using SimpleStateMachineNodeEditorAvalonia.Helpers.Extensions;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class RightConnector
    {
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                this.Events().PointerPressed.Subscribe(OnRightConnectorPointerPressed).DisposeWith(disposable);
                EllipseConnector.Events().PointerPressed.Subscribe(OnEllipsePointerPressed).DisposeWith(disposable);
                EllipseConnector.Events().PointerReleased.Subscribe(OnEllipsePointerReleased).DisposeWith(disposable);
                TextBoxConnector.AddHandler(PointerPressedEvent, OnTextBoxPointerPressed, RoutingStrategies.Bubble,true);
            });
           
        }
        protected virtual void OnRightConnectorPointerPressed(PointerPressedEventArgs e)
        {
            if (e.Source is not TextBox)
            {
                e.Source = this;
            }
            
            if (ViewModel.Name.IsNullOrEmpty())
            {
                e.Handled = true;
            }
        }
        protected virtual void OnTextBoxPointerPressed(object sender, PointerPressedEventArgs e)
        {
            //for select connector on texbox click
            e.Handled = false;
            e.Source = TextBoxConnector;
        }

        protected virtual void OnEllipsePointerPressed(PointerPressedEventArgs e)
        {
            var positionConnectPoint = EllipseConnector.TranslatePoint(new Point(EllipseConnector.Width / 2, EllipseConnector.Height / 2), this);
            positionConnectPoint = this.TranslatePoint(positionConnectPoint.Value, NodesCanvas.Current);
            
            e.Handled = true;
            ViewModel.AddConnectCommand.ExecuteWithSubscribe(positionConnectPoint.Value);
            DataObject data = new DataObject();
            data.SetDraggable(ViewModel.Connect);
            DragDrop.DoDragDrop(e, data, DragDropEffects.Move);
        }
        protected virtual void OnEllipsePointerReleased(PointerEventArgs e)
        {
            //Trace.WriteLine(e.GetPosition(this).ToString());
            //Console.WriteLine("232");
        }
    }
}
