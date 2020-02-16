using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace StateMachineNodeEditorNerCore.Helpers.Transformations
{
    public class Translate : ReactiveObject
    {
        [Reactive] public MyPoint Translates { get; set; } = new MyPoint(1, 1);
    }
}
