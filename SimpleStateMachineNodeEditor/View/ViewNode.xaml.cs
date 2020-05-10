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

using SimpleStateMachineNodeEditor.ViewModel;
using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Transformations;
using SimpleStateMachineNodeEditor.Helpers.Enums;

namespace SimpleStateMachineNodeEditor.View
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
                Canvas.SetZIndex((UIElement)this.VisualParent,this.ViewModel.Zindex);

                this.OneWayBind(this.ViewModel, x => x.BorderBrush, x => x.BorderElement.BorderBrush).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Name, x => x.NodeHeaderElement.TextBox.Text).DisposeWith(disposable);

                this.Bind(this.ViewModel, x => x.NameEnable, x => x.NodeHeaderElement.TextBox.IsEnabled).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Point1.X, x => x.TranslateTransformElement.X).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Point1.Y, x => x.TranslateTransformElement.Y).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.TransitionsVisible, x => x.ItemsControlTransitions.Visibility).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.RollUpVisible, x => x.NodeHeaderElement.ButtonCollapse.Visibility).DisposeWith(disposable);

                this.WhenAnyValue(v => v.BorderElement.ActualWidth, v => v.BorderElement.ActualHeight, (width, height) => new Size(width, height))
                     .BindTo(this, v => v.ViewModel.Size).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Input, x => x.Input.ViewModel).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Output, x => x.Output.ViewModel).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Transitions, x => x.ItemsControlTransitions.ItemsSource).DisposeWith(disposable);


                
            });
        }
        #endregion Setup Binding
        #region Setup Commands
        private void SetupCommands()
        {
            this.WhenActivated(disposable =>
            {
                this.BindCommand(this.ViewModel, x => x.CommandSelect, x => x.BindingSelect).DisposeWith(disposable);
            });
        }
        #endregion Setup Commands
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

                this.NodeHeaderElement.ButtonCollapse.Events().Click.Subscribe(_ => OnEventCollapse()).DisposeWith(disposable);
                this.NodeHeaderElement.Events().LostFocus.Subscribe(e => Validate(e)).DisposeWith(disposable);
            });
        }

        private void OnEventMouseLeftDowns(MouseButtonEventArgs e)
        {
            Keyboard.Focus(this);
            this.ViewModel.CommandSelect.Execute(SelectMode.Click);
        }
        private void Validate(RoutedEventArgs e)
        {
            ViewModel.CommandValidateName.Execute(NodeHeaderElement.TextBox.Text);
            if (NodeHeaderElement.TextBox.Text != ViewModel.Name)
                NodeHeaderElement.TextBox.Text = ViewModel.Name;
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

        private void OnEventMouseEnter(MouseEventArgs e)
        {
            if (this.ViewModel.Selected != true)
                this.ViewModel.BorderBrush = Application.Current.Resources["ColorSelectedElement"] as SolidColorBrush;
        }
        private void OnEventMouseMouseLeave(MouseEventArgs e)
        {
            if (this.ViewModel.Selected != true)
                this.ViewModel.BorderBrush = Application.Current.Resources["ColorNodeBorder"] as SolidColorBrush;
        }


        private void OnEventCollapse()
        {
            bool visible = (this.NodeHeaderElement.ButtonRotate.Angle != 0);
            this.NodeHeaderElement.ButtonRotate.Angle = visible ? 0 : 180;
            ViewModel.CommandCollapse.Execute(visible);
            
        }
        #endregion Setup Events

    }
}
