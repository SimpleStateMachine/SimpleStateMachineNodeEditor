using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodeElements
{
    public partial class Header
    {
        protected override void SetupBinding()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                this.OneWayBind(this.ViewModel, x => x.Name.Value, x => x.TextBoxHeader.Text).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Name.IsEnabled, x => x.TextBoxHeader.IsEnabled).DisposeWith(disposable);
                this.Bind(this.ViewModel, x => x.IsCollapse, x => x.ToggleButtonHeader.IsChecked).DisposeWith(disposable);
            });
        }
    }
}
