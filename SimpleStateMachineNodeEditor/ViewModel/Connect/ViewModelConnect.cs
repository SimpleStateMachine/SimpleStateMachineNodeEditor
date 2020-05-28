using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Extensions;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelConnect : ReactiveObject
    {
        [Reactive] public Point StartPoint { get; set; }
        [Reactive] public Point EndPoint { get; set; }
        [Reactive] public Point Point1 { get; set; }
        [Reactive] public Point Point2 { get; set; }

        [Reactive] public Brush Stroke { get; set; } = Application.Current.Resources["ColorConnect"] as SolidColorBrush;

        [Reactive] public ViewModelConnector FromConnector { get; set; }

        [Reactive] public ViewModelConnector ToConnector { get; set; }

        [Reactive] public ViewModelNodesCanvas NodesCanvas { get; set; }

        [Reactive] public DoubleCollection StrokeDashArray { get; set; }

        [Reactive] public double StrokeThickness { get; set; } = 1;

        private IDisposable subscriptionOnConnectorPositionChange;
        private IDisposable subscriptionOnOutputPositionChange;

        public ViewModelConnect(ViewModelNodesCanvas viewModelNodesCanvas, ViewModelConnector fromConnector)
        {
            Initial(viewModelNodesCanvas, fromConnector);
            SetupSubscriptions();     
        }
        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.StartPoint, x => x.EndPoint).Subscribe(_ => UpdateMedium());
            this.WhenAnyValue(x => x.FromConnector.Node.IsCollapse).Subscribe(value => UpdateSubscriptionForPosition(value));           
            this.WhenAnyValue(x => x.ToConnector.PositionConnectPoint).Subscribe(value => EndPointUpdate(value));
            this.WhenAnyValue(x => x.FromConnector.Selected).Subscribe(value => Select(value));
            this.WhenAnyValue(x => x.NodesCanvas.Theme).Subscribe(_ => Select(this.FromConnector.Selected));
        }
        private void UpdateSubscriptionForPosition(bool nodeIsCollapse)
        {
            if(!nodeIsCollapse)
            {
                subscriptionOnOutputPositionChange?.Dispose();
                subscriptionOnConnectorPositionChange = this.WhenAnyValue(x => x.FromConnector.PositionConnectPoint).Subscribe(value => StartPointUpdate(value));
                
            }
            else
            {
                subscriptionOnConnectorPositionChange?.Dispose();
                subscriptionOnOutputPositionChange = this.WhenAnyValue(x => x.FromConnector.Node.Output.PositionConnectPoint).Subscribe(value => StartPointUpdate(value));              
            }
        }
        private void Initial(ViewModelNodesCanvas viewModelNodesCanvas, ViewModelConnector fromConnector)
        {
            NodesCanvas = viewModelNodesCanvas;
            FromConnector = fromConnector;
            FromConnector.Connect = this;           
        }
        private void Select(bool value)
        {
            //this.StrokeDashArray = value ? new DoubleCollection() { 10, 3 } : null;
            this.Stroke =  Application.Current.Resources[value ? "ColorSelectedElement": "ColorConnect"] as SolidColorBrush;
        }
        private void StartPointUpdate(Point point)
        {
            StartPoint = point;
        }
        private void EndPointUpdate(Point point)
        {
            EndPoint = point;
        }
        private void UpdateMedium()
        {
            Point different = EndPoint.Subtraction(StartPoint);
            Point1 = new Point(StartPoint.X + 3 * different.X / 8, StartPoint.Y + 1 * different.Y / 8);
            Point2 = new Point(StartPoint.X + 5 * different.X / 8, StartPoint.Y + 7 * different.Y / 8);
        }

        #endregion Setup Subscriptions

    }
}
