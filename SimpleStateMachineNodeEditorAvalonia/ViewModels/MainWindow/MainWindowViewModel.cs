using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class MainWindowViewModel: BaseViewModel
    {
        [Reactive] public NodesCanvasViewModel NodesCanvas { get; set; } = new NodesCanvasViewModel();

        public MainWindowViewModel()
        {

        }
    }
}
