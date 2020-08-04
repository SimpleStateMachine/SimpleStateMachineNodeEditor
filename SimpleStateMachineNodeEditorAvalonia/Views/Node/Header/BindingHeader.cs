using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodeElements
{
    public partial class Header
    {
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {
                this.OneWayBind(this.ViewModel, x => x.Name.Value, x => x.TextBoxHeader.Text).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Name.Enable, x => x.TextBoxHeader.IsEnabled).DisposeWith(disposable);
            });
        }
    }
}
