using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class RightConnector
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
