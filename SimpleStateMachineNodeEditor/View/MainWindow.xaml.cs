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
using System.IO;
using SimpleStateMachineNodeEditor.Helpers;
using Newtonsoft.Json;
using System.Linq;
using SimpleStateMachineNodeEditor.Helpers.Commands;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using System.Threading;
using System.Reactive;
using System.Collections.Generic;

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

                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExportToJPEG, x => x.BindingExportToJPEG).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExportToJPEG, x => x.ItemExportToJPEG).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.NodesCanvas.CommandExportToJPEG, x => x.ButtonExportToJPEG).DisposeWith(disposable);

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
                this.WhenAnyValue(x=>x.ViewModel.NodesCanvas.SchemePath).Subscribe(value=> UpdateSchemeName(value)).DisposeWith(disposable);
                this.WhenAnyValue(x => x.NodesCanvas.ViewModel.Messages.Count).Subscribe(_ => UpdateLabels()).DisposeWith(disposable);
                this.WhenAnyValue(x => x.NodesCanvas.ViewModel.NeedExit).Where(x=>x).Subscribe(_ => this.Close()).DisposeWith(disposable);
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
                this.LabelErrorListUpdate.Events().MouseLeftButtonDown.Subscribe(_ => NodesCanvas.ViewModel.CommandErrorListUpdate.ExecuteWithSubscribe()).DisposeWith(disposable);
            });
        }

        private void UpdateLabels()
        {
           var counts =  this.NodesCanvas.ViewModel.Messages.GroupBy(x => x.TypeMessage).ToDictionary(x=>x.Key,x=>x.Count());
           var countError = counts.Keys.Contains(TypeMessage.Error) ? counts[TypeMessage.Error].ToString() : "0";
           var countWarning = counts.Keys.Contains(TypeMessage.Warning) ? counts[TypeMessage.Warning].ToString() : "0";
           var countInformation = counts.Keys.Contains(TypeMessage.Information) ? counts[TypeMessage.Information].ToString() : "0";
           var countDebug = counts.Keys.Contains(TypeMessage.Debug) ? counts[TypeMessage.Debug].ToString() : "0";

            LabelError.Content = countError + " Error";
            LabelWarning.Content = countWarning + " Warning";
            LabelInformation.Content = countInformation + " Information";
            LabelDebug.Content = countDebug + " Debug";
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
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                this.ButtonMaxRectangle.Fill = System.Windows.Application.Current.Resources["IconRestore"] as DrawingBrush;
                this.ButtonMaxRectangle.ToolTip = "Maximize";
            }
            else
            {
                this.WindowState = WindowState.Normal;
                this.ButtonMaxRectangle.Fill = System.Windows.Application.Current.Resources["IconMaximize"] as DrawingBrush;
                this.ButtonMaxRectangle.ToolTip = "Restore down";
            }
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

        #endregion SetupEvents


    }
}
