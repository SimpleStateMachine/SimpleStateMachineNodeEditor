using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using SimpleStateMachineNodeEditor.ViewModel;

namespace SimpleStateMachineNodeEditor.View
{
    /// <summary>
    /// Interaction logic for ViewConnect.xaml
    /// </summary>
    public partial class Connect : UserControl, IViewFor<ConnectViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ConnectViewModel), typeof(Connect), new PropertyMetadata(null));

        public ConnectViewModel ViewModel
        {
            get { return (ConnectViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ConnectViewModel)value; }
        }
        #endregion ViewModel
        public Connect()
        {
            InitializeComponent();
            SetupBinding();
        }

        #region SetupBinding
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {
                Panel.SetZIndex((UIElement)VisualParent, 999);

                this.OneWayBind(ViewModel, x => x.Stroke, x => x.PathElement.Stroke).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.StartPoint, x => x.PathFigureElement.StartPoint).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.Point1, x => x.BezierSegmentElement.Point1).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.Point2, x => x.BezierSegmentElement.Point2).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.EndPoint, x => x.BezierSegmentElement.Point3).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.StrokeDashArray, x => x.PathElement.StrokeDashArray).DisposeWith(disposable);

                //this.OneWayBind(this.ViewModel, x => x.FromConnector.NodesCanvas.Scale.Value, x => x.PathElement.StrokeThickness).DisposeWith(disposable);

                this.WhenAnyValue(x => x.ViewModel.ToConnector).Where(x=>x!=null).Subscribe(_ => UpdateZindex()).DisposeWith(disposable);
            });
        }

        private void UpdateZindex()
        {
            ViewModel.WhenAnyValue(
                    x => x.FromConnector.Node.Point1.X, 
                    x=>x.ToConnector.Node.Point1.X)
                    .Subscribe(x => 
                    Panel.SetZIndex((UIElement) VisualParent,  (int)Math.Min(
                        ViewModel.FromConnector.Node.Point1.X, 
                        ViewModel.ToConnector.Node.Point1.X)));
        }
        #endregion SetupBinding
    }
}
