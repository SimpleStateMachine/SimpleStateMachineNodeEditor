using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class MainWindowViewModel: BaseViewModel
    {
        [Reactive] public NodesCanvasViewModel NodesCanvas { get; set; } = new();

        public MainWindowViewModel()
        {
        }
    }
}
