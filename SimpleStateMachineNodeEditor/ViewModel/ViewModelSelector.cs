using System;
using System.Windows;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Commands;
using SimpleStateMachineNodeEditor.Helpers.Transformations;
using System.Reactive;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelSelector : ReactiveObject
    {
        [Reactive] public Size Size { get; set; }
        [Reactive] public bool? Visible { get; set; } = false;
        [Reactive] public MyPoint Point1 { get; set; } = new MyPoint();
        [Reactive] public MyPoint Point2 { get; set; } = new MyPoint();
        [Reactive] public Scale Scale { get; set; } = new Scale();

        public MyPoint Point1WithScale
        {
            get
            {
                double X = Point1.X;
                double Y = Point1.Y;

                if (Scale.ScaleX < 0)
                    X -= Size.Width;
                if (Scale.ScaleY < 0)
                    Y -= Size.Height;

                return new MyPoint(X, Y);
            }
        }
        public MyPoint Point2WithScale
        {
            get
            {
                double X = Point1.X;
                double Y = Point1.Y;

                if (Scale.ScaleX > 0)
                    X += Size.Width;
                if (Scale.ScaleY > 0)
                    Y += Size.Height;

                return new MyPoint(X, Y);
            }
        }



        public ViewModelSelector()
        {
            this.WhenAnyValue(x => x.Point1.Value, x => x.Point2.Value).Subscribe(_ => UpdateSize());
            SetupCommands();
        }
        private void UpdateSize()
        {
            MyPoint different = Point2 - Point1;
            Size = new Size(Math.Abs(different.X), Math.Abs(different.Y));
            Scale.Scales.Set(((different.X > 0) ? 1 : -1), ((different.Y > 0) ? 1 : -1));
        }

        #region Setup Commands
        public SimpleCommandWithParameter<MyPoint> CommandStartSelect { get; set; }
        public ReactiveCommand<Unit,Unit> CommandEndSelect { get; set; }

        private void SetupCommands()
        {
            CommandStartSelect = new SimpleCommandWithParameter<MyPoint>(StartSelect);
            CommandEndSelect =  ReactiveCommand.Create(EndSelect);
        }

        private void StartSelect(MyPoint point)
        {
            Visible = true;
            Point1.Set(point);
        }
        private void EndSelect()
        {

        }

        #endregion Setup Commands
    }
}
