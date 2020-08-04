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
        [Reactive] public HeaderNodeViewModel Header { get; set; } = new HeaderNodeViewModel();
        [Reactive] public ConnectorsNodeViewModel Connectors { get; set; } = new ConnectorsNodeViewModel();
        [Reactive] public ConnectorViewModel Input { get; set; }
        [Reactive] public ConnectorViewModel Output { get; set; }
        [Reactive] public ConnectorViewModel FreeConnector { get; set; }
        [Reactive] public Point Position { get; set; }
        [Reactive] public Size Size { get; set; }
        [Reactive] public bool DeleteEnable { get; set; } = true;
        [Reactive] public bool IsCollapse { get; set; }       
    }
}
