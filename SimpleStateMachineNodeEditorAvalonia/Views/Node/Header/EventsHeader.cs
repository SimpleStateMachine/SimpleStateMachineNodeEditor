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
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
            });
        }
    }
}
