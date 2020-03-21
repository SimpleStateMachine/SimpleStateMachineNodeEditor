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
using System.Reactive.Linq;
using System.Reactive.Disposables;

using ReactiveUI;

using DynamicData;

using StateMachineNodeEditorNerCore.ViewModel;
using StateMachineNodeEditorNerCore.Helpers;
using StateMachineNodeEditorNerCore.Helpers.Transformations;

namespace StateMachineNodeEditorNerCore.View
{
    /// <summary>
    /// Interaction logic for ViewNode.xaml
    /// </summary>
    public partial class ViewNode : UserControl, IViewFor<ViewModelNode>, CanBeMove
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ViewModelNode), typeof(ViewNode), new PropertyMetadata(null));

        public ViewModelNode ViewModel
        {
            get { return (ViewModelNode)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ViewModelNode)value; }
        }
        #endregion ViewModel

        public ViewNode()
        {
            InitializeComponent();
            SetupBinding();
            SetupEvents();
            SetupCommands();
        }
        #region Setup Binding
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {
                //BorderBrush (Рамка вокруг узла)
                this.OneWayBind(this.ViewModel, x => x.BorderBrush, x => x.Border.BorderBrush).DisposeWith(disposable);

                //Name (заголовок узла)
                this.OneWayBind(this.ViewModel, x => x.Name, x => x.Header.Text).DisposeWith(disposable);

                //Можно ли менять заголовок
                this.Bind(this.ViewModel, x => x.NameEnable, x => x.Header.IsEnabled).DisposeWith(disposable);

                //Позиция X от левого верхнего угла
                this.OneWayBind(this.ViewModel, x => x.Point1.X, x => x.Translate.X).DisposeWith(disposable);

                //Позиция Y от левого верхнего угла
                this.OneWayBind(this.ViewModel, x => x.Point1.Y, x => x.Translate.Y).DisposeWith(disposable);

                //Отображаются ли переходы
                this.OneWayBind(this.ViewModel, x => x.TransitionsVisible, x => x.Transitions.Visibility).DisposeWith(disposable);

                //Отображается ли кнопка свернуть
                this.OneWayBind(this.ViewModel, x => x.RollUpVisible, x => x.ButtonCollapse.Visibility).DisposeWith(disposable);
       
                //Размеры
                this.WhenAnyValue(v => v.Border.ActualWidth, v => v.Border.ActualHeight, (width, height) => new Size(width, height))
                     .BindTo(this, v => v.ViewModel.Size).DisposeWith(disposable);

                //Вход для соединения с этим узлом
                this.OneWayBind(this.ViewModel, x => x.Input, x => x.Input.ViewModel).DisposeWith(disposable);

                //Выход ( используется, когда список переходов свернут )
                this.OneWayBind(this.ViewModel, x => x.Output, x => x.Output.ViewModel).DisposeWith(disposable);

                //Переходы
                this.OneWayBind(this.ViewModel, x => x.Transitions, x => x.Transitions.ItemsSource).DisposeWith(disposable);


                
            });
        }
        #endregion Setup Binding
        #region Setup Events
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                this.Events().MouseLeftButtonDown.Subscribe(e => OnEventMouseLeftDowns(e)).DisposeWith(disposable);
                this.Events().MouseLeftButtonUp.Subscribe(e => OnEventMouseLeftUp(e)).DisposeWith(disposable);
                this.Events().MouseRightButtonDown.Subscribe(e => OnEventMouseRightDown(e)).DisposeWith(disposable);
                this.Events().MouseRightButtonUp.Subscribe(e => OnEventMouseRightUp(e)).DisposeWith(disposable);
                this.Events().MouseDown.Subscribe(e => OnEventMouseDown(e)).DisposeWith(disposable);
                this.Events().MouseUp.Subscribe(e => OnEventMouseUp(e)).DisposeWith(disposable);
                this.Events().MouseMove.Subscribe(e => OnMouseMove(e)).DisposeWith(disposable);
                this.Events().MouseEnter.Subscribe(e => OnEventMouseEnter(e)).DisposeWith(disposable);
                this.Events().MouseLeave.Subscribe(e => OnEventMouseMouseLeave(e)).DisposeWith(disposable);

                this.ButtonCollapse.Events().Click.Subscribe(_ => OnEventCollapse()).DisposeWith(disposable);
                this.Header.Events().LostFocus.Subscribe(e => Validate(e)).DisposeWith(disposable);


                //this.Border.Events().DragLeave()


            });
        }

        private void OnEventMouseLeftDowns(MouseButtonEventArgs e)
        {
            Keyboard.Focus(this);
            this.ViewModel.CommandSelect.Execute(true);
        }
        private void Validate(RoutedEventArgs e)
        {
            ViewModel.CommandValidateName.Execute(Header.Text);
            if (Header.Text != ViewModel.Name)
                Header.Text = ViewModel.Name;
        }

        private void OnEventMouseLeftUp(MouseButtonEventArgs e)
        {
        }
        private void OnEventMouseRightDown(MouseButtonEventArgs e)
        {

        }
        private void OnEventMouseRightUp(MouseButtonEventArgs e)
        {
        }

        private void OnEventMouseDown(MouseButtonEventArgs e)
        {
            if (Mouse.Captured == null)
            {
                Keyboard.ClearFocus();
                this.CaptureMouse();
                Keyboard.Focus(this);
            }
            e.Handled = true;
        }
        private void OnEventMouseUp(MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
        }
        //private void OnEventMouseMove(MouseButtonEventArgs e)
        //{
        //}
        private void OnEventMouseEnter(MouseEventArgs e)
        {
            if (this.ViewModel.Selected != true)
                this.ViewModel.BorderBrush = Application.Current.Resources["ColorNodeSelectedBorder"] as SolidColorBrush;
        }
        private void OnEventMouseMouseLeave(MouseEventArgs e)
        {
            if (this.ViewModel.Selected != true)
                this.ViewModel.BorderBrush = Application.Current.Resources["ColorNodeBorder"] as SolidColorBrush;
        }


        private void OnEventCollapse()
        {
            bool visible = (this.Rotate.Angle != 0);
            this.Rotate.Angle = visible ? 0 : 180;
            ViewModel.CommandCollapse.Execute(visible);
        }
        #endregion Setup Events
        #region Setup Commands
        private void SetupCommands()
        {
            this.WhenActivated(disposable =>
            {
                this.BindCommand(this.ViewModel, x => x.CommandSelect, x => x.BindingSelect);
            });
        }
        #endregion Setup Commands
    }
}
