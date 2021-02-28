using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class LeftConnector
    {
        protected override void SetupBinding()
        {
            base.SetupBinding();
            this.WhenViewModelAnyValue(disposable =>
            {
               
            });
           
        }
    }
}
