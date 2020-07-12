using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {          
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            SetupBinding();
        }

        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {
               
            });
        }
    }
}
