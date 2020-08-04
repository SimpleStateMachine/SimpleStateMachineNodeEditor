using DynamicData;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
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
            Nodes.Connect().Bind(NodesForView).Subscribe();
            Nodes.Add(new NodeViewModel());
        }
    }
}
