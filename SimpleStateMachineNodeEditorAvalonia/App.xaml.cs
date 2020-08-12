using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;
using SimpleStateMachineNodeEditorAvalonia.Views;

namespace SimpleStateMachineNodeEditorAvalonia
{
    public class App : Application
    {
        public override void Initialize()
        {
            
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    ViewModel = new MainWindowViewModel(),
                };
                //var window = new MainWindow();
                //window.ViewModel = new MainWindowViewModel();
                //desktop.MainWindow = window;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
