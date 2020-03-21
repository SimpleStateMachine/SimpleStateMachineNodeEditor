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
    /// Interaction logic for ViewSelector.xaml
    /// </summary>
    public partial class ViewSelector : UserControl, IViewFor<ViewModelSelector>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(ViewModelSelector), typeof(ViewSelector), new PropertyMetadata(null));

        public ViewModelSelector ViewModel
        {
            get { return (ViewModelSelector)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ViewModelSelector)value; }
        }
        #endregion ViewModel
        public ViewSelector()
        {
            InitializeComponent();
            SetupBinding();
            SetupEvents();
            SetupCommands();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

        }
        #region Setup Binding 
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {
                // Отображается ли выделение
                this.OneWayBind(this.ViewModel, x => x.Visible, x => x.Visibility).DisposeWith(disposable);

                //Ширина
                this.OneWayBind(this.ViewModel, x => x.Size.Width, x => x.Form.Width).DisposeWith(disposable);

                //Высота
                this.OneWayBind(this.ViewModel, x => x.Size.Height, x => x.Form.Height).DisposeWith(disposable);

                //Позиция X от левого верхнего угла
                this.OneWayBind(this.ViewModel, x => x.Point1.Value.X, x => x.Translate.X).DisposeWith(disposable);

                //Позиция Y от левого верхнего угла
                this.OneWayBind(this.ViewModel, x => x.Point1.Value.Y, x => x.Translate.Y).DisposeWith(disposable);

                //Масштаб по оси X
                this.OneWayBind(this.ViewModel, x => x.Scale.Scales.Value.X, x => x.Scale.ScaleX).DisposeWith(disposable);

                //Масштаб по оси Y
                this.OneWayBind(this.ViewModel, x => x.Scale.Scales.Value.Y, x => x.Scale.ScaleY).DisposeWith(disposable);

                ////Точка масштабирования, координата X
                //this.Bind(this.ViewModel, x => x.Scale.Center.Value.X, x => x.Scale.CenterX);

                ////Точка масштабирования, координата Y
                //this.Bind(this.ViewModel, x => x.Scale.Center.Value.Y, x => x.Scale.CenterY);


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
                this.Events().MouseMove.Subscribe(e => OnMouseMoves(e));
                this.Events().MouseLeftButtonUp.Subscribe(e => OnMouseLeftButtonUp(e));

            });
        }

        private void OnMouseMoves(MouseEventArgs e)
        {
            //Ищем Canvas
            ViewNodesCanvas NodesCanvas = MyUtils.FindParent<ViewNodesCanvas>(this);

            ViewModel.Point2.Set(e.GetPosition(NodesCanvas));
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
