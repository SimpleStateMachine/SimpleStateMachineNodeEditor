using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodesCanvasElements
{
    public partial class Nodes : ReactiveUserControl<NodesNodesCanvasViewModel>
    {
        public Nodes()
        {
            this.SetupBinding();
            this.InitializeComponent();
           
        }

    }
}
