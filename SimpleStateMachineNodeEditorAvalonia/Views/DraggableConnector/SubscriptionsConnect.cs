using Avalonia;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class DraggableConnector
    {
        protected override void SetupSubscriptions()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                var points = ViewModel
                    .WhenAnyValue(x => x.StartPoint,
                        x => x.EndPoint)
                    .Select(points => Connect.UpdateMediumPoints(points.Item1, points.Item2));
                
                points.Select(x => x.Point1)
                    .BindTo(BezierSegmentConnect,
                        x => x.Point1).DisposeWith(disposable);
                
                points.Select(x => x.Point2)
                    .BindTo(BezierSegmentConnect, 
                        x => x.Point2).DisposeWith(disposable);
            });
        }
    }
}
