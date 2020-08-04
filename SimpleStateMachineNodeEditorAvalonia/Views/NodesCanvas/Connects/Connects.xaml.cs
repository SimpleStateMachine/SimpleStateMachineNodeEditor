using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodesCanvasElements
{
    public partial class Connects : ReactiveUserControl<ConnectsNodesCanvasViewModel>
    {
        public Connects()
        {
            this.SetupBinding();
            this.InitializeComponent();     
        }

    }
}
