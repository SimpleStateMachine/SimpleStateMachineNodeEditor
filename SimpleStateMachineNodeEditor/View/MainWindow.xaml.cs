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
using System.Windows.Markup;
using SimpleStateMachineNodeEditor.ViewModel;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using System.Windows.Forms;

namespace SimpleStateMachineNodeEditor.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<ViewModelMainWindow>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ViewModelMainWindow), typeof(MainWindow), new PropertyMetadata(null));

        public ViewModelMainWindow ViewModel
        {
            get { return (ViewModelMainWindow)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ViewModelMainWindow)value; }
        }
        #endregion ViewModel

        public MainWindow()
        {

            InitializeComponent();
            SetupBinding();
            SetupEvents();
            SetupCommands();
        }

        #region SetupBinding
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {

            });
        }
        #endregion SetupBinding
       
        #region SetupEvents
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                this.Header.Events().PreviewMouseLeftButtonDown.Subscribe(e => HeaderClick(e)).DisposeWith(disposable);
                this.ButtonClose.Events().Click.Subscribe(e => ButtonCloseClick(e)).DisposeWith(disposable);
                this.ButtonMin.Events().Click.Subscribe(e => ButtonMinClick(e)).DisposeWith(disposable);
                this.ButtonMax.Events().Click.Subscribe(e => ButtonMaxClick(e)).DisposeWith(disposable);
                this.ItemExportToJPEG.Events().Click.Subscribe(_ => SaveAsPNG(ImageFormats.PNG)).DisposeWith(disposable);
                this.ItemExportToPNG.Events().Click.Subscribe(_ => SaveAsPNG(ImageFormats.JPEG)).DisposeWith(disposable);
            });
        }

        void StateNormalMaximaze()
        {
            this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }
        void ButtonCloseClick(RoutedEventArgs e)
        {
            this.Close();
        }
        void ButtonMinClick(RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        void ButtonMaxClick(RoutedEventArgs e)
        {
            StateNormalMaximaze();
        }

        private void HeaderClick(MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DockPanel)
            {
                if (e.ClickCount == 1)
                {

                    if (this.WindowState == WindowState.Maximized)
                    {
                        var point = PointToScreen(e.MouseDevice.GetPosition(this));

                        if (point.X <= RestoreBounds.Width / 2)
                            Left = 0;
                        else if (point.X >= RestoreBounds.Width)
                            Left = point.X - (RestoreBounds.Width - (this.ActualWidth - point.X));
                        else
                            Left = point.X - (RestoreBounds.Width / 2);

                        Top = point.Y - (this.Header.ActualHeight / 2);
                        WindowState = WindowState.Normal;
                    }

                    this.DragMove();
                }
                else
                {
                    StateNormalMaximaze();
                }
                e.Handled = true;
            }
        }

        void SaveAsPNG(ImageFormats format)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "SimpleStateMachine"; 
            dlg.Filter = (format == ImageFormats.JPEG)? "JPEG Image (.jpeg)|*.jpeg":"Png Image (.png)|*.png";

            DialogResult dialogResult = dlg.ShowDialog();
            if(dialogResult==System.Windows.Forms.DialogResult.OK)
            {
                this.NodesCanvas.SaveCanvasToImage(dlg.FileName, format);
            }              
        }

        #endregion SetupEvents

        #region Setup Commands
        private void SetupCommands()
        {          

        }
        #endregion Setup Commands
    }
}
