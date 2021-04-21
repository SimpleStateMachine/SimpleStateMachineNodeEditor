using System.Reactive.Disposables;
using Avalonia.Input;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using Avalonia;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connect
    {
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {

            });
        }
    }
}
