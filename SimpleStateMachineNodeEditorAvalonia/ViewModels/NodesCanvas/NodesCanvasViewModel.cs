using DynamicData;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public class NodesCanvasViewModel: BaseViewModel
    {
        [Reactive] public NodeViewModel StartState { get; set; }
        [Reactive] public SelectorViewModel Selector { get; set; } = new SelectorViewModel();
        [Reactive] public CutterViewModel Cutter { get; set; }
        [Reactive] public ConnectViewModel DraggedConnect { get; set; }

        public SourceList<NodeViewModel> Nodes = new SourceList<NodeViewModel>();
        public ObservableCollectionExtended<NodeViewModel> NodesForView = new ObservableCollectionExtended<NodeViewModel>();

        public SourceList<ConnectViewModel> Connects = new SourceList<ConnectViewModel>();
        public ObservableCollectionExtended<ConnectViewModel> ConnectsForView = new ObservableCollectionExtended<ConnectViewModel>();
    }
}
