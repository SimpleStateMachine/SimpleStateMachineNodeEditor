using Avalonia.ReactiveUI;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using SimpleStateMachineNodeEditorAvalonia.Helpers;


namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connector<TViewModel> : BaseView<TViewModel>
    where TViewModel: ConnectorViewModel
    {
        public Connector()
        {
       
        }

    }
}
