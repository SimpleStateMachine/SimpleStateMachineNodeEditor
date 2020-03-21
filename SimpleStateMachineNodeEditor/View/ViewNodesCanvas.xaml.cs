using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using ReactiveUI;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.ViewModel;
using SimpleStateMachineNodeEditor.Helpers.Transformations;

namespace SimpleStateMachineNodeEditor.View
{
    /// <summary>
    /// Interaction logic for ViewNodesCanvas.xaml
    /// </summary>
    public partial class ViewNodesCanvas : UserControl, IViewFor<ViewModelNodesCanvas>, CanBeMove
    {
        enum MoveNodes
        {
            No = 0,
            MoveAll,
            MoveSelected
        }
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(ViewModelNodesCanvas), typeof(ViewNodesCanvas), new PropertyMetadata(null));

        public ViewModelNodesCanvas ViewModel
        {
            get { return (ViewModelNodesCanvas)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ViewModelNodesCanvas)value; }
        }
        #endregion ViewModel

        private MyPoint PositionDragOver { get; set; } = new MyPoint();
        private MyPoint PositionRightClick { get; set; } = new MyPoint();
        private MyPoint PositionLeftClick { get; set; } = new MyPoint();
        private MyPoint PositionMove { get; set; } = new MyPoint();

        private MyPoint SumMove { get; set; } = new MyPoint();
        private MoveNodes Move { get; set; } = MoveNodes.No;

        public ViewNodesCanvas()
        {
            InitializeComponent();
            ViewModel = new ViewModelNodesCanvas();
            SetupBinding();
            SetupEvents();
            BindingCommands();
        }
        #region Setup Binding
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {
                this.OneWayBind(this.ViewModel, x => x.Nodes, x => x.Nodes.ItemsSource).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Connects, x => x.Connects.ItemsSource).DisposeWith(disposable);
                //this.OneWayBind(this.ViewModel, x => x.DraggedConnector, x => x.Connector.ViewModel).DisposeWith(disposable);

                //Масштаб по оси X
                this.OneWayBind(this.ViewModel, x => x.Scale.Scales.Value.X, x => x.Scale.ScaleX).DisposeWith(disposable);

                //Масштаб по оси Y
                this.OneWayBind(this.ViewModel, x => x.Scale.Scales.Value.Y, x => x.Scale.ScaleY).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Selector, x => x.Selector.ViewModel).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Cutter, x => x.Cutter.ViewModel).DisposeWith(disposable);
            });
        }
        #endregion Setup Binding

        #region Setup Commands
        private void BindingCommands()
        {
            this.WhenActivated(disposable =>
            {
                var positionLeftClickObservable = this.ObservableForProperty(x => x.PositionLeftClick).Select(x => x.Value);
                var positionRightClickObservable = this.ObservableForProperty(x => x.PositionRightClick).Select(x => x.Value);

                this.BindCommand(this.ViewModel, x => x.CommandRedo, x => x.BindingRedo).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandUndo, x => x.BindingUndo).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandSelectAll, x => x.BindingSelectAll).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandDeleteSelectedNodes, x => x.BindingDeleteNode).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandSelect, x => x.BindingSelect, positionLeftClickObservable).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandCut, x => x.BindingCut, positionLeftClickObservable).DisposeWith(disposable);



                this.BindCommand(this.ViewModel, x => x.CommandAddNode, x => x.BindingAddNode, positionLeftClickObservable).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandAddNode, x => x.ItemAddNode, positionRightClickObservable).DisposeWith(disposable);
                this.WhenAnyValue(x => x.ViewModel.Selector.Size).InvokeCommand(ViewModel.CommandSelectorIntersect).DisposeWith(disposable);
                this.WhenAnyValue(x => x.ViewModel.Cutter.EndPoint.Value).InvokeCommand(ViewModel.CommandCutterIntersect).DisposeWith(disposable);

            });
        }
        #endregion Setup Commands

        #region Setup Events
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                this.Events().MouseLeftButtonDown.Subscribe(e => OnEventMouseLeftDown(e)).DisposeWith(disposable);
                this.Events().MouseLeftButtonUp.Subscribe(e => OnEventMouseLeftUp(e));
                this.Events().MouseRightButtonDown.Subscribe(e => OnEventMouseRightDown(e)).DisposeWith(disposable);
                this.Events().MouseRightButtonUp.Subscribe(e => OnEventMouseRightUp(e)).DisposeWith(disposable);
                this.Events().MouseDown.Subscribe(e => OnEventMouseDown(e)).DisposeWith(disposable);
                this.Events().MouseUp.Subscribe(e => OnEventMouseUp(e)).DisposeWith(disposable);
                this.Events().MouseMove.Subscribe(e => OnEventMouseMove(e)).DisposeWith(disposable);
                this.Events().MouseWheel.Subscribe(e => OnEventMouseWheel(e)).DisposeWith(disposable);
                this.Events().DragOver.Subscribe(e => OnEventDragOver(e)).DisposeWith(disposable);
                this.Events().DragEnter.Subscribe(e => OnEventDragEnter(e)).DisposeWith(disposable);
                this.Events().DragLeave.Subscribe(e => OnEventDragLeave(e)).DisposeWith(disposable);
                //Эти события срабатывают раньше команд
                this.Events().PreviewMouseLeftButtonDown.Subscribe(e => OnEventPreviewMouseLeftButtonDown(e)).DisposeWith(disposable);
                this.Events().PreviewMouseRightButtonDown.Subscribe(e => OnEventPreviewMouseRightButtonDown(e)).DisposeWith(disposable);
                this.WhenAnyValue(x => x.ViewModel.Scale.Value).Subscribe(value => { this.Grid.Height /= value; this.Grid.Width /= value; }).DisposeWith(disposable);
            });
        }
        private void OnEventMouseLeftDown(MouseButtonEventArgs e)
        {
            PositionMove = new MyPoint(Mouse.GetPosition(this.Grid));

            if (Mouse.Captured == null)
            {
                Keyboard.ClearFocus();
                this.CaptureMouse();
                Keyboard.Focus(this);

                this.ViewModel.CommandUnSelectAll.Execute();
            }
        }
        private void UpdateConnector()
        {
            //this.Connector.Visibility = (this.ViewModel.DraggedConnector == null) ? Visibility.Collapsed : Visibility.Visible;

        }
        private void OnEventMouseLeftUp(MouseButtonEventArgs e)
        {
            if (Move == MoveNodes.No)
                return;

            if (Move == MoveNodes.MoveAll)
                this.ViewModel.CommandFullMoveAllNode.Execute(SumMove);
            else if (Move == MoveNodes.MoveSelected)
                this.ViewModel.CommandFullMoveAllSelectedNode.Execute(SumMove);

            Move = MoveNodes.No;
            SumMove = new MyPoint();
        }
        private void OnEventMouseRightDown(MouseButtonEventArgs e)
        {
            Keyboard.Focus(this);

        }
        private void OnEventMouseRightUp(MouseButtonEventArgs e)
        {
        }
        private void OnEventMouseDown(MouseButtonEventArgs e)
        {
        }
        private void OnEventMouseWheel(MouseWheelEventArgs e)
        {
            this.ViewModel.CommandZoom.Execute(e.Delta);
        }
        private void OnEventMouseUp(MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            PositionMove.Clear();
            Keyboard.Focus(this);
        }
        private void OnEventMouseMove(MouseEventArgs e)
        {
            //if ((Mouse.Captured == null)||(!(Mouse.Captured is CanBeMove)))
            if (!(Mouse.Captured is CanBeMove))
                return;


            MyPoint delta = GetDeltaMove();

            if (delta.IsClear)
                return;

            SumMove += delta;
            if (this.IsMouseCaptured)
            {
                ViewModel.CommandPartMoveAllNode.Execute(delta);
                Move = MoveNodes.MoveAll;
            }
            else
            {
                ViewModel.CommandPartMoveAllSelectedNode.Execute(delta);
                Move = MoveNodes.MoveSelected;
            }
        }
        private void OnEventDragEnter(DragEventArgs e)
        {

        }
        private void OnEventDragOver(DragEventArgs e)
        {
            MyPoint point = new MyPoint(e.GetPosition(this));
            if (this.ViewModel.DraggedConnect != null)
            {
                point -= 2;
                this.ViewModel.DraggedConnect.EndPoint.Set(point);
            }
        }
        private void OnEventDragLeave(DragEventArgs e)
        {

        }
        private void OnEventPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            PositionLeftClick.Set(e.GetPosition(this.Grid));
        }
        private void OnEventPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            PositionRightClick.Set(e.GetPosition(this.Grid));
        }

        #endregion Setup Events
        private MyPoint GetDeltaMove()
        {
            MyPoint CurrentPosition = new MyPoint(Mouse.GetPosition(this.Grid));
            MyPoint result = new MyPoint();

            if (!PositionMove.IsClear)
            {
                result = CurrentPosition - PositionMove;
            }

            PositionMove = CurrentPosition;
            return result;
        }
        private MyPoint GetDeltaDragOver(DragEventArgs e)
        {
            MyPoint CurrentPosition = new MyPoint(e.GetPosition(this));

            MyPoint result = new MyPoint();

            if (!PositionDragOver.IsClear)
            {
                result = CurrentPosition - PositionDragOver;
            }
            PositionDragOver = CurrentPosition;

            return result;
        }
    }
}
