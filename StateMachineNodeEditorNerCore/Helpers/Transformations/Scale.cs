using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;

namespace SimpleStateMachineNodeEditor.Helpers.Transformations
{
    public class Scale : ReactiveObject
    {
        [Reactive] public MyPoint Scales { get; set; } = new MyPoint(1, 1);
        [Reactive] public MyPoint Center { get; set; } = new MyPoint();

        [Reactive] public double Value { get; set; } = 1.0;

        public double ScaleX
        {
            get { return Scales.X; }
        }
        public double ScaleY
        {
            get { return Scales.Y; }
        }

        public double CenterX
        {
            get { return Center.X; }
        }
        public double CenterY
        {
            get { return Center.Y; }
        }

        public Scale()
        {
            this.WhenAnyValue(x => x.Value).Subscribe(value => Scales.Set(value, value));
        }
    }
}
