using System;
using System.Windows;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Transformations;
using System.Reactive;
using SimpleStateMachineNodeEditor.Helpers.Extensions;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class SelectorViewModel : ReactiveObject
    {
        [Reactive] public Size Size { get; set; }
        [Reactive] public bool? Visible { get; set; } = false;
        [Reactive] public Point Point1 { get; set; }
        [Reactive] public Point Point2 { get; set; }
        [Reactive] public Scale Scale { get; set; } = new Scale();

        public Point Point1WithScale
        {
            get
            {
                double X = Point1.X;
                double Y = Point1.Y;

                if (Scale.ScaleX < 0)
                    X -= Size.Width;
                if (Scale.ScaleY < 0)
                    Y -= Size.Height;

                return new Point(X, Y);
            }
        }
        public Point Point2WithScale
        {
            get
            {
                double X = Point1.X;
                double Y = Point1.Y;

                if (Scale.ScaleX > 0)
                    X += Size.Width;
                if (Scale.ScaleY > 0)
                    Y += Size.Height;

                return new Point(X, Y);
            }
        }


        public SelectorViewModel()
        {   
            SetupCommands();
            SetupSubscriptions();
        }
        
        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.Point1, x => x.Point2).Subscribe(_ => UpdateSize());
        }
        private void UpdateSize()
        {
            Point different =  Point2.Subtraction(Point1);
            Size = new Size(Math.Abs(different.X), Math.Abs(different.Y));
            Scale.Scales = Scale.Scales.CreateNew(((different.X > 0) ? 1 : -1), ((different.Y > 0) ? 1 : -1));
        }

        #endregion Setup Subscriptions

        #region Setup Commands
        public ReactiveCommand<Point, Unit> CommandStartSelect { get; set; }

        private void SetupCommands()
        {
            CommandStartSelect = ReactiveCommand.Create<Point>(StartSelect);
        }

        private void StartSelect(Point point)
        {
            Visible = true;
            Point1 = point;
        }

        #endregion Setup Commands
    }
}
