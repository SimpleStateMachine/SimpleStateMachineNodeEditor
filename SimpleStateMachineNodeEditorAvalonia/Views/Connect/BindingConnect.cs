using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connect
    {
        protected override void SetupBinding()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                this.OneWayBind(ViewModel, x => x.StartPoint, x=>x.PathFigureConnect.StartPoint).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.EndPoint, x => x.BezierSegmentConnect.Point3).DisposeWith(disposable);
            });
        }
    }
}
