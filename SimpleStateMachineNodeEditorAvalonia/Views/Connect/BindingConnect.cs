using Avalonia;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connect
    {
        protected override void SetupBinding()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                this.OneWayBind(this.ViewModel, x => x.StartPoint, x=>x.PathFigureConnect.StartPoint).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.EndPoint, x => x.BezierSegmentConnect.Point3).DisposeWith(disposable);
            });
        }
    }
}
