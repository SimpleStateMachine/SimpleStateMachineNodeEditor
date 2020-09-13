using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Views.NodeElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodeElements
{
    public partial class Header
    {
        protected virtual void SetupSubscriptions()
        {
            this.WhenViewModelAnyValue(disposable =>
            {


            });
        }
        protected virtual void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                //ToggleButtonHeader.WhenAnyValue(x=>x.is)
            });
        }
    }
}
