using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SimpleStateMachineNodeEditor.Helpers.Transformations
{
    public class Translate : ReactiveObject
    {
        [Reactive] public MyPoint Translates { get; set; } = new MyPoint(1, 1);
    }
}
