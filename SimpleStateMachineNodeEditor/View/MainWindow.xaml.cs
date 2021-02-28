using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using ReactiveUI;
using SimpleStateMachineNodeEditor.ViewModel;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using System.IO;
using SimpleStateMachineNodeEditor.Helpers.Extensions;

namespace SimpleStateMachineNodeEditor.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainWindowViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(MainWindowViewModel), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainWindowViewModel)value; }
        }
        #endregion ViewModel

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel(NodesCanvas.ViewModel);
            SetupSubscriptions();
            SetupBinding();
            SetupEvents();
            
        }

        #region Setup Binding

        private void SetupBinding()
        {
            
            this.WhenActivated(disposable =>
            {
                //this.OneWayBind(this.ViewModel, x => x.Tests, x => x.TableOfTransitions.ItemsSource).DisposeWith(disposable);

                var SelectedItem = this.ObservableForProperty(x => x.MessageList.SelectedItem).Select(x=>(x.Value as MessageViewModel)?.Text);
                this.BindCommand(ViewModel, x => x.CommandCopyError, x => x.BindingCopyError, SelectedItem).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.CommandCopyError, x => x.ItemCopyError, SelectedItem).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.NodesCanvas.Dialog, x => x.Dialog.ViewModel).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.Transitions, x => x.TableOfTransitions.ItemsSource).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.Messages, x => x.MessageList.ItemsSource).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.DebugEnable, x => x.LabelDebug.Visibility).DisposeWith(disposable);

                this.OneWayBind(ViewModel, x => x.CountError, x => x.LabelError.Content, value=>value.ToString()+" Error").DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.CountWarning, x => x.LabelWarning.Content, value => value.ToString() + " Warning").DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.CountInformation, x => x.LabelInformation.Content, value => value.ToString() + " Information").DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.CountDebug, x => x.LabelDebug.Content, value => value.ToString() + " Debug").DisposeWith(disposable);


                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandChangeTheme, x => x.ButtonChangeTheme).DisposeWith(disposable);

                this.BindCommand(ViewModel, x => x.CommandCopySchemeName, x => x.ItemCopySchemeName).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandSelectAll,        x => x.ItemSelectAll).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandZoomIn,           x => x.ButtonZoomIn).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandZoomOut,          x => x.ButtonZoomOut).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandZoomOriginalSize, x => x.ButtonZoomOriginalSize).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandCollapseUpAll,    x => x.ButtonCollapseUpAll).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandExpandDownAll,    x => x.ButtonExpandDownAll).DisposeWith(disposable);

                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandUndo , x => x.ItemUndo).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandUndo,  x => x.ButtonUndo).DisposeWith(disposable);

                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandRedo,  x => x.ItemRedo).DisposeWith(disposable); 
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandRedo,  x => x.ButtonRedo).DisposeWith(disposable);

                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandExportToPNG, x => x.BindingExportToPNG).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandExportToPNG, x => x.ItemExportToPNG).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandExportToPNG, x => x.ButtonExportToPNG).DisposeWith(disposable);

                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandExportToJPEG, x => x.BindingExportToJPEG).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandExportToJPEG, x => x.ItemExportToJPEG).DisposeWith(disposable);

                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandNew, x => x.BindingNew).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandNew, x => x.ItemNew).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandNew, x => x.ButtonNew).DisposeWith(disposable);

                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandOpen, x => x.BindingOpen).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandOpen, x => x.ItemOpen).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandOpen, x => x.ButtonOpen).DisposeWith(disposable);

                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandSave, x => x.BindingSave).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandSave, x => x.ItemSave).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandSave, x => x.ButtonSave).DisposeWith(disposable);

                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandSaveAs, x => x.BindingSaveAs).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandSaveAs, x => x.ItemSaveAs).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandSaveAs, x => x.ButtonSaveAs).DisposeWith(disposable);

                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandExit, x => x.BindingExit).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandExit, x => x.ItemExit).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.NodesCanvas.CommandExit, x => x.ButtonClose).DisposeWith(disposable);
            });
        }
        #endregion Setup Binding

        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenActivated(disposable =>
            {
                this.WhenAnyValue(x => x.ViewModel.NodesCanvas.SchemePath).Subscribe(value=> UpdateSchemeName(value)).DisposeWith(disposable);              
                this.WhenAnyValue(x => x.NodesCanvas.ViewModel.NeedExit).Where(x=>x).Subscribe(_ => Close()).DisposeWith(disposable);
                this.WhenAnyValue(x => x.ViewModel.CountError).Buffer(2, 1).Where(x => x[1] > x[0]).Subscribe(_ => ShowError()).DisposeWith(disposable);
                this.WhenAnyValue(x => x.ViewModel.NodesCanvas.Theme).Subscribe(_ => UpdateButton()).DisposeWith(disposable);

                this.WhenAnyValue(x => x.ActualWidth).Subscribe(value => TableOfTransitionsColumn.MaxWidth = value - 50).DisposeWith(disposable);
                this.WhenAnyValue(x => x.ActualHeight).Subscribe(value => Fotter.MaxHeight = value - 150).DisposeWith(disposable);
            });
        }
        private void UpdateSchemeName(string newName)
        {
            LabelSchemeName.Visibility = string.IsNullOrEmpty(newName) ? Visibility.Hidden : Visibility.Visible;
            if (!string.IsNullOrEmpty(newName))
            {
                LabelSchemeName.Content = Path.GetFileNameWithoutExtension(newName);
                LabelSchemeName.ToolTip = newName;
            }
        }

        private void ShowError()
        {
            if (!ErrorListExpander.IsExpanded)
            {
                ErrorListExpander.IsExpanded = true;
                ErrorListExpanded();
            }
        }
        #endregion Setup Subscriptions

        #region SetupEvents
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                MessageList.Events().MouseDoubleClick.Subscribe(_ => ViewModel.CommandCopyError.ExecuteWithSubscribe((MessageList.SelectedItem as MessageViewModel)?.Text)).DisposeWith(disposable);
                LabelSchemeName.Events().MouseDoubleClick.WithoutParameter().InvokeCommand(ViewModel.CommandCopySchemeName).DisposeWith(disposable);
                Header.Events().PreviewMouseLeftButtonDown.Subscribe(e => HeaderClick(e)).DisposeWith(disposable);              
                ButtonMin.Events().Click.Subscribe(e => ButtonMinClick(e)).DisposeWith(disposable);
                ButtonMax.Events().Click.Subscribe(e => ButtonMaxClick(e)).DisposeWith(disposable);

                ErrorListExpander.Events().Collapsed.Subscribe(_=> ErrorListCollapse()).DisposeWith(disposable);
                ErrorListExpander.Events().Expanded.Subscribe(_ => ErrorListExpanded()).DisposeWith(disposable);

                TableOfTransitionsExpander.Events().Collapsed.Subscribe(_ => TableOfTransitionsCollapse()).DisposeWith(disposable);
                TableOfTransitionsExpander.Events().Expanded.Subscribe(_ => TableOfTransitionsExpanded()).DisposeWith(disposable);

                LabelError.Events().PreviewMouseLeftButtonDown.Subscribe(e => SetDisplayMessageType(e, TypeMessage.Error)).DisposeWith(disposable);
                LabelWarning.Events().PreviewMouseLeftButtonDown.Subscribe(e => SetDisplayMessageType(e, TypeMessage.Warning)).DisposeWith(disposable);
                LabelInformation.Events().PreviewMouseLeftButtonDown.Subscribe(e => SetDisplayMessageType(e, TypeMessage.Information)).DisposeWith(disposable);
                LabelDebug.Events().PreviewMouseLeftButtonDown.Subscribe(e => SetDisplayMessageType(e, TypeMessage.Debug)).DisposeWith(disposable);
                LabelErrorList.Events().PreviewMouseLeftButtonDown.Subscribe(e=> SetDisplayMessageType(e, TypeMessage.All)).DisposeWith(disposable);
                LabelErrorListUpdate.Events().MouseLeftButtonDown.WithoutParameter().InvokeCommand(NodesCanvas.ViewModel.CommandErrorListUpdate).DisposeWith(disposable);

                ButtonAddNode.Events().PreviewMouseLeftButtonDown.Subscribe(e=> RadioButtonUnChecked(ButtonAddNode, NodeCanvasClickMode.AddNode, e )).DisposeWith(disposable);
                ButtonDeleteNode.Events().PreviewMouseLeftButtonDown.Subscribe(e => RadioButtonUnChecked(ButtonDeleteNode, NodeCanvasClickMode.Delete, e)).DisposeWith(disposable);
                ButtonStartSelect.Events().PreviewMouseLeftButtonDown.Subscribe(e => RadioButtonUnChecked(ButtonStartSelect, NodeCanvasClickMode.Select, e)).DisposeWith(disposable);
                ButtonStartCut.Events().PreviewMouseLeftButtonDown.Subscribe(e => RadioButtonUnChecked(ButtonStartCut, NodeCanvasClickMode.Cut, e)).DisposeWith(disposable);
            });
        }

        private void SetDisplayMessageType(MouseButtonEventArgs e, TypeMessage  typeMessage)
        {          
            if ((ErrorListExpander.IsExpanded)&&(ViewModel.NodesCanvas.DisplayMessageType != typeMessage))
                e.Handled = true;

            ViewModel.NodesCanvas.DisplayMessageType = typeMessage;
        }
        private void RadioButtonUnChecked(RadioButton radioButton, NodeCanvasClickMode clickMode, MouseButtonEventArgs e)
        {
            if(radioButton.IsChecked==true)
            {
                radioButton.IsChecked = false;
                e.Handled = true;

                ViewModel.NodesCanvas.ClickMode = NodeCanvasClickMode.Default;
            }
            else
            {
                ViewModel.NodesCanvas.ClickMode = clickMode;
            }
        }
        private void ErrorListCollapse()
        {
            ErrorListSplitter.IsEnabled = false;
            Fotter.Height = new GridLength();
            Fotter.MinHeight = 18;
            
        }
        private void ErrorListExpanded()
        {
            ErrorListSplitter.IsEnabled = true;
            Fotter.Height = new GridLength(ViewModel.DefaultHeightMessagePanel);
            Fotter.MinHeight = 52;
        }

        private void TableOfTransitionsCollapse()
        {
            TableOfTransitionsSplitter.IsEnabled = false;
            TableOfTransitionsColumn.Width = new GridLength();
            TableOfTransitionsColumn.MinWidth = 18;
        }
        private void TableOfTransitionsExpanded()
        {
            TableOfTransitionsSplitter.IsEnabled = true;
            //this.TableOfTransitionsColumn.Width = new GridLength(this.ViewModel.DefaultWidthTransitionsTable);
            TableOfTransitionsColumn.MinWidth = 52;
        }

        private void ButtonMinClick(RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void ButtonMaxClick(RoutedEventArgs e)
        {
            StateNormalMaximaze();
        }
        private void StateNormalMaximaze()
        {
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
            UpdateButton();          
        }
        private void UpdateButton()
        {
            if (WindowState == WindowState.Normal)
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
            ButtonMaxRectangle.Fill = Application.Current.Resources["IconRestore"] as DrawingBrush;
            ButtonMaxRectangle.ToolTip = "Maximize";
        }
        private void StateNormal()
        {
            ButtonMaxRectangle.Fill = Application.Current.Resources["IconMaximize"] as DrawingBrush;
            ButtonMaxRectangle.ToolTip = "Restore down";
        }
        private void HeaderClick(MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DockPanel)
            {
                if (e.ClickCount == 1)
                {

                    if (WindowState == WindowState.Maximized)
                    {
                        var point = PointToScreen(e.MouseDevice.GetPosition(this));

                        if (point.X <= RestoreBounds.Width / 2)
                            Left = 0;

                        else if (point.X >= RestoreBounds.Width)
                            Left = point.X - (RestoreBounds.Width - (ActualWidth - point.X));

                        else
                            Left = point.X - (RestoreBounds.Width / 2);

                        Top = point.Y - (Header.ActualHeight / 2);

                        StateNormal();
                        WindowState = WindowState.Normal;
                    }

                    DragMove();
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
