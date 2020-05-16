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
            ViewModel = new ViewModelMainWindow();         
            SetupBinding();
            SetupEvents();
            SetupCommands();
        }

        #region SetupBinding
         
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {
                this.ViewModel.NodesCanvas = this.NodesCanvas.ViewModel;
                var SelectedItem = this.ObservableForProperty(x => x.MessageList.SelectedItem).Select(x=>(x.Value as ViewModelMessage)?.Text);
                this.BindCommand(this.ViewModel,x=>x.CommandCopyError, x=>x.BindingCopyError, SelectedItem).DisposeWith(disposable);
                this.BindCommand(this.ViewModel, x => x.CommandCopyError, x => x.ItemCopyError, SelectedItem).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x=>x.Messages, x=>x.MessageList.ItemsSource).DisposeWith(disposable);
            });
        }
        #endregion SetupBinding

        #region Setup Commands
        private void SetupCommands()
        {
            this.WhenActivated(disposable =>
            {
                this.Events().KeyUp.Where(x => x.Key == Key.S && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))).Subscribe(_ => Save()).DisposeWith(disposable);
                this.Events().KeyUp.Where(x => x.Key == Key.F4 && (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))).Subscribe(_ => Close()).DisposeWith(disposable);
            });
        }
        #endregion Setup Commands

        #region SetupEvents
        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                this.Header.Events().PreviewMouseLeftButtonDown.Subscribe(e => HeaderClick(e)).DisposeWith(disposable);
                this.ButtonClose.Events().Click.Subscribe(_ => WithoutSaving(ButtonCloseClick)).DisposeWith(disposable);
                this.ButtonMin.Events().Click.Subscribe(e => ButtonMinClick(e)).DisposeWith(disposable);
                this.ButtonMax.Events().Click.Subscribe(e => ButtonMaxClick(e)).DisposeWith(disposable);
                this.ItemExportToJPEG.Events().Click.Subscribe(_ => ExportToImage(ImageFormats.JPEG)).DisposeWith(disposable);
                this.ItemExportToPNG.Events().Click.Subscribe(_ => ExportToImage(ImageFormats.PNG)).DisposeWith(disposable);
                this.ItemSave.Events().Click.Subscribe(_=> Save()).DisposeWith(disposable);
                this.ItemSaveAs.Events().Click.Subscribe(_ => SaveAs()).DisposeWith(disposable);
                this.ItemOpen.Events().Click.Subscribe(_ => WithoutSaving(Open)).DisposeWith(disposable);
                this.ItemExit.Events().Click.Subscribe(_=> WithoutSaving(ButtonCloseClick)).DisposeWith(disposable);
                this.ItemNew.Events().Click.Subscribe(_ => WithoutSaving(New)).DisposeWith(disposable);
                this.ItemUndo.Events().Click.Subscribe(_=>this.NodesCanvas.ViewModel.CommandUndo.Execute()).DisposeWith(disposable);
                this.ItemRedo.Events().Click.Subscribe(_ => this.NodesCanvas.ViewModel.CommandRedo.Execute()).DisposeWith(disposable);
                this.ErrorListExpander.Events().Collapsed.Subscribe(_=> ErrorListCollapse()).DisposeWith(disposable);
                this.ErrorListExpander.Events().Expanded.Subscribe(_ => ErrorListExpanded()).DisposeWith(disposable);
                //this.ErrorListExpander.Events().Expanded.Subscribe(_ => ErrorListExpanded()).DisposeWith(disposable);
            });
        }
        void ErrorListCollapse()
        {
            this.ErrorListSplitter.IsEnabled = false;
            this.Fotter.Height = new GridLength();
        }
        void ErrorListExpanded()
        {
            this.ErrorListSplitter.IsEnabled = true;
            //if(this.MessageList.Items.Count>this.ViewModel.CountShowingMessage)
                this.Fotter.Height = new GridLength(this.ViewModel.MaxHeightMessagePanel);
        }
        void StateNormalMaximaze()
        {
            if(this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                this.ButtonMaxRectangle.Fill = System.Windows.Application.Current.Resources["IconRestore"] as DrawingBrush;
            }
            else
            {
                this.WindowState = WindowState.Normal;
                this.ButtonMaxRectangle.Fill = System.Windows.Application.Current.Resources["IconMaximize"] as DrawingBrush;
            }

            //this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }
        void ButtonCloseClick()
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

        void ExportToImage(ImageFormats format)
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
        void New()
        {
            this.NodesCanvas.ViewModel.CommandNewScheme.Execute();
        }
        void WithoutSaving(Action action)
        {
            var result = MessageBoxResult.Yes;
            if (!this.NodesCanvas.ViewModel.ItSaved)
            {
                result = System.Windows.MessageBox.Show("Exit without saving ?", "Test", MessageBoxButton.YesNo);
            }

            if (result == MessageBoxResult.Yes)
                action.Invoke();
        }

        void Save()
        {
            if (string.IsNullOrEmpty(this.ViewModel.Path))
            {
                SaveAs();
            }
            else
            {
                this.NodesCanvas.ViewModel.CommandSave.Execute(this.ViewModel.Path);
            }
        }
        void SaveAs()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "SimpleStateMachine";
            dlg.Filter = "XML-File | *.xml";
            
            DialogResult dialogResult = dlg.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this.NodesCanvas.ViewModel.CommandSave.Execute(dlg.FileName);
                this.ViewModel.Path = dlg.FileName;
            }
        }

        void Open()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "SimpleStateMachine";
            dlg.Filter = "XML-File | *.xml";

            DialogResult dialogResult = dlg.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this.NodesCanvas.ViewModel.CommandOpen.Execute(dlg.FileName);
            }
       
        }
        #endregion SetupEvents


    }
}
