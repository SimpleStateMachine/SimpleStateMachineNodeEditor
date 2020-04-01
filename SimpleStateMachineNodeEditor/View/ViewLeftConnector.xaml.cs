using System;
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


namespace SimpleStateMachineNodeEditor.View
{
    /// <summary>
    /// Interaction logic for ViewLeftConnector.xaml
    /// </summary>
    public partial class ViewLeftConnector : UserControl, IViewFor<ViewModelConnector>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ViewModelConnector), typeof(ViewLeftConnector), new PropertyMetadata(null));

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
        public ViewLeftConnector()
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

                this.ViewModel.FormFill = Application.Current.Resources["ColorLeftConnectorEllipseEnableBackground"] as SolidColorBrush;
                this.ViewModel.FormStroke = Application.Current.Resources["ColorLeftConnectorEllipseEnableBorder"] as SolidColorBrush;

                // Имя перехода ( вводится в узле)
                this.OneWayBind(this.ViewModel, x => x.Name, x => x.Text.Text).DisposeWith(disposable);


                // Доступно ли имя перехода для редактирования
                this.OneWayBind(this.ViewModel, x => x.TextEnable, x => x.Text.IsEnabled).DisposeWith(disposable);

                // Доступен ли переход для создания соединия
                this.OneWayBind(this.ViewModel, x => x.FormEnable, x => x.Form.IsEnabled).DisposeWith(disposable);

                // Цвет рамки, вокруг перехода
                this.OneWayBind(this.ViewModel, x => x.FormStroke, x => x.Form.Stroke).DisposeWith(disposable);

                // Цвет перехода
                this.OneWayBind(this.ViewModel, x => x.FormFill, x => x.Form.Fill).DisposeWith(disposable);

                // Отображается ли переход
                this.OneWayBind(this.ViewModel, x => x.Visible, x => x.LeftConnector.Visibility).DisposeWith(disposable);

                // При изменении размера, позиции или zoom узла
                this.WhenAnyValue(x => x.ViewModel.Node.Size, x => x.ViewModel.Node.Point1.Value, x => x.ViewModel.Node.NodesCanvas.Scale.Scales.Value)
                .Subscribe(_ => UpdatePosition()).DisposeWith(disposable);


            });
        }
        #endregion SetupBinding

        #region SetupEvents
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                this.Form.Events().Drop.Subscribe(e => OnEventDrop(e)).DisposeWith(disposable);
            });
        }
        #endregion SetupEvents

        private void OnEventDrop(DragEventArgs e)
        {
            this.ViewModel.CommandConnectPointDrop.Execute();
            e.Handled = true;
        }
        void UpdatePosition()
        {
            // Координата центра
            Point InputCenter = Form.TranslatePoint(new Point(1, Form.Height / 2), this);

            //Ищем Canvas
            ViewNodesCanvas NodesCanvas = MyUtils.FindParent<ViewNodesCanvas>(this);
            if (NodesCanvas == null)
                return;
            //Получаем позицию центру на канвасе
            Point Position = this.TransformToAncestor(NodesCanvas).Transform(InputCenter);

            this.ViewModel.PositionConnectPoint.Set(Position);
        }
    }
}
