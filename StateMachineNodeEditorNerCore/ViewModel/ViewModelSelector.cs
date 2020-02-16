using System;
using System.Windows;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using StateMachineNodeEditorNerCore.Helpers;
using StateMachineNodeEditorNerCore.Helpers.Commands;
using StateMachineNodeEditorNerCore.Helpers.Transformations;

namespace StateMachineNodeEditorNerCore.ViewModel
{
    public class ViewModelSelector : ReactiveObject
    {
        /// <summary>
        /// Размер Селектора
        /// </summary>
        [Reactive] public Size Size { get; set; }

        /// <summary>
        /// Отображается ли выделение
        /// </summary>
        [Reactive] public bool? Visible { get; set; } = false;

        /// <summary>
        /// Точка левого верхнего угла без учета отражений
        /// </summary>
        [Reactive] public MyPoint Point1 { get; set; } = new MyPoint();

        /// <summary>
        /// Точка нижнего правого угла без учета отражений
        /// </summary>
        [Reactive] public MyPoint Point2 { get; set; } = new MyPoint();


        /// <summary>
        /// Точка левого верхнего угла c учетом отражений
        /// </summary>
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

        /// <summary>
        /// Точка левого верхнего угла c учетом отражений
        /// </summary>
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

        /// <summary>
        /// Масштаб
        /// </summary>
        [Reactive] public Scale Scale { get; set; } = new Scale();

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
        public SimpleCommand CommandEndSelect { get; set; }

        private void SetupCommands()
        {
            CommandStartSelect = new SimpleCommandWithParameter<MyPoint>(this, StartSelect);
            CommandEndSelect = new SimpleCommand(this, EndSelect);
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
