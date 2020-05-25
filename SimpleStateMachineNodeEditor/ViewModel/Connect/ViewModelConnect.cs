using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using SimpleStateMachineNodeEditor.ViewModel.NodesCanvas;
using SimpleStateMachineNodeEditor.ViewModel.Connector;

namespace SimpleStateMachineNodeEditor.ViewModel.Connect
{
    public class ViewModelConnect : ReactiveObject
    {
        [Reactive] public MyPoint StartPoint { get; set; } = new MyPoint();
        [Reactive] public MyPoint EndPoint { get; set; } = new MyPoint();
        [Reactive] public MyPoint Point1 { get; set; } = new MyPoint();
        [Reactive] public MyPoint Point2 { get; set; } = new MyPoint();

        [Reactive] public Brush Stroke { get; set; } = Application.Current.Resources["ColorConnect"] as SolidColorBrush;

        [Reactive] public ViewModelConnector FromConnector { get; set; }

        [Reactive] public ViewModelConnector ToConnector { get; set; }

        [Reactive] public ViewModelNodesCanvas NodesCanvas { get; set; }

        [Reactive] public DoubleCollection StrokeDashArray { get; set; }

        [Reactive] public double StrokeThickness { get; set; } = 1;

        public ViewModelConnect(ViewModelNodesCanvas viewModelNodesCanvas, ViewModelConnector fromConnector)
        {
            SetupSubscriptions();
            Initial(viewModelNodesCanvas, fromConnector);
            
        }
        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.StartPoint.Value, x => x.EndPoint.Value).Subscribe(_ => UpdateMedium());
            this.WhenAnyValue(x => x.FromConnector.PositionConnectPoint.Value).Subscribe(value => StartPointUpdate(value));
            this.WhenAnyValue(x => x.ToConnector.PositionConnectPoint.Value).Subscribe(value => EndPointUpdate(value));                     
            this.WhenAnyValue(x => x.ToConnector).Where(x => x != null).Subscribe(_ => ToConnectChanged());
            this.WhenAnyValue(x => x.FromConnector.Node.NodesCanvas.Scale.Value).Subscribe(value => StrokeThickness = value);      
        }
        private void Initial(ViewModelNodesCanvas viewModelNodesCanvas, ViewModelConnector fromConnector)
        {
            NodesCanvas = viewModelNodesCanvas;
            FromConnector = fromConnector;
            FromConnector.Connect = this;

            StartPointUpdate((FromConnector.Node.IsCollapse ? FromConnector.Node.Output : FromConnector).PositionConnectPoint.ToPoint());
            this.WhenAnyValue(x => x.FromConnector.Selected).Subscribe(value => Select(value));
        }
        private void Select(bool value)
        {
            //this.StrokeDashArray = value ? new DoubleCollection() { 10, 3 } : null;
            this.Stroke = value ? Application.Current.Resources["ColorSelectedElement"].Cast<SolidColorBrush>(): Application.Current.Resources["ColorConnect"].Cast<SolidColorBrush>();
        }
        private void ToConnectChanged()
        {
            EndPointUpdate(ToConnector.PositionConnectPoint.ToPoint());
        }
        private void StartPointUpdate(Point point)
        {
            StartPoint.Set(point);
        }
        private void EndPointUpdate(Point point)
        {
            EndPoint.Set(point);
        }
        private void UpdateMedium()
        {
            MyPoint different = EndPoint - StartPoint;
            Point1.Set(StartPoint.X + 3 * different.X / 8, StartPoint.Y + 1 * different.Y / 8);
            Point2.Set(StartPoint.X + 5 * different.X / 8, StartPoint.Y + 7 * different.Y / 8);
        }

        #endregion Setup Subscriptions

    }
}
