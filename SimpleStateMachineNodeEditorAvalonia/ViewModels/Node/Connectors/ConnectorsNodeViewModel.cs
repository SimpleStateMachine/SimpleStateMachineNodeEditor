using DynamicData;
using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class ConnectorsNodeViewModel
    {
        public SourceList<ConnectorViewModel> Connectors { get; set; } = new SourceList<ConnectorViewModel>();

        public ObservableCollectionExtended<ConnectorViewModel> ConnectsForView = new ObservableCollectionExtended<ConnectorViewModel>();
    }
}
