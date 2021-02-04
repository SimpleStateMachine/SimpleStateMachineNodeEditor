using Avalonia;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class NodesNodesCanvasViewModel : BaseViewModel
    {
      

        IObservableList<NodeViewModel> SelectedNodes;

        // public ObservableCollectionExtended<NodeViewModel> NodesForView { get; } = new ObservableCollectionExtended<NodeViewModel>();
        

      
        [Reactive] public NodeViewModel StartState { get; set; }

        
        
        
        SourceList<NodeViewModel> Nodes = new SourceList<NodeViewModel>();
        
        public ReadOnlyObservableCollection<NodeViewModel> NodesForView;
        
        public NodesNodesCanvasViewModel(NodesCanvasViewModel nodesCanvas)
        {

            var comparer = SortExpressionComparer<NodeViewModel>.Ascending(l => l.Point1.X);
            
            // Nodes.Connect().AutoRefresh(x=>x.Point1)
            //     .Sort(SortExpressionComparer<NodeViewModel>.Descending(t => t.Point1.X))
            //     .ObserveOn(RxApp.MainThreadScheduler)
            //     .Bind(out NodesForView)
            //     .Subscribe();
            
            SelectedNodes = Nodes.Connect().AutoRefresh(x=>x.IsSelect).Filter(x => x.IsSelect).AsObservableList();
            
            Nodes.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(out NodesForView).Subscribe();
            Nodes.Add(new NodeViewModel(nodesCanvas, "State 5", new Point(150, 100)));
            Nodes.Add(new NodeViewModel(nodesCanvas, "State 3", new Point(100, 100)));
           
        }
    }
}
