using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Reactive.Disposables;
using ReactiveUI;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.ViewModel;
using SimpleStateMachineNodeEditor.Helpers.Transformations;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using System.Collections.Generic;

namespace SimpleStateMachineNodeEditor.View
{
    /// <summary>
    /// Interaction logic for ViewConnector.xaml
    /// </summary>
    public partial class RightConnector : UserControl, IViewFor<ConnectorViewModel>, CanBeMove
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ConnectorViewModel), typeof(RightConnector), new PropertyMetadata(null));

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
        public RightConnector()
        {
            InitializeComponent();
            SetupBinding();
            SetupEvents();
            SetupSubcriptions(); 
        }

        #region SetupBinding
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {

                this.OneWayBind(ViewModel, x => x.Visible, x => x.RightConnectorElement.Visibility).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.Name, x => x.TextBoxElement.Text).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.TextEnable, x => x.TextBoxElement.IsEnabled).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.Foreground, x => x.TextBoxElement.Foreground).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.FormEnable, x => x.EllipseElement.IsEnabled).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.FormStroke, x => x.EllipseElement.Stroke).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.FormFill, x => x.EllipseElement.Fill).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.FormStrokeThickness, x => x.EllipseElement.StrokeThickness).DisposeWith(disposable);


            });
        }
        #endregion SetupBinding

        #region Setup Subcriptions
        private void SetupSubcriptions()
        {
            this.WhenActivated(disposable =>
            {
                this.WhenAnyValue(x => x.EllipseElement.IsMouseOver).Subscribe(value => OnEventMouseOver(value)).DisposeWith(disposable);
            });
        }
        #endregion Setup Subcriptions

        #region SetupEvents

        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                EllipseElement.Events().MouseLeftButtonDown.Subscribe(e => ConnectDrag(e)).DisposeWith(disposable);
                TextBoxElement.Events().LostFocus.Subscribe(e => Validate(e)).DisposeWith(disposable);
                GridElement.Events().PreviewMouseLeftButtonDown.Subscribe(e => ConnectorDrag(e)).DisposeWith(disposable);
                GridElement.Events().PreviewDragEnter.Subscribe(e => ConnectorDragEnter(e)).DisposeWith(disposable);
                GridElement.Events().PreviewDrop.Subscribe(e => ConnectorDrop(e)).DisposeWith(disposable);
            });
        }
        private void OnEventMouseOver(bool value)
        {
                ViewModel.FormStroke = value ? Application.Current.Resources["ColorConnector"] as SolidColorBrush
                                                 : Application.Current.Resources["ColorNodesCanvasBackground"] as SolidColorBrush;
        }
        private void Validate(RoutedEventArgs e)
        {
            if (TextBoxElement.Text != ViewModel.Name)
                ViewModel.CommandValidateName.ExecuteWithSubscribe(TextBoxElement.Text);
            if (TextBoxElement.Text != ViewModel.Name)
                TextBoxElement.Text = ViewModel.Name;
        }

        private void ConnectDrag(MouseButtonEventArgs e)
        {
            if (ViewModel.NodesCanvas.ClickMode == NodeCanvasClickMode.Default)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    ViewModel.CommandSetAsLoop.ExecuteWithSubscribe();
                    ViewModel.NodesCanvas.CommandAddConnectorWithConnect.Execute(ViewModel);
                }
                else
                {
                    ViewModel.CommandConnectPointDrag.ExecuteWithSubscribe();
                    DataObject data = new DataObject();
                    data.SetData("Node", ViewModel.Node);
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                    ViewModel.CommandCheckConnectPointDrop.ExecuteWithSubscribe();
                    e.Handled = true;
                }
            }
        }

        private void ConnectorDrag(MouseButtonEventArgs e)
        {
            if (ViewModel.NodesCanvas.ClickMode == NodeCanvasClickMode.Default)
            {
                if (!ViewModel.TextEnable)
                    return;
                if (Keyboard.IsKeyDown(Key.LeftAlt))
                {
                    ViewModel.CommandConnectorDrag.ExecuteWithSubscribe();
                    DataObject data = new DataObject();
                    data.SetData("Connector", ViewModel);
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
                }
                else if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    ViewModel.CommandSelect.ExecuteWithSubscribe(SelectMode.ClickWithShift);
                }
                else if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    ViewModel.CommandSelect.ExecuteWithSubscribe(SelectMode.ClickWithCtrl);
                }
                else
                {
                    ViewModel.CommandSelect.ExecuteWithSubscribe(SelectMode.Click);
                    return;
                }
            } 
            else if (ViewModel.NodesCanvas.ClickMode == NodeCanvasClickMode.Cut)
            {
                if (ViewModel != ViewModel.Node.CurrentConnector)
                    ViewModel.NodesCanvas.CommandDeleteSelectedConnectors.Execute(new List<ConnectorViewModel>() {ViewModel });
            }
            else if (ViewModel.NodesCanvas.ClickMode == NodeCanvasClickMode.Select)
            {
                ViewModel.CommandSelect.ExecuteWithSubscribe(SelectMode.ClickWithCtrl);
            }
                e.Handled = true;
           
        }

        private void ConnectorDragEnter(DragEventArgs e)
        {
            if (ViewModel.NodesCanvas.ConnectorPreviewForDrop == null)
                return;

            if (ViewModel.NodesCanvas.ConnectorPreviewForDrop == ViewModel)
                return;
            ViewModel.CommandConnectorDragEnter.ExecuteWithSubscribe();
            e.Handled = true;
        }
        private void ConnectorDrop(DragEventArgs e)
        {
            if (ViewModel.NodesCanvas.ConnectorPreviewForDrop == null)
                return;
           
            ViewModel.CommandConnectorDrop.ExecuteWithSubscribe();

            e.Handled = true;
        }

        #endregion SetupEvents


        private  void UpdatePosition()
        {
            Point positionConnectPoint;

            if((!ViewModel.Node.IsCollapse)||(ViewModel.Node.IsCollapse && ViewModel.Name == "Output"))
            {
                positionConnectPoint = EllipseElement.TranslatePoint(new Point(EllipseElement.Width/2, EllipseElement.Height / 2), this);

                NodesCanvas NodesCanvas = MyUtils.FindParent<NodesCanvas>(this);

                positionConnectPoint = TransformToAncestor(NodesCanvas).Transform(positionConnectPoint);

                //positionConnectPoint = positionConnectPoint.Division(this.ViewModel.NodesCanvas.Scale.Value);

            }
            else
            {
                positionConnectPoint = ViewModel.Node.Output.PositionConnectPoint;

            }

            if (ViewModel.Name == "Output")
            {
                ViewModel.NodesCanvas.LogDebug(positionConnectPoint.ToString());
            }
            ViewModel.PositionConnectPoint = positionConnectPoint;
        }

    }
}
