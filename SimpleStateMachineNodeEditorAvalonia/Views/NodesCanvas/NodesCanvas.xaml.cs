using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class NodesCanvas : ReactiveUserControl<NodesCanvasViewModel>
    {
        public NodesCanvas()
        {
            this.SetupBinding();
            this.InitializeComponent();
           
        }
    }
}
