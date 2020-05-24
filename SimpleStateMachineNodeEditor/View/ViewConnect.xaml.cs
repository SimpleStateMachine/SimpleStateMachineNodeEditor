using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ReactiveUI;
using System;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Controls.Primitives;
using System.Reactive.Linq;
using System.Windows.Markup;
using SimpleStateMachineNodeEditor.ViewModel;
using SimpleStateMachineNodeEditor.Helpers;

namespace SimpleStateMachineNodeEditor.View
{
    /// <summary>
    /// Interaction logic for ViewConnect.xaml
    /// </summary>
    public partial class ViewConnect : UserControl, IViewFor<ViewModelConnect>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ViewModelConnect), typeof(ViewConnect), new PropertyMetadata(null));

        public ViewModelConnect ViewModel
        {
            get { return (ViewModelConnect)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ViewModelConnect)value; }
        }
        #endregion ViewModel
        public ViewConnect()
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
                // Цвет линии
                this.OneWayBind(this.ViewModel, x => x.Stroke, x => x.PathElement.Stroke).DisposeWith(disposable);

                // Точка, из которой выходит линия
                this.OneWayBind(this.ViewModel, x => x.StartPoint.Value, x => x.PathFigureElement.StartPoint).DisposeWith(disposable);

                // Первая промежуточная точка линии 
                this.OneWayBind(this.ViewModel, x => x.Point1.Value, x => x.BezierSegmentElement.Point1).DisposeWith(disposable);

                // Вторая промежуточная точка линии
                this.OneWayBind(this.ViewModel, x => x.Point2.Value, x => x.BezierSegmentElement.Point2).DisposeWith(disposable);

                // Точка, в которую приходит линия
                this.OneWayBind(this.ViewModel, x => x.EndPoint.Value, x => x.BezierSegmentElement.Point3).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.StrokeDashArray, x => x.PathElement.StrokeDashArray).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.StrokeThickness, x => x.PathElement.StrokeThickness).DisposeWith(disposable);

                this.WhenAnyValue(x => x.ViewModel.ToConnector).Where(x=>x!=null).Subscribe(_ => UpdateZindex()).DisposeWith(disposable);

                Canvas.SetZIndex((UIElement)this.VisualParent, this.ViewModel.FromConnector.Node.Zindex+1);

            });
        }
        #endregion SetupBinding

        #region SetupEvents
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {

            });
        }
        private void UpdateZindex()
        {
            if(this.ViewModel.FromConnector.Node!=this.ViewModel.ToConnector.Node)
                Canvas.SetZIndex((UIElement)this.VisualParent, this.ViewModel.ToConnector.Node.Zindex);
        }

        #endregion SetupEvents
    }
}
