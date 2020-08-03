using Avalonia;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class NodeViewModel:BaseViewModel
    {
        [Reactive] public StringEnabled Name { get; set; }
        [Reactive] public Point Position { get; set; }
        [Reactive] public Size Size { get; set; }
        [Reactive] public ConnectorViewModel Input { get; set; }
        [Reactive] public ConnectorViewModel Output { get; set; }
        [Reactive] public ConnectorViewModel FreeConnector { get; set; }
        [Reactive] public bool DeleteEnable { get; set; } = true;
        [Reactive] public bool IsCollapse { get; set; }

        public SourceList<ConnectorViewModel> Transitions { get; set; } = new SourceList<ConnectorViewModel>();

        public ObservableCollectionExtended<ConnectorViewModel> TransitionsForView = new ObservableCollectionExtended<ConnectorViewModel>();
    }
}
