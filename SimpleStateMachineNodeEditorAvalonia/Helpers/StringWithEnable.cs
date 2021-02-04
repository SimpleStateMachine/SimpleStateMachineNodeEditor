using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public class StringWithEnable:ReactiveObject
    {
        [Reactive] public string Value { get; set; }
        [Reactive] public bool IsEnabled { get; set; }

        public StringWithEnable(string value = "", bool isEnabled = true)
        {
            Value = value;
            IsEnabled = isEnabled;
        }

        public override string ToString()
        {
            return Value;
        }
        
        
        
    }
}
