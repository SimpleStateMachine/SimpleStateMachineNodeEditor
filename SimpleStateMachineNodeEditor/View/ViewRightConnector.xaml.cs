using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using ReactiveUI;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.ViewModel;
using SimpleStateMachineNodeEditor.Helpers.Transformations;
using SimpleStateMachineNodeEditor.Helpers.Enums;

namespace SimpleStateMachineNodeEditor.View
{
    /// <summary>
    /// Interaction logic for ViewConnector.xaml
    /// </summary>
    public partial class ViewRightConnector : UserControl, IViewFor<ViewModelConnector>, CanBeMove
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ViewModelConnector), typeof(ViewRightConnector), new PropertyMetadata(null));

        public ViewModelConnector ViewModel
        {
            get { return (ViewModelConnector)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ViewModelConnector)value; }
        }
        #endregion ViewModel
        public ViewRightConnector()
        {
            InitializeComponent();
            SetupCommands();
            SetupBinding();
            SetupEvents();          
        }

        #region SetupBinding
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {
                Canvas.SetZIndex((UIElement)this.VisualParent, this.ViewModel.Node.Zindex+2);

                this.OneWayBind(this.ViewModel, x => x.Visible, x => x.RightConnector.Visibility).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Name, x => x.TextBoxElement.Text).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.TextEnable, x => x.TextBoxElement.IsEnabled).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Foreground, x => x.TextBoxElement.Foreground).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.FormEnable, x => x.EllipseElement.IsEnabled).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.FormStroke, x => x.EllipseElement.Stroke).DisposeWith(disposable);

                this.Bind(this.ViewModel, x => x.FormFill, x => x.EllipseElement.Fill).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.FormStrokeThickness, x => x.EllipseElement.StrokeThickness).DisposeWith(disposable);

                this.WhenAnyValue(x => x.ViewModel.Node.Size, x => x.ViewModel.Node.Point1.Value, x => x.ViewModel.Node.NodesCanvas.Scale.Scales.Value)
                .Subscribe(_ => { UpdatePositionConnectPoin(); }).DisposeWith(disposable);

                this.WhenAnyValue(x =>x.ViewModel.ItsLoop).Subscribe(value=> test(value)).DisposeWith(disposable);
            });
        }
        #endregion SetupBinding
        #region Setup Commands
        private void SetupCommands()
        {
            this.WhenActivated(disposable =>
            {
                //this.BindCommand(this.ViewModel, x => x.CommandSelect, x => x.BindingSelectWithCtrl).DisposeWith(disposable);
                //this.BindCommand(this.ViewModel, x => x.CommandSelect, x => x.BindingSelectWithShift).DisposeWith(disposable);
            });
        }
        #endregion Setup Commands
        #region SetupEvents

        private void test(bool value)
        {
            if (value)
                this.ViewModel.CommandSetAsLoop.Execute();
        }
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                this.EllipseElement.Events().MouseLeftButtonDown.Subscribe(e => ConnectDrag(e)).DisposeWith(disposable);
                this.TextBoxElement.Events().LostFocus.Subscribe(e => Validate(e)).DisposeWith(disposable);
                //this.BorderElement.Events().PreviewMouseLeftButtonDown.Subscribe(e => ConnectorDrag(e)).DisposeWith(disposable);
                this.BorderElement.Events().PreviewDragEnter.Subscribe(e => ConnectorDragEnter(e)).DisposeWith(disposable);
                this.BorderElement.Events().PreviewDrop.Subscribe(e => ConnectorDrop(e)).DisposeWith(disposable);
            });
        }

        private void Validate(RoutedEventArgs e)
        {
            ViewModel.CommandValidateName.Execute(TextBoxElement.Text);
            if (TextBoxElement.Text != ViewModel.Name)
                TextBoxElement.Text = ViewModel.Name;
        }

        private void ConnectDrag(MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                this.ViewModel.CommandSetAsLoop.Execute();
                this.ViewModel.NodesCanvas.CommandAddConnectorWithConnect.Execute(this.ViewModel);

                this.ViewModel.NodesCanvas.LogDebug("Зашел 2 ");
            }          
            else 
            {
                this.ViewModel.CommandConnectPointDrag.Execute();
                DataObject data = new DataObject();
                data.SetData("Node", this.ViewModel.Node);
                DragDrop.DoDragDrop(this, data, DragDropEffects.Link);
                this.ViewModel.CommandCheckConnectPointDrop.Execute();
                e.Handled = true;

                this.ViewModel.NodesCanvas.LogDebug("Зашел 1 ");
            }
        }

        private void ConnectorDrag(MouseButtonEventArgs e)
        {        
            if (!this.ViewModel.TextEnable)
                return;
            if (Keyboard.IsKeyDown(Key.LeftAlt))
            {
                this.ViewModel.CommandConnectorDrag.Execute();
                DataObject data = new DataObject();
                data.SetData("Connector", this.ViewModel);
                DragDrop.DoDragDrop(this, data, DragDropEffects.Link);
            }
            else if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                this.ViewModel.CommandSelect.Execute(SelectMode.ClickWithShift);
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                this.ViewModel.CommandSelect.Execute(SelectMode.ClickWithCtrl);              
            }
            else
            {
                this.ViewModel.CommandSelect.Execute(SelectMode.Click);
                return;
            }
            e.Handled = true;
        }

        private void ConnectorDragEnter(DragEventArgs e)
        {
            if (this.ViewModel.NodesCanvas.ConnectorPreviewForDrop == null)
                return;

            if (this.ViewModel.NodesCanvas.ConnectorPreviewForDrop == this.ViewModel)
                return;

            this.ViewModel.CommandConnectorDragEnter.Execute();

            e.Handled = true;
        }
        private void ConnectorDrop(DragEventArgs e)
        {
            if (this.ViewModel.NodesCanvas.ConnectorPreviewForDrop == null)
                return;
           
            this.ViewModel.CommandConnectorDrop.Execute();

            e.Handled = true;
        }

        #endregion SetupEvents

        /// <summary>
        /// Обновить координату центра круга
        /// </summary>
        void UpdatePositionConnectPoin()
        {
            Point positionConnectPoint;
            MyPoint Position;

            if (this.IsVisible)
            {
                positionConnectPoint = EllipseElement.TranslatePoint(new Point(EllipseElement.Width/2, EllipseElement.Height / 2), this);

                ViewNodesCanvas NodesCanvas = MyUtils.FindParent<ViewNodesCanvas>(this);

                positionConnectPoint = this.TransformToAncestor(NodesCanvas).Transform(positionConnectPoint);

                Position  = MyPoint.CreateFromPoint(positionConnectPoint) / this.ViewModel.NodesCanvas.Scale.Value;

            }
            else
            {
                positionConnectPoint = this.ViewModel.Node.Output.PositionConnectPoint.Value;

                Position = MyPoint.CreateFromPoint(positionConnectPoint);
            }
         
            this.ViewModel.PositionConnectPoint.Set(Position);
        }

        //void UpdatePosition()
        //{
        //    if (!this.IsVisible)
        //        return;

        //    Point position = new Point();

        //    //Ищем Canvas
        //    ViewNodesCanvas NodesCanvas = MyUtils.FindParent<ViewNodesCanvas>(this);

        //    position = this.TransformToAncestor(NodesCanvas).Transform(position);

        //    this.ViewModel.Position.Set(position);

        //    this.ViewModel.NodesCanvas.Text = "UpdatePosition for " + this.ViewModel.Name;
        //}
    }
}
