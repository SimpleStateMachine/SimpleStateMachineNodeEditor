﻿using Avalonia;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using SimpleStateMachineNodeEditorAvalonia.Helpers.Extensions;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class ConnectorsNodeViewModel: BaseViewModel
    {
        public SourceList<RightConnectorViewModel> Connectors { get; set; } = new SourceList<RightConnectorViewModel>();
        public IObservableList<RightConnectorViewModel> SelectedConnectors;


        public ObservableCollectionExtended<RightConnectorViewModel> ConnectorsForView = new ObservableCollectionExtended<RightConnectorViewModel>();

        public ConnectorsNodeViewModel(NodeViewModel node)
        {
            Connectors.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(ConnectorsForView).Subscribe();
            SelectedConnectors = Connectors.Connect().AutoRefresh(x=>x.IsSelect)
                .Filter(x => x.IsSelect && x.Name.IsNotNullAndEmpty()).AsObservableList();
            
            
            Connectors.Add(new RightConnectorViewModel(node, "", false) {Position= node.Point1 + new Point(75.5, 52) });
            
            Connectors.Add(new RightConnectorViewModel(node, "Transition") { Position= node.Point1 + new Point(75.5, 52) });
        }
        
        
    }
}
