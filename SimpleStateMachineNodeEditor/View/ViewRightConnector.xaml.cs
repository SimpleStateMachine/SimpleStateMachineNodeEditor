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
            SetupBinding();
            SetupEvents();
           
        }


        #region SetupBinding
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {
                Canvas.SetZIndex((UIElement)this.VisualParent, this.ViewModel.Node.Zindex+2);

                this.ViewModel.FormFill = Application.Current.Resources["ColorRightConnectorEllipseEnableBackground"] as SolidColorBrush;
                this.ViewModel.FormStroke = Application.Current.Resources["ColorRightConnectorEllipseEnableBorder"] as SolidColorBrush;

                // Имя перехода ( вводится в узле)
                this.OneWayBind(this.ViewModel, x => x.Name, x => x.Text.Text).DisposeWith(disposable);

                // Доступно ли имя перехода для редактирования
                this.OneWayBind(this.ViewModel, x => x.TextEnable, x => x.Text.IsEnabled).DisposeWith(disposable);

                // Доступен ли переход для создания соединия
                this.OneWayBind(this.ViewModel, x => x.FormEnable, x => x.Form.IsEnabled).DisposeWith(disposable);

                // Цвет рамки, вокруг перехода
                this.OneWayBind(this.ViewModel, x => x.FormStroke, x => x.Form.Stroke).DisposeWith(disposable);

                //Размеры
                this.WhenAnyValue(v => v.Grid.ActualWidth, v => v.Grid.ActualHeight, (width, height) => new Size(width, height))
                     .BindTo(this, v => v.ViewModel.Size).DisposeWith(disposable);

                // Цвет перехода
                //this.Bind(this.ViewModel, x => x.FormFill, x => x.Form.Fill).DisposeWith(disposable);

                // Отображается ли переход
                this.OneWayBind(this.ViewModel, x => x.Visible, x => x.RightConnector.Visibility).DisposeWith(disposable);

                // При изменении размера, позиции или масштаба узла
                this.WhenAnyValue(x => x.ViewModel.Node.Size, x => x.ViewModel.Node.Point1.Value, x => x.ViewModel.Node.NodesCanvas.Scale.Scales.Value)
                .Subscribe(_ => { UpdatePositionConnectPoin(); }).DisposeWith(disposable);
            });
        }
        #endregion SetupBinding

        #region SetupEvents
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                this.Form.Events().MouseLeftButtonDown.Subscribe(e => ConnectDrag(e)).DisposeWith(disposable);
                this.Text.Events().LostFocus.Subscribe(e => Validate(e)).DisposeWith(disposable);
                this.Grid.Events().PreviewMouseLeftButtonDown.Subscribe(e => ConnectorDrag(e)).DisposeWith(disposable);
                this.Grid.Events().PreviewDragEnter.Subscribe(e => ConnectorDragEnter(e)).DisposeWith(disposable);
                this.Grid.Events().PreviewDrop.Subscribe(e => ConnectorDrop(e)).DisposeWith(disposable);
               

            });
        }
        private void Validate(RoutedEventArgs e)
        {
            ViewModel.CommandValidateName.Execute(Text.Text);
            if (Text.Text != ViewModel.Name)
                Text.Text = ViewModel.Name;
        }

        private void ConnectDrag(MouseButtonEventArgs e)
        {          
            this.ViewModel.CommandConnectPointDrag.Execute();
            DataObject data = new DataObject();
            data.SetData("Node", this.ViewModel.Node);
            DragDrop.DoDragDrop(this, data, DragDropEffects.Link);
            this.ViewModel.CommandCheckConnectPointDrop.Execute();
            e.Handled = true;
        }

        private void ConnectorDrag(MouseButtonEventArgs e)
        {
            
            if (!this.ViewModel.TextEnable)
                return;
            if (!Keyboard.IsKeyDown(Key.LeftShift))
                return;
            this.ViewModel.CommandConnectorDrag.Execute();
            DataObject data = new DataObject();
            data.SetData("Connector", this.ViewModel);
            DragDrop.DoDragDrop(this, data, DragDropEffects.Link);
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
            //Если отображается
            if (this.IsVisible)
            {
                // Координата центра
                positionConnectPoint = Form.TranslatePoint(new Point(Form.Width/2, Form.Height / 2), this);

                //Ищем Canvas
                ViewNodesCanvas NodesCanvas = MyUtils.FindParent<ViewNodesCanvas>(this);

                //Получаем позицию центру на канвасе
                positionConnectPoint = this.TransformToAncestor(NodesCanvas).Transform(positionConnectPoint);

                Position  = MyPoint.CreateFromPoint(positionConnectPoint) / this.ViewModel.NodesCanvas.Scale.Value;

            }
            else
            {
                //Позиция выхода
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
