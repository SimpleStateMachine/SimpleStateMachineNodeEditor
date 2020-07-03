using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ReactiveUI;
using System;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Reactive.Linq;
using System.Windows.Markup;
using SimpleStateMachineNodeEditor.ViewModel;
using SimpleStateMachineNodeEditor.Helpers;

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
                Canvas.SetZIndex((UIElement)this.VisualParent, 999);

                this.OneWayBind(this.ViewModel, x => x.Stroke, x => x.PathElement.Stroke).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.StartPoint, x => x.PathFigureElement.StartPoint).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Point1, x => x.BezierSegmentElement.Point1).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Point2, x => x.BezierSegmentElement.Point2).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.EndPoint, x => x.BezierSegmentElement.Point3).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.StrokeDashArray, x => x.PathElement.StrokeDashArray).DisposeWith(disposable);

                //this.OneWayBind(this.ViewModel, x => x.FromConnector.NodesCanvas.Scale.Value, x => x.PathElement.StrokeThickness).DisposeWith(disposable);

                this.WhenAnyValue(x => x.ViewModel.ToConnector).Where(x=>x!=null).Subscribe(_ => UpdateZindex()).DisposeWith(disposable);
            });
        }

        private void UpdateZindex()
        {
            int toIndex = this.ViewModel.ToConnector.Node.Zindex;
            int fromIndex = this.ViewModel.FromConnector.Node.Zindex;
           
            Canvas.SetZIndex((UIElement)this.VisualParent, Math.Min(toIndex, fromIndex));
        }
        #endregion SetupBinding
    }
}
