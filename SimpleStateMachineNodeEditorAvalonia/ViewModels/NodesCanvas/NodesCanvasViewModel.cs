using DynamicData;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class NodesCanvasViewModel : BaseViewModel
    {
        [Reactive] public NodesNodesCanvasViewModel Nodes { get; set; }
        [Reactive] public ConnectsNodesCanvasViewModel Connects { get; set; } = new ConnectsNodesCanvasViewModel();
        [Reactive] public SelectorViewModel Selector { get; set; } = new SelectorViewModel();
        [Reactive] public CutterViewModel Cutter { get; set; } = new CutterViewModel();

        public NodesCanvasViewModel()
        {
            Nodes = new NodesNodesCanvasViewModel(this);
        }
    }
}
