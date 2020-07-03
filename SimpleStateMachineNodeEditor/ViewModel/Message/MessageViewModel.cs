using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Enums;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class MessageViewModel : ReactiveObject
    {
        public TypeMessage TypeMessage { get; set; }
        [Reactive] public string Text { get; set; }
        public MessageViewModel(TypeMessage typeMessage, string text)
        {
            TypeMessage = typeMessage;
            Text = text;
        }
    }
}
