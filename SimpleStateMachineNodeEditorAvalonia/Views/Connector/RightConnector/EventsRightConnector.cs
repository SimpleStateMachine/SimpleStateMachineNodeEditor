using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using SimpleStateMachineNodeEditorAvalonia.Helpers.Extensions;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class RightConnector
    {
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                this.Events().DoubleTapped.Subscribe(OnTextBoxHeaderDoubleTappedEvent).DisposeWith(disposable);
                this.TextBoxConnector.Events().LostFocus.Subscribe(OnLostFocusEvent).DisposeWith(disposable);
                // this.Events().PointerPressed.Subscribe(OnRightConnectorPointerPressed).DisposeWith(disposable);
                EllipseConnector.Events().PointerPressed.Subscribe(OnEllipsePointerPressed).DisposeWith(disposable);
                // TextBoxConnector.AddHandler(PointerPressedEvent, OnTextBoxPointerPressed, RoutingStrategies.Bubble,true);
            });
           
        }
        protected virtual void OnTextBoxHeaderDoubleTappedEvent(RoutedEventArgs e)
        {
            if(!TextBoxConnector.IsEnabled)
                return;
            TextBoxConnector.IsHitTestVisible = true;
            TextBoxConnector.Focus();
            TextBoxConnector.CaretIndex = TextBoxConnector.Text.Length;
            TextBoxConnector.ClearSelection();
        }
        
        protected virtual void OnLostFocusEvent(RoutedEventArgs e)
        {
            TextBoxConnector.IsHitTestVisible = false;
        }
        
        
        
        protected virtual void OnRightConnectorPointerPressed(PointerPressedEventArgs e)
        {
            if (e.Source is not TextBox)
            {
                e.Source = this;
            }
            
            //empty connector will not be selected
            if (ViewModel.Name.IsNullOrEmpty())
            {
                e.Handled = true;
            }
        }
        protected virtual void OnTextBoxPointerPressed(object sender, PointerPressedEventArgs e)
        {
            //if without shift and control - connect will not be selected
            e.Handled = !e.HasShift() && !e.HasControl();
            e.Source = TextBoxConnector;
        }

        protected virtual void OnEllipsePointerPressed(PointerPressedEventArgs e)
        {
            var nodesCanvas = this.FindAncestorOfType<NodesCanvas>();
            var positionConnectPoint = EllipseConnector.TranslatePoint(new Point(EllipseConnector.Width / 2,
                EllipseConnector.Height / 2), this);
            positionConnectPoint = this.TranslatePoint(positionConnectPoint.Value, nodesCanvas);
            nodesCanvas.StartDrag(e, positionConnectPoint.Value);
   
            // positionConnectPoint = this.TranslatePoint(positionConnectPoint.Value, nodesCanvas);
            // e.Handled = true;
            // ViewModel.AddConnectCommand.ExecuteWithSubscribe(positionConnectPoint.Value);
            
            e.Handled = true;
        }
    }
}
