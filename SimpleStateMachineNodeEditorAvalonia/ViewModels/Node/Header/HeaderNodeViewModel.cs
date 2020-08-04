using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class HeaderNodeViewModel:BaseViewModel
    {
        [Reactive] public StringEnabled Name { get; set; } = new StringEnabled();
    }
}
