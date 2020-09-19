using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            SetupBinding();
           

     
            //KeyDown += 

            InitializeComponent();            
#if DEBUG
            this.AttachDevTools();
#endif
           
        }
    }
}
