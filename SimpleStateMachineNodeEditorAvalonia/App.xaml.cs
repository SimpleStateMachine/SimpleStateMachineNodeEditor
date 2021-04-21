using System;
using System.Diagnostics;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Live.Avalonia;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;
using SimpleStateMachineNodeEditorAvalonia.Views;

namespace SimpleStateMachineNodeEditorAvalonia
{
    public class App : Application//, ILiveView
    {
//         public override void Initialize()
//         {
//             AvaloniaXamlLoader.Load(this);
//             Keyboard.Init();
//         }
//         public override void OnFrameworkInitializationCompleted()
//         {
//             if (Debugger.IsAttached || IsRelease())
//             {
//                 var window = new Window
//                 {
//                   Title = "SimpleStateMachineNodeEditor"
//                 };
//                 
//                 AttachDevTools(window);
//                 window.Content = CreateView(window);
//                 window.Show();
//             }
//             else
//             {
//                 var window = new LiveViewHost(this, Console.WriteLine)
//                 {
//                     Content = "Please wait for the app to rebuild from sources...",
//                     HorizontalContentAlignment = HorizontalAlignment.Center,
//                     VerticalContentAlignment = VerticalAlignment.Center,
//                 };
//
//                 AttachDevTools(window);
//                 window.StartWatchingSourceFilesForHotReloading();
//                 window.Show();
//             }
//             
//             RxApp.DefaultExceptionHandler = Observer.Create<Exception>(Console.WriteLine);
//             base.OnFrameworkInitializationCompleted();
//         }
//         public object CreateView(Window window)
//         {
//             var view = new NodesCanvas();
//             view.DataContext ??= CreateViewModel(window);
//             return view;
//         }
//         private static NodesCanvasViewModel CreateViewModel(Window window)
//         {
//             return  new NodesCanvasViewModel();
//         }
//         private static void AttachDevTools(TopLevel window)
//         {
// #if DEBUG
//             window.AttachDevTools();
// #endif
//         }
//         private static bool IsRelease()
//         {
// #if RELEASE
//         return true;
// #else
//             return false;
// #endif
//         }
        
        public override void Initialize()
        {
            Keyboard.Init();
        }
        
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    ViewModel = new MainWindowViewModel(),
                };
            }
        
            base.OnFrameworkInitializationCompleted();
        
        }
    }
}
