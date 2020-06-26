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
            SetupBinding();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {
                this.OneWayBind(this.ViewModel, x => x.Greeting, x => x.TextBlockElement.Text).DisposeWith(disposable);
            });
        }
    }
}
