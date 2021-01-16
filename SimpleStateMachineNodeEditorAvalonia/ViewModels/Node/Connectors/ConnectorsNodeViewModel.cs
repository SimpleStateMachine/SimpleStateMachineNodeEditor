using Avalonia;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class ConnectorsNodeViewModel: BaseViewModel
    {
        public SourceList<RightConnectorViewModel> Connectors { get; set; } = new SourceList<RightConnectorViewModel>();

        public ObservableCollectionExtended<RightConnectorViewModel> ConnectorsForView = new ObservableCollectionExtended<RightConnectorViewModel>();

        public ConnectorsNodeViewModel(NodeViewModel node)
        {
            Connectors.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(ConnectorsForView).Subscribe();
            
            Connectors.Add(new RightConnectorViewModel(node, "", false) { Position= node.Point1 + new Point(75.5, 52) });
            
            Connectors.Add(new RightConnectorViewModel(node, "Transiti") { Position= node.Point1 + new Point(75.5, 52) });
        }
        
        
    }
}
