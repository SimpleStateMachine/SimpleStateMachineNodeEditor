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

using StateMachineNodeEditorNerCore.Helpers;
using StateMachineNodeEditorNerCore.ViewModel;
using StateMachineNodeEditorNerCore.Helpers.Transformations;

namespace StateMachineNodeEditorNerCore.View
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
                // Имя перехода ( вводится в узле)
                this.OneWayBind(this.ViewModel, x => x.Name, x => x.Text.Text).DisposeWith(disposable);

                // Доступно ли имя перехода для редактирования
                this.OneWayBind(this.ViewModel, x => x.TextEnable, x => x.Text.IsEnabled).DisposeWith(disposable);

                // Доступен ли переход для создания соединия
                this.OneWayBind(this.ViewModel, x => x.FormEnable, x => x.Form.IsEnabled).DisposeWith(disposable);

                // Цвет рамки, вокруг перехода
                this.OneWayBind(this.ViewModel, x => x.FormStroke, x => x.Form.Stroke).DisposeWith(disposable);

                ////Позиция X от левого верхнего угла
                //this.OneWayBind(this.ViewModel, x => x.Position.X, x => x.Translate.X).DisposeWith(disposable);

                ////Позиция Y от левого верхнего угла
                //this.OneWayBind(this.ViewModel, x => x.Position.Y, x => x.Translate.Y).DisposeWith(disposable);

                //Размеры
                this.WhenAnyValue(v => v.Grid.ActualWidth, v => v.Grid.ActualHeight, (width, height) => new Size(width, height))
                     .BindTo(this, v => v.ViewModel.Size).DisposeWith(disposable);

                // Цвет перехода
                this.OneWayBind(this.ViewModel, x => x.FormFill, x => x.Form.Fill).DisposeWith(disposable);

                // Отображается ли переход
                this.OneWayBind(this.ViewModel, x => x.Visible, x => x.RightConnector.Visibility).DisposeWith(disposable);

                // При изменении размера, позиции или zoom узла
                this.WhenAnyValue(x => x.ViewModel.Node.Size, x => x.ViewModel.Node.Point1.Value, x => x.ViewModel.Node.NodesCanvas.Scale.Scales.Value)
                .Subscribe(_ => { UpdatePositionConnectPoin(); }).DisposeWith(disposable);


                //// При изменении размера, позиции или zoom узла
                //this.WhenAnyValue(x => x.ViewModel.Node.Size, x => x.ViewModel.Node.Point1.Value, x => x.ViewModel.Node.NodesCanvas.Scale.Scales.Value, x => x.ViewModel.Position).
                //Subscribe(_ => { UpdatePositionConnectPoin(); }).DisposeWith(disposable);
                //this.WhenAnyValue(x=>x.ViewModel.Node.Transitions.Count).Subscribe(_ => { Test(); }).DisposeWith(disposable);

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

                this.Text.Events().PreviewMouseLeftButtonDown.Subscribe(e => TextDrag(e)).DisposeWith(disposable);
                this.Text.Events().PreviewDrop.Subscribe(e => TextDrop(e)).DisposeWith(disposable);
                this.Text.Events().PreviewDragOver.Subscribe(e => TextDragOver(e)).DisposeWith(disposable);
                this.Text.Events().PreviewDragEnter.Subscribe(e => TextDragEnter(e)).DisposeWith(disposable);
                this.Text.Events().PreviewDragLeave.Subscribe(e => TextDragLeave(e)).DisposeWith(disposable);

                this.Grid.Events().PreviewMouseLeftButtonDown.Subscribe(e => ConnectorDrag(e)).DisposeWith(disposable);
                this.Grid.Events().PreviewDragEnter.Subscribe(e => ConnectorDragEnter(e)).DisposeWith(disposable);
                this.Grid.Events().PreviewDragOver.Subscribe(e => ConnectorDragOver(e)).DisposeWith(disposable);
                this.Grid.Events().PreviewDragLeave.Subscribe(e => ConnectorDragLeave(e)).DisposeWith(disposable);
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

        private void TextDrag(MouseButtonEventArgs e)
        {
            ConnectorDrag(e);
            e.Handled = true;
        }

        private void TextDragOver(DragEventArgs e)
        {
            ConnectorDragOver(e);
            e.Handled = true;
        }

        private void TextDragEnter(DragEventArgs e)
        {
            ConnectorDragEnter(e);
            e.Handled = true;
        }

        private void TextDragLeave(DragEventArgs e)
        {
            ConnectorDragLeave(e);
            e.Handled = true;
        }

        private void TextDrop(DragEventArgs e)
        {
            ConnectorDrop(e);
            e.Handled = true;
        }


        private void ConnectorDrag(MouseButtonEventArgs e)
        {
            if (!this.ViewModel.TextEnable)
                return;

            this.ViewModel.CommandConnectorDrag.Execute();
            DataObject data = new DataObject();
            data.SetData("Connector", this.ViewModel);
            DragDrop.DoDragDrop(this, data, DragDropEffects.Link);
            this.ViewModel?.CommandCheckConnectorDrop.Execute();

            //this.UpdatePosition();
            //e.Handled = true;
        }
        private void Test()
        {
            //if(this.ViewModel ==this.ViewModel.NodesCanvas.DraggedConnector)
            //{
            //    UpdatePosition();
            //}
        }
        private void ConnectorDragOver(DragEventArgs e)
        {
            

            //if (this.ViewModel.NodesCanvas.ConnectorPreviewForDrop != this.ViewModel)
            //{
            //    this.ViewModel.Node.Point1 += 0.0001;

               
            //    return;
            //}
            //e.Handled = true;
            //this.UpdatePosition();

            //this.ViewModel.CommandConnectorDragOver.Execute();

            

            return;
        }

        private void ConnectorDragEnter(DragEventArgs e)
        {
            if (this.ViewModel.NodesCanvas.ConnectorPreviewForDrop == null)
                return;
            if (this.ViewModel.NodesCanvas.ConnectorPreviewForDrop == this.ViewModel)
                return;

            this.ViewModel.CommandConnectorDragEnter.Execute();
            this.ViewModel.Node.Point1 += 0.001;

            e.Handled = true;
        }

        private void ConnectorDragLeave(DragEventArgs e)
        {

            //new
            if (this.ViewModel.NodesCanvas.DraggedConnector == null)
                return;
            //new
            if (this.ViewModel.NodesCanvas.ConnectorPreviewForDrop != null)
                return;

            //if (this.ViewModel.NodesCanvas.DraggedConnector.Name == this.ViewModel.Name)
            //{
            //    this.UpdatePosition();
            //    e.Handled = true;
            //    return;
            //}
            //else
            //{
                this.ViewModel.CommandConnectorDragLeave.Execute();
            //}


           

            //if (this.ViewModel.NodesCanvas.ConnectorPreviewForDrop == null)
            //    return;

            e.Handled = true;

            return;
        }

        private void ConnectorDrop(DragEventArgs e)
        {
            if (this.ViewModel.NodesCanvas.ConnectorPreviewForDrop == null)
                return;

            this.ViewModel.CommandConnectorDrop.Execute();

            //e.Handled = true;
        }

        #endregion SetupEvents

        /// <summary>
        /// Обновить координату центра круга
        /// </summary>
        void UpdatePositionConnectPoin()
        {

            Point positionConnectPoint;
            //Если отображается
            if (this.IsVisible)
            {
                // Координата центра
                positionConnectPoint = Form.TranslatePoint(new Point(Form.Width / 2, Form.Height / 2), this);

                //Ищем Canvas
                ViewNodesCanvas NodesCanvas = MyUtils.FindParent<ViewNodesCanvas>(this);

                //Получаем позицию центру на канвасе
                positionConnectPoint = this.TransformToAncestor(NodesCanvas).Transform(positionConnectPoint);

            }
            else
            {
                //Позиция выхода
                positionConnectPoint = this.ViewModel.Node.Output.PositionConnectPoint.Value;
            }

            this.ViewModel.PositionConnectPoint.Set(positionConnectPoint);
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
