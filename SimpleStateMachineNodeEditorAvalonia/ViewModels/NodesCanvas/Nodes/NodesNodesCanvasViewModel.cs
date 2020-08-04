using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class NodesNodesCanvasViewModel
    {
        public SourceList<NodeViewModel> Nodes = new SourceList<NodeViewModel>();
        public ObservableCollectionExtended<NodeViewModel> NodesForView = new ObservableCollectionExtended<NodeViewModel>();
        [Reactive] public NodeViewModel StartState { get; set; }

        public NodesNodesCanvasViewModel()
        {
            //Nodes.Connect().ObserveOnDispatcher().Bind(NodesForView).Subscribe();
            Nodes.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(NodesForView).Subscribe();
            Nodes.Add(new NodeViewModel());
        }
    }
}
