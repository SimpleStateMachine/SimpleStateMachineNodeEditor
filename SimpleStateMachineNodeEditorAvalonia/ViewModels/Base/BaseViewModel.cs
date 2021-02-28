using ReactiveUI;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public abstract class BaseViewModel : ReactiveObject
    {
        protected abstract void SetupCommands();
        protected abstract void SetupSubscriptions();

        public BaseViewModel()
        {
            SetupCommands();
            SetupSubscriptions();
        }
    }
}
