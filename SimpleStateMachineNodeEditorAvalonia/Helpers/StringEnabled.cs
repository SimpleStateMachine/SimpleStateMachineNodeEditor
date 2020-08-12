using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public class StringEnabled:ReactiveObject
    {
        [Reactive] public string Value { get; set; }
        [Reactive] public bool IsEnabled { get; set; }

        public StringEnabled(string value = "", bool isEnabled = false)
        {
            Value = value;
            IsEnabled = isEnabled;
        }
    }
}
