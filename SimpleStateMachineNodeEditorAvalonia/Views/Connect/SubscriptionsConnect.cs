using System.Diagnostics;
using Avalonia;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connect
    {
        protected override void SetupSubscriptions()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                var points = this.ViewModel.WhenAnyValue(x => x.StartPoint, x => x.EndPoint).Select(points => UpdateMediumPoints(points.Item1, points.Item2));
                points.Select(x => x.Point1).BindTo(this.BezierSegmentConnect, x => x.Point1).DisposeWith(disposable);
                points.Select(x => x.Point2).BindTo(this.BezierSegmentConnect, x => x.Point2).DisposeWith(disposable);
            });
        }

        protected (Point Point1, Point Point2) UpdateMediumPoints(Point startPoint, Point endPoint)
        {
            Trace.WriteLine("Пересчет остальных координат");
            Point different = endPoint - startPoint;
            Point Point1 = new Point(startPoint.X + 3 * different.X / 8, startPoint.Y + 1 * different.Y / 8);
            Point Point2 = new Point(startPoint.X + 5 * different.X / 8, startPoint.Y + 7 * different.Y / 8);

            return (Point1, Point2);
        }
    }
}
