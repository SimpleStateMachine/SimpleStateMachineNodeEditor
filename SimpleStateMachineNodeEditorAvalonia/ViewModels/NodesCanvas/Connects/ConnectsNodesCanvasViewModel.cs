using DynamicData;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class ConnectsNodesCanvasViewModel
    {
        public SourceList<ConnectViewModel> Connects = new SourceList<ConnectViewModel>();
        public ObservableCollectionExtended<ConnectViewModel> ConnectsForView = new ObservableCollectionExtended<ConnectViewModel>();
        [Reactive] public ConnectViewModel DraggedConnect { get; set; }
    }
}
