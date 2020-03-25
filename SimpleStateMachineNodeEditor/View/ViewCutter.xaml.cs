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
    /// Interaction logic for ViewCutter.xaml
    /// </summary>
    public partial class ViewCutter : UserControl, IViewFor<ViewModelCutter>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ViewModelCutter), typeof(ViewCutter), new PropertyMetadata(null));

        public ViewModelCutter ViewModel
        {
            get { return (ViewModelCutter)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ViewModelCutter)value; }
        }
        #endregion ViewModel
        public ViewCutter()
        {
            InitializeComponent();
            SetupBinding();
            SetupEvents();
        }
        #region Setup Binding 
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {
                // Отображается ли линия среза
                this.OneWayBind(this.ViewModel, x => x.Visible, x => x.Visibility).DisposeWith(disposable);
                // Точка из которой выходит линия среза
                this.OneWayBind(this.ViewModel, x => x.StartPoint.Value.X, x => x.Line.X1).DisposeWith(disposable);
                // Точка из которой выходит линия среза
                this.OneWayBind(this.ViewModel, x => x.StartPoint.Value.Y, x => x.Line.Y1).DisposeWith(disposable);

                // Точка в которую приходит линия среза
                this.OneWayBind(this.ViewModel, x => x.EndPoint.Value.X, x => x.Line.X2).DisposeWith(disposable);
                // Точка в которую приходит линия среза
                this.OneWayBind(this.ViewModel, x => x.EndPoint.Value.Y, x => x.Line.Y2).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.StrokeThickness, x => x.Line.StrokeThickness).DisposeWith(disposable);

                this.WhenAnyValue(x => x.Visibility).Subscribe(_ => Update()).DisposeWith(disposable);

            });
        }

        #endregion Setup Binding 

        #region Setup Events

        private void Update()
        {
            if (this.IsVisible)
            {
                Mouse.Capture(this);
                Keyboard.Focus(this);
            }
        }
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                this.Events().MouseMove.Subscribe(e => OnMouseMoves(e)).DisposeWith(disposable);
                this.Events().MouseLeftButtonUp.Subscribe(e => OnMouseLeftButtonUp(e)).DisposeWith(disposable);

            });
        }

        private void OnMouseMoves(MouseEventArgs e)
        {
            //Ищем Canvas
            ViewNodesCanvas NodesCanvas = MyUtils.FindParent<ViewNodesCanvas>(this);
            ViewModel.EndPoint.Set(e.GetPosition(NodesCanvas.Grid));
            e.Handled = true;

        }
        private void OnMouseLeftButtonUp(MouseEventArgs e)
        {
            this.ViewModel.Visible = null;
        }
        #endregion Setup Events

        #region Setup Commands
        private void SetupCommands()
        {
            this.WhenActivated(disposable =>
            {


            });
        }
        #endregion Setup Commands
    }
}
