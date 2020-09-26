using Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class ConnectorViewModel 
    {
        protected override void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.Node).Where(x => x != null).Subscribe(_ => SubscriptToNode());
    
        }

        private void SubscriptToNode()
        {
            Node.WhenAnyValue(n => n.Point1).Buffer(2, 1).Select(value => value[1] - value[0]).Subscribe(x => Position = Position + x);
        }
    }
}
