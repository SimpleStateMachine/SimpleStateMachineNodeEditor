using Avalonia.Controls;
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
        protected override void SetupSubscriptions()
        {
            this.WhenViewModelAnyValue(disposable =>
            {

            });
        }
    }
}
