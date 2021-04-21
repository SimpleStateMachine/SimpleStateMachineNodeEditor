using Avalonia.ReactiveUI;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public abstract class BaseView<TViewModel>: ReactiveUserControl<TViewModel>
    where TViewModel:class
    {
        protected abstract void SetupBinding();
        protected abstract void SetupEvents();
        protected abstract void SetupSubscriptions();

        public BaseView()
        {
            SetupBinding();
            SetupEvents();
            SetupSubscriptions();
        }
    }
}
