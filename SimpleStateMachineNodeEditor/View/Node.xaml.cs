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
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using System.Collections.Generic;

namespace SimpleStateMachineNodeEditor.View
{
    /// <summary>
    /// Interaction logic for ViewNode.xaml
    /// </summary>
    public partial class Node : UserControl, IViewFor<NodeViewModel>, CanBeMove
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(NodeViewModel), typeof(Node), new PropertyMetadata(null));

        public NodeViewModel ViewModel
        {
            get { return (NodeViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (NodeViewModel)value; }
        }
        #endregion ViewModel
        public Node()
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

                this.OneWayBind(this.ViewModel, x => x.Name, x => x.NodeHeaderElement.TextBoxElement.Text).DisposeWith(disposable);

                this.Bind(this.ViewModel, x => x.NameEnable, x => x.NodeHeaderElement.TextBoxElement.IsEnabled).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Point1.X, x => x.TranslateTransformElement.X).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Point1.Y, x => x.TranslateTransformElement.Y).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.TransitionsVisible, x => x.ItemsControlTransitions.Visibility).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.RollUpVisible, x => x.NodeHeaderElement.ButtonCollapse.Visibility).DisposeWith(disposable);

                this.WhenAnyValue(v => v.BorderElement.ActualWidth, v => v.BorderElement.ActualHeight, (width, height) => new Size(width, height))
                     .BindTo(this, v => v.ViewModel.Size).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Input, x => x.Input.ViewModel).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.Output, x => x.Output.ViewModel).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.TransitionsForView, x => x.ItemsControlTransitions.ItemsSource).DisposeWith(disposable);

                this.WhenAnyValue(v => v.NodeHeaderElement.ActualWidth).BindTo(this, v => v.ViewModel.HeaderWidth).DisposeWith(disposable);

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
                this.WhenAnyValue(x=>x.IsMouseOver).Subscribe(value=> OnEventMouseOver(value)).DisposeWith(disposable);
                this.Events().MouseLeftButtonDown.Subscribe(e => OnEventMouseLeftDowns(e)).DisposeWith(disposable);
                this.Events().MouseDown.Subscribe(e => OnEventMouseDown(e)).DisposeWith(disposable);
                this.Events().MouseUp.Subscribe(e => OnEventMouseUp(e)).DisposeWith(disposable);
                this.Events().MouseMove.Subscribe(e => OnMouseMove(e)).DisposeWith(disposable);

                this.NodeHeaderElement.ButtonCollapse.Events().Click.Subscribe(_ => ViewModel.IsCollapse=!ViewModel.IsCollapse).DisposeWith(disposable);
                this.NodeHeaderElement.Events().LostFocus.Subscribe(e => Validate(e)).DisposeWith(disposable);
                this.ViewModel.WhenAnyValue(x=>x.IsCollapse).Subscribe(value=> OnEventCollapse(value)).DisposeWith(disposable);
            });
        }
        private void OnEventMouseOver(bool value)
        {
            if (this.ViewModel.Selected != true)
                this.ViewModel.BorderBrush = value?Application.Current.Resources["ColorSelectedElement"] as SolidColorBrush
                                                 : Application.Current.Resources["ColorNodeBorderBrush"] as SolidColorBrush;
        }
        private void OnEventMouseLeftDowns(MouseButtonEventArgs e)
        {
            NodeCanvasClickMode clickMode = this.ViewModel.NodesCanvas.ClickMode;
            if (clickMode == NodeCanvasClickMode.Delete)
            {
                 this.ViewModel.NodesCanvas.CommandDeleteSelectedNodes.Execute(new List<NodeViewModel>() { this.ViewModel });
            }
            else
            {
                Keyboard.Focus(this);
                this.ViewModel.CommandSelect.ExecuteWithSubscribe(SelectMode.Click);
            }
        }
        private void Validate(RoutedEventArgs e)
        {
            if (NodeHeaderElement.TextBoxElement.Text != ViewModel.Name)
                ViewModel.CommandValidateName.ExecuteWithSubscribe(NodeHeaderElement.TextBoxElement.Text);
            if (NodeHeaderElement.TextBoxElement.Text != ViewModel.Name)
                NodeHeaderElement.TextBoxElement.Text = ViewModel.Name;
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
        private void OnEventCollapse(bool isCollapse)
        {
            this.NodeHeaderElement.ButtonRotate.Angle = isCollapse ? 180 : 0;
        }
        #endregion Setup Events

    }
}
