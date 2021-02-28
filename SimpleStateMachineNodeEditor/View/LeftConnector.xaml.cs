using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Reactive.Disposables;

using ReactiveUI;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.ViewModel;
using SimpleStateMachineNodeEditor.Helpers.Extensions;

namespace SimpleStateMachineNodeEditor.View
{
    /// <summary>
    /// Interaction logic for ViewLeftConnector.xaml
    /// </summary>
    public partial class LeftConnector : UserControl, IViewFor<ConnectorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ConnectorViewModel), typeof(LeftConnector), new PropertyMetadata(null));

        public ConnectorViewModel ViewModel
        {
            get { return (ConnectorViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ConnectorViewModel)value; }
        }
        #endregion ViewModel
        public LeftConnector()
        {
            InitializeComponent();
            SetupBinding();
            SetupEvents();
        }

        #region SetupBinding
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {

                this.OneWayBind(ViewModel, x => x.Name, x => x.TextBoxElement.Text).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.TextEnable, x => x.TextBoxElement.IsEnabled).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.FormEnable, x => x.EllipseElement.IsEnabled).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.Foreground, x => x.TextBoxElement.Foreground).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.FormStroke, x => x.EllipseElement.Stroke).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.FormFill, x => x.EllipseElement.Fill).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.Visible, x => x.LeftConnectorElement.Visibility).DisposeWith(disposable);

            });
        }
        #endregion SetupBinding

        #region SetupEvents
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {               
                EllipseElement.Events().Drop.Subscribe(e => OnEventDrop(e)).DisposeWith(disposable);
                EllipseElement.Events().DragEnter.Subscribe(e => OnEventDragEnter(e)).DisposeWith(disposable);
                EllipseElement.Events().DragLeave.Subscribe(e => OnEventDragLeave(e)).DisposeWith(disposable);
            });
        }

        #endregion SetupEvents
        private void OnEventDragEnter(DragEventArgs e)
        {
            ViewModel.FormStroke = Application.Current.Resources["ColorConnector"] as SolidColorBrush;
            e.Handled = true;
        }
        private void OnEventDragLeave(DragEventArgs e)
        {
            ViewModel.FormStroke = Application.Current.Resources["ColorNodesCanvasBackground"] as SolidColorBrush;
            e.Handled = true;
        }
        private void OnEventDrop(DragEventArgs e)
        {
            ViewModel.FormStroke = Application.Current.Resources["ColorNodesCanvasBackground"] as SolidColorBrush;
            ViewModel.CommandConnectPointDrop.ExecuteWithSubscribe();
            e.Handled = true;
        }
        void UpdatePosition()
        {
            Point positionConnectPoint = EllipseElement.TranslatePoint(new Point(EllipseElement.Width/2, EllipseElement.Height / 2), this);

            NodesCanvas NodesCanvas = MyUtils.FindParent<NodesCanvas>(this);
            if (NodesCanvas == null)
                return;

            positionConnectPoint = TransformToAncestor(NodesCanvas).Transform(positionConnectPoint);

            //this.ViewModel.PositionConnectPoint = positionConnectPoint.Division(this.ViewModel.NodesCanvas.Scale.Value);
            ViewModel.PositionConnectPoint = positionConnectPoint;
        }
    }
}
