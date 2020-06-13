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
using System.IO;
using System.Linq;
using SimpleStateMachineNodeEditor.Helpers.Extensions;

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
            ViewModel = new ViewModelMainWindow(this.NodesCanvas.ViewModel);
            SetupSubscriptions();
            SetupBinding();
            SetupEvents();
            
        }

        #region Setup Binding

        private void SetupBinding()
        {
            
            this.WhenActivated(disposable =>
            {
                var SelectedItem = this.ObservableForProperty(x => x.MessageList.SelectedItem).Select(x=>(x.Value as ViewModelMessage)?.Text);
                this.BindCommand(this.ViewModel, x => x.CommandCopyError, x => x.BindingCopyError, SelectedItem).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandCopyError, x => x.ItemCopyError, SelectedItem).DisposeWith(disposable);               

                this.OneWayBind(this.ViewModel, x => x.Messages, x => x.MessageList.ItemsSource).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.DebugEnable, x => x.LabelDebug.Visibility).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x => x.CountError, x => x.LabelError.Content, value=>value.ToString()+" Error").DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.CountWarning, x => x.LabelWarning.Content, value => value.ToString() + " Warning").DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.CountInformation, x => x.LabelInformation.Content, value => value.ToString() + " Information").DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.CountDebug, x => x.LabelDebug.Content, value => value.ToString() + " Debug").DisposeWith(disposable);


                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandChangeTheme, x => x.ButtonChangeTheme).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.CommandCopySchemeName, x => x.ItemCopySchemeName).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandSelectAll,        x => x.ItemSelectAll).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandZoomIn,           x => x.ButtonZoomIn).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandZoomOut,          x => x.ButtonZoomOut).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandZoomOriginalSize, x => x.ButtonZoomOriginalSize).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandCollapseUpAll,    x => x.ButtonCollapseUpAll).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExpandDownAll,    x => x.ButtonExpandDownAll).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandUndo , x => x.ItemUndo).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandUndo,  x => x.ButtonUndo).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandRedo,  x => x.ItemRedo).DisposeWith(disposable); 
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandRedo,  x => x.ButtonRedo).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExportToPNG, x => x.BindingExportToPNG).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExportToPNG, x => x.ItemExportToPNG).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExportToPNG, x => x.ButtonExportToPNG).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExportToJPEG, x => x.BindingExportToJPEG).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExportToJPEG, x => x.ItemExportToJPEG).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandNew, x => x.BindingNew).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandNew, x => x.ItemNew).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandNew, x => x.ButtonNew).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandOpen, x => x.BindingOpen).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandOpen, x => x.ItemOpen).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandOpen, x => x.ButtonOpen).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandSave, x => x.BindingSave).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandSave, x => x.ItemSave).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandSave, x => x.ButtonSave).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandSaveAs, x => x.BindingSaveAs).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandSaveAs, x => x.ItemSaveAs).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandSaveAs, x => x.ButtonSaveAs).DisposeWith(disposable);

                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExit, x => x.BindingExit).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExit, x => x.ItemExit).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExit, x => x.ButtonClose).DisposeWith(disposable);
            });
        }
        #endregion Setup Binding

        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenActivated(disposable =>
            {
                this.WhenAnyValue(x => x.ViewModel.NodesCanvas.SchemePath).Subscribe(value=> UpdateSchemeName(value)).DisposeWith(disposable);              
                this.WhenAnyValue(x => x.NodesCanvas.ViewModel.NeedExit).Where(x=>x).Subscribe(_ => this.Close()).DisposeWith(disposable);
                this.WhenAnyValue(x => x.ViewModel.CountError).Buffer(2, 1).Where(x => x[1] > x[0]).Subscribe(_ => ShowError()).DisposeWith(disposable);
                this.WhenAnyValue(x => x.ViewModel.NodesCanvas.Theme).Subscribe(_ => UpdateButton()).DisposeWith(disposable);
            });
        }
        private void UpdateSchemeName(string newName)
        {
            this.LabelSchemeName.Visibility = string.IsNullOrEmpty(newName) ? Visibility.Hidden : Visibility.Visible;
            if (!string.IsNullOrEmpty(newName))
            {
                this.LabelSchemeName.Content = Path.GetFileNameWithoutExtension(newName);
                this.LabelSchemeName.ToolTip = newName;
            }
        }
        #endregion Setup Subscriptions

        #region SetupEvents
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                this.MessageList.Events().MouseDoubleClick.Subscribe(_ => ViewModel.CommandCopyError.ExecuteWithSubscribe((MessageList.SelectedItem as ViewModelMessage)?.Text)).DisposeWith(disposable);
                this.LabelSchemeName.Events().MouseDoubleClick.WithoutParameter().InvokeCommand(ViewModel.CommandCopySchemeName).DisposeWith(disposable);
                this.Header.Events().PreviewMouseLeftButtonDown.Subscribe(e => HeaderClick(e)).DisposeWith(disposable);              
                this.ButtonMin.Events().Click.Subscribe(e => ButtonMinClick(e)).DisposeWith(disposable);
                this.ButtonMax.Events().Click.Subscribe(e => ButtonMaxClick(e)).DisposeWith(disposable);

                this.ErrorListExpander.Events().Collapsed.Subscribe(_=> ErrorListCollapse()).DisposeWith(disposable);
                this.ErrorListExpander.Events().Expanded.Subscribe(_ => ErrorListExpanded()).DisposeWith(disposable);

                this.LabelError.Events().PreviewMouseLeftButtonDown.Subscribe(e => SetDisplayMessageType(e, TypeMessage.Error)).DisposeWith(disposable);
                this.LabelWarning.Events().PreviewMouseLeftButtonDown.Subscribe(e => SetDisplayMessageType(e, TypeMessage.Warning)).DisposeWith(disposable);
                this.LabelInformation.Events().PreviewMouseLeftButtonDown.Subscribe(e => SetDisplayMessageType(e, TypeMessage.Information)).DisposeWith(disposable);
                this.LabelDebug.Events().PreviewMouseLeftButtonDown.Subscribe(e => SetDisplayMessageType(e, TypeMessage.Debug)).DisposeWith(disposable);
                this.LabelErrorList.Events().PreviewMouseLeftButtonDown.Subscribe(e=> SetDisplayMessageType(e, TypeMessage.All)).DisposeWith(disposable);
                this.LabelErrorListUpdate.Events().MouseLeftButtonDown.WithoutParameter().InvokeCommand(NodesCanvas.ViewModel.CommandErrorListUpdate).DisposeWith(disposable);
            });
        }

        private void SetDisplayMessageType(MouseButtonEventArgs e, TypeMessage  typeMessage)
        {          
            if ((ErrorListExpander.IsExpanded)&&(this.ViewModel.NodesCanvas.DisplayMessageType != typeMessage))
                e.Handled = true;

            this.ViewModel.NodesCanvas.DisplayMessageType = typeMessage;
        }
        private void ErrorListCollapse()
        {
            this.ErrorListSplitter.IsEnabled = false;
            this.Fotter.Height = new GridLength();
        }
        private void ErrorListExpanded()
        {
            this.ErrorListSplitter.IsEnabled = true;
            this.Fotter.Height = new GridLength(this.ViewModel.MaxHeightMessagePanel);
        }
        private void ShowError()
        {
            if (!this.ErrorListExpander.IsExpanded)
            {
                this.ErrorListExpander.IsExpanded = true;
                ErrorListExpanded();
            }
        }

        private void ButtonMinClick(RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void ButtonMaxClick(RoutedEventArgs e)
        {
            StateNormalMaximaze();
        }
        private void StateNormalMaximaze()
        {
            this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
            UpdateButton();          
        }
        private void UpdateButton()
        {
            if (this.WindowState == WindowState.Normal)
            {
                StateNormal();              
            }
            else
            {
                StateMaximize();
            }
        }
        private void StateMaximize()
        {         
            this.ButtonMaxRectangle.Fill = Application.Current.Resources["IconRestore"] as DrawingBrush;
            this.ButtonMaxRectangle.ToolTip = "Maximize";
        }
        private void StateNormal()
        {
            this.ButtonMaxRectangle.Fill = Application.Current.Resources["IconMaximize"] as DrawingBrush;
            this.ButtonMaxRectangle.ToolTip = "Restore down";
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
                        StateNormal();
                        this.WindowState = WindowState.Normal;
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

        #endregion SetupEvents


    }
}
