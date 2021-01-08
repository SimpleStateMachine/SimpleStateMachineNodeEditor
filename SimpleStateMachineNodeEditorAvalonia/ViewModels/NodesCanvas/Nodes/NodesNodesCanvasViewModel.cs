using Avalonia;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class NodesNodesCanvasViewModel : BaseViewModel
    {
        SourceList<NodeViewModel> Nodes = new SourceList<NodeViewModel>();

        IObservableList<NodeViewModel> SelectedNodes;

        public ObservableCollectionExtended<NodeViewModel> NodesForView { get; } = new ObservableCollectionExtended<NodeViewModel>();
      
        [Reactive] public NodeViewModel StartState { get; set; }

        public NodesNodesCanvasViewModel(NodesCanvasViewModel nodesCanvas)
        {
            Nodes.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(NodesForView).Subscribe();
            SelectedNodes = Nodes.Connect().AutoRefresh(x=>x.IsSelect).Filter(x => x.IsSelect).AsObservableList();

            Nodes.Add(new NodeViewModel(nodesCanvas, "State 3", new Point(100, 100)));
            Nodes.Add(new NodeViewModel(nodesCanvas, "State 5", new Point(700, 100)));
        }
    }
}
