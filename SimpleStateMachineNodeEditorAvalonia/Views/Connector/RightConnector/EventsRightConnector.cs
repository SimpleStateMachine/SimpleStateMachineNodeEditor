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
using Avalonia.LogicalTree;
using SimpleStateMachineNodeEditorAvalonia.Helpers.Extensions;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class RightConnector
    {
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                base.SetupEvents();
                this.Events().PointerPressed.Subscribe(OnRightConnectorPointerPressed).DisposeWith(disposable);
                this.EllipseConnector.Events().PointerPressed.Subscribe(OnEllipsePointerPressed).DisposeWith(disposable);
                this.EllipseConnector.Events().PointerReleased.Subscribe(OnEllipsePointerReleased).DisposeWith(disposable);
                this.TextBoxConnector.AddHandler(TextBox.PointerPressedEvent, OnTextBoxPointerPressed, RoutingStrategies.Bubble,true);
            });
           
        }
        protected virtual void OnRightConnectorPointerPressed(PointerPressedEventArgs e)
        {
            if (e.Source is not TextBox)
            {
                e.Source = this;
            }
            
            if (this.ViewModel.Name.IsNullOrEmpty())
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
            var positionConnectPoint = this.EllipseConnector.TranslatePoint(new Point(EllipseConnector.Width / 2, EllipseConnector.Height / 2), this);
            positionConnectPoint = this.TranslatePoint(positionConnectPoint.Value, NodesCanvas.Current);
            
            e.Handled = true;
            this.ViewModel.AddConnectCommand.ExecuteWithSubscribe(positionConnectPoint.Value);
            DataObject data = new DataObject();
            data.SetDraggable(this.ViewModel.Connect);
            DragDrop.DoDragDrop(e, data, DragDropEffects.Move);
        }
        protected virtual void OnEllipsePointerReleased(PointerEventArgs e)
        {
            //Trace.WriteLine(e.GetPosition(this).ToString());
            //Console.WriteLine("232");
        }
    }
}
