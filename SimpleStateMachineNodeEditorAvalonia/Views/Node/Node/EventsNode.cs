using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Views.NodeElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Node
    {
        protected override void SetupEvents()
        {
   
            this.WhenViewModelAnyValue(disposable =>
            {
            });
        }
    }
}
