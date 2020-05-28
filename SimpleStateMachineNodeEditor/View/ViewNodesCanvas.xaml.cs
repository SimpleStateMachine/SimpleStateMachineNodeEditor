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
using SimpleStateMachineNodeEditor.Helpers.Transformations;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditor.ViewModel;

namespace SimpleStateMachineNodeEditor.View
{
    /// <summary>
    /// Interaction logic for ViewNodesCanvas.xaml
    /// </summary>
    public partial class ViewNodesCanvas : UserControl, IViewFor<ViewModelNodesCanvas>, CanBeMove
    {
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
        private Point PositionMove { get; set; }

        private Point SumMove { get; set; }
        private TypeMove Move { get; set; } = TypeMove.None;

        public ViewNodesCanvas()
        {
            InitializeComponent();
            ViewModel = new ViewModelNodesCanvas();
            SetupCommands();
            SetupSubscriptions();
            SetupBinding();
            SetupEvents();
           
        }
        #region Setup Binding
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {

                this.OneWayBind(this.ViewModel, x => x.Nodes, x => x.Nodes.Collection).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Connects, x => x.Connects.Collection).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Scale.Scales.X, x => x.Scale.ScaleX).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Scale.Scales.Y, x => x.Scale.ScaleY).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Selector, x => x.Selector.ViewModel).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Cutter, x => x.Cutter.ViewModel).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Dialog, x => x.Dialog.ViewModel).DisposeWith(disposable);
            });
        }
        #endregion Setup Binding

        #region Setup Commands
        private void SetupCommands()
        {
            this.WhenActivated(disposable =>
            {

                this.BindCommand(this.ViewModel, x => x.CommandSelect,              x => x.BindingSelect, x => x.PositionRight).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandCut,                 x => x.BindingCut, x => x.PositionRight).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandAddNodeWithUndoRedo, x => x.BindingAddNode, x => x.PositionRight).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandAddNodeWithUndoRedo, x => x.ItemAddNode, x => x.PositionLeft).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandRedo,                x => x.BindingRedo).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandUndo,                x => x.BindingUndo).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandSelectAll,           x => x.BindingSelectAll).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandUndo,                x => x.ItemCollapsUp).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandSelectAll,           x => x.ItemExpandDown).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandDeleteSelectedElements, x => x.BindingDeleteSelectedElements).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandDeleteSelectedElements, x => x.ItemDelete).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandCollapseUpSelected,  x => x.ItemCollapsUp).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandExpandDownSelected,  x => x.ItemExpandDown).DisposeWith(disposable);
            });
        }
        #endregion Setup Commands
        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenActivated(disposable =>
            {
                this.WhenAnyValue(x => x.ViewModel.Selector.Size).WithoutParameter().InvokeCommand(ViewModel, x => x.CommandSelectorIntersect).DisposeWith(disposable);
                this.WhenAnyValue(x => x.ViewModel.Cutter.EndPoint).WithoutParameter().InvokeCommand(ViewModel, x => x.CommandCutterIntersect).DisposeWith(disposable);
                this.WhenAnyValue(x => x.ViewModel.JPEGPath).Where(x => !string.IsNullOrEmpty(x)).Subscribe(value => SaveCanvasToImage(value, ImageFormats.JPEG)).DisposeWith(disposable);
            });
        }

        #endregion Setup Subscriptions
        #region Setup Events
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                this.Events().MouseLeftButtonDown.Subscribe(e => OnEventMouseLeftDown(e)).DisposeWith(disposable);
                this.Events().MouseLeftButtonUp.Subscribe(e => OnEventMouseLeftUp(e));
                this.Events().MouseRightButtonDown.Subscribe(e => OnEventMouseRightDown(e)).DisposeWith(disposable);
                this.Events().MouseUp.Subscribe(e => OnEventMouseUp(e)).DisposeWith(disposable);
                this.Events().MouseMove.Subscribe(e => OnEventMouseMove(e)).DisposeWith(disposable);
                this.Events().MouseWheel.Subscribe(e => OnEventMouseWheel(e)).DisposeWith(disposable);
                this.Events().DragOver.Subscribe(e => OnEventDragOver(e)).DisposeWith(disposable);
                this.Cutter.Events().MouseLeftButtonUp.InvokeCommand(this.ViewModel.CommandDeleteSelectedConnectors).DisposeWith(disposable);
                this.Events().PreviewMouseLeftButtonDown.Subscribe(e => OnEventPreviewMouseLeftButtonDown(e)).DisposeWith(disposable);
                this.Events().PreviewMouseRightButtonDown.Subscribe(e => OnEventPreviewMouseRightButtonDown(e)).DisposeWith(disposable);
                this.WhenAnyValue(x => x.ViewModel.Scale.Value).Subscribe(value => { this.Canvas.Height /= value; this.Canvas.Width /= value; }).DisposeWith(disposable);
            });
        }
        private void OnEventMouseLeftDown(MouseButtonEventArgs e)
        {
            PositionMove = Mouse.GetPosition(this.Canvas);

            if (Mouse.Captured == null)
            {
                Keyboard.ClearFocus();
                this.CaptureMouse();
                Keyboard.Focus(this);

                this.ViewModel.CommandUnSelectAll.ExecuteWithSubscribe();
            }
        }

        private void OnEventMouseLeftUp(MouseButtonEventArgs e)
        {
            if (Move == TypeMove.None)
                return;

            if (Move == TypeMove.MoveAll)
                this.ViewModel.CommandFullMoveAllNode.Execute(SumMove);
            else if (Move == TypeMove.MoveSelected)
                this.ViewModel.CommandFullMoveAllSelectedNode.Execute(SumMove);

            Move = TypeMove.None;
            SumMove = new Point();
        }
        private void OnEventMouseRightDown(MouseButtonEventArgs e)
        {
            Keyboard.Focus(this);

        }
        private void OnEventMouseWheel(MouseWheelEventArgs e)
        {
            this.ViewModel.CommandZoom.ExecuteWithSubscribe(e.Delta);
        }
        private void OnEventMouseUp(MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            PositionMove = new Point();
            Keyboard.Focus(this);
        }

        private void OnEventMouseMove(MouseEventArgs e)
        {
            if (!(Mouse.Captured is CanBeMove))
                return;

            Point delta = GetDeltaMove();

            if (delta.IsClear())
                return;

            SumMove = SumMove.Addition(delta);
            if (this.IsMouseCaptured)
            {
                ViewModel.CommandPartMoveAllNode.ExecuteWithSubscribe(delta);
                Move = TypeMove.MoveAll;
            }
            else
            {
                ViewModel.CommandPartMoveAllSelectedNode.ExecuteWithSubscribe(delta);
                Move = TypeMove.MoveSelected;
            }
        }
        private void OnEventDragOver(DragEventArgs e)
        {
            Point point = e.GetPosition(this);
            if (this.ViewModel.DraggedConnect != null)
            {
                point = point.Subtraction(2);
                this.ViewModel.DraggedConnect.EndPoint = point.Division(this.ViewModel.Scale.Value);
            }
        }
        private void OnEventPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.ViewModel.PositionRight = e.GetPosition(this.Canvas);
        }
        private void OnEventPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            this.ViewModel.PositionLeft = e.GetPosition(this.Canvas);
        }

        #endregion Setup Events
        private Point GetDeltaMove()
        {
            Point CurrentPosition = Mouse.GetPosition(this.Canvas);
            Point result = new Point();

            if (!PositionMove.IsClear())
            {
                result = CurrentPosition.Subtraction(PositionMove);
            }

            PositionMove = CurrentPosition;
            return result;
        }

        private void SaveCanvasToImage(string filename, ImageFormats format)
        {
            MyUtils.PanelToImage(this.Canvas, filename, format);
            ViewModel.CommandLogDebug.ExecuteWithSubscribe(String.Format("Scheme was exported to \"{0}\"", filename));
        }

    }
}
