using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
    public partial class Cutter : UserControl, IViewFor<CutterViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(CutterViewModel), typeof(Cutter), new PropertyMetadata(null));

        public CutterViewModel ViewModel
        {
            get { return (CutterViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (CutterViewModel)value; }
        }
        #endregion ViewModel
        public Cutter()
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

                this.OneWayBind(ViewModel, x => x.Visible, x => x.Visibility).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.StartPoint.X, x => x.LineElement.X1).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.StartPoint.Y, x => x.LineElement.Y1).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.EndPoint.X, x => x.LineElement.X2).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.EndPoint.Y, x => x.LineElement.Y2).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.StrokeThickness, x => x.LineElement.StrokeThickness).DisposeWith(disposable);

                this.WhenAnyValue(x => x.Visibility).Where(x=>x==Visibility.Visible).Subscribe(_ => Update()).DisposeWith(disposable);

            });
        }
        private void Update()
        {
            Mouse.Capture(this);
            Keyboard.Focus(this);
        }

        #endregion Setup Binding 

        #region Setup Events

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
            NodesCanvas NodesCanvas = MyUtils.FindParent<NodesCanvas>(this);
            ViewModel.EndPoint = e.GetPosition(NodesCanvas.CanvasElement);

            e.Handled = true;

        }
        private void OnMouseLeftButtonUp(MouseEventArgs e)
        {
            ViewModel.Visible = null;
        }

        #endregion Setup Events

    }
}
