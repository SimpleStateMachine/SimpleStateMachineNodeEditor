using Avalonia;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class NodesNodesCanvasViewModel : BaseViewModel
    {
        protected IObservableList<NodeViewModel> SelectedNodes;
        [Reactive] public NodeViewModel StartState { get; set; }
        protected SourceList<NodeViewModel> Nodes = new();
        public ReadOnlyObservableCollection<NodeViewModel> NodesForView;
        
        public NodesNodesCanvasViewModel(NodesCanvasViewModel nodesCanvas)
        {
            
            SelectedNodes = Nodes.Connect()
                .AutoRefresh(x=>x.IsSelect)
                .Filter(x => x.IsSelect)
                .AsObservableList();
            
            Nodes.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(out NodesForView).Subscribe();
            Nodes.Add(new NodeViewModel(nodesCanvas, "State 5", new Point(150, 100)));
            Nodes.Add(new NodeViewModel(nodesCanvas, "State 3", new Point(100, 100)));
           
        }
    }
}
