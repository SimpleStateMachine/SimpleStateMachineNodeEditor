﻿using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connector
    {
        protected override void SetupBinding()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                this.OneWayBind(this.ViewModel, x => x.Name.Value, x => x.TextBoxConnector.Text).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Name.IsEnabled, x => x.TextBoxConnector.IsEnabled).DisposeWith(disposable);
            });
        }
    }
}
