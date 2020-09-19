using Avalonia;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class NodesNodesCanvasViewModel : BaseViewModel
    {
        public SourceList<NodeViewModel> Nodes = new SourceList<NodeViewModel>();
        public ObservableCollectionExtended<NodeViewModel> NodesForView = new ObservableCollectionExtended<NodeViewModel>();
        [Reactive] public NodeViewModel StartState { get; set; }

        public NodesNodesCanvasViewModel(NodesCanvasViewModel nodesCanvas)
        {
            Nodes.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(NodesForView).Subscribe();

            Nodes.Add(new NodeViewModel(nodesCanvas, "State 3", new Point(100, 100)));
        }
    }
}
