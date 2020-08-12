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
        public SourceList<ConnectorViewModel> Connectors { get; set; } = new SourceList<ConnectorViewModel>();

        public ObservableCollectionExtended<ConnectorViewModel> ConnectsForView = new ObservableCollectionExtended<ConnectorViewModel>();

        public ConnectorsNodeViewModel()
        {
            Connectors.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(ConnectsForView).Subscribe();
        }
    }
}
